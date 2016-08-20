 using UnityEngine;
using System.Collections;

public class BoardMove : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		
		//時間の更新
		transform.localPosition = new Vector3 (0, - TimeManager.tick * 0.05f, 0);

	}
}