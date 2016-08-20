using UnityEngine;
using System.Collections;

public class DamageShake : MonoBehaviour {


//	void Start () {
//
//	}
	
//	void Update () {
//	
//	}
		
	// 他のスクリプトに統合してもOK
	public void Shake(){
		float degree = 0.08f;
		iTween.ShakePosition ( gameObject,
							   iTween.Hash("x", degree,
										   "y", degree,
										   "isLocal", false,
										   "time", 1f
										  )
							 );
	}
}
