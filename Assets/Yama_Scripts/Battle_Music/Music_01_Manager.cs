using UnityEngine;
using System.Collections;

public class Music_01_Manager : MonoBehaviour {

	// GameControllerを曲分作ると修正が入ったとき大変なので、このスクリプトを曲の文だけ複製して、使用する。
	// 尚、今のところdelayTimeは変数自動化出来ず、手動で管理するしかない。（曲ごとにこのスクリプトを複製してdelayTimeを書き換える）

	public float delayTime;


//	// iOSにBuild時のフレームレート問題の為 ***********************************************
//	void Awake(){
//		Application.targetFrameRate = 60;
//	}
//	// *******************************************************************************


	void Start () {

		delayTime = 1.85f;
		GetComponent<GameController> ().delayTime = delayTime;
	}

//	void Update () {
//		
//	}

}
