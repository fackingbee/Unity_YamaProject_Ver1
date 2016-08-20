using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class OpeningToTopMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Move2 ("op_sound.mp4");
		SceneManager.LoadScene ("TopMenu");
	}
		
	// Build時のmp４使用関数
	void Move2(string path){
		
		Handheld.PlayFullScreenMovie (
			path, 
			Color.blue, 
			FullScreenMovieControlMode.CancelOnInput
		);
	}
}
