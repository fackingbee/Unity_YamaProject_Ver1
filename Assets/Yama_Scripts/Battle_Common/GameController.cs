using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class GameController : MonoBehaviour {


	// オーディオソース取得用
	AudioSource gameAudio;

	// 再生されているかどうかを見る為の変数
	bool isPausing;

	// AudioManagerのif文管理用
	public static bool isPlaying;

	// startでもupdateでも使うのでグローバル変数化
	float audioLength;

	// 曲の始まるタイミングを管理する変数
	public float delayTime;

	// 再生停止ボタン
	GameObject pauseObj;
	GameObject unPauseObj;

	//private Animator cameraAnim;

	private Animator cameraRotate;


	void Awake(){
		Application.targetFrameRate = 60;
	}


	void Start () {

		// ゲーム開始時、カメラを回転
		cameraRotate = GameObject.Find ("Main Camera").GetComponent<Animator> ();

		// 変数にボタンのオブジェクト代入
		pauseObj   = GameObject.Find ("Pause");
		unPauseObj = GameObject.Find ("UnPause");

		// 最初に再生ボタンの方は非表示にしておく 
		unPauseObj.SetActive (false);

		// 取得
		gameAudio = GetComponent<AudioSource> ();

		// 曲の長さを取得(曲が終わったらisPlayingをfalseにしたい)
		audioLength = gameAudio.clip.length;

		// AudioManagerのif内に入らないように(Audioが止まるとGameOverになるので、一時停止中もisPlayingはTrueにしておく)
		isPlaying = true;

		// ゲーム開始時は一時停止ではない
		isPausing = false;

		// ゲーム開始
		Invoke ("GameStart", 8.0f);

		// カメラローテーション開始
		Invoke ("CameraRotateOn",3.6f);

	}


	void Update () {
		
		// ポーズ
		if(!isPausing && Input.GetKeyDown("q")){
			Pause ();

		// 再開
		} else if (isPausing && Input.GetKeyDown("q")){
			UnPause ();
		}

		// デバッグ用（曲の途中まで飛ばす）
		if(Input.GetKeyDown("s")){
			TimeManager.time += 20.0f;
			gameAudio.time   += 20.0f;
		}


		// デバッグ用（強制的にゲーム終了）
		if(Input.GetKeyDown("z")){

			//Debug.Log ("強制終了モード");

			// 強制的に経験値を100加算
			variableManage.currentExp += 100;

			// 強制的にゲームクリア
			GameDate.GagePoint = 512;	

			// 強制終了
			GameStop ();

			// データを保存するかどうか
			bool svChk = KiiManage.saveKiiData ();

			Debug.Log ("variableManage.currentExp : " + variableManage.currentExp);
			Debug.Log ("variableManage.nextExp : " + variableManage.nextExp);

			Debug.Log ("svChk : " + svChk);

			// データを保存してシーン移動
			if(!svChk){

				Debug.Log ("データを保存します");

				svChk = KiiManage.saveKiiData ();
			}

			Invoke ("DebugBattleEnd",1f);

		}



//		// TimeManager.timeが曲の長さを超えたら止める
//		// +1.7fはGameOver等を遅らせて表示させる為
//		if(isPlaying && TimeManager.time >= audioLength + 1.7f){
//			GameStop ();
//		}


		// ここを1.7fにすると、曲数分書き換えないといけないので変数化 (Music_01_Manager.csで時間管理)
		if(isPlaying && TimeManager.time >= audioLength + delayTime){
			GameStop ();
		}
	}


	public void GameStart(){

		// Inspector上では、TimeManagerスクリプトをOFFにしておく
		GetComponent<TimeManager> ().enabled = true;

		// 曲始まりのオフセット（スクリプトを分けて引数は変数化(Music_01_Manager.csで時間管理）
		// gameAudio.PlayDelayed (1.7f); と同義
		gameAudio.PlayDelayed (delayTime); 

	}


	// 曲が終わったら、isPlayingをfalseにするメソッド
	public void GameStop(){ 
		isPlaying = false; 
	}



	// デバッグ終了（KiiCloudの確認の為、経験値を与えて強制終了）
	public void DebugBattleEnd(){

		SceneManager.LoadScene("KiiStart");
		Debug.Log ("KiiStartへ遷移しました");

	}



	// 一時停止メソッド
	public void Pause(){

		// 一瞬重なったりするので順番に気をつける
		pauseObj.SetActive   (false);
		unPauseObj.SetActive (true);

		// 再生されているかどうか
		isPausing = true;

		// Update自体を一時停止
		Time.timeScale = 0f;

		// と同時にオーディオを一時停止
		gameAudio.Pause ();
	}


	// 再開メソッド
	public void UnPause(){

		// 一瞬重なったりするので順番に気をつける
		unPauseObj.SetActive (false);
		pauseObj.SetActive   (true);

		// 再生されているかどうか（されていない）
		isPausing = false;

		// Updateを再開
		Time.timeScale = 1.0f;

		// と同時に一時停止解除
		gameAudio.UnPause();
	}

	// カメラを回転させるメソッド
	public void CameraRotateOn(){
		cameraRotate.enabled = true; 
	}
}
