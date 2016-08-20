using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	// こんな方法もあるよ（文字列は後々使い難くなるので極力数値で管理）
	//public List<GameObject> characterList = new List<GameObject>();
	//public Dictionary<string, GameObject> characterList2 = new Dictionary<string, GameObject>();

	// 連想配列（Playerの番号をつけて固定）
	public Dictionary<int, GameObject> characterList = new Dictionary<int, GameObject>();

	//（例）：SwordManをInspector上で格納
	public GameObject character01;

	// バトルシーンに遷移して、セットしたPlayerを渡す。
	public static GameObject useChara01;



	void Awake() {

		// 0番にSwordmanがセットされた //
		characterList.Add(0, character01);

		//（例）：文字列を使うなら //
		//characterList2.Add("poti", character01);

	}

	void Start () {
		
		//（例）：文字列を使うなら //
		//SelectChara(characterList2["poti"]);	

		// セットされたSwordmanを引数として渡す
		SelectChara(characterList[0]);

	}

	void SelectChara(GameObject selected) {

		// 渡されたSwordmanを別シーンに渡す
		useChara01 = selected;

	}
}
