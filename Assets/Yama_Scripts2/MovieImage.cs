using UnityEngine;
using System.Collections;

public class MovieImage : MonoBehaviour {



	// RogoToOpening、OpeningToTopMenuに直接書いたのでこのスクリプトは現在必要ない。
	// 参考までに残しておく。



	// iOSにBuild時のフレームレート問題の為＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
	void Awake()
	{
		Application.targetFrameRate = 60;
	}

	// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝


	// Use this for initialization
	void Start () {

#if UNITY_5_EDITOR
		// Mobieはそのままだと再生されないのでこのコードが必要
		// Unityでの確認用
		(GetComponent<Renderer> ().material.mainTexture as MovieTexture).Play ();

#elif UNITY_IPHONE
		// MovieTextureはモバイル時に使用できないのでこちらに切り替える
		Move("op_sound.mp4");

#elif UNITY_ANDROID
		// MovieTextureはモバイル時に使用できないのでこちらに切り替える
		Move("op_sound.mp4");

#endif

	}


//	// Update is called once per frame
//	void Update () {
//	
//	}


	void Move(string path){
		Handheld.PlayFullScreenMovie (
			path, 
			Color.blue, 
			FullScreenMovieControlMode.CancelOnInput
		);
	}
}
