using UnityEngine;
using UnityEngine.UI;


#region コメント群
// テーブルビューで扱うリスト項目のデータクラスとして、ShopItemDateクラスもここで定義
// 継承元であるTableViewCell<T>クラスのTにShopItemDateクラスを指定して、
// オーバーライドするUpdateContentメソッドでShopItemDateクラスの引数を扱えるようにする
#endregion

// リスト項目のデータクラスを定義
public class ShopItemData {
	public string  iconName;	// アイコン名
	public string  name;		// アイテム名
	public int     price;		// 価格
	public string  description;	// 説明
}

// TableViewCell<T>クラスを継承する
public class ShopItemTableViewCell : TableViewCell<ShopItemData> {

	[SerializeField] private Image iconImage;	// アイコンを表示するイメージ
	[SerializeField] private Text  nameLabel;	// アイテム名を表示するテキスト
	[SerializeField] private Text  priceLabel;	// 価格を表示するテキスト

	// セルの内容を更新するメソッドのオーバーライド
	public override void UpdateContent(ShopItemData itemData) {

		// アイテム名を表示
		nameLabel.text  = itemData.name;

		// 価格を表示
		priceLabel.text = itemData.price.ToString();

		// スプライトシート名とスプライト名を指定してアイコンのスプライトを変更する
		iconImage.sprite = SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);

	}
}
