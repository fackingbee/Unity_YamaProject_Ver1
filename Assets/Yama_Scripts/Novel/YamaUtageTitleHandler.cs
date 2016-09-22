using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 起動時は非アクティブなので『SaveLoad』にアタッチする

public class YamaUtageTitleHandler : MonoBehaviour {

	private GameObject autoSaveItem;
	private GameObject title;
	private Button     button;


	void Start () {


		autoSaveItem = GameObject.Find ("SaveLoadItem(Clone)");		// オートセーブアイテムを検索して取得
		title        = GameObject.Find ("Title");					// Titleを検索して取得
		button       = autoSaveItem.GetComponent <Button>();		// Buttonのinteractableが、TureかFalseかを見る

		//Debug.Log ("SaveLoadItem(Clone)を取得 : " + autoSaveItem.name);
		//Debug.Log ("interactable : " + button.interactable);

		// Trueの時 = AutoSaveが存在：ページを存続
		if (button.interactable) {
			
			Debug.Log ("interactableはTrue");

			// セーブデータがある場合、何もしなければページはアクティブ
			Debug.Log ("gameObject.activeSelf : " + gameObject.activeSelf);

			title.SetActive (false);



		// Falseの時 = Empty：ページを非アクティブ（はじめからに戻る）
		} else {
			
			//Debug.Log ("interactableはFalse");

			// ロードボタンを取得
			GameObject backLoad = GameObject.Find ("LoadGame");

			// セーブデータが無い時はスタート画面に戻る前にロードボタンを非アクティブ
			backLoad.SetActive (false);

			// 戻る処理は『SaveLoad』を非アクティブにするだけで良い
			gameObject.SetActive(false);

		}
	
	}
	
//	void Update () {
//
//	
//	}

}
