using UnityEngine;
using System.Collections;

public class PlayerPowerGageHandler : MonoBehaviour {

	// マテリアルにアクセスする為の変数（味方用）
	public Renderer playerPowerProgress;


	void Start () {
		
		// このRendererのマテリアルのProgressの値を1フレームで段階的に数値を上昇させたいのでコルーチンを使う
		playerPowerProgress = GetComponent<Renderer> ();

	}


	// 引数を増やす
	public void SetPlayerGage(float prePoint,float point){

		StopCoroutine ("PowerGageAnimation");

		StartCoroutine (
			
			PowerGageAnimation(
				prePoint,				// 一つ前のplayerValue
				point,					// 最新のplayerValue
				0.2f					// 時間
			
			)
		);
	}


	// ゲージアニメーション（ポイントアニメーションのコピー）
	private IEnumerator PowerGageAnimation(float start, float end, float time){

		// アニメーション開始時間
		float startTime = TimeManager.time;

		// アニメーション終了時間
		float endTime = startTime + time;

		while (TimeManager.time < endTime){
		
			// アニメーション中の今の経過時間を計算
			float t = (TimeManager.time - startTime) / time;

			// 数値を更新
			float updateValue = (((end - start) * t) + start);

			// ゲージの高さを更新
			playerPowerProgress.material.SetFloat("_Progress", updateValue);

			// 1フレーム待つ
			yield return null;

		}

		// 数値を最終値に合わせる
		playerPowerProgress.material.SetFloat ("_Progress", end);

	}
}
