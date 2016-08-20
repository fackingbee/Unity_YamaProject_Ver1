using UnityEngine;
using System.Collections;

public class PathRotate : MonoBehaviour {

	private Animator pathRotate1;
	private Animator pathRotate2;
	private Animator pathRotate3;
	private Animator pathRotate4;

	void Start () {

		pathRotate1 = GameObject.Find ("Path1").GetComponent<Animator> ();
		pathRotate2 = GameObject.Find ("Path2").GetComponent<Animator> ();
		pathRotate3 = GameObject.Find ("Path3").GetComponent<Animator> ();
		pathRotate4 = GameObject.Find ("Path4").GetComponent<Animator> ();

		Invoke ("PathRotate1",4.8f);
		Invoke ("PathRotate2",5.4f);
		Invoke ("PathRotate3",5.7f);
		Invoke ("PathRotate4",6.0f);
	
	}
		
	void PathRotate1(){ pathRotate1.enabled = true; }

	void PathRotate2(){ pathRotate2.enabled = true; }

	void PathRotate3(){ pathRotate3.enabled = true; }

	void PathRotate4(){ pathRotate4.enabled = true; }

}
