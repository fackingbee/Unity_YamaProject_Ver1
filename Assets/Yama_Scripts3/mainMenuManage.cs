using UnityEngine;
using System.Collections;

public class mainMenuManage : MonoBehaviour {


	void Start () {


		//Kiiから前回までの経験値を取得する
		Debug.Log ("variableManage.currentExp : " + variableManage.currentExp);


		// レベルから次の必要経験値を計算（Lv1のとき、必要経験値数が100、Lv2のとき、必要経験値数が200）
		// 先にnextExpを取得しないと、デフォルトが100なので起動の度にレベルが上がってしまう。
		variableManage.nextExp = variableManage.currentLv * 100;


		//currentExpからnextExpを算出
		Debug.Log ("variableManage.nextExp : " + variableManage.nextExp);


		//レベルアップ処理
		if(variableManage.currentExp >= variableManage.nextExp){

			// レベルアップ
			variableManage.currentLv += 1;

			// ちょっと後で確認
			variableManage.currentExp  = variableManage.currentExp - variableManage.nextExp ;

			// メッセージを表示
			variableManage.showLvupMes = true;

		}

		// レベルから次の必要経験値を計算（Lv1のとき、必要経験値数が100、Lv2のとき、必要経験値数が200）
		// ここで分母を上げておく
		variableManage.nextExp = variableManage.currentLv * 100;

	}


//	void Update () {
//	
//	}

}
