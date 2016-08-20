using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 
public class RogoToOpening : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Move ("rogo.mp4");
		SceneManager.LoadScene ("Opening");
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
