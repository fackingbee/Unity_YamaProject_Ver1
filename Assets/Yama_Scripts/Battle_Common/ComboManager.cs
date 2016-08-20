using UnityEngine;
using System.Collections;

public class ComboManager : MonoBehaviour {

	// Combo用変数
	public static int maxCombo     = 0;
	public static int combo	       = 0;
	public static int perfectCount = 0;

	void Awake(){
		maxCombo     = 0;
		combo	     = 0;
		perfectCount = 0;
	}

}
