using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UImainMenu : MonoBehaviour {

	//テキスト格納
	public Text playerStatusText;
	public Text lvupNum;
//	public Text battleStartBtn;
//	public Text unlockText;
//	public Text unlockBtn;


	//オブジェクト関連
	public GameObject lvupObj;
//	public GameObject unlockBtnObj;


	//レベルアップメッセージ用
	private float mesTimer;

	void Start () {

		mesTimer = 0f;
		//Debug.Log ("mesTimer : " + mesTimer);
	
	}
	
	void Update () {

		variableManage.levelUp ();

		lvupNum.text = variableManage.currentLv.ToString ();

		playerStatusText.text = "Player Lv : " + variableManage.currentLv  +
								" Next Lv : "  + variableManage.currentExp +
								" / "          + variableManage.nextExp;


		//レベルアップメッセージ
		if(variableManage.showLvupMes){
			
			if (mesTimer == 0f) {
				
				lvupObj.SetActive (true);

			} else if (mesTimer > 3.0f){
				
				mesTimer = 0f;
				variableManage.showLvupMes = false;
				lvupObj.SetActive (false);

			}
			mesTimer += Time.deltaTime;
		}
	
	}
}
