using UnityEngine;
using System.Collections;

public class InstantiateCharacter : MonoBehaviour {

	// 最初に選んだ任意のキャラクターを所定の位置にセットする
	void Awake() { Instantiate(CharacterManager.useChara01); }
}
