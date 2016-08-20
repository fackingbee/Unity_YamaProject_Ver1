using UnityEngine;
using System.Collections;

public class ScoreLongUpController : MonoBehaviour {

	GameObject[] objcts;
	GameObject   longTapStartObj = null;

	void Start() {

		objcts = GameObject.FindGameObjectsWithTag(gameObject.tag);

		int minIndex = 100;

		foreach (GameObject obj in objcts) {
			if (obj.name == "Score_Long(Clone)") {
				int tmpIndex = obj.transform.GetSiblingIndex();
				if (tmpIndex < minIndex) {
					minIndex = tmpIndex;
					longTapStartObj = obj;
				}
			}
		}

		if (longTapStartObj != null) {
			longTapStartObj.GetComponent<Shadow_AfterImage>().afterImageEnabled = false;
		}
	}
}
