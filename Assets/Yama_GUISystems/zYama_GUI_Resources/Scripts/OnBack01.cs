using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack01 : MonoBehaviour {

	/// <summary>
	/// 以下、OnBack00と同義
	/// </summary>

	private Toggle      toggle01;
	private GameObject  onBackImage01;
	private GameObject  tabShadow01;
	private Image       image01;
	private Image       imageShadow01;
	private bool        isChecked;
	private bool        isTouch;
	public  AudioClip   onButton01;
	private AudioSource audioSource;
	private Color       onBackColor01  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color       offBackColor01 = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Awake(){
		onBackImage01 = GameObject.Find ("PlayBack");
		image01       = onBackImage01.GetComponent<Image> ();
		tabShadow01   = GameObject.Find ("TabShadow01");
		imageShadow01 = tabShadow01.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();
		image01.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	}

	void Start () {
		toggle01      = GetComponent<Toggle> ();
		toggle01.isOn = false;
		isChecked     = true;
		isTouch       = false;
	}
		
	void Update () {
		if(isChecked == true && toggle01.isOn == true){
			OnImage01 ();
			//Debug.Log ("hoge_1_1");
		}else if(isChecked == false && toggle01.isOn == false){
			OffImage01 ();
			//Debug.Log ("hoge_1_2");
		}
		if(isTouch && toggle01.isOn == true){
			audioSource.PlayOneShot (onButton01,0.6f);
			isTouch = false;
		}
	}
		
	public void OnImage01(){
		if(toggle01.isOn == true){
			image01.color = onBackColor01;
			imageShadow01.enabled = true;
			isChecked             = false;
			isTouch               = true;
		}
	}

	public void OffImage01(){
		if(toggle01.isOn == false){
			image01.color = offBackColor01;
			imageShadow01.enabled = false;
			isChecked             = true;
		}
	}
}
