using UnityEngine;
using System.Collections;

public class GameFrame : MonoBehaviour {

	// iOSにBuild時のフレームレート問題の為
	void Awake() { Application.targetFrameRate = 60; }
}
