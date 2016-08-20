using UnityEngine;
using System.Collections;

public class PlayerAttackCreate : MonoBehaviour {

	public  GameObject[] playerAttackArray;		//プレハブを配列格納
	private Vector3      createAttackVector;	// ランダム生成座標用
	private int          attackIndex;			// 要素番号格納
	public  float        attackSeconds;

	void Start () {

		attackSeconds = 10f;
		//StartSpawn ();
	
	}


	void Update () {

		// プレハブ要素番号をランダムで取得
		attackIndex = Random.Range(0, playerAttackArray.Length);

		// 座標ランダムに生成
		createAttackVector = new Vector3(
			
			Random.Range(-0.5f,    0.5f  ), 
			Random.Range(-5.33f,   5.0f ), 
			Random.Range(-232.5f, -232.5f)

		);
	}

	// 繰り返し生成処理
	IEnumerator SpawnAttack(){
		
		// 1フレーム待つ（これがないと最初に原点に生成されてしまう）
		yield return null;

		// プレハブをランダムで生成（playerAttack単体生成から変更）/ 後に回転させたい
		while(true){
			
			Instantiate ( playerAttackArray[attackIndex], createAttackVector, Quaternion.identity );
			//yield return new WaitForSeconds (0.5f / ComboManager.perfectCount);

			yield return new WaitForSeconds (attackSeconds / ComboManager.perfectCount);

		}
	}


	// コルーチンの開始
	public void StartSpawn(){
		StartCoroutine ("SpawnAttack");
	}


	// コルーチンの停止
	public void StopSpawn(){
		StopCoroutine ("SpawnAttack");
	}


}
