 using UnityEngine;
using System.Collections;
using MiniJSON; 					// これによりJSONに変換したMIDIを分析出来る
using System.Collections.Generic;	// 型を指定出来るリスト・配列

public class ScoreCreator : MonoBehaviour {


	// スコアのXの位置
	private static float[] ScorePositionXList = new float[]{-375f, -132.5f, 132.5f, 375f};

	// MIDIのValueを第一引数に、レーンの番号を第二引数に渡して、KeyとvalueでロングタップのStartとEndを管理（連想配列という）
	Dictionary<int, int> randDic = new Dictionary<int, int>();


	public GameObject            scorePrefab;			// Scoreプレハブ（通常用）	 
	public GameObject[]          scoreFlickType;		// Scoreプレハブ（フリック用）
	public GameObject            scorePrefabLongDown;	// Scoreプレハブ（ロングタップ・ダウン用）
	public GameObject            scorePrefabLongUp;	    // Scoreプレハブ（ロングタップ・アップ用）
	public TextAsset             jsonDate;			    // Jsonテキストデータ
	public List<MusicDate>       scoreDate;			    // 音楽データを格納する構造体の配列
	public GameObject[]          touchBars;			    // TouchBar格納メンバ（4つのTouchBarをScoreHandlerに渡す）
	public  long 			     tmpTick;				// tmp.Tickを一時保存する為の変数
	public  int  			     tmpRand    = -1;	    // 重複回避用変数（0で初期化すると最初に重なる恐れがあるので負の数で初期化）
	private int  			     longTapNum = 0;
	private int  			     seed       = 100;		// 乱数を固定する為の変数
	private int  			     seedFlick  = 777;
	private PlayerAttackCreate   pac;					// PlayerTurn時の自動生成用
	private CutInFillAmount      cutInFillAmount;		// CutInFillAmount開始用
	private CutInFillAmountEnemy cutInFillAmountEnemy;  // CutInFillAmount（敵用）開始用
	private EnemyPowerProgress   enemyPowerProgress;	// PlayerTurn中は敵のゲージを溜めない


	void Start () {

		// 敵のゲージを溜めない為のフラグ管理用に取得
		enemyPowerProgress = FindObjectOfType<EnemyPowerProgress> ();
		
		// scoreDataを初期化
		scoreDate = new List<MusicDate> ();	

		// ランダム生成のシードをセット
		Random.seed = seed;						
//		Random.seed = 100;// ランダム生成のシードをセット（ここで固定しても他のスクリプトのUpdateが割り込んで固定されない）

		// テキストデータを配列に変換 
		// DeserializeはJSONデータを読み込むメソッド 
		// IDictionaryはキー付きの配列 
		// 型変換も忘れずに（IDictionaryはオブジェクト型で返ってくる）
		IDictionary tmpDate = (IDictionary)Json.Deserialize(jsonDate.text);

		// 値「”score”」に配列が格納されている
		List<object> arrayDate = (List<object>)tmpDate["score"];	

		long tick  = 0;
		int  value = 0;

		// ロングタップダウンとアップの紐付けの為（96と97のセットなど）新たに連想配列
		Dictionary<int,MusicDate> dic = new Dictionary<int, MusicDate>();

		// arrayDataを解析
		foreach(IDictionary val in arrayDate){


			// eventがnote_onの時のみ格納
			if((string)val["event"] == "note_on"){

				tick = (long)val["tick"];
				value = (int)(long)val["value"];

				// mdはつまり、スコアのことで、スコアはtickとvalueを持っている
				MusicDate md = new MusicDate (tick,value);

				// わざわざ一旦mdをインスタンス化したのは、md（つまり、ロングタップ）のtaptypeが見たいから
				scoreDate.Add(md);

				// もし、スコアのtaptypeがロングタップダウンだったら…
				if(md.tapType == 3){

					//（例としてMIDIが96だったら）96のKeyがあるかどうか判定
					if (dic.ContainsKey (value)) {
						
						// あれば、96keyにロングタップダウンのtickを格納
						dic [value] = md;

					// なければ…
					} else {
						
						// keyを96として、md、すなわちダウン時のtickを入れる
						dic.Add (value,md);
					}

					// もしMIDIが97だったら…
				}else if(md.tapType == 4){
					
					// 現在のアップのスコアが97なので紐づけたいのは-1されたMIDI96のダウンスコア
					// 96のtickは97が生成されて初めて分かり、↓の式だと96の次のスコアはたった今生成された97のスコア
					dic [value - 1].nextTick = tick;

					// 生成後は必ずRemove、通常の配列とは違い、前に詰めない。
					dic.Remove (value - 1);

					// ※Dictionary型の連想配列はKeyとvalueをセットで格納
					// ※同じtickでもvalue96のロングタップなのか、98のロングタップなのかを判定し、それと対になる97のアップなのか99のアップなのかを紐づける

				}
			}
				

			// eventがset_tempoの時はテンポ情報（↑『else ifでまとめられそう）
			// 整数系はlong型になってるので一旦変換して再度int型に変換する
			if( (string)val["event"] == "set_tempo" ){
				
				TimeManager.tempo = (int)(long)val["value"];

			}
		}


		pac     			 = GameObject.Find("PlayerTurn").GetComponent<PlayerAttackCreate>();
		cutInFillAmount      = GameObject.Find("Cut_In_Player_BackGround").GetComponent<CutInFillAmount>();
		cutInFillAmountEnemy = GameObject.Find("Cut_In_Enemy_BackGround").GetComponent<CutInFillAmountEnemy> ();

	}


