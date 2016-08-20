using UnityEngine;
using System.Collections;

public class EnemyPowerProgress : MonoBehaviour {


	private Renderer enemyPowerProgress;	// Enemy側のPowerGage（味方もも同様）
	private Animator enemyAttackAnim;		// 敵の攻撃時カットイン
	private bool     isStartingEnemy;		// Animatorフラグ管理（Start）
	private bool     isStopingEnemy;		// Animatorフラグ管理（Stop）
	public  bool     enemyTurn;				// PlayerTurn中はゲージを溜めたくないので、ScoreCreatorのタップタイプを見てフラグ管理
	public  float 	 enemyValue    = 0f;	// ゲージ増減変数（初期化、すなわちPowerGageが全く溜まっていない状態）
	public  float    preValueEnemy = 0f;	// 増減する前のenemyValueを格納

	// ゲージアニメーションコルーチン（敵用）
	private EnemyPowerGageHandler enemyPowerGageHandler;

	// ゲージMAX時エフェクト格納変数（Enemy）
	public GameObject enemyEffectCircleFloor;	// 敵攻撃時、床に円陣発生
	public GameObject enemyEffectCircleBack;	// 敵攻撃時、背後に円陣発生
	public GameObject enemyEffectAttackOrbit;	// 敵の攻撃軌道

	[SerializeField]
	private AudioSource[] audioSources;

	// 敵攻撃時のSE
	private AudioSource enemyCircleSE;
	private AudioSource enemyAttackSE;

	void Start () {
		
		// スコアが降ってくる、通常の状態
		enemyTurn = true;

		// プロパティ『Progress』がゲージの増減を司り、そこに値を渡すことでゲージを上げ下げする
		enemyPowerProgress = GetComponent<Renderer> ();

		// アニメーションコルーチン用EnemyPowerGageHandlerをセット
		enemyPowerGageHandler = FindObjectOfType<EnemyPowerGageHandler> ();

		// まずはProgressを初期化、0のときはPowerGageは全く溜まっていない状態
		enemyPowerProgress.material.SetFloat ("_Progress",enemyValue);

		// カットインアニメーションさせるオブジェクトを探す(後に変数化したいので、名前で取得する時は注意する)
		enemyAttackAnim = GameObject.Find("Inst_Normal_Small").GetComponent<Animator>();

		enemyCircleSE   = audioSources[0];
		enemyAttackSE   = audioSources[1];

	}


	void Update () {

		// TimeManager.timeが始まったら敵のアクションも開始（スコアが降ってきだしたらという条件も追加しないといけないであろう）
		if(TimeManager.time > 0){

			// valueのMaxは『1』で上限まで溜め続ける。但し、PlayerTurnなら加算しない(ここで敵の妨害により、下がるような要素（逆も然り）が欲しい)
			if(enemyValue < 1f && enemyTurn){

				// ゲージが溜まるまでアニメーションへは遷移出来ない
				isStartingEnemy = false;

				// Animatorをスクリプトから操作
				enemyAttackAnim.SetBool ("isStartingEnemy",isStartingEnemy);

				// ゲージを溜める（減らす）処理
				enemyPowerProgress.material.SetFloat ("_Progress",enemyValue);

				// 敵のゲージ増加は自動（現状は単純にインクリメント ※後に変数化すること）
				enemyValue += 0.001f;

				// デバッグ用でさっさと溜める
				//enemyValue += 0.01f;

				// pointGageと同じようにGameDateに値を保持させる
				GameDate.enemyPowerGagePoint = enemyValue;

				// pointGageと同じようにアニメーションさせながら、値を設定
				enemyPowerGageHandler.SetEnemyGage (preValueEnemy, GameDate.enemyPowerGagePoint);

			}

			// playerValueがMAXになったら攻撃アクション&初期化
			if(enemyValue >= 1f){

				// アニメーション生成
				Instantiate (enemyEffectCircleFloor);
				Instantiate (enemyEffectCircleBack);

				// アニメーション生成時のSE
				enemyCircleSE.Play();

				// 1秒後に攻撃生成
				Invoke ("EnemyAttack", 1f);
				
				// 溜まったゲージは初期化
				enemyValue = 0f;

				// IdleからStartへの遷移を許可
				isStartingEnemy = true;

				// Idle状態からアニメーションONの状態へ遷移
				enemyAttackAnim.SetBool ("isStartingEnemy", isStartingEnemy);

				// その後Has Exit Timeを１でもたせて、Idle状態へ遷移
				isStopingEnemy = true;

				// アニメーションON状態からIdele状態へ遷移
				enemyAttackAnim.SetBool("isStopingEnemy", isStopingEnemy);

			}

			// ifを抜けた後で、値を別の変数に代入することで、一つ前の値と現在の値を保持し、その差分をアニメーションさせる
			preValueEnemy = enemyValue;

		}
	}


	void EnemyAttack(){

		// 攻撃アニメーション生成
		Instantiate (enemyEffectAttackOrbit);

		// と、同時にSEをならす
		enemyAttackSE.Play ();
	}
}
