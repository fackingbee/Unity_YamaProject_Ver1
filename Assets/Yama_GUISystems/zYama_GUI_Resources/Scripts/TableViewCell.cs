using UnityEngine;


#region コメント群
// 他のデータを表示するテーブルビューでも汎用的に利用出来るように、
// 共通の処理を実装したベースとなるジェネリッククラスを作成
// 同クラスを継承したクラスで個別の処理

// ジェネリック：抽象的なデータ型で変数を宣言しておいて、実行する際に具体的な型を指定する機能
//           ：複数のデータ型で共通の処理を実行する時に用いる
//           ：つまり、プロパティにはどんな型でも使用でき、それに応じて型が決まる
#endregion


// ViewControllerクラスを継承する
public class TableViewCell<T> : ViewController {


	// セルの内容を更新するメソッド
	public virtual void UpdateContent(T itemData){

		// 実際の処理は継承したクラスで実装する
		// ShopItemTableViewCellのセルの内容を更新するメソッドのオーバーライドなど

	}


	// セルに対応するリスト項目のインデックスを保持	
	// ShopItemTableViewControllerで使用
	public int DataIndex { get; set; }

	// セルの高さを取得、設定するプロパティ
	public float Height{

		get { return CachedRectTransform.sizeDelta.y; }

		set {
			Vector2 sizeDelta = CachedRectTransform.sizeDelta;
			sizeDelta.y = value;
			CachedRectTransform.sizeDelta = sizeDelta;
		}
	}

	// セルの上端の位置を取得、設定するプロパティ
	public Vector2 Top {
		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[1].y);
		}

		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[1].y);
		}
	}

	// セルの下端の位置を取得、設定するプロパティ
	public Vector2 Bottom{

		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[3].y);
		}

		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[3].y);

		}
	}
}
