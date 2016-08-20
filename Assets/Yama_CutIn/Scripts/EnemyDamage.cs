using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

	// 空のGameObjectにコライダーをつけて、エフェクトの動き（剣の軌道など）に合わせてアニメーションさせている。
	// メリットは自由に衝突判定を決められる。デメリットはAnimator、Animationファイルがものすごく増える。

	private Animator   enemyDamage;
	public  GameObject enemyDamageBomb;
	private bool       isDamage;
	private bool       isNotDamage;

	// 攻撃を受けたらダメージエフェクトとしてブレ発生
	public DamageShake damageShake;


	void Start () {
		isDamage          = true;
		isNotDamage       = true;
		enemyDamage       = GetComponent<Animator> ();
		damageShake       = GetComponent<DamageShake> ();

	}

//	void Update () {
//
//	
//	}
		
	void OnTriggerEnter(Collider col){
		
		if(col.gameObject.tag == "PlayerAttack" ){
			
			Instantiate (enemyDamageBomb);

			isDamage    = true;

			enemyDamage.SetBool ("isDamage", isDamage);
			enemyDamage.SetBool ("isNotDamage", isNotDamage);

			damageShake.Shake ();

		}
	}


	void OnTriggerExit(Collider col_02){
		
		if(col_02.gameObject.tag == "PlayerAttack"){
			
			isDamage    = false;

			enemyDamage.SetBool ("isDamage", isDamage);

		}
	}
}
