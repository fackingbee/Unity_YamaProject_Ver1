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
	public string  Type;        // ショップのタイプ（道具、武具、etc...）

	// ShopItemDataのコンストラクタ化
	// この時点でハードコードで直接『new List<ShopItemData>() 』をインスタンス化することは出来ない。
	public ShopItemData ( string iconName, string name, int price, string description, string Type ){

		this.iconName    = iconName;
		this.name        = name;
		this.price       = price;
		this.description = description;
		this.Type        = Type;

	}

/// ちなみに引数の数を変えると、同じ名前のコンストラクタでも勝手に区別してくれるので
//	public ShopItemData ( string iconName, string name, int price, string description, string Type, int number ){
//
//		this.iconName    = iconName;
//		this.name        = name;
//		this.price       = price;
//		this.description = description;
//		this.Type        = Type;
//
//	}
///などとして、武具でShopItemDataを呼んでもよさそう

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
