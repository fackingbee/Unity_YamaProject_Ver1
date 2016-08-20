using UnityEngine;
using System.Collections;

public class PlayerAttackMove : MonoBehaviour {

	// 移動変数 //
	public float move;

	// ターゲット //
	GameObject   target;

	// 方向 //
	Vector3      direction;


	void Start () {

		// Targetオブジェクトをタグで検索
		target    = GameObject.FindGameObjectWithTag("Target");

	}


	void Update () {

		// TargetがPlayerAttackHitからどの方向にあるかを計算
		direction = transform.position - target.gameObject.transform.position;

		// 方向を単位ベクトルで正規化
		direction.Normalize ();

		// Targetに向かって単純移動
		//transform.position += -direction * move;

		// Targetに向かって放物線移動
		transform.position += new Vector3(-direction.x,-direction.y,-direction.z) * move;
	}


	// Targetに接触すると自身を削除
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Target"){
			Destroy (gameObject);
		}
	}
}