	void Update () {
		
		// scoreDataをチェック
		foreach( MusicDate tmp in scoreDate ){

			// 指定したTickを超えたものから生成 (『!tmp.isCreated』がないと、既に生成したものに対して再度生成してしまう)
			//if (!tmp.isCreated && TimeManager.tick >= tmp.tick) {
			if (tmp.isCreated) {
				
				continue;

			} else if (TimeManager.tick >= tmp.tick){
				
				// 生成フラグをセット（※ここ重要）
				tmp.isCreated = true;

				// フリック込みの生成 **************************************************************
				GameObject scoreObject = null;

				// tapTypeが1、つまりvalueが84だったら、スコアはFlickType
				if (tmp.tapType == 1) {

					Random.seed = seedFlick++;

					// StartでSeedを設けてるので、一定のRandom性で生成する番号を決める。
					int typeIndex = Random.Range (0, scoreFlickType.Length);

					// 配列のscoreFlickTypeに格納したプレハブをtypeIndexに基づいて生成
					scoreObject = (GameObject)Instantiate (scoreFlickType [typeIndex]);

					// 生成されたフリックプレハブの名前に応じて、flickFlgに番号をつける (0:Left, 1:Top_Left, 2:Top, 3:Top_Right, 4:Right)
					ScoreHandler scoreHandler = scoreObject.GetComponent<ScoreHandler>();
					scoreHandler.flickFlag = typeIndex;
					scoreHandler.tapType = tmp.tapType;


				} else if (tmp.tapType == 3) {

					// ロングタップ・ダウン
					scoreObject = Instantiate (scorePrefabLongDown);

					ScoreHandler scoreHandler = scoreObject.GetComponent<ScoreHandler>();
					scoreHandler.longTapStartTick = tmp.tick;
					scoreHandler.longTapEndTick = tmp.nextTick;
					scoreHandler.tapType = tmp.tapType;


					Shadow_AfterImage saImg = scoreObject.GetComponent <Shadow_AfterImage>();
					saImg.longTapEndPosY = tmp.nextTick * 0.05f + 3000;

					longTapNum++;

				} else if (tmp.tapType == 4) {

					// ロングタップ・アップ
					scoreObject = Instantiate (scorePrefabLongUp);

					longTapNum--;

				} else if (tmp.tapType == 5) {
					
					pac.StartSpawn();						// 味方ターン時に攻撃開始
					cutInFillAmount.fadeIn       = true;	// 味方カットイン開始
					enemyPowerProgress.enemyTurn = false;	// 敵のゲージ加算はPlayerTurn時は停止

					//※後にカットイン用のMIDIのValueを設けた方が発生タイミングを制御しやすいだろう

				} else if (tmp.tapType == 6) {
					
					pac.StopSpawn();						// 味方の攻撃停止
					enemyPowerProgress.enemyTurn = true;	// 敵のゲージ加算を再始動
					cutInFillAmountEnemy.enabled = true;	// 敵のカットインスクリプトON
					cutInFillAmountEnemy.fadeIn  = true;	// 敵のカットイン開始

					//// 後にカットイン用のMIDIのValueを設けた方が発生タイミングを制御しやすいだろう ////

				} else {

					// 通常のスコア生成
					scoreObject = Instantiate(scorePrefab);
					scoreObject.GetComponent<ScoreHandler>().tapType = tmp.tapType;
				}


				// 譜面のヒエラルキーを移動
				if (scoreObject != null){
					scoreObject.transform.SetParent(transform);



				//// ランダム生成のシードを再セット / ここでインクリメントしないと乱数が固定しない（※超重要）////
				//// 音ゲーでは毎回同じスコア配置にしたい。スコアの位置を決めるときにシードを100、101、102…と渡すと固定する ////
				//// 基本一つのUpdateが走っているときは他のスクリプトのUpdateは待機している ////
					Random.seed = seed++;

				// xの位置を決めるため、0~3の乱数を生成
					int rand = Random.Range(0, ScoreCreator.ScorePositionXList.Length);

				// ロングタップエンドは、必ずロングタップスタートと同じラインに生成する
				// 先に下記のtapType==3を見に行く
				// taptype==3でrandDic[key=96、第○レーン]と指定しておく。そしてtapType==4で生成されたときに…
					if (tmp.tapType == 4) {
						
						// この97のスコアは-1のrandDic[96]のレーンであると指定
						rand = randDic[tmp.value - 1];

						// randDic[96]のレーンと97のレーンを紐づけた段階でいらないのでRemoveしておく
						randDic.Remove (tmp.value - 1);

						// longTapNumが0じゃないということは、つまりロングタップが生成されているということ
					} else if (longTapNum != 0) {
						
						// ロングタップ中は、その他の音符はロングタップと違うラインに生成する → もし重なったらRondom.Seedで再度選び直す処理
						// randDicのvalueがある間はループする。falseになって抜けるのは、taptype==4が生成されて、randDic.Removeされたとき
						while (randDic.ContainsValue(rand)){
							
							rand = Random.Range(0, ScoreCreator.ScorePositionXList.Length);

						}
					}



					// tick数が同じ且つ、randが同じ場合は、乱数を再生成
					if (tmp.tick == tmpTick && rand == tmpRand) {
						
						int count = 0;

						while (tmpRand == rand) {
							rand = Random.Range(0, ScoreCreator.ScorePositionXList.Length);
							count++;
							if (count >= 10) {
								break;
							}
						}
					}



					// 生成した乱数を格納
					tmpRand = rand;

					// ボタンアニメーションの修正(Shimo)
					scoreObject.tag = (rand + 1).ToString();

					// 譜面のXの位置を決定
					float x = ScoreCreator.ScorePositionXList[rand];

					// 譜面のYの位置を決定
					float y = tmp.tick * 0.05f + 3000;

					// 譜面の位置を移動※Y軸はボードの移動によって決定
					scoreObject.transform.localPosition = new Vector3 (x, y, 0);

					// 譜面のスケールをリセット
					scoreObject.transform.localScale = Vector3.one;

					//出現したものの表示順を最奥に設定
					scoreObject.transform.SetAsFirstSibling ();

					//TouchBarを渡す
					if (tmp.tapType != 4) {
						scoreObject.GetComponent<ScoreHandler>().touchBar = touchBars[rand];
					}

					// 生成された音符のtickを一時保存
					tmpTick = tmp.tick;


					if (tmp.tapType == 3) {
						
						// 特定のキーや値を持つ項目がハッシュテーブルに格納されているかどうかを調べるには、ContainsKeyメソッドあるいはContainsValueメソッドを使用する
						// ここのif文の条件式の意味は、「96というvalueがあれば、ifに入り、96にレーンを指定」
						if (randDic.ContainsKey (tmp.value)) {
							
							// randDic[96]に第○レーンを指定
							randDic[tmp.value] = rand;

						} else {
							
						// なければ、elseにはいり、randDicにkeyを96としてセットし、第○レーンと指定
							randDic.Add (tmp.value, rand);

						// taptype == 4へ戻る

						}
	                }
				}

			} else if (tmp.tick > (TimeManager.tick + 9600)) {
				
				break;

			}
		} // foreach end
	} // Update end
}