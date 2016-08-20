


/// <summary>
/// 未使用（使うかもしれないので残しておく）
/// </summary>




using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ドロップ操作を制御する為のインターフェースを実装
public class Droppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {


	#region OnPointerEnterメソッドの実装
	[SerializeField] private Image iconImage;			// ドロップエリアに表示しているアイコン
	[SerializeField] private Color highlightedColor;	// ドロップエリアに表示しているアイコンのハイライト色
	                 private Color normalColor;			// ドロップエリアに表示しているアイコンの元の色を保持
	#endregion


	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	void Start(){

		// ドロップエリアに表示しているアイコンの元の色を保持しておく
		normalColor = iconImage.color;

	}


	// マウスカーソルが領域に入ったときに呼ばれる
	public void OnPointerEnter(PointerEventData pointerEventData){


		/// .dragging:ドラッグ中かどうかを見れるPointerEventDataの機能 ///
		/// ドラッグ中ならTrueが返る								  ///
		//Debug.Log (pointerEventData.dragging);


		// ドラッグ中だったら
		if(pointerEventData.dragging){
			
			// ドロップエリアに表示しているアイコンの色をハイライト色に変更する
			iconImage.color = highlightedColor;

		}
	}


	// マウスカーソルが領域から出たら何事もなかったかのように元に戻る
	public void OnPointerExit(PointerEventData pointerEventData){

		// ドラッグ中だったら、
		if(pointerEventData.dragging){
			
			// ドロップエリアに表示しているアイコンの色を元の色に戻す
			iconImage.color = normalColor;

		}
	}


	// ドロップした時のメソッド
	public void OnDrop(PointerEventData pointerEventData){
		
		// ドラッグしていたアイコンのImageコンポーネントを取得する
		Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();

		// ドロップエリアに表示しているアイコンのスプライトを、ドロップされたアイコンと同じスプライトに変更して…
		iconImage.sprite = droppedImage.sprite;

		// 色を元の色に戻す
		iconImage.color = normalColor;

		//iconImage.transform.localScale = new Vector3 (0.88f,0.88f,0.88f);

	}
}
