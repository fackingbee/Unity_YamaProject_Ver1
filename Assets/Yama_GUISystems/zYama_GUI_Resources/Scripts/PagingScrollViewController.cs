using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 今回から可読性を上げるため、#region〜#endregionを使用していく
/// </summary>

// ScrollRectを必須にする
[RequireComponent(typeof(ScrollRect))]

// ViewControllerを継承してIBeginDragHandlerインターフェースとIEndDragHandlerインターフェースを実装する
public class PagingScrollViewController : ViewController, IBeginDragHandler, IEndDragHandler{


	#region ページコントロールを制御する処理の追加

	[SerializeField] private PageControl pageControl;
	#endregion

	#region アニメーションカーブ用変数郡
	[SerializeField] private float animationDuration = 0.3f;
	[SerializeField] private float key1InTangent     = 0.0f;
	[SerializeField] private float key1OutTangent    = 0.1f;
	[SerializeField] private float key2InTangent     = 0.0f;
	[SerializeField] private float key2OutTangent    = 0.0f;
	#endregion

	#region OnBeginDrag関数、OnEndDrag関数の実装

	// 変数宣言：ScrollRectコンポーネントをキャッシュ
	private ScrollRect cachedScrollRect;

	// RectTransformのGetComponentは処理が重いので、一度取得したものをキャッシュし使い回す
	private ScrollRect CachedScrollRect{
		get{
			if(cachedScrollRect == null){
				cachedScrollRect = GetComponent<ScrollRect> ();
			}
			return cachedScrollRect;
		}
	}


	private bool           isAnimating = false;		// アニメーション中フラグ
	private Vector2        destPosition;			// 最終的なスクロール位置
	private Vector2        initialPosition;			// 自動スクロール開始時のスクロール位置
	private AnimationCurve animationCurve;			// 自動スクロールのアニメーションカーブ
	private int            prevPageIndex = 0;		// 前のページのインデックスを保持


	// ドラッグが開始された時に呼ばれる
	public void OnBeginDrag(PointerEventData eventData){

		// PointerEventData eventDataでドラッグした座標とどれくらいフリックしたかが確認出来る
		//Debug.Log ("eventData : " + eventData);

		// アニメーション中フラグをリセットする
		isAnimating = false;

	}


	// ドラッグが終了した時に呼ばれる
	public void OnEndDrag(PointerEventData eventData){

		// 取得
		GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();

		// スクロールビューの現在の動きを止める
		CachedScrollRect.StopMovement();

		// GridLayoutGroupのcellSizeとspacingから1ページの幅を算出する
		float pageWidth = -(grid.cellSize.x + grid.spacing.x);

		// つまり、ページのフレーム枠の幅と隣のページまでの距離が振れ幅
		//Debug.Log ("grid.cellSize.x : " + grid.cellSize.x); 
		//Debug.Log ("grid.spacing.x : " + grid.spacing.x);
		//Debug.Log ("pageWidth : " + pageWidth);


		// 現在のスクロール位置からフィットさせるページのインデックスを算出する
		int pageIndex = Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);

		//Debug.Log (CachedScrollRect.content.anchoredPosition.x);
		//Debug.Log ("pageIndex : " + pageIndex);


		if(pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 4){
			
			// 一定以上の速度でドラッグしていた場合、その方向に1ページ進める
			CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
			pageIndex += (int)Mathf.Sign(-eventData.delta.x);

		}

		// 先頭や末尾のページの場合、それ以上先にスクロールしないようにする
		if(pageIndex < 0){
			//Debug.Log ("pageIndex : " + pageIndex);
			//Debug.Log ("grid.transform.childCount : " + grid.transform.childCount);
			// 先頭の場合
			pageIndex = 0;

		}

		else if(pageIndex > grid.transform.childCount-1){
			// 末尾の場合
			//Debug.Log ("pageIndex : " + pageIndex);
			//Debug.Log ("grid.transform.childCount : " + grid.transform.childCount);

			pageIndex = grid.transform.childCount-1;
		}

		// 現在のページのインデックスを保持しておく（PowerProgressでもやったね）
		prevPageIndex = pageIndex;

		// 最終的なスクロール位置を算出する
		float destX  = pageIndex * pageWidth;
		destPosition = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

		// 開始時のスクロール位置を保持しておく
		initialPosition = CachedScrollRect.content.anchoredPosition;

		// 0.3秒でスムーズに変化するアニメーションカーブを作成する
		Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
		Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
		animationCurve     = new AnimationCurve(keyFrame1, keyFrame2);

		// アニメーション中フラグをセットする
		isAnimating = true;

		// ページコントロールの表示を更新する
		pageControl.SetCurrentPage(pageIndex);

	}

	#endregion



	void LateUpdate(){
		
		if(isAnimating){
			
			if(Time.time >= animationCurve.keys[animationCurve.length-1].time){
				
				// アニメーションカーブの最後のキーフレームを過ぎたら、アニメーションを終了する
				CachedScrollRect.content.anchoredPosition = destPosition;
				isAnimating = false;
				return;
			}

			// アニメーションカーブから現在のスクロール位置を算出してスクロールビューを移動させる
			Vector2 newPosition = initialPosition + (destPosition - initialPosition) * animationCurve.Evaluate(Time.time);

			CachedScrollRect.content.anchoredPosition = newPosition;

		}
	}


	private Rect currentViewRect;	// スクロールビューの矩形を保持

	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	void Start() {
		
		// 「Scroll Content」のPaddingを初期化する
		UpdateView();

		// ページ数を5に設定する
		pageControl.SetNumberOfPages(2);

		// ページコントロールの表示を初期化する
		pageControl.SetCurrentPage(0);
	
	}
		

	void Update() {
		
		if(CachedRectTransform.rect.width != currentViewRect.width || CachedRectTransform.rect.height != currentViewRect.height){
			
			// スクロールビューの幅や高さが変化したら「Scroll Content」のPaddingを更新する
			UpdateView();

		}
	}


	// 「Scroll Content」のPaddingを更新するメソッド
	private void UpdateView(){
		
		// スクロールビューの矩形を保持しておく
		currentViewRect = CachedRectTransform.rect;

		// GridLayoutGroupのcellSizeから「Scroll Content」のPaddingを算出して設定する
		GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
		int paddingH = Mathf.RoundToInt((currentViewRect.width - grid.cellSize.x) / 2.0f);
		int paddingV = Mathf.RoundToInt((currentViewRect.height - grid.cellSize.y) / 2.0f);
		grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);

	}
}
