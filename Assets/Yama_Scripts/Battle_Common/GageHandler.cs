using UnityEngine;
using System.Collections;

public class GageHandler : MonoBehaviour {


	// ゲージセット
	public void setGage(float point){
		
		// ゲージの長さを計算（正規化）
		float length = point / 100f;

		// ゲージ変更
		//transform.localScale = new Vector3(length, 1, 1);

		// アニメーションを止める
		StopCoroutine( "GageAnimation" );

		// アニメーションスタート
		StartCoroutine(
			GageAnimation(
				transform.localScale.x,
				length,
				0.2f
			)
		);
	}


	// ゲージアニメーション（ポイントアニメーションのコピー）
	private IEnumerator GageAnimation(float start, float end, float time){

		// アニメーション開始時間
		float startTime = TimeManager.time;

		// アニメーション終了時間
		float endTime = startTime + time;

		// 1フレームごとに数値を上昇させる
		do{
			// アニメーション中の今の経過時間を計算
			float t = (TimeManager.time - startTime) / time;

			// 数値を更新
			float updateValue = ( ((end - start) * t) + start );

			// ゲージの長さを更新
			transform.localScale = new Vector3(updateValue, 1, 1);

			// 1フレーム待つ
			yield return null;

		}while(TimeManager.time < endTime);

		// 数値を最終値に合わせる
		transform.localScale = new Vector3(end, 1, 1);
	}
}
