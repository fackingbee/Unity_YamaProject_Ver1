using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


// ViewControllerクラスを継承
[RequireComponent(typeof(ScrollRect))]
public class TableViewController<T> : ViewController{

	/// <summary>
	/// このスクリプトはShopItemTableViewControllerに継承されて、具体的に内容はそちらに記載（具体的）
	/// ここでは、その定義関数を作成（抽象的）
	/// ShopItemTableViewControllerは長いので、以下『SITVC』と略す
	/// </summary>


	#region グローバル変数群

	// リスト項目のデータを保持【SITVCにて具体的内容を記載】
	protected List<T> tableData = new List<T>();

	// スクロールさせる内容のパディング
	[SerializeField] private RectOffset padding;

	// 各セルの間隔
	[SerializeField] private float spacingHeight = 4.0f;

	// Scroll Rectコンポーネントをキャッシュ（するためのプロパティを先にセット）
	private ScrollRect cachedScrollRect;

	// 取得してプロパティセット
	public ScrollRect CachedScrollRect{
		get {
			if(cachedScrollRect == null) { 
				cachedScrollRect = GetComponent<ScrollRect>(); }
			return cachedScrollRect;
		}
	}

	// コピー元のセル
	[SerializeField] private GameObject cellBase;

	// セルを保持
	private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();

	// visibleRectのパディング
	[SerializeField] private RectOffset visibleRectPadding;

	// リスト項目をセルとして表示する範囲を示す矩形【このままだと初期化されないみたいで黄色警告表示】
	//private Rect visibleRect;

	// こうしてみたが、問題が出れば元に戻す【デバッグしたら初期値がオール0なので、多分同じ事】
	private Rect visibleRect = new Rect(0,0,0,0);

	// 前回のスクロール位置を保持
	private Vector2 prevScrollPos;

	#endregion



