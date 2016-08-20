using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	public GameObject GameClear; 	// GameClearアニメーション
	public GameObject GameOver;		// GameOverアニメーション 
	public AudioClip  onMiss;		// Miss時効果音 
	public AudioClip  onGameClear;	// ゲームクリア時効果音    
	public AudioClip  onGameOver;	// ゲームオーバー時効果音  
	public AudioClip  onTouch;		// タッチが成功時効果音
	public bool       isTouch;      // ScoreHandlerでフラグを管理

	public AudioSource audioSource;


	// iOSにBuild時のフレームレート問題の為 ***********************************************
	void Awake(){
		Application.targetFrameRate = 60;
	}
	// *******************************************************************************


	void Start () {

		audioSource = GetComponent<AudioSource> ();

		
//		// オーディオを再生（GameControllerで管理するようになってからは必要なくなる）
//		gameObject.GetComponent<AudioSource>().PlayDelayed(1.7f);

//		// デバッグ用、再生位置を移動(曲の途中から始められるデバックで便利だが、スコアとは同期しない)
//		// 終わったらスコアは100％にする。
//		GameDate.GagePoint = 100;
//		gameObject.GetComponent<AudioSource>().time = gameObject.GetComponent<AudioSource>().clip.length -1f;

	}


	void Update () {

		// 構文：public void PlayOneShot(AudioClip clip, float volumeScale = 1.0F);
		if(isTouch){
			audioSource.PlayOneShot(onTouch, 1f);
			isTouch = false;
		}

		
		// ゲームの終了判定（音楽がいつ終了したかをチェック）
		// オーディオが止まった時にゲーム終了
		// isPlayingはオーディオが再生されているか、いないか。
		//if(!gameObject.GetComponent<AudioSource>().isPlaying){

		// 今までは曲が終わったらisPlayingはfalseと同義だが、GameControllerで一時停止等を手動で管理
		if (GameController.isPlaying) {
			
			// 抜ける
			return;

		// GameControllerのGameStopでisPlayingがfalseになったら入る
		}else{
			
			// ゲージが75％以上だった場合はクリア
			if (GameDate.GagePoint >= 380) {
				
				// 効果音を鳴らす（GameClear）// 構文：public void PlayOneShot(AudioClip clip, float volumeScale = 1.0F);
				gameObject.GetComponent<AudioSource>().PlayOneShot(onGameClear, 1f);

				// GameClearアニメーションを表示
				GameClear.SetActive (true);

			} else {
				
				// 効果音を鳴らす（GameOver）
				gameObject.GetComponent<AudioSource>().PlayOneShot(onGameOver, 0.7f);

				// それ以外はゲームオーバーでGameOverアニメーションを表示
				GameOver.SetActive(true);

			}

			// 自身を停止してループを防ぐ / Updateに記載しているので、終了してもゲーム（フレーム）が進むごとに上記が実行されてしまう / Gameごとfalseにしてしまうと、UIが消えるので今回はスクリプトを非アクティブにする
			enabled = false;

		}
	}
}