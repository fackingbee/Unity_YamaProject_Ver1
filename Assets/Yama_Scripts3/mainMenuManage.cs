using UnityEngine;
using System.Collections;

public class mainMenuManage : MonoBehaviour {


	void Start () {

		//レベルアップ処理
		if(variableManage.currentExp >= variableManage.nextExp){

			// レベルアップ
			variableManage.currentLv += 1;

			// 現在のレベルを確認
			Debug.Log (variableManage.currentLv);

			// ちょっと後で確認
			variableManage.currentExp  = variableManage.currentExp - variableManage.nextExp ;

			// メッセージを表示
			variableManage.showLvupMes = true;
		}

		//レベルから次の必要経験値を計算（Lv1のとき、必要経験値数が100、Lv2のとき、必要経験値数が200）
		variableManage.nextExp = variableManage.currentLv * 100;

	}


//	void Update () {
//	
//	}

}