	// インスタンスのロード時に呼ばれる
	protected virtual void Awake(){

		// なぜか現状ここは空っぽ。後に必要になる？
		// 後に分かった事だが、SITVCでオーバーライドする為にここでvirtualで定義しているのだと思う
		// これが継承の機能とその構文だと推察	

	}


	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	protected virtual void Start() {

		// コピー元のセルは非アクティブにしておく
		cellBase.SetActive(false);

		// Scroll RectコンポーネントのOn Value Changedイベントのイベントリスナーを設定する
		CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);

	}
		


	// リスト項目に対応するセルの高さを返すメソッド
	protected virtual float CellHeightAtIndex(int index){

		// ここでいうIndexは各アイテム項目につけた0~17番のこと（WaterはIndexが0）
		// 下記のメソッド『UpdateCellForIndex』でIndexを付けていく
		// SITVCのLoadData()にて、tableDataに格納した配列の要素番号が引数に渡される
		// waterの場合はindex = 0
		//Waterの場合、高さ128.0fが渡されている
		// 実際の値を返す処理は継承したクラスで実装する

		return 0.0f;

	}



	// スクロールさせる内容全体の高さを更新するメソッド
	protected void UpdateContentSize(){

		// スクロールさせる内容全体の高さを算出する
		float contentHeight = 0.0f;

		for(int i=0; i<tableData.Count; i++){

			// Waterの場合は（0）で、128.0fが渡されている
			contentHeight += CellHeightAtIndex(i);

			if(i > 0) { 
				// セルの間隔はグローバルで初期化【自身のゲーム内では要微調整】
				contentHeight += spacingHeight; 
			}
		}

		// スクロールさせる内容の高さを設定する
		Vector2 sizeDelta   = CachedScrollRect.content.sizeDelta;
		        sizeDelta.y = padding.top + contentHeight + padding.bottom;

		CachedScrollRect.content.sizeDelta = sizeDelta;
	}
		

	#region セルを作成するメソッドとセルの内容を更新するメソッドの実装

	// セルを作成するメソッド
	private TableViewCell<T> CreateCellForIndex(int index){

		// コピー元のセルから新しいセルを作成する
		GameObject obj = Instantiate(cellBase) as GameObject;

		// 元となるセルは非アクティブにしておくのでクローンはアクティブにする
		obj.SetActive(true);

		// TableViewCell.csのプロパティを取得
		TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

		// 親要素の付け替えをおこなうとスケールやサイズが失われるため、変数に保持しておく
		Vector3 scale     = cell.transform.localScale;
		Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
		Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
		Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

		cell.transform.SetParent(cellBase.transform.parent);

		// セルのスケールやサイズを設定する
		cell.transform.localScale          = scale;
		cell.CachedRectTransform.sizeDelta = sizeDelta;
		cell.CachedRectTransform.offsetMin = offsetMin;
		cell.CachedRectTransform.offsetMax = offsetMax;

		#region Debug.Log群
//		Debug.Log ("cell.CachedRectTransform.offsetMin :" + cell.CachedRectTransform.offsetMin);
//		Debug.Log ("cell.CachedRectTransform.offsetMax :" + cell.CachedRectTransform.offsetMax);
//		Debug.Log ("cell.CachedRectTransform.sizeDelta :" + cell.CachedRectTransform.sizeDelta);
		#endregion

		// 指定したインデックスのリスト項目に対応するセルとして内容を更新する
		UpdateCellForIndex(cell, index);

		cells.AddLast(cell);

		return cell;
	}


	// セルの内容を更新するメソッド
	private void UpdateCellForIndex(TableViewCell<T> cell, int index) {

		// ここでのIndexは画面に表示されているセルの分だけ【再生時はIndex0〜5】
		//Debug.Log ("index :" + index);

		// セルに対応するリスト項目のインデックスを設定する【waterのindexは0でそれをcell.DataIndexに代入】
		cell.DataIndex = index;

		// セルの項目数（この場合は18個）を超えたら、入らない
		if(cell.DataIndex >= 0 && cell.DataIndex <= tableData.Count-1){

			// セルに対応するリスト項目があれば、セルをアクティブにして…高さを設定する
			cell.gameObject.SetActive(true);

			// 内容を更新し…
			cell.UpdateContent(tableData[cell.DataIndex]);

			// 高さを設定する。
			cell.Height = CellHeightAtIndex(cell.DataIndex);

		} else {

			// セルに対応するリスト項目がなかったら、セルを非アクティブにして表示しない
			cell.gameObject.SetActive(false);

		}
	}
	#endregion



	#region visibleRectの定義とvisibleRectを更新するメソッドの実装

	// visibleRectを更新するためのメソッド
	private void UpdateVisibleRect() {

		// visibleRectの位置はスクロールさせる内容の基準点からの相対位置
		visibleRect.x =  CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
		visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

		// visibleRectのサイズはスクロールビューのサイズ + パディング
		visibleRect.width  = CachedRectTransform.rect.width  + visibleRectPadding.left + visibleRectPadding.right;
		visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top  + visibleRectPadding.bottom;

		#region Debug.Log群

		//Debug.Log ("visibleRect.width :" + visibleRect.width);

		//Debug.Log ("visibleRect.x :" + visibleRect.x);

		//visibleRectPaddingはLeft,Right,Top,Bottom、それぞれに16,16,16,16が入っている
		//Debug.Log ("visibleRectPadding :" + visibleRectPadding);

		//CanvasのWidthがそのまま、表示可能幅
		//Debug.Log ("CachedRectTransform.rect.width :" + CachedRectTransform.rect.width);

		//CanvasのRectのHeightからTableViewオブジェクトのTop88を差し引いた実際に表示させる範囲を算出
		//Debug.Log ("CachedRectTransform.rect.height :" + CachedRectTransform.rect.height);

		//Debug.Log ("visibleRect.x :" + visibleRect.x);
		//Debug.Log ("visibleRect.y :" + visibleRect.y);
		//Debug.Log ("visibleRect.width :" + visibleRect.width);
		//Debug.Log ("visibleRect.height :" + visibleRect.height);
		#endregion

		// 要するに表示可能領域はセルとその左右上下の余白分を足したもの

	}

	#endregion




	#region テーブルビューの表示内容を更新する処理の実装
	protected void UpdateContents(){

		// スクロールさせる内容のサイズを更新する
		UpdateContentSize();

		// visibleRectを更新する
		UpdateVisibleRect();


		// セルが1つもない場合、visibleRectの範囲に入る最初のリスト項目を探して、それに対応するセルを作成する
		if(cells.Count < 1){

			// 最初表示領域内の右端と上端からどれくらいの距離に作るか【現在は右から3.0f、上から-16.0fの位置に生成】
			Vector2 cellTop = new Vector2( 3.0f, (-padding.top - 2.0f) );

			for(int i=0; i<tableData.Count; i++){

				float cellHeight = CellHeightAtIndex(i);

				Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);

				if((cellTop.y <= visibleRect.y && 
					cellTop.y >= visibleRect.y - visibleRect.height) || 
					(cellBottom.y <= visibleRect.y && 
						cellBottom.y >= visibleRect.y - visibleRect.height))
				{
					TableViewCell<T> cell = CreateCellForIndex(i);
					cell.Top = cellTop;
					break;
				}
				cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();

		} else {

			// すでにセルがある場合、先頭のセルから順に対応するリスト項目の
			// インデックスを設定し直し、位置と内容を更新する
			LinkedListNode<TableViewCell<T>> node = cells.First;


			UpdateCellForIndex(node.Value, node.Value.DataIndex);
			node = node.Next;

			while(node != null) {

				UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);

				node.Value.Top = node.Previous.Value.Bottom + new Vector2(0.0f, -spacingHeight);

				node = node.Next;

			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();

		}
	}




	// visibleRectの範囲内に表示される分のセルを作成するメソッド
	private void FillVisibleRectWithCells() {

		// セルがなければ何もしない
		if(cells.Count < 1) { 
			return; 
		}

		// 表示されている最後のセルに対応するリスト項目の次のリスト項目があり、         【Beerの次にはCOCKTAILがある】
		// かつ、そのセルがvisibleRectの範囲内に入るようであれば、対応するセルを作成する【スクロールしたら範囲内に入る】

		// まずは関数内変数宣言【表示されうる一番下のセル？】
		TableViewCell<T> lastCell = cells.Last.Value;

		// 表示されている最後のセルの次のセルのIndex
		int nextCellDataIndex = lastCell.DataIndex + 1;

		// 最後のセルから下端余白分
		Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

		// 全項目数より少ない要素番号、尚且つ、次に表示されるべきセルの上端が表示可能領域に入ったら
		while(nextCellDataIndex < tableData.Count && nextCellTop.y >= visibleRect.y - visibleRect.height) {

			TableViewCell<T> cell = CreateCellForIndex(nextCellDataIndex);

			cell.Top          = nextCellTop;
			lastCell          = cell;
			nextCellDataIndex = lastCell.DataIndex + 1;
			nextCellTop       = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

		}
	}

	#endregion





	#region セルを再利用する処理の実装


	// スクロールビューがスクロールされたときに呼ばれる
	public void OnScrollPosChanged(Vector2 scrollPos) {

		// visibleRectを更新する
		UpdateVisibleRect();

		// スクロールした方向によって、セルを再利用して表示を更新する【三項演算子】
		ReuseCells((scrollPos.y < prevScrollPos.y)? 1: -1);

		// 理解の為置き換えておく
		//		if (scrollPos.y < prevScrollPos.y) {
		//			ReuseCells (1);
		//		} else {
		//			ReuseCells (-1);
		//		}

		// この次の処理の際に、現在のスクロール位置を前のスクロール位置として保持
		prevScrollPos = scrollPos;

	}



	// セルを再利用して表示を更新するメソッド
	private void ReuseCells(int scrollDirection) {

		//スクロールの方向が上か下かの正規化値として1か-1を使用
		//Debug.Log ("scrollDirection:" + scrollDirection);

		// セルがない
		if(cells.Count < 1){
			return;
		}


		if( scrollDirection > 0 ) {

			// 上にスクロールしている場合、visibleRectの範囲より上にあるセルを、順に下に移動して内容を更新する
			TableViewCell<T> firstCell = cells.First.Value;

			//例えば、クローン生成されたセルの下端はセルの高さと上の余白分の合計
			//Debug.Log ("cells.First.Value.Bottom.y :" + cells.First.Value.Bottom.y);

			// 表示可能領域
			//Debug.Log ("visibleRect.y :" + visibleRect.y);

			while( firstCell.Bottom.y > visibleRect.y ){

				TableViewCell<T> lastCell = cells.Last.Value;
				UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
				firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);


				// この段階でやっと分かった事
				// 作られたセルの一番上Waterが消えた時点で、Cherryが作られる
				// そのCherryがWaterの変わりで、waterのvalueを引き継ぐ
				// つまりまず最初に要素数6でセルが作成された場合
				// 0 → 1 → 2 → 3 → 4 → 5 （初期段階）
				// 1 → 2 → 3 → 4 → 5 → 0 （Waterが消えてCherryが作られた時）
				// 2 → 3 → 4 → 5 → 0 → 1 （以下同様）
				// を繰り返す


				// 次に表示可能領域内に入ってくるセルは一番最初のセルで、それが配列の最後に追加
				cells.AddLast(firstCell);

				// スクロールして見えなくなったセルは削除
				cells.RemoveFirst();

				// WaterからSodaにトップがかわったとき、firstCellがSodaであると更新（×これが誤り）
				// 正しくはSodaの要素番号は2番目のままで、
				// 新しく作られた一番下のセルが、今しがた消えた一番目のセルであり
				// firstCellとして追加され、cells.First.Valueプロパティをセットされる（これがcherry）
				firstCell = cells.First.Value;

			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();


		} else if( scrollDirection < 0 ) {

			// 下にスクロールしている場合、visibleRectの範囲より下にあるセルを、順に上に移動して内容を更新する
			TableViewCell<T> lastCell = cells.Last.Value;

			while( lastCell.Top.y < visibleRect.y - visibleRect.height ){

				TableViewCell<T> firstCell = cells.First.Value;
				UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
				lastCell.Bottom = firstCell.Top + new Vector2(0.0f, spacingHeight);

				cells.AddFirst(lastCell);
				cells.RemoveLast();
				lastCell = cells.Last.Value;
			}
		}
	}

	#endregion


}
