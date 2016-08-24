using UnityEngine;
using System.Collections;
using JsonOrg;
using KiiCorp.Cloud.Storage;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Collections.Generic;

public class KiiManage : MonoBehaviour {


	private string kiiUserName;	 //Kii用(名前)
	private string kiiPassWord;	 //Kii用（パスワード）
	public  Text   statusText;	 //UI

	void Start () {
		
		//作成されたユーザがあるか判断し、なければ新規作成
		if(PlayerPrefs.HasKey("userID")){
			
			kiiUserName = PlayerPrefs.GetString("userID");	// 既存ユーザーのIDを使う
			kiiPassWord = PlayerPrefs.GetString("userPW");	// 既存ユーザーのPassを使う

		}else{
			
			kiiUserName = randomCodeGenerate(10);			// 新規ユーザーは先に名前と
			kiiPassWord = randomCodeGenerate(6);			// パスを取得

		}
			
		bool registCheck = RegistUser(kiiUserName,kiiPassWord);

		if(registCheck){
			
			//新規登録完了、Kiiへログイン
			statusText.text = "Created new data";

			bool loginCheck = loginUser(kiiUserName,kiiPassWord);

			if(loginCheck){

				//Debug.Log ("ログイン成功、Kiiに保存するデータを初期化");
				
				//ログイン成功、Kiiに保存するデータを初期化
				bool initData = kiiDataInitialize();

				if(!initData){
					initData = kiiDataInitialize();
				}

				if(!initData){

					//Debug.Log ("保存失敗でシーンを再読み込み");
					
					//保存失敗でシーンを再読み込み
					SceneManager.LoadScene ("KiiStart");

					// このオブジェクトは消去
					Destroy(this.gameObject);
				}

				//初期化成功
				if(initData){

					//Debug.Log ("初期化成功");
					
					//ローカルにIDとPWを保存しメインメニューへ
					PlayerPrefs.SetString("userID",kiiUserName);
					PlayerPrefs.SetString("userPW",kiiPassWord);

					SceneManager.LoadScene ("OrganizeMenu");

				}

			} else {
				
				//接続失敗で再読み込み
				SceneManager.LoadScene ("KiiStart");

				// このオブジェクトは消去
				Destroy(this.gameObject);

			}

		} else {

			//Debug.Log ("既に登録されており、Kiiへログイン");
			
			//すでに登録されているユーザはKiiへログイン
			statusText.text = "Loading server data";

			bool loginCheck = loginUser(kiiUserName,kiiPassWord);

			if(loginCheck){
				
				//ログイン成功 データロード
				bool loadDataCheck = loadKiiData();

				if(!loadDataCheck){
					loadDataCheck = loadKiiData();
				}

				if(!loadDataCheck){

					//Debug.Log ("読み込み失敗で再ロード");
					
					//読み込み失敗で再ロード
					SceneManager.LoadScene ("KiiStart");

					Destroy(this.gameObject);
				}

				if(loadDataCheck){
					
					//Debug.Log ("接続成功");

					//読み込み成功　シーン移動
					SceneManager.LoadScene ("OrganizeMenu");

				}

			} else {

				//Debug.Log ("接続失敗");
				
				//接続失敗で再読み込み
				SceneManager.LoadScene ("KiiStart");

				// このオブジェクトは消去
				Destroy(this.gameObject);

			}
		}
	}


	//KiiCloudへログインする
	public bool loginUser( string userName,string password ){

		Debug.Log ("userName : " + userName);
		
		//KiiUser user;

		try {
			
			//user = KiiUser.LogIn(userName,password);
			KiiUser.LogIn(userName,password);
			Debug.Log ( "Success user login : " + userName );

		} catch ( System.Exception exception ) {
			
			Debug.LogError( "Failed user login : " + userName + " : " + exception );
			//user = null;
			return false;
		}

		return true;
	}

