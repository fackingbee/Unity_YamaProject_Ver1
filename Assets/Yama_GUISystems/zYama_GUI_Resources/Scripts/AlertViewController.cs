using UnityEngine;
using UnityEngine.UI;

// デリゲートとは関数を入れられる変数
// アラートビューの表示オプションを指定するためのクラスを定義
public class AlertViewOptions{
	public string        cancelButtonTitle;			// キャンセルボタンのタイトル
	public System.Action cancelButtonDelegate;		// 〃を押したときに実行されるデリゲート
	public string        okButtonTitle;				// OKボタンのタイトル
	public System.Action okButtonDelegate;			// 〃を押したときに実行されるデリゲート
}


public class AlertViewController : ViewController {	// ViewControllerクラスを継承する

	[SerializeField] Text     titleLabel;			// タイトルを表示するテキスト
	[SerializeField] Text     messageLabel;			// メッセージを表示するテキスト
	[SerializeField] Button   cancelButton;			// キャンセルボタン
	[SerializeField] Text     cancelButtonLabel;	// 〃のタイトルを表示するテキスト
	[SerializeField] Button   okButton;				// OKボタン
	[SerializeField] Text     okButtonLabel;		// 〃のタイトルを表示するテキスト

	private static GameObject prefab = null;		// アラートビューのプレハブを保持（nullで初期化）

	private System.Action     cancelButtonDelegate;	// キャンセルボタンを押したときに、実行されるデリゲートを保持
	private System.Action     okButtonDelegate;		// OKボタンを押したときに、// 実行されるデリゲートを保持


	// アラートビューを表示するstaticメソッド
	public static AlertViewController Show ( string title, string message, AlertViewOptions options = null ) {

		if( prefab == null ) {
			// プレハブを読み込む（Resourcesフォルダに格納しておく）
			prefab = Resources.Load("Alert View") as GameObject;
		}

		// プレハブをインスタンス化してアラートビューを表示する
		GameObject          obj       = Instantiate(prefab) as GameObject;

		// スクリプトを取得し、使う準備が整ったオブジェクト
		AlertViewController alertView = obj.GetComponent<AlertViewController>();

		// UpdateContentで処理された3つの情報を引っ張って来て…
		alertView.UpdateContent(title, message, options);

		// 返す
		return alertView;

	}

	// アラートビューの内容を更新するメソッド
	public void UpdateContent( string title, string message, AlertViewOptions options=null ){

		// 選択したアイテムの情報を引き継いで進入
		//Debug.Log ("UpdateContent内に入った");

		// タイトルとメッセージを設定する
		titleLabel.text   = title;
		messageLabel.text = message;

		if (options != null) {
			
			// 表示オプションが指定されている場合、オプションの内容に合わせてボタンを表示/非表示する
			cancelButton.transform.parent.gameObject.SetActive (options.cancelButtonTitle != null || options.okButtonTitle != null);
			cancelButton.gameObject.SetActive (options.cancelButtonTitle != null);
			cancelButtonLabel.text = options.cancelButtonTitle ?? "";
			cancelButtonDelegate   = options.cancelButtonDelegate;
			okButton.gameObject.SetActive (options.okButtonTitle != null);
			okButtonLabel.text = options.okButtonTitle ?? "";
			okButtonDelegate   = options.okButtonDelegate;

			//Debug.Log (options.cancelButtonTitle != null);

		} else {
			
			// 表示オプションが指定されていない場合、デフォルトのボタン表示にする
			cancelButton .gameObject.SetActive(false);
			okButton     .gameObject.SetActive(true);
			okButtonLabel.text = "OK";

		}
	}


	// アラートビューを閉じるメソッド
	public void Dismiss(){
		Destroy(gameObject);
	}


	// キャンセルボタンが押されたときに呼ばれるメソッド
	public void OnPressCancelButton(){
		if(cancelButtonDelegate != null){
			cancelButtonDelegate.Invoke();
		}
		Dismiss();
	}


	// OKボタンが押されたときに呼ばれるメソッド
	public void OnPressOKButton(){
		if(okButtonDelegate != null){
			okButtonDelegate.Invoke();
		}
		Dismiss();
	}

}
