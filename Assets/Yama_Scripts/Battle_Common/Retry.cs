using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour {

	public void Reload() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("TopMenu");
	}
}
