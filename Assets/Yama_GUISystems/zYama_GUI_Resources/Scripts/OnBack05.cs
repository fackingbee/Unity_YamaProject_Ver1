using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack05 : MonoBehaviour {

	/// <summary>
	/// 以下、OnBack00と同義
	/// </summary>

	private Toggle      toggle05;
	private GameObject  onBackImage05;
	private GameObject  tabShadow05;
	private Image       image05;
	private Image       imageShadow05;
	private bool        isChecked;
	private bool        isTouch;
	public  AudioClip   onButton05;
	private AudioSource audioSource;
	private Color       onBackColor05  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color       offBackColor05 = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Awake(){
		onBackImage05 = GameObject.Find ("OptionBack");
		image05       = onBackImage05.GetComponent<Image> ();
		tabShadow05   = GameObject.Find ("TabShadow05");
		imageShadow05 = tabShadow05.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();
		image05.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	}

	void Start () {
		toggle05      = GetComponent<Toggle> ();
		toggle05.isOn = false;
		isChecked     = true;
		isTouch       = false;
	}

	void Update () {
		if(isChecked == true && toggle05.isOn == true){
			OnImage05 ();
			//Debug.Log ("hoge_5_1");
		}else if(isChecked == false && toggle05.isOn == false){
			OffImage05 ();
			//Debug.Log ("hoge_5_2");
		}
		if(isTouch && toggle05.isOn == true){
			audioSource.PlayOneShot (onButton05,0.6f);
			isTouch = false;
		}
	}

	public void OnImage05(){
		if(toggle05.isOn == true){
			image05.color = onBackColor05;
			imageShadow05.enabled = true;
			isChecked             = false;
			isTouch               = true;
		}
	}

	public void OffImage05(){
		if(toggle05.isOn == false){
			image05.color = offBackColor05;
			imageShadow05.enabled = false;
			isChecked             = true;
		}
	}
}
