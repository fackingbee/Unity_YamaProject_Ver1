using UnityEngine;
using System.Collections;

public class AutoDestroy02 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("PaticleDestroy",1.5f);
	}

	public void PaticleDestroy(){
		Destroy (gameObject);
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
