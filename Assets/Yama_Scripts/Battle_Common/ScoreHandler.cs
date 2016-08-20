using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {

	// 各ポイントのスプライト用変数 
	public enum PointTextKey{ Miss, Bad, Good, Great, Perfect }

	public  GameObject   touchRingPrefab;	//TouchRingプレハブ
	public  GameObject   pointText;			//ポイントテキストプレハブ
	public  Sprite[]     textSprite;		//各ポイント評価用のテキストスプライト
	public  PointHandler pointHandler;		//ポイントハンドラ（PointHandlerのセット)
	public  GageHandler  gageHandler; 		//ゲージハンドラー
	public  ComboHandler comboHandler;		//コンボハンドラー
	private GameObject   buttonChild;   	//一度全て生成してから移動するような修正に使う（後にいらない）
	public  GameObject   touchBar;	  		//TouchBar
	public  Animator     buttonAnim;	  	//ボタンアニメーションの修正に使う変数（※ここ重要）
	public  GameObject   flickAnim;			// フリック成功時のアニメーション
	public  Vector3      flickVct;			// フリックの向きを正規化
	private int          flickCount = 0; 	// フリックしたと見なされる回数
	private int          flickDirection; 	// フリックの向き
	public  int          flickFlag;			// ScoreCreatorで生成されたフリックスコアの矢印の方向に番号をふる

	// フリックされた起点から移動点を司る変数
	Vector3 flickStartPos;
	Vector3 flickEndPos;
	Vector3 startScreenPos;
	Vector3 endScreenPos;
	Vector3 startWorldPos;
	Vector3 endWorldPos;

	private Vector3    PositionInGameLong;	// ロングタップ用の変数
	public  bool       isLongTap;			// ロングタップされているかどうかを管理する変数
	public  GameObject longTapAnim;         // ロングタップ成功時のアニメーション

	public long longTapStartTick;			// ScoreHandlerより、tmp.tickが代入される
	public long longTapEndTick;				// ScoreHandlerより、ttmp.nextTickが代入される

	// Update内のif文に入るかどうかを管理
	private bool isChecked;

	//何度も使うのでOnScoreClick内の処理を関数化
	private float distancePoint;
	private float longTapEndPoint;

	RectTransform touchBarRect;
	RectTransform myRect;
	RectTransform endRect;

	// タッチ判定で毎回GetComponemtして処理が重くなるのに対処
	BoardMove  boardMove;
	GameObject longTapAnimObj;

	// プレイヤーゲージの増減のため、値をPowerProgressに値を渡す
	private PowerProgress powerProgress;

	public AudioManager audioManager;

	// ロングタップエンドかスタートかを見る
	public int tapType;
	GameObject longTapEndObj;

	// 生成されてから削除されるまでのTick
	int tick = 79000;

	// *******************************************************************************************************************
	void Start () {

		Invoke (
			"AutoDestroy",
			(60 * tick * 2) / (TimeManager.tempo * 9600f)
		); 
			

		pointHandler  = FindObjectOfType<PointHandler> ();	 // PointHandlerのセット
		gageHandler   = FindObjectOfType<GageHandler>  ();	 // GageHandlerのセット 
		comboHandler  = FindObjectOfType<ComboHandler> ();	 // ComboHandlerのセット
		powerProgress = FindObjectOfType<PowerProgress>();	 // PowerProgressのセット
		audioManager  = FindObjectOfType<AudioManager> ();	 // AudioManagerのセット


		// タッチ時のボタンアニメーションの修正（※ここ重要）
		string buttonNum  = gameObject.tag;
			   buttonAnim = GameObject.Find ("TouchBar" + buttonNum).GetComponent<Animator> ();


		touchBarRect  = GameObject.Find("TouchBar1").GetComponent<RectTransform>();
		myRect		  = GetComponent<RectTransform>();

		longTapEndObj = null;
		endRect		  = null;

		// タッチ判定で毎回GetComponemtして処理が重くなるのに対処
		boardMove = GetComponentInParent<BoardMove>();

	} // *******************************************************************************************************************


	void Update() {

		// 後にTouchFailed（）メソッドなんかを作り、そっちにお引越すればいいかなぁと。

		//Debug.Log("myRect : " + myRect.position.y);
		//Debug.Log("touchBarRect1 : " + touchBarRect.position.y);

		if (myRect.position.y <= touchBarRect.position.y && !isChecked && tapType != 3) {
			
			showText(0);

			// 効果音を鳴らす（miss時）
			// 第2引数で音量を調整できる（この場合0.6f）
			// Update内でFind関数を使う事は非推奨だが、条件を満たさないと呼ばれないif内なので、現状このまま。
			FindObjectOfType<AudioManager>().
			GetComponent<AudioSource>().
			PlayOneShot(FindObjectOfType<AudioManager>().
			onMiss,0.6f
			);

			// ゲージを減算(missしたときなのでここに記載)
			GameDate.GagePoint -= 5;

			// ゲージを制限
			if (GameDate.GagePoint < 0)
				GameDate.GagePoint = 0;

			// ゲージを表示
			gageHandler.setGage(GameDate.GagePoint);

			//生成されたスコアに関しては一度呼ばれたらもう呼ばない
			isChecked = true;

		}

		//Debug.Log("tapType : " + tapType);
		//Debug.Log("TimeManager.tick : " + TimeManager.tick);
		//Debug.Log("longTapEndTick + 9600 : " + (longTapEndTick + 9600));
		//Debug.Log("longTapEndObj : " + longTapEndObj);
		//Debug.Log("longTapEndTick : " + longTapEndTick);
		if (tapType == 3 && TimeManager.tick > (longTapEndTick + 9600)) {

			if (longTapEndObj == null) {

				// 先に下記のFindLongTapEndObj関数を見てね♡
				longTapEndObj = FindLongTapEndObj();
				endRect       = longTapEndObj.GetComponent<RectTransform>();

			} else {
				
				//Debug.Log("endRect : " + endRect.position.y);
				//Debug.Log("touchBarRect2 : " + touchBarRect.position.y);
				//Debug.Log("isChecked : " + isChecked);

				// !isCheckということは、このif内にまだ入っていないということ。一度入ったらもう入らない
				//// こうする事で（isCheckedでフラグ管理する事で）、二度Missが表示されるといった事象を防ぐ
				if (endRect.position.y <= touchBarRect.position.y && !isChecked) {
					
					//Debug.Log("ははは");

					isChecked = true;
					showText(0);

					// いい加減ここは直そうぜ…
					FindObjectOfType<AudioManager>().
					GetComponent<AudioSource>().
					PlayOneShot(FindObjectOfType<AudioManager>().
					onMiss, 0.6f
					);

					GameDate.GagePoint -= 5;

					if (GameDate.GagePoint < 0)
						GameDate.GagePoint = 0;

					gageHandler.setGage(GameDate.GagePoint);

					// マージを設けて ロングタップスコアをある一定の時間が過ぎたら削除
					Destroy( gameObject,    (60 * tick * 2) / (TimeManager.tempo * 9600f) );

					// あまり速すぎると、ロングタップエンドスコアが画面上から急に消えるので、マージを設ける
					Destroy( longTapEndObj, (60 * tick * 2) / (TimeManager.tempo * 9600f) );

				}
			}
		}

	} // *******************************************************************************************************************


	// タッチ判定
	public void OnScoreClick(){

		audioManager.isTouch    = true;

		// Board外の位置を計算 / 要するに、タッチした座標を取得（Shimo）/ GetComponentInParentは親のコンポーネントを取得する(Yama)
		Vector3 PositionInGame = boardMove.transform.localPosition + transform.localPosition;

		// 取得した座標をもとに、タッチバーからの距離を数値化（単位として正規化）
		distancePoint = GetDistancePoint (PositionInGame);

		// ポイントが0より大きければ譜面を削除
		if(distancePoint > 0){

			// タッチに成功したオブジェクトを削除して、画面のスコアとゲージを更新
			TouchSuccess(distancePoint);

			// 評価用テキスト表示
			showText(distancePoint);

		}
	} // *******************************************************************************************************************


	//フリック判定
	public void OnScoreFlick(){

		//移動するとは、何度もフリックしていると同じ事なのでフリックの回数を加算
		flickCount++;

		// フリックしたときだけ呼ばれればいい。（これがないと何度も呼ばれて重くなる）
		if(flickCount == 1){

			audioManager.isTouch    = true;

			// 画面をタッチした位置
			startScreenPos = Input.mousePosition;
			flickStartPos  = boardMove.transform.localPosition + transform.localPosition;
			flickEndPos    = flickStartPos;

		}
			

		// 取得した座標をもとに、タッチバーからの距離を点数化
		distancePoint = GetDistancePoint (flickStartPos);


		//  点数が0より大きければ、フリックの成功判定（フリック範囲、要微調整）
		if(distancePoint > 0){

			// タッチして動いた後の座標
			endScreenPos = Input.mousePosition;

			// フリックした向きを正規化して取得（当初はここで変数宣言していたが、他のスクリプトから参照するためpublic化）
			flickVct = (endScreenPos - startScreenPos).normalized;

			if (        flickVct.x < 0      && flickVct.y >= -0.5f && flickVct.y <= 0.4f) {		//左
				flickDirection = 0;
			} else if ( flickVct.x < 0      && flickVct.y > 0.2f   && flickVct.y <= 0.8f) {		//左上
				flickDirection = 1;
			} else if ( flickVct.x >= -0.5f && flickVct.x <= 0.5f  && flickVct.y  > 0) {		//真上
				flickDirection = 2;
			} else if ( flickVct.x > 0      && flickVct.y > 0.2f   && flickVct.y <= 0.8f) {		//右上
				flickDirection = 3;
			} else if ( flickVct.x > 0      && flickVct.y >= -0.5f && flickVct.y <= 0.4f) {		//右
				flickDirection = 4;
			} else {																			//ノーフリック
				flickDirection = 5;															
			}
				
			// フリックの移動量flickDistanceを算出
			      flickEndPos   = boardMove.transform.localPosition + transform.localPosition;
			float flickDistance = Vector3.Distance (flickStartPos, flickEndPos);

			// 上のコードと同じ内容
			// float flickDistance = (flickStartPos-flickEndPos).magnitude;

			// フリック開始から、規定フレーム数以内に、規定移動量を満たした場合、フリック成功
			// フリックした方向とflickFlagの向きが一致した場合、フリック成功
			if (flickFlag == flickDirection && flickCount <= 3 && flickDistance > 50.0f) {

				// TouchSuccessメソッドでDestroy
				TouchSuccess (distancePoint);

				// showTextメソッドにdistancePointを引数として渡す
				showText(distancePoint);

				//
				FlickAnim ();

			}
		}
	}// *******************************************************************************************************************


	// フリック時のアニメーション（フリックした方向にパーティクルを飛ばす）
	public void FlickAnim(){

		// まずは生成（問題なくレーンごとに生成される）
		GameObject flickAnimObj = Instantiate(flickAnim);

		// 高さなど、細かい座標調整
		flickAnimObj.transform.position = touchBar.transform.position + new Vector3( 0f, -4f, -4.2f );

		// CFX_Demo_Translateに正規化された方向を渡す
		flickAnimObj.GetComponent<CFX_Demo_Translate> ().dir = flickVct;

		//// 注意！！: CFX_Demo_Translateはアセットで用意された関数をそのまま使っている（JMOアセット） ////

	}// *******************************************************************************************************************


	// ロングタップ判定（Start）
	public void OnScoreLongTapStart() {

		audioManager.isTouch = true;
		PositionInGameLong   = boardMove.transform.localPosition + transform.localPosition;
		distancePoint        = GetDistancePoint(PositionInGameLong);

		if (distancePoint > 0) {
			
			// タップされたかどうか
			isLongTap = true;

			// ロングタップアニメーションスタート（FlickAnimと同じ要領で考える）
			longTapAnimObj = Instantiate (longTapAnim);

			//取得
			ParticleSize particleSize = longTapAnimObj.GetComponent<ParticleSize>();

			// longTapStartTick、longTapEndTickはスコアが生成されたら既に値を持つ。その値をparticleSizeに渡す
			particleSize.startTick    = (float)longTapStartTick;
			particleSize.endTick      = (float)longTapEndTick;

			// 生成されたアニメーションプレハブの座標を決める → タッチバーの位置に生成
			longTapAnimObj.transform.position = touchBar.transform.position + new Vector3(0f, -7.0f, -7.0f);

		}
	}// *******************************************************************************************************************


	GameObject FindLongTapEndObj() {
		
		GameObject[] objcts = GameObject.FindGameObjectsWithTag(gameObject.tag);
		GameObject findObj  = null;

		int maxIndex = -1;
		int myIndex  = transform.GetSiblingIndex();


		foreach (GameObject obj in objcts) {

			if (obj.name == "Score_LongUp(Clone)") {
				int tmpIndex = obj.transform.GetSiblingIndex();
				if (tmpIndex > myIndex) {
					continue;
				}
				if (tmpIndex > maxIndex) {
					maxIndex = tmpIndex;
					findObj = obj;

				}
			}
		}
		return findObj;
	}// *******************************************************************************************************************


	// ロングタップ判定（End）
	public void OnScoreLongTapEnd() {

		// Upされたとき、その直前のDownプレハブがちゃんと成功していたら、if文に入り評価する。
		if (!isChecked && isLongTap) {

			isChecked = true;

			if (longTapEndObj != null) {
				Vector3 PositionInGame = boardMove.transform.localPosition + longTapEndObj.transform.localPosition;

				distancePoint = GetDistancePoint(PositionInGame);
			} else {
				distancePoint = 0;
			}

			// TouchSuccessメソッドでDestroy（ここではDownプレハブを消すという処理）
			TouchSuccess(distancePoint);

			// showTextメソッドにdistancePointを引数として渡す
			showText(distancePoint);

			// longTapEndObjには『Score_LongUp(Clone)』が格納されており、それも消す。
			Destroy(longTapEndObj);

			// ついでにButterflyBrokenエフェクトも発生させておく（変数名も一応変えておく）
			GameObject longUptouchObject = Instantiate(touchRingPrefab);

			// エフェクトの位置の移動とサイズをリセット(なぜ別記するかというと、他のタイプと違って相対的に画面外下に生成されてしまうため座標を再設定する為。無駄な書き方かもしれないが)
			longUptouchObject.transform.position   = new Vector3(transform.position.x,
														   		 transform.position.y + 65f,
														   		 transform.position.z + -5f
														  		);

			// 通常より少し大きめ
			longUptouchObject.transform.localScale = new Vector3(1f, 1f, 1f);

			// アニメーションを開始
			longUptouchObject.GetComponent<Animator>().Play( 0 );

			//// LongDownプレハブのEventTriggerにStartもEndもつける ////
			//// 理由はPointerUpはDown後にしか呼ばれないので、Upのプレハブは見た目とタイミングの為だけに存在 ////

        }
	}
// *******************************************************************************************************************


	// 評価用テキスト作成 
	private void showText(float point){

		// PointTextプレハブ（エフェクトオブジェクトを生成
		GameObject pointObj =Instantiate(pointText);


//		// 初期化（念の為残しておく）
//		pointObj.transform.position    = transform.position + new Vector3(0.6f, -5f, -10f);

		//（修正）ScoreCreatorで生成されたxの位置を受け取って（touchBar）、タッチバーにプレハブを出現させる
		pointObj.transform.position    = touchBar.transform.position + new Vector3(0.6f, 14f, -2.4f);
		pointText.transform.localScale = new Vector3(6f, 6f, 6f);



		// ポイントに応じて画像を切替(Badは『point > 0』で0にしておかないとタッチバーより上でmissが表示されてしまう)
		if (point > 0.7f) {

			pointObj.GetComponentInChildren<SpriteRenderer> ().sprite = textSprite [(int)PointTextKey.Perfect];

			// Perfectの時はその数をカウント
			ComboManager.perfectCount++;

			// タップに成功したら、Perfect係数をPowerProgressにわたす。（係数が固定だと使いにくいので後に変数化）
			powerProgress.PlayerValueChange(point * 0.03f);

		// 以下同義
		} else if (point > 0.4f) {
			
			pointObj.GetComponentInChildren<SpriteRenderer> ().sprite = textSprite [(int)PointTextKey.Great  ];
			powerProgress.PlayerValueChange(point * 0.03f);

		} else if (point > 0.2f) {

			pointObj.GetComponentInChildren<SpriteRenderer> ().sprite = textSprite [(int)PointTextKey.Good   ];
			powerProgress.PlayerValueChange(point * 0.03f);

		} else if (point > 0   ) {

			pointObj.GetComponentInChildren<SpriteRenderer> ().sprite = textSprite [(int)PointTextKey.Bad    ];
			powerProgress.PlayerValueChange(point * 0.03f);

		// point＝0ならmissと表示させる
		} else{
			
			pointObj.GetComponentInChildren<SpriteRenderer>().sprite  = textSprite [(int)PointTextKey.Miss   ];
			powerProgress.PlayerValueChange(point * 0.03f);

			// pointが0なのは、ゲーム中なのか、そうじゃないのか。ゲーム中以外はパラメーターの増減は加味しない。
			powerProgress.isMissed = true;

		}


		// アニメーションを開始 //
		pointObj.GetComponentInChildren<Animator>().Play( 0 );


		// コンボカウント（ワークショップ6）
		if (point > 0) {
			ComboManager.combo++;
		} else {
			ComboManager.combo = 0;
		}
			

		// 最初maxは０なので、一回でも成功したら、現在のコンボ数がmaxCombo
		if(ComboManager.combo > ComboManager.maxCombo){
			
			// maxComboをcomboで更新、つまりmissしたら0に戻る
			ComboManager.maxCombo = ComboManager.combo;
			//Debug.Log (ComboManager.maxCombo);
		}
			

		// コンボ表示（コンボを更新）
		comboHandler.setCombo(ComboManager.combo);

	} // *******************************************************************************************************************


	// タッチバーとタッチした座標の間の距離を数値化（単位として正規化）して返すメソッド
	float GetDistancePoint(Vector3 touchPos){

		// タッチバーからの距離を計算
		int Distance = (int)Mathf.Abs(touchPos.y * 0.4f + 160);

		// 距離からポイントを計算し正規化
		distancePoint = (100 - Distance) / 100f;

		// ポイントの最小値は0とする（マイナス不可）/ポイントが0以下の時はポイントを0にする
		if (distancePoint < 0){
			distancePoint = 0;
		}

		return distancePoint;

	} // *******************************************************************************************************************



	// タッチに成功したオブジェクトを削除して、画面のスコアとゲームオーバーを更新するメソッド
	void TouchSuccess(float point){

		// 譜面の削除
		Destroy (gameObject);

		// エフェクトオブジェクトを生成 (touchuRingPrefab = Butterfly Broken)
		GameObject touchObject = Instantiate(touchRingPrefab);

		// エフェクトの位置の移動とサイズをリセット
		// ※BrokenEffectの大きさを微調整 / ButterFlyBorkenはタッチバーから出現する必要はない
		touchObject.transform.position   = new Vector3(transform.position.x,
													   transform.position.y + -22.5f,
													   transform.position.z + -4.9f);

//		//（修正）ScoreCreatorで生成されたxの位置を受け取って（touchBar）、タッチバーにプレハブを出現させる
//		touchObject.transform.position   = new Vector3(touchBar.transform.position.x ,
//													   touchBar.transform.position.y + -12f,
//													   touchBar.transform.position.z + -19f);

		// BrokenEffectの大きさはtransform.positionとの兼ね合いもあって、破綻するかもしれないので、要検討!
		touchObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

		// アニメーションを開始
		touchObject.GetComponent<Animator>().Play( 0 );

		// GameDataのscoreにポイントを設定 （pointの基準値が変わったのでその分を*4.4f乗算）
		GameDate.score += (int)(point * 1000 * 4.4f);

		// ポイント表示(スコアを更新)
		pointHandler.setPoint(GameDate.score);

		// GamaDateのGagePointを設定 （pointの基準値が変わったのでその分を*4.4f乗算）
		GameDate.GagePoint += point * 1.1f * 4.4f;

		// ゲージ表示(ゲージを更新)
		gageHandler.setGage(GameDate.GagePoint);


		// ゲージを制限
		if (GameDate.GagePoint > 512) {
			
			GameDate.GagePoint = 512;

		}
	}
// *******************************************************************************************************************


	// 自動で削除（miss時の処理）
	public void AutoDestroy(){

		if (tapType != 3) {
			// 削除
			Destroy(gameObject);
		}
			
		// 評価用テキスト表示（自動で削除時はshowTextに0を渡す/missを表示させる）
		// メソッドを分けたので必要なくなる
		//showText(0);

		//// 効果音を鳴らす（miss時）
		//FindObjectOfType<AudioManager>().
		//GetComponent<AudioSource>().
		//PlayOneShot(FindObjectOfType<AudioManager>().
		//onMiss
		//);

		//// ゲージを減算(missしたときなのでここに記載)
		//GameDate.GagePoint -= 5;

		//// ゲージを制限
		//if(GameDate.GagePoint < 0) GameDate.GagePoint = 0;

		//// ゲージを表示
		//gageHandler.setGage(GameDate.GagePoint);

	} // *******************************************************************************************************************		


	//タッチした瞬間レンズフレア発生
	public void TouchEffect() { buttonAnim.SetTrigger ("Touch"); }

	// ロングタップアニメーションを止めるメソッド(EventTriggerでUp時に実行される)
	public void LongTapAnimStop() { Destroy (longTapAnimObj); }

	//// プレハブ名を検索して、それをDestroyすれば当然消える ////
	//// 問題は二つ同時にアニメーションが発生して、片方を先にUpした場合、両方消えるはず ////
	//// なので、『そのレーンタップを離したら、その同レーンのプレハブだけ消える』ようにしなければならない ////
	//// マウスだとダブルタップ、およびデバックが難しいので、保留 ////

}
