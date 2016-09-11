using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GageHandler : MonoBehaviour {

	private Slider mySlider;			// ゲージ増減変数
	private float  basePerfectPoint;	// Perfect増加量
	private float  increaseLvPoint;		// レベル差によるゲージ増減の補正(Perfect~Good)
	private float  decreaseLvPoint;		// レベル差によるゲージ増減の補正(Miss)
	private int    kariPlayerLV;		// 味方レベル
	private int    kariEnemyLV;			// 敵レベル
	private float  setGageValue;		// スタート時に算出したゲージ開始値を格納

	void Start() {

		setGageValue = GameDate.setValue;
		//Debug.Log ("setGageValue : " + setGageValue);

		mySlider = GetComponent<Slider>();
		mySlider.value = setGageValue;
		//mySlider.value = mySlider.maxValue / 2;

		//Debug.Log ("mySlider.maxValue : " + mySlider.maxValue);
		//Debug.Log ("mySlider.value : " + mySlider.value);

		// まず味方と敵のレベルを暫定で設定する(Lv1~99の差の最大値は98)
		kariPlayerLV = 8;
		kariEnemyLV  = 5;

		//スコアの総数を取得する
		//Debug.Log ("スコア総数 : " + GameDate.totalScoreNum);

		// スコア数に応じてPerfect増加量を決める
		basePerfectPoint = (mySlider.value * 1.4f) / GameDate.totalScoreNum;
		//Debug.Log ("Perfect増加量 : " + basePerfectPoint);

		// レベル差をStart時に算出
		increaseLvPoint = 1 + (kariPlayerLV - kariEnemyLV) * 0.01f;
		decreaseLvPoint = 1 - (kariPlayerLV - kariEnemyLV) * 0.01f;
		//Debug.Log ("increaseLvPoint : " + increaseLvPoint);
		//Debug.Log ("decreaseLvPoint : " + decreaseLvPoint);

	}
		
	// ゲージセット
	public void setGage(string evaluation){

		float point = 0;
	
		// 開発段階テスト用
		// 現状はMissが多いとなかなかSに到達出来ないようにしている
		if (evaluation.Equals("Perfect")){
			point = (5.0f * increaseLvPoint) * basePerfectPoint * 0.14f;
			//Debug.Log ("Perfect : " + point);
		}else if(evaluation.Equals("Great")){
			point = (3.0f * increaseLvPoint) * basePerfectPoint * 0.14f;
			//Debug.Log ("Great : " + point);
		}else if(evaluation.Equals("Good")){
			point = (1.5f * increaseLvPoint) * basePerfectPoint * 0.14f;
			//Debug.Log ("Good : " + point);
		}else if(evaluation.Equals("Bad")){
			point = (0.5f * increaseLvPoint) * basePerfectPoint * 0.14f;
			//Debug.Log ("Bad : " + point);
		}else if(evaluation.Equals("Miss")){
			point = -(10.0f * decreaseLvPoint) * basePerfectPoint * 0.15f;
			//Debug.Log ("Miss : " + point);
		}

		// 一旦GameDataへ格納する
		GameDate.GagePoint += point;
	
		// アニメーション停止
		StopCoroutine( "GageAnimation" );

		// アニメーション開始
		StartCoroutine(
			GageAnimation(
				GameDate.GagePoint,
				0.2f
			)
		);
	}


	// ゲージアニメーション
	private IEnumerator GageAnimation(float point, float time){

		//Debug.Log (point);

		float startTime  = TimeManager.time;			// アニメーション開始時間
		float endTime    = startTime + time;			// アニメーション終了時間
		float startValue = mySlider.value;				// アニメーション開始時のゲージ
		//float endValue   = mySlider.value + point;	// アニメーション終了時のゲージ

		// 1フレームごとに数値を上昇させる
		//while (TimeManager.time < endTime || mySlider.value < endValue){

		while (TimeManager.time < endTime){

			// アニメーション中の今の経過時間を計算
			float t = (TimeManager.time - startTime) / time;

			// 数値を更新
			mySlider.value = startValue + (point * t);

			// 1フレーム待つ
			yield return null;

			// もし1秒経ってもループしてたら強制的にブレイク
			if (TimeManager.time > startTime + 2.0f) {
				break;
			}

//*			// 上限に達してもしばらくコルーチンが動きMissをしてもvalueが減らない現象を防ぐ為
			if( mySlider.value >= 1000f && point * t != 0 ){
				yield break;
				//「point * t != 0」が必要な理由:
				// 一度1000に達すると必ず「yield break」してしまうが、
				// コルーチン開始時は「point * t = 0」なので
				// point * t != 0」とすることで進入を回避出来るし、
				// 次のフレームでは「point * t > 0」だが、
				// 今度は「mySlider.value < 1000」なのでやはり進入を回避出来る。
			}
		} 
		// コルーチンが終わったら初期化する
		GameDate.GagePoint = 0;
	}
}




//public class GageHandler : MonoBehaviour {
//
//
//	// ゲージセット
//	public void setGage(float point){
//		
//		// ゲージの長さを計算（正規化）
//		float length = point / 100f;
//
//		// ゲージ変更
//		//transform.localScale = new Vector3(length, 1, 1);
//
//		// アニメーションを止める
//		StopCoroutine( "GageAnimation" );
//
//		// アニメーションスタート
//		StartCoroutine(
//			GageAnimation(
//				transform.localScale.x,
//				length,
//				0.2f
//			)
//		);
//	}
//
//
//	// ゲージアニメーション（ポイントアニメーションのコピー）
//	private IEnumerator GageAnimation(float start, float end, float time){
//
//		// アニメーション開始時間
//		float startTime = TimeManager.time;
//
//		// アニメーション終了時間
//		float endTime = startTime + time;
//
//		// 1フレームごとに数値を上昇させる
//		do{
//			// アニメーション中の今の経過時間を計算
//			float t = (TimeManager.time - startTime) / time;
//
//			// 数値を更新
//			float updateValue = ( ((end - start) * t) + start );
//
//			// ゲージの長さを更新
//			transform.localScale = new Vector3(updateValue, 1, 1);
//
//			// 1フレーム待つ
//			yield return null;
//
//		}while(TimeManager.time < endTime);
//
//		// 数値を最終値に合わせる
//		transform.localScale = new Vector3(end, 1, 1);
//	}
//}
