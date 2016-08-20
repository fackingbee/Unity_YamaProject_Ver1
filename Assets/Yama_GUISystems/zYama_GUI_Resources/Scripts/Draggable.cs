


/// <summary>
/// 未使用（使うかもしれないので残しておく）
/// </summary>



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// IBeginDragHandler： ドラッグ開始時に発生
/// IDragHandler     ： ドラッグ中に発生
/// IEndDragHandler  ： ドラッグ終了時に発生
/// </summary>

[RequireComponent(typeof(Image))]
public class Draggable :
	MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
// それぞれ『OnDrag』『OnBeginDrag』『OnEndDrag』メソッドが必要になる


	/// <summary>
	/// ドラッグ操作を制御する為のインターフェースを実装
	/// OnDrag、OnEndDragメソッドを記載して初めて機能する（なければコンパイルエラー）
	/// なお、タッチ操作ではドラッグ中のアイコンが指に隠れて見えないので
	/// </summary>


	#region ドラッグ中アイコンの為の変数を宣言
	[SerializeField]
	//調整	// ドラッグ中のアイコンのオフセット格納変数
	private Vector2       draggingOffset = new Vector2(0.0f, 40.0f);

	// ドラッグ中のアイコンのゲームオブジェクトを保持する為の変数
	private GameObject    draggingObject;

	// カンバスのRectTransformを保持する為の変数
	private RectTransform canvasRectTransform;
	#endregion


	// ドラッグ中のアイコンの位置を設定するメソッドの実装
	private void UpdateDraggingObjectPos(PointerEventData pointerEventData){

		if(draggingObject != null){

			// ドラッグ中のアイコンのスクリーン座標を算出（タッチ位置 + Y座標を40.0f分上に）
			Vector3 screenPos = pointerEventData.position + draggingOffset;

//※			// スクリーン座標をワールド座標に変換
//※			// pressEventCamera：最後にOnPointerPressイベントに関連付けられたカメラ
			Camera camera = pointerEventData.pressEventCamera;

			// 宣言
			Vector3 newPos;

//※			// UI座標からワールド座標への変換の構文
//※			// メソッドの引数定義にoutを指定することで、その引数を出力用の変数として宣言することができる
			if(RectTransformUtility.ScreenPointToWorldPointInRectangle(
				canvasRectTransform, screenPos,camera, out newPos)){
				
				// ドラッグ中のアイコンの位置をワールド座標に変換、設定する
				draggingObject.transform.position = newPos;
				draggingObject.transform.rotation = canvasRectTransform.rotation;

			}
		}
	}


	// OnBeginDragメソッドの実装 ： ドラッグ開始時に発生
	public void OnBeginDrag(PointerEventData pointerEventData){

		if(draggingObject != null){
			Destroy (draggingObject);
		}

		// 元のアイコンのImageコンポーネントを取得
		Image sourceImage = GetComponent<Image>();

		// ここでタグも見れる
		//Debug.Log (sourceImage.tag);

		// ドラッグ中のアイコンのゲームオブジェクトを作成
		draggingObject    = new GameObject ("Dragging Object");

		//Debug.Log (draggingObject.tag);

		// 元のアイコンのカンバスの子要素にして、最前面に表示
		draggingObject.transform.SetParent(sourceImage.canvas.transform);

		// 順番の入れ替えでよく見る
		draggingObject.transform.SetAsLastSibling();

		// スケールを調整
		draggingObject.transform.localScale = Vector3.one;



		// Canvas GroupコンポーネントのBlock Raycastsプロパティを使って、レイキャストがブロックされないようにする
		// レイキャストがブロックされないということは、UIとの衝突判定を検出し、UIを操作でき、
		// 奥にあるオブジェクトにはレイが届かないので操作できないということ？
		CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();


		// ここはさすがに現段階の自分の技量ではわからないところ
		// レイキャストをブロックしないので、衝突判定がなされず、UI要素に対する操作も不可
		// つまり、生成された"Dragging Object"は操作できないということ？
		canvasGroup.blocksRaycasts = false;



		// ドラッグ中のアイコンのゲームオブジェクトにImageコンポーネントをアタッチする
		Image draggingImage = draggingObject.AddComponent<Image>();

		// 元のアイコンと同じアピアランスを設定する
		draggingImage.sprite                  = sourceImage.sprite;
		draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
		draggingImage.color                   = sourceImage.color;
		draggingImage.material                = sourceImage.material;

		// カンバスのRect Transformを保持しておく
		canvasRectTransform = draggingImage.canvas.transform as RectTransform;

		// ドラッグ中のアイコンの位置を更新する
		UpdateDraggingObjectPos(pointerEventData);

		// ドラッグ開始時につかんだポジションと方向が見て取れる
		//Debug.Log (pointerEventData);

	}


	// OnDragメソッドの実装 : ドラッグ中は位置を更新し続ける
	public void OnDrag(PointerEventData pointerEventData){
		UpdateDraggingObjectPos(pointerEventData);
	}

	// OnEndDragメソッドの実装 : ドラッグが終わると生成したdraggingObjectは不要
	public void OnEndDrag(PointerEventData pointerEventData){
		Destroy(draggingObject);
	}

}
