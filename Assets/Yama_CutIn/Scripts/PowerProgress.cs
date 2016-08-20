using UnityEngine;
using System.Collections;

public class PowerProgress : MonoBehaviour {

	// その他追加要望
	// 1) アイテムで開始時にいきなり満タンになるようなものが実装できれば…

	private Renderer playerPowerProgress;		// Player側のPowerGage（敵も同様）
	private Animator playerAttackAnim;			// 味方の攻撃時カットイン
	private bool     isStartingPlayer;			// Animatorフラグ管理（Start）
	private bool     isStopingPlayer;			// Animatorフラグ管理（Stop）
	public  float    preValue = 0f;				// 増減する前のplayerValueを格納
	public  float    playerValue = 0f;			// ゲージ増減変数
	public  bool     isMissed;					// タッチされているかどうか（されていなければ）


	// ゲージアニメーションコルーチン（味方用）
	private PlayerPowerGageHandler playerPowerGageHandler;


	// ゲージMAX時エフェクト格納変数（Player）
	public GameObject playerEffectCircle;
	public GameObject playerEffectAttack;

	[SerializeField]
	private AudioSource[] audioSources;

	// 味方攻撃時のSE
	private AudioSource playerCircleSE;
	private AudioSource playerAttackSE;



	// playerPowerPointやplayerValueはスコアが降り始める前の「0」という値を持ち得るので、フラグ管理する
	void Start () {

		isMissed  = false;	// Missが表示されないなら、ゲーム中ではない

		// プロパティ『Progress』がゲージの増減を司り、そこに値を渡すことでゲージを上げ下げする
		playerPowerProgress = GetComponent<Renderer> ();

		// アニメーションコルーチン用PlayerPowerGageHandlerをセット
		playerPowerGageHandler = FindObjectOfType<PlayerPowerGageHandler> ();

		// Progressを初期化、0のときはPowerGageは全く溜まっていない状態
		playerPowerProgress.material.SetFloat ("_Progress",playerValue);

		// カットインアニメーションさせるオブジェクトを探す(後に変数化したいので、名前で取得する時は注意する)
		playerAttackAnim = GameObject.Find ("Sword_Normal_Small").GetComponent<Animator> ();

		// ゲージが溜まるまでアニメーションへは遷移出来ない
		isStartingPlayer = false;

		playerCircleSE   = audioSources[0];
		playerAttackSE   = audioSources[1];

	}
	

	void Update () {
		
		if (TimeManager.time > 0) {

			// ちょいと微妙な位置に条件を付けた気がするが、現状見た目は問題ないのでしばらくこれで様子を見る
			if(playerValue == 0){
				
				isStartingPlayer = false;
				playerAttackAnim.SetBool("isStartingPlayer", isStartingPlayer);
			}

			// 下記だと0より小さくなってもエフェクトが呼ばれてしまうから
			if(playerValue >= 1f){

				// 味方のゲージが溜まったら攻撃合図用サークルを発生
				Instantiate (playerEffectCircle);

				// サークル発生時のSE再生
				playerCircleSE.Play ();

				// その後攻撃開始
				Invoke ("PlayerAttack",1f);

			}
				
			// 0より大きいときにデクリメントされてマイナスになったときの条件を補えなかった
			if(playerValue < 0f){
				playerValue = 0f;
			}



			// playerValueがMAXになったら攻撃アクション & 初期化(playerValue < 0fがないと減り続けて溜められないから)
			//if (playerValue >= 1f || playerValue < 0f && isPlaying) {
			if (playerValue >= 1f || playerValue < 0f) {

				//初期化（ここまでコルーチンでアニメーションするかは保留中）
				playerValue = 0f;

				// IdleからStartへの遷移を許可
				isStartingPlayer = true;

				// Idle状態からアニメーションONの状態へ遷移
				playerAttackAnim.SetBool("isStartingPlayer", isStartingPlayer);

				// その後Has Exit Timeを１でもたせて、Idle状態へ遷移
				isStopingPlayer = true;

				// アニメーションON状態からIdele状態へ遷移
				playerAttackAnim.SetBool("isStopingPlayer", isStopingPlayer);

				//少々冗長だが、同じようにやっておく。本来はGameDate.playerPowerGagePoint = 0fでもいい気がする
				GameDate.playerPowerGagePoint = playerValue;

			}
			// ifを抜けた後で、値を別の変数に代入することで、一つ前の値と現在の値を保持し、その差分をアニメーションさせる
			preValue = playerValue;
		}
	}


	void PlayerAttack(){

		// 攻撃アニメーション生成
		Instantiate (playerEffectAttack);

		// と、同時にSEをならす
		playerAttackSE.Play ();

	}


	// ScoreHandlerから実行する
	public void PlayerValueChange(float playerPowerPoint) {

		// playerValueがMAXじゃない & ゲーム中の状態でif文に入る // (ここで敵の妨害により、下がるような要素（逆も然り）が欲しい)
		//if (playerValue >= 0f && playerValue < 1f && isPlaying) {
		if (playerValue >= 0f && playerValue < 1f) {

			// ゲージが溜まるまでアニメーションへは遷移出来ない
			isStartingPlayer = false;

			// タップの評価に合わせて、ゲージ加算を決める（ScoreHandlerの評価pointと合わせる）
			if (playerPowerPoint > 0.7f * 0.03f) {

				// タップに成功したら評価の係数分、playerValueに加算
				playerValue += playerPowerPoint * 1.2f;

				// デバッグ用（さっさと溜める）
				//playerValue += 1f;

				// 以下同義
			} else if (playerPowerPoint > 0.4f * 0.03f) {
				playerValue += playerPowerPoint * 1.2f;
			} else if (playerPowerPoint > 0.2f * 0.03f) {
				playerValue += playerPowerPoint * 1.2f;
			} else if (playerPowerPoint > 0.0f * 0.03f) {
				playerValue += playerPowerPoint * 1.2f;

			// isMissedフラグがないと、フレームごとに減算されてしまう
			} else if (playerPowerPoint == 0.0f && isMissed && playerValue > 0f) { 
				
				// こちらは減算																	
				playerValue -= 0.02f;

				// 一度減算されたらもう入る必要はない
				isMissed = false;

			}

			// pointGageと同じようにGameDateに値を保持させる
			GameDate.playerPowerGagePoint = playerValue;

			// pointGageと同じようにアニメーションさせながら、値を設定
			playerPowerGageHandler.SetPlayerGage(preValue, GameDate.playerPowerGagePoint);


		}
	}
}
	