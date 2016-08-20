using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack02 : MonoBehaviour {

	/// <summary>
	/// 以下、OnBack00と同義
	/// </summary>

	private Toggle      toggle02;
	private GameObject  onBackImage02;
	private GameObject  tabShadow02;
	private Image       image02;
	private Image       imageShadow02;
	private bool        isChecked;
	private bool        isTouch;
	public  AudioClip   onButton02;
	private AudioSource audioSource;
	private Color       onBackColor02  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color       offBackColor02 = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Awake(){
		onBackImage02 = GameObject.Find ("ListBack");
		image02       = onBackImage02.GetComponent<Image> ();
		tabShadow02   = GameObject.Find ("TabShadow02");
		imageShadow02 = tabShadow02.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();
		image02.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	}

	void Start () {
		toggle02      = GetComponent<Toggle> ();
		toggle02.isOn = false;
		isChecked     = true;
		isTouch       = false;
	}

	void Update () {
		if(isChecked == true && toggle02.isOn == true){
			OnImage02 ();
			//Debug.Log ("hoge_2_1");
		}else if(isChecked == false && toggle02.isOn == false){
			OffImage02 ();
			//Debug.Log ("hoge_2_2");
		}
		if(isTouch && toggle02.isOn == true){
			audioSource.PlayOneShot (onButton02,0.6f);
			isTouch = false;
		}
	}

	public void OnImage02(){
		if(toggle02.isOn == true){
			image02.color = onBackColor02;
			imageShadow02.enabled = true;
			isChecked             = false;
			isTouch               = true;
		}
	}

	public void OffImage02(){
		if(toggle02.isOn == false){
			image02.color = offBackColor02;
			imageShadow02.enabled = false;
			isChecked             = true;
		}
	}
}
