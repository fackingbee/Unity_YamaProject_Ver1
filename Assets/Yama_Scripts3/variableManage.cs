using UnityEngine;
using System.Collections;

/// <summary>
/// Variable manage.
/// どこからでも見れるようStaticで
/// </summary>

public class variableManage : MonoBehaviour {

	// プレイヤー情報
	static public int  currentExp    = 0;
	static public int  nextExp       = 100;
	static public int  currentLv     = 1;
	static public bool showLvupMes   = false;
	static public bool openMachine02 = false;
	static public bool openMachine03 = false;
	static public int  myWP          = 0;

	//レベルアップ処理
	public static void levelUp(){

		while(currentExp >= nextExp){

			Debug.Log ("variableManage.currentExp : " + currentExp);
			Debug.Log ("variableManage.nextExp : " + nextExp);

			currentLv  += 1;
			currentExp  = currentExp - nextExp ;
			nextExp     = currentLv * 100;
			showLvupMes = true;

		}
	}



//	void Start () {
//	
//	}
//	

//	void Update () {
//	
//	}

}
