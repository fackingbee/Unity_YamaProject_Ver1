using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015, Jean Moreno

public class CFX_Demo_Translate : MonoBehaviour{

//	public float    speed;

//	public Vector3 rotation = Vector3.forward;

//	public Vector3 axis     = Vector3.forward;

//	public bool     gravity;

	public Vector3 dir;


	void Start (){
		
		// スピードを初期化
//		speed = 1f;

		// GameDateを経由して、ScoreHandlerから正規化されたフリックの方向を取得
//		dir = GameDate.flickDirection;

//		dir = new Vector3(Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f));

//		// 方向の決定に癖があるので、慣れるまでしばらくログを出しておく
//		Debug.Log ("dir :" + dir);
//		Debug.Log ("Position :" + transform.position);


//		//dir.Scale(rotation);
//		//this.transform.localEulerAngles = dir;

	}


	void Update (){
		
		//this.transform.Translate(axis * speed * Time.deltaTime, Space.Self);

		//左 （ScoreHandlerのOnScoreFlickの条件式と同じなら追従する…はず…）
		if (dir.x < 0 && dir.y >= -0.5f && dir.y <= 0.4f){
			
			transform.position += new Vector3 (-1f, 0f, 0f) * 2f;
			transform.Rotate (Vector3.forward, 10, Space.World);		// RotateでYを回転

		// 左上
		} else if ( dir.x < 0 && dir.y > 0.2f && dir.y <= 0.8f){
			
			transform.position += new Vector3 (-1f,1f,-1f) * 2f;
			transform.Rotate (Vector3.forward, 10, Space.World);		// RotateでYを回転

		// 真上
		} else if ( dir.x >= -0.5f && dir.x <= 0.5f  && dir.y > 0){
			
			transform.position += new Vector3 (0f,1f,-1f) * 2f;
			transform.Rotate (Vector3.forward, 10, Space.World);		// RotateでYを回転

		// 右上
		} else if ( dir.x > 0 && dir.y > 0.2f && dir.y <= 0.8f){
			
			transform.position += new Vector3 (1f,1f,-1f) * 2f;
			transform.Rotate (Vector3.forward, -10, Space.World);		// RotateでYを回転

		// 右
		} else if ( dir.x > 0 && dir.y >= -0.5f && dir.y <= 0.4f){
			
			transform.position += new Vector3 (1f,0f,0f) * 2f;
			transform.Rotate (Vector3.forward, -10, Space.World);		// RotateでYを回転

		}
	}
}
