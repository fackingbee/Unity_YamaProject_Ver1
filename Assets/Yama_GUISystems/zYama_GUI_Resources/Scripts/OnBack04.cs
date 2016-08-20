using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack04 : MonoBehaviour {

	/// <summary>
	/// 以下、OnBack00と同義
	/// </summary>

	private Toggle      toggle04;
	private GameObject  onBackImage04;
	private GameObject  tabShadow04;
	private Image       image04;
	private Image       imageShadow04;
	private bool        isChecked;
	private bool        isTouch;
	public  AudioClip   onButton04;
	private AudioSource audioSource;
	private Color       onBackColor04  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color       offBackColor04 = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Awake(){
		onBackImage04 = GameObject.Find ("ShopBack");
		image04       = onBackImage04.GetComponent<Image> ();
		tabShadow04   = GameObject.Find ("TabShadow04");
		imageShadow04 = tabShadow04.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();
		image04.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	}

	void Start () {
		toggle04      = GetComponent<Toggle> ();
		toggle04.isOn = false;
		isChecked     = true;
		isTouch       = false;
	}

	void Update () {
		if(isChecked == true && toggle04.isOn == true){
			OnImage04 ();
			//Debug.Log ("hoge_4_1");
		}else if(isChecked == false && toggle04.isOn == false){
			OffImage04 ();
			//Debug.Log ("hoge_4_2");
		}
		if(isTouch && toggle04.isOn == true){
			audioSource.PlayOneShot (onButton04,0.6f);
			isTouch = false;
		}
	}

	public void OnImage04(){
		if(toggle04.isOn == true){
			image04.color = onBackColor04;
			imageShadow04.enabled = true;
			isChecked             = false;
			isTouch               = true;
		}
	}

	public void OffImage04(){
		if(toggle04.isOn == false){
			image04.color = offBackColor04;
			imageShadow04.enabled = false;
			isChecked             = true;
		}
	}
}
