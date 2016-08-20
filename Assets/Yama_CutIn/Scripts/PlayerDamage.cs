using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour {

	private Animator playerDamage;
	private bool     isPlayerDamage;
	private bool     isPlayerNotDamage;

	// 攻撃を受けたらボム発生
	public GameObject damageBomb;

	// 攻撃を受けたらダメージエフェクトとしてブレ発生
	public DamageShake damageShake;

	// Use this for initialization
	void Start () {

		isPlayerDamage    = true;
		isPlayerNotDamage = true;
		playerDamage      = GetComponent<Animator> ();
		damageShake       = GetComponent<DamageShake> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "EnemyAttack" ){

			Instantiate (damageBomb);

			isPlayerDamage    = true;

			playerDamage.SetBool ("isPlayerDamage", isPlayerDamage);
			playerDamage.SetBool ("isPlayerNotDamage", isPlayerNotDamage);

			damageShake.Shake ();

		}
	}


	void OnTriggerExit(Collider col_02){
		if(col_02.gameObject.tag == "EnemyAttack"){

			isPlayerDamage    = false;

			playerDamage.SetBool ("isPlayerDamage", isPlayerDamage);

		}
	}
}
