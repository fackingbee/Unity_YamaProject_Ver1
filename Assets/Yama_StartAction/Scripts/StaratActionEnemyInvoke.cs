using UnityEngine;
using System.Collections;

public class StaratActionEnemyInvoke : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {

		gameObject.SetActive (false);
		//anim.enabled = true;
		anim = GetComponent<Animator> ();

		Invoke ("EnemyOn",2f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void EnemyOn(){
		gameObject.SetActive (true);
		anim.enabled = true;
	}

}
