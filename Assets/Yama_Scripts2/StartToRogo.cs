using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class StartToRogo : MonoBehaviour {

	float startTime = 0.0f;

//	// Use this for initialization
//	void Start () {
//	
//	}
	
//	// Update is called once per frame
	void Update () {
		startTime = startTime + Time.deltaTime;
		if(startTime >= 5.0f){
			SceneManager.LoadScene ("Rogo");
		}
	}
		
//	void ToRogo() {
//	//ロゴシーンに切替
//		SceneManager.LoadScene ("Rogo");
//	}
		
}
