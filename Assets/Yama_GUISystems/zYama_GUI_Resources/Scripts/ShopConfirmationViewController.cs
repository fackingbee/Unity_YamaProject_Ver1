using UnityEngine;
using UnityEngine.UI;

public class ShopConfirmationViewController : ViewController {
	
	[SerializeField] private Text messageLabel;	// メッセージを表示するテキスト

	// ビューのタイトルを返す
	public override string Title { get { return "購入"; } }

	// 確認画面の内容を更新するメソッド
	public void UpdateContent(ShopItemData itemData){
		
		messageLabel.text = string.Format("Buy {0} for {1} coins?", 
										  itemData.name, 
										  itemData.price.ToString()
										 );
	}

	#region アラートビューを表示する処理の追加
	// 「CONFIRM」ボタンが押されたときに呼ばれるメソッド
	public void OnPressConfirmButton() {
		
		string title = "ARE YOU SURE?";

//		string title = "本当によろしいですか？";

		string message = messageLabel.text;

		// アラートビューを表示する
		AlertViewController.Show(title, message, new AlertViewOptions {
			
			// キャンセルボタンのタイトルと押されたときに実行されるデリゲートを設定
			cancelButtonTitle = "CANCEL", cancelButtonDelegate = ()=>{
				Debug.Log("Cancelled.");

//			cancelButtonTitle = "キャンセル", cancelButtonDelegate = ()=>{
//				Debug.Log("キャンセルした");
//
			}, 

			// OKボタンのタイトルと押されたときに実行されるデリゲートを設定
			okButtonTitle = "BUY", okButtonDelegate = ()=>{
				Debug.Log("Bought.");

//			okButtonTitle = "購入", okButtonDelegate = ()=>{
//				Debug.Log("購入した");

			}, 
		});
	}
	#endregion

}
