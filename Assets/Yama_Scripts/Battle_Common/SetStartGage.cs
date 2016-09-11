using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 後に敵と味方のレベル差でゲージパラメーターの初期値を増減させる。


public class SetStartGage : MonoBehaviour {

	private Slider       startValue;
	private Slider       setValue;
	public  GageHandler  gageHandler;

	private bool   isFinished;

	void Start () {

		gageHandler = FindObjectOfType<GageHandler> ();
		gageHandler.enabled = false;
		startValue       = GetComponent<Slider> ();
		setValue         = GetComponent<Slider> ();
		startValue.value = 0f;
		isFinished       = true;

	}
	
	void Update () {

		if(setValue.value < startValue.maxValue/2 ){
			setValue.value += 2.0f;
			//Debug.Log ("増加中");
		}

		if(setValue.value == startValue.maxValue/2 && isFinished){

			gageHandler.enabled = true;
			isFinished          = false;
			GameDate.setValue   = startValue.maxValue / 2;

			//Debug.Log ("完了");
			//Debug.Log ("startValue.maxValue/2 : " + startValue.maxValue/2 );

			this.enabled = false;
		}
	}
}
