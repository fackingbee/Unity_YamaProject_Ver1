using UnityEngine;
using System.Collections;

public class AutoEffectsDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Invoke ("EffectsDestroy", 4f);
	
	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void EffectsDestroy(){

		Destroy (gameObject);

	}
}
