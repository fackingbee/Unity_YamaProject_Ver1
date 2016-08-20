using UnityEngine;
using System.Collections;

public class EnemyPowerGageHandler : MonoBehaviour {

	// マテリアルにアクセスする為の変数（敵用）
	public Renderer enemyPowerProgress;


	void Start () {

		// このRendererのマテリアルのProgressの値を1フレームで段階的に数値を上昇させたいのでコルーチンを使う
		enemyPowerProgress = GetComponent<Renderer> ();

	}


	// 引数を増やす
	public void SetEnemyGage(float prePoint,float point){

		StopCoroutine ("PowerGageAnimation_Enemy");

		StartCoroutine (

			PowerGageAnimation_Enemy(
				prePoint,				// 一つ前のplayerValue
				point,					// 最新のplayerValue
				0.2f					// 時間

			)
		);
	}


	// ゲージアニメーション（ポイントアニメーションのコピー）
	private IEnumerator PowerGageAnimation_Enemy(float start, float end, float time){

		//Debug.Log ("start :" + start);
		//Debug.Log ("end :" + end);


		// アニメーション開始時間
		float startTime = TimeManager.time;

		// アニメーション終了時間
		float endTime = startTime + time;

		// 1フレームごとに数値を上昇させる
		while(TimeManager.time < endTime){
			// アニメーション中の今の経過時間を計算
			float t = (TimeManager.time - startTime) / time;

			// 数値を更新
			float updateValue = (((end - start) * t) + start);

			// ゲージの高さを更新
			enemyPowerProgress.material.SetFloat("_Progress", updateValue);

			// 1フレーム待つ
			yield return null;
		}
		// 数値を最終値に合わせる
		enemyPowerProgress.material.SetFloat ("_Progress", end);

	}

}
