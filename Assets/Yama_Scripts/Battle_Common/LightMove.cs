using UnityEngine;
using System.Collections;

public class LightMove : MonoBehaviour {

	public float speed = 1f;
	public float width = 5f;  

	// iOSにBuild時のフレームレート問題の為 ***********************************************

	void Awake(){
		
		Application.targetFrameRate = 60;

	}
	// *******************************************************************************

	void Update () {
		
		transform.position = 
			new Vector3(Mathf.Sin (Time.time * speed) * width, transform.position.y , transform.position.z);
		
	}
}
