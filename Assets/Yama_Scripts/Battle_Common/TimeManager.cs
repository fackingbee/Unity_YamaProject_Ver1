 using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	// 時間管理（どこからでも呼べるように静的メンバーstaticで宣言）(staticをつけるとpublicでもInspector上に表示されない)
	public static float time;

	// MIDI時間管理用
	public static long tick;
//	public static float tick;

	// テンポ情報
	//public static int tempo;
	public static float tempo;

//	public static bool startFlg;


	// iOSにBuild時のフレームレート問題の為 ***********************************************
	void Awake () {
		
		//Application.targetFrameRate = 60;
		TimeManager.time            = 0f;
		TimeManager.tick            = 0;

	}
	// *******************************************************************************


	// Use this for initialization
	void Start () {
		// 初期化
	}


	// Update is called once per frame
	void Update () {

//		if(Input.GetKeyDown(KeyCode.Space)){
//			startFlg = true;
//			AudioManager.AudioPlay ();
//		}
//		if(startFlg){

		// timeを更新
		TimeManager.time += Time.deltaTime;

		// timeからtickを計算 (ProToolsで出力したMIDIは一拍9600tick)
//		TimeManager.tick = (long)(TimeManager.time * (TimeManager.tempo * 9600f) / 60f);
//		TimeManager.tick = (float)(TimeManager.time * (TimeManager.tempo * 9558f) / 60.1523f) ;
		TimeManager.tick = (long)(TimeManager.time * (TimeManager.tempo * 9558f) / 60.1523f);
//		TimeManager.tick = (long)(TimeManager.time * (TimeManager.tempo * 9559f) / 60.1523f);
//		TimeManager.tick = (long)(TimeManager.time * (TimeManager.tempo * 9561f) / 60.1523f);
//		}
	}
}
