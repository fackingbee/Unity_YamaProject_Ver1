using UnityEngine;
using System.Collections;
using Utage;

public class UtageShake : MonoBehaviour {

	// エクセルシートのパラメータを取得する為の変数
	public AdvEngine engine;

	// 背景格納変数
	private GameObject bg;

	//揺れの範囲を格納する変数
	public float degree;



	void Start () {

		// 揺れ幅の初期化
		degree = 0.08f;

	}
		

	void Update () {

		// 再生後は基本bgが存在するので、一度しか進入しない
		if ( bg==null ){
			
			Debug.Log ("最初に背景を取得する");

			bg = GameObject.Find("Bg Default");
		}
	}


	public void Shake(){

		// "DamageEffect"がTrueならif内に入り、エフェクト実行
		if((bool)engine.Param.GetParameter ("Shake")){

			Debug.Log ("ShakeはTrueです");

			// iTweenで揺らす
			iTween.ShakePosition (gameObject, iTween.Hash(
				"x",degree,
				"y",degree,
				"islocal",true,
				"time",1.5f
			));

			// エクセルシートのフラグ管理をスクリプト側から操作
			engine.Param.TrySetParameter("Shake",false);

		}
	}
}
