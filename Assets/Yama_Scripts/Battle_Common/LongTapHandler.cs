using UnityEngine;
using System.Collections;

public class LongTapHandler : MonoBehaviour {

	RectTransform touchBarRect;
	RectTransform myRect;
	ScoreHandler  myParentScoreHandler;

	void Start () {
		touchBarRect         = GameObject.Find("TouchBar1").GetComponent<RectTransform>();
		myRect               = GetComponent<RectTransform>();
		myParentScoreHandler = GetComponentInParent<ScoreHandler>();

	}
		
	void Update () {
		if (myRect.position.y <= touchBarRect.position.y && myParentScoreHandler.isLongTap) {
			Destroy(gameObject);
		}
	}
}
