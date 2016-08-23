using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MiniJSON;
using System.Collections;

// TableViewController<T>クラスを継承
[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData>{


	public TextAsset    itemJasonData; // jsonデータを格納する
	public ShopItemData shopItem;      // 解析されたjsonデータを格納


	// インスタンスのロード時に呼ばれる【TableViewControllerにてvirtualで許可】
	protected override void Awake(){

		// ベースクラスのAwakeメソッドを呼ぶ【TableViewControllerのvirtual Awake】
		base.Awake();

		// アイコンのスプライトシートに含まれるスプライトをキャッシュしておく【何度も呼ばない、一度呼んだら使い回す】
		SpriteSheetManager.Load("IconAtlas");

	}


	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	protected override void Start(){

		// ベースクラスのStartメソッドを呼ぶ
		base.Start();

		// リスト項目のデータを読み込む
		LoadData();

		//アイテム一覧画面をナビゲーションビューに対応させる
		if(navigationView != null) {

			// ナビゲーションビューの最初のビューとして設定する
			navigationView.Push(this);

		}
	}



	// リスト項目のデータを読み込むメソッド
	private void LoadData(){

		// 変数を用意
		string iconName;
		string name;
		int    price;
		string description;
		string Type;

		// 格納されたjsonファイルを解析
		IDictionary itemDataDic = (IDictionary)Json.Deserialize(itemJasonData.text);

		// 解析されたjsonファイルは"ItemList"に格納されており、型変換して一旦Listに格納
		List<object>arrayData = (List<object>)itemDataDic["ItemList"];

		// Listを解析(一回目のループ：iconName=drink1,name=WATER,Price=100...)
		foreach(IDictionary itemVal in arrayData){

			if((string)itemVal["Type"] == "item"){

				// jsonファイルの各項目を用意した変数に格納（要型変換）
				iconName    = (string)   itemVal["iconName"];
				name        = (string)   itemVal["name"];
				price       = (int)(long)itemVal["price"];
				description = (string)   itemVal["description"];
				Type        = (string)   itemVal["Type"];

				// たった今格納された項目をshopItemにまとめて格納
				shopItem = new ShopItemData( iconName, name, price, description, Type );

				// 解析され、アイテムごとに紐づいているか一度確認
				Debug.Log ("shopItem.iconName : " + shopItem.iconName);
				Debug.Log ("shopItem.name: " + shopItem.name);
				Debug.Log ("shopItem.price : " + shopItem.price);
				Debug.Log ("shopItem.description : " + shopItem.description);

				// ShopItemData型のtableDataに配列として格納
				tableData.Add (shopItem);

			}
		}

		// 一度要素数を見ておく
		Debug.Log ("tableData.Count : " + tableData.Count);

		// 念のためランダムにデバッグ
		Debug.Log("tableData[1].iconName : " + tableData[1].iconName);
		Debug.Log("tableData[18].price : " + tableData[17].price);

		// ただ、shopItemはListで宣言していないのに、
		// 何故、tableDataをnew List<ShopItemData>()とインスタンス化しなくても動作するのかが謎


//		// 通常データはデータソースから取得しますが、ここではハードコードで定義する
//		tableData = new List<ShopItemData>() {
//			
//			new ShopItemData { iconName="drink1", name="WATER",        price=100,   description="Nothing else, just water." }, 
//			new ShopItemData { iconName="drink2", name="SODA",         price=150,   description="Sugar free and low calorie." }, 
//			new ShopItemData { iconName="drink3", name="COFFEE",       price=200,   description="How would you like your coffee?" }, 
//			new ShopItemData { iconName="drink4", name="ENERGY DRINK", price=300,   description="It will give you wings." }, 
//			new ShopItemData { iconName="drink5", name="BEER",         price=500,   description="It's a drink for grown-ups." }, 
//			new ShopItemData { iconName="drink6", name="COCKTAIL",     price=1000,  description="A cocktail made of tropical fruits." }, 
//			new ShopItemData { iconName="fruit1", name="CHERRY",       price=100,   description="Do you like cherries?" }, 
//			new ShopItemData { iconName="fruit2", name="ORANGE",       price=150,   description="It contains much vitamin C." }, 
//			new ShopItemData { iconName="fruit3", name="APPLE",        price=300,   description="Enjoy the goodness without peeling it." }, 
//			new ShopItemData { iconName="fruit4", name="BANANA",       price=400,   description="Don't slip on its peel." }, 
//			new ShopItemData { iconName="fruit5", name="GRAPE",        price=600,   description="It's not a grapefruit." }, 
//			new ShopItemData { iconName="fruit6", name="PINEAPPLE",    price=800,   description="It's not a hand granade." }, 
//			new ShopItemData { iconName="gun1",   name="MINI GUN",     price=1000,  description="A tiny concealed carry gun." }, 
//			new ShopItemData { iconName="gun2",   name="CLASSIC GUN",  price=2000,  description="The gun that was used by a pirate." }, 
//			new ShopItemData { iconName="gun3",   name="STANDARD GUN", price=4000,  description="Just a standard weapon." }, 
//			new ShopItemData { iconName="gun4",   name="REVOLVER",     price=5000,  description="It can hold a maximum of 6 bullets." }, 
//			new ShopItemData { iconName="gun5",   name="AUTO RIFLE",   price=10000, description="It can fire automatically and rapidly." }, 
//			new ShopItemData { iconName="gun6",   name="SPACE GUN",    price=20000, description="A weapon that comes from the future." }, 
//
//		};


		// スクロールさせる内容のサイズを更新する【TableViewControllerより】
		UpdateContents();

	}


	// リスト項目に対応するセルの高さを返すメソッド（金額によって高さを分ける）
	protected override float CellHeightAtIndex(int index) {

		//tableDataには18個の項目がある
		//Debug.Log ("tableData.Count :" + tableData.Count);

		//各アイテムのIndexを一旦みている18項目あるので0〜17Indexが存在（WaterはIndexが0）
		//Debug.Log ("index :" + index);

		//上記LoadData()にて、配列として格納した際に、順番に要素番号がついている【index0はWater】
		//Debug.Log ("tableData[0] :" + tableData[0].name);

		// ここは自身のゲームのUIに合わせて、微調整しなければいけない
		// 下記は完全に冗長だが、セルの高さは視認性上変えないつもりでも、
		// ランクなど価格によってなにか条件をつけるかもしれないので、現状このままにしておく。
		if(index >= 0 && index <= tableData.Count-1){

			if(tableData[index].price >= 1000){
				// 価格が1000以上のアイテムを表示するセルの高さを返す
				return 140.0f;
			}

			if(tableData[index].price >= 500){
				// 価格が500以上のアイテムを表示するセルの高さを返す
				return 140.0f;
			}
		}
		return 140.0f;
	}


	#region アイテム一覧画面をナビゲーションビューに対応させる
	// ナビゲーションビューを保持
	[SerializeField] private NavigationViewController navigationView;

	// ビューのタイトルを返す
	public override string Title { 
		get { return "ITEM"; } 
	}
	#endregion


	#region アイテム詳細画面に遷移させる処理の実装（一旦アイテム詳細に遷移する方式）
	// アイテム詳細画面のビューを保持
	[SerializeField] private ShopDetailViewController detailView;

	// セルが選択されたときに呼ばれるメソッド
	public void OnPressCell(ShopItemTableViewCell cell) {
		if(navigationView != null) {

			// 選択されたセルからアイテムのデータを取得して、アイテム詳細画面の内容を更新する
			detailView.UpdateContent(tableData[cell.DataIndex]);

			// アイテム詳細画面に遷移する
			navigationView.Push(detailView);

			//Debug.Log ("cell.DataIndex: " + cell.DataIndex);


		}
	}
	#endregion

	#region アイテム詳細画面に遷移させる処理の実装（直接決定画面に遷移する方式）
	//	// アイテム詳細画面のビューを保持
	//	[SerializeField] private ShopConfirmationViewController buyWindow;
	//
	//	// セルが選択されたときに呼ばれるメソッド
	//	public void OnPressCell(ShopItemTableViewCell cell) {
	//		if(navigationView != null) {
	//
	//			// 選択されたセルからアイテムのデータを取得して、アイテム詳細画面の内容を更新する
	//			buyWindow.UpdateContent(tableData[cell.DataIndex]);
	//
	//			// アイテム詳細画面に遷移する
	//			navigationView.Push(buyWindow);
	//
	//			//Debug.Log ("cell.DataIndex: " + cell.DataIndex);
	//
	//
	//		}
	//	}
	#endregion


}
