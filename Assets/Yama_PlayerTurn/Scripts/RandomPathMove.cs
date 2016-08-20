using UnityEngine;
using System.Collections;

public class RandomPathMove : MonoBehaviour {


	// パスを配列で格納
	Vector3[] movepath;


	void Start () {
		

		// 目的地まで何回くねるか
		movepath = new Vector3[2];

		// 1番目[0]はランダムでパスを生成
		for (int i=0 ; i<1 ; ++i) {
			
			movepath[i].Set(Random.Range( -9f, 9f ),
				            Random.Range( -8f, 16f ),
				            Random.Range( -215f, -215f )

						   );
		}
			
		// 最後はTargetに着地するようにダイレクトに設定（Targetオブジェクトがある位置）/ 要素数は2だが、0、1番となるため、最後は[1]
		movepath[1].Set( 0f, 9.5f, -200f );

		// iTweenのMoveTo関数にPath数と必要時間と動き方を渡す / その後動かすオブジェクトに添付、プレハブ化
		iTween.MoveTo(gameObject,iTween.Hash("path",movepath,"time",2,"easetype",iTween.EaseType.easeOutSine));
	
	}



//	void Update () {
//	
//	}
		


	// Targetに接触すると自身を削除
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Target"){
			Destroy (gameObject);
		}
	}
}
