using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 


public class SceneHandler : MonoBehaviour {

	public void InvokeTrainingScene(){
		Invoke ("TrainingScene",1.0f);
	}

	public void InvokeNovelScene(){
		Invoke ("NovelScene",1.0f);
	}

	public void InvokeHomeScene(){
		Invoke ("HomeScene",1.0f);
	}

	public void HomeScene(){
		SceneManager.LoadScene ("GUISystem");
	}

	public void NovelScene(){
		SceneManager.LoadScene ("1st_Peridot");
	}

	public void TrainingScene(){
		SceneManager.LoadScene ("Battle");
	}


}
