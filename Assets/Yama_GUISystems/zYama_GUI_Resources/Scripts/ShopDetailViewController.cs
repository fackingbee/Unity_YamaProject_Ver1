using UnityEngine;
using UnityEngine.UI;

public class ShopDetailViewController : ViewController	// ViewControllerクラスを継承
{
	// ナビゲーションビューを保持
	[SerializeField] private NavigationViewController navigationView;

	[SerializeField] private Image        iconImage;			// アイテムのアイコンを表示するイメージ
	[SerializeField] private Text         nameLabel;			// アイテム名を表示するテキスト
	[SerializeField] private Text         descriptionLabel;		// 説明を表示するテキスト
	[SerializeField] private Text         priceLabel;			// 価格を表示するテキスト
	                 private ShopItemData itemData;				// アイテムのデータを保持


	// ビューのタイトルを返す
	public override string Title {
		get { return (itemData != null)? itemData.name: ""; } 
	}


	// アイテム詳細画面の内容を更新するメソッド
	public void UpdateContent(ShopItemData itemData) {
		
		// アイテムのデータを保持しておく
		this.itemData = itemData;

		iconImage.sprite      = SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);
		nameLabel.text        = itemData.name;
		priceLabel.text       = itemData.price.ToString();
		descriptionLabel.text = itemData.description;

		//Debug.Log ("iconImage.sprite : " + iconImage.sprite);
	}


	#region 確認画面へ遷移させる処理の実装
	// 確認画面のビューを保持
	[SerializeField] private ShopConfirmationViewController confirmationView;

	// 「BUY」ボタンが押されたときに呼ばれるメソッド
	public void OnPressBuyButton(){
		
		// 確認画面の内容を更新する
		confirmationView.UpdateContent(itemData);

		// 確認画面に遷移する
		navigationView.Push(confirmationView);
	}
	#endregion

}
