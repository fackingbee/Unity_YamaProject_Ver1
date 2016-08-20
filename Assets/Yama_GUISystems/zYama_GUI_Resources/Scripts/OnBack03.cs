using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack03 : MonoBehaviour {

	/// <summary>
	/// 以下、OnBack00と同義
	/// </summary>

	private Toggle      toggle03;
	private GameObject  onBackImage03;
	private GameObject  tabShadow03;
	private Image       image03;
	private Image       imageShadow03;
	private bool        isChecked;
	private bool        isTouch;
	public  AudioClip   onButton03;
	private AudioSource audioSource;
	private Color       onBackColor03  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color       offBackColor03 = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Awake(){
		onBackImage03 = GameObject.Find ("RecordBack");
		image03       = onBackImage03.GetComponent<Image> ();
		tabShadow03   = GameObject.Find ("TabShadow03");
		imageShadow03 = tabShadow03.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();
		image03.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	}

	void Start () {
		toggle03      = GetComponent<Toggle> ();
		toggle03.isOn = false;
		isChecked     = true;
		isTouch       = false;
	}

	void Update () {
		if(isChecked == true && toggle03.isOn == true){
			OnImage03 ();
			//Debug.Log ("hoge_3_1");
		}else if(isChecked == false && toggle03.isOn == false){
			OffImage03 ();
			//Debug.Log ("hoge_3_2");
		}
		if(isTouch && toggle03.isOn == true){
			audioSource.PlayOneShot (onButton03,0.6f);
			isTouch = false;
		}
	}

	public void OnImage03(){
		if(toggle03.isOn == true){
			image03.color = onBackColor03;
			imageShadow03.enabled = true;
			isChecked             = false;
			isTouch               = true;
		}
	}

	public void OffImage03(){
		if(toggle03.isOn == false){
			image03.color = offBackColor03;
			imageShadow03.enabled = false;
			isChecked             = true;
		}
	}
}
