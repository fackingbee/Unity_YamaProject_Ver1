using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class ToGame : MonoBehaviour {

	void Start() {
		//TimeManager.time = 0f;
		//TimeManager.tick = 0;
	}

	//void Update() {

	//}

	public void toGame() {

		//ロゴシーンに切替
		SceneManager.LoadScene ("Battle");

	}

}