	//入力されたユーザID、パスワードのユーザが存在しなければ新規作成（Invalid = 無効）
	public bool RegistUser( string userName,string password ) {
		
		if( !KiiUser.IsValidUserName( userName ) || !KiiUser.IsValidPassword( password ) ) {
			Debug.LogError( "Invalid user name or password : " + userName );
			return false;
		}

		KiiUser.Builder builder = KiiUser.BuilderWithName(userName);
		KiiUser _User           = builder.Build ();

		// try内でエラーが起きたらcatchへ飛ぶ
		try {
			
			_User.Register( password );
			Debug.Log ( "Success user regist : " + userName );

			// exceptionにエラーの内容が入り、falseを返す
		} catch ( System.Exception exception ){
			
			Debug.Log( "Failed user regist : " + userName + " : " + exception );
			_User = null;
			return false;

		}

		// 問題なければtrueを返す
		return true;
	}


	//KiiCloud上のデータ保存枠を初期化
	bool kiiDataInitialize() {
		
		//ユーザーバケットを定義
		KiiBucket userBucket   = KiiUser.CurrentUser.Bucket("myBasicData");
		KiiObject basicDataObj = userBucket.NewKiiObject();

		//保存するデータを定義
		basicDataObj["lv"]    = 1;
		basicDataObj["exp"]   = 0;
		basicDataObj["open2"] = false;
		basicDataObj["open3"] = false;
		basicDataObj["wp"]    = 0;



		//オブジェクトを保存
		try {
			
			basicDataObj.Save();

		} catch (System.Exception e) {
			
			Debug.LogError(e);
			return false;

		}
		return true;
	}



//	void Update () {
//	
//	}

	//保存されているデータを読み込み
	bool loadKiiData(){
		
		KiiQuery allQuery = new KiiQuery();

		try {
			
			//検索条件を指定
			KiiQueryResult<KiiObject> result = KiiUser.CurrentUser.Bucket("myBasicData").Query(allQuery);

			foreach(KiiObject obj in result){
				
				//データを読み込み
				variableManage.currentLv     = (int )obj["lv"];
				variableManage.currentExp    = (int )obj["exp"];
				variableManage.openMachine02 = (bool)obj["open2"];
				variableManage.openMachine03 = (bool)obj["open3"];
				variableManage.myWP          = (int )obj["wp"];

				// nextExpを算出してvariableManageにセット
				variableManage.nextExp = variableManage.currentLv * 100;




			}

		} catch (System.Exception e) {
			
			Debug.Log(e);
			return false;

		}
		return true;
	}

	//現在のデータを保存
	public static bool saveKiiData(){
		
		KiiQuery allQuery = new KiiQuery();

		try {
			
			//検索条件を指定
			KiiQueryResult<KiiObject> result = KiiUser.CurrentUser.Bucket("myBasicData").Query(allQuery);

			foreach (KiiObject obj in result){
				
				//データを保存
				obj["lv"]    = variableManage.currentLv;
				obj["exp"]   = variableManage.currentExp;
				obj["open2"] = variableManage.openMachine02;
				obj["open3"] = variableManage.openMachine03;
				obj["wp"]    = variableManage.myWP;

				// Set a JSON array field
				JsonArray jsonArray = new JsonArray();
				JsonObject arrayElement1 = new JsonObject("{\"Name\": \"Alice\", \"age\": 30}");
				jsonArray.Put(arrayElement1);
				JsonObject arrayElement2 = new JsonObject("{\"Name\": \"Bob\", \"age\": 28}");
				jsonArray.Put(arrayElement2);
				obj["myArray"] = jsonArray;

				obj.Save();

			}
		} catch (System.Exception e) {
			
			Debug.Log(e);
			return false;

		}

		return true;

	}



	//ランダムの文字列を作成する（引数10回を受け取る）
	string randomCodeGenerate(int codeLength){

		// 全ての英数字を保持
		string allCode    = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";

		// 出力変数
		string outPutCode = "";

		// 引数codeLength（10）を受け取ってその回数分、繰り返す
		for(int i = 0; i<codeLength; i++){

			// 全ての英数字からランダムで格納
			int rndTmp = Random.Range(0,allCode.Length);

			// 何文字目かを先頭に1文字づつ10回抜き出す
			outPutCode += allCode.Substring(rndTmp,1);

		}
		// 10文字を返す
		return outPutCode;
	}

}
