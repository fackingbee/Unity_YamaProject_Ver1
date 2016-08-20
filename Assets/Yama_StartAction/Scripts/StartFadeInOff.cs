using UnityEngine;
using System.Collections;

public class StartFadeInOff : MonoBehaviour {

	void Start () {

		Invoke ("Off",3f);
	
	}

	void Off(){
		gameObject.SetActive (false);
	}
}
