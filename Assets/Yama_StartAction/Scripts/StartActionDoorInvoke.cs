using UnityEngine;
using System.Collections;

public class StartActionDoorInvoke : MonoBehaviour {

	private Animator animBottom;
	private Animator animTop;

	private AudioSource doorSE;


	// Use this for initialization
	void Start () {

		doorSE = GetComponent<AudioSource> ();

		animBottom = GameObject.Find ("Bottom_blast_Door").GetComponent<Animator> ();
		animTop    = GameObject.Find ("Top_blast_Door").GetComponent<Animator> ();

		animBottom.enabled = false;
		animTop.enabled    = false;

		Invoke ("DoorOn",7f);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DoorOn(){
		animBottom.enabled = true;
		animTop.enabled = true;
		doorSE.Play ();
	}
}
