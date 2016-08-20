using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboHandler : MonoBehaviour {


	// コンボの設定
	public void setCombo(int combo){

		// アニメーションを止める
		StopCoroutine("PointAnimation");

		// アニメーションスタート
		StartCoroutine(
			ComboAnimation(
				int.Parse(gameObject.GetComponent<Text>().text),
				combo,
				0.2f
			)
		);
	}


	// コンボアニメーション（ポイントアニメーションを流用）
	private IEnumerator ComboAnimation(int start, int end, float time){

		// アニメーション開始時間
		float startTime = TimeManager.time;

		// アニメーション終了時間
		float endTime = startTime + time;

		// 1フレームごとに数値を上昇させる
		do{
			// アニメーション中の今の経過時間を計算
			float t = (TimeManager.time - startTime) / time;

			// 数値を更新
			int updateValue = (int)( ((end - start) * t) + start );

			// テキストを更新
			GetComponent<Text>().text = updateValue.ToString();

			// 1フレーム待つ
			yield return null;
		}while(TimeManager.time < endTime);

		// 数値を最終値に合わせる
		GetComponent<Text>().text = end.ToString();
	}
}
