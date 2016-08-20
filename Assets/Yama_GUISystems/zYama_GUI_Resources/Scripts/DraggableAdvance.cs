
/// <summary>
/// Box内が空でないと機能しないので、ラベルなどは独立して設置すること
/// </summary>



using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// CanvasGroupを必須にする(すでにコンポーネントが存在しているとエラーが出るので注意)
[RequireComponent(typeof(CanvasGroup))]
public class DraggableAdvance : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


	// thisで自身であると指定する
	public static DraggableAdvance dragObject;

	// どの階層にあっても、イベント中は一度CanvasのTransformに差し替えるのに必要
	public  Transform parentTransform;

	// 実際の位置とドラッグ開始位置の差分を格納
	private Vector3 tapRefPosition;

	// 開始時にコンポーネントを追加する為の変数
	private CanvasGroup canvasGroup;

	// イベント中に差し替えたcanvasのTransformを格納
	private Transform canvasTransform;

	// null置換演算子：スタート時にCanvasGroupを追加
	public CanvasGroup CanvasGroup     {get{return canvasGroup     ?? (canvasGroup     = gameObject.AddComponent<CanvasGroup>());}}

	// null置換演算子：ドラッグ中は一度Canvas直属の子要素に移動（現在は孫、ひ孫で移動しないと描画順的に表示されない可能性があるので）
	public Transform   CanvasTransform {get{return canvasTransform ?? (canvasTransform = GameObject.Find("Canvas").transform);}}



	public void OnBeginDrag(PointerEventData eventData) {

		// 実際の位置とドラッグ開始位置の差分を格納
		tapRefPosition = (Vector2)transform.position - eventData.position;

		// Staticで同じスクリプトをアタッチしているが、タッチされたオブジェクトについてだけ処理する
		dragObject = this;

		// 左オベランドはBox[]、右オベランドはCanvasになるので、親のTransformをCanvasに差し替える
		parentTransform = transform.parent;

		// 開始時に一度Canvasの子要素に移動
		transform.SetParent(CanvasTransform);

		// ドラッグ中は他のUI操作を禁止する
		CanvasGroup.blocksRaycasts = false;

		// ドラッグ中の表示は薄くする
		CanvasGroup.alpha = 0.5f;

		//Debug.Log ("tapRefPosition :" + tapRefPosition);
		//Debug.Log ("(Vector2)transform.position :" + (Vector2)transform.position);
		//Debug.Log ("eventData.position :" + eventData.position);
		//Debug.Log ("transform.parent :" + transform.parent);
		//Debug.Log ("parentTransform :" + parentTransform);

	}

	public void OnDrag(PointerEventData eventData){
		transform.position = Input.mousePosition + tapRefPosition;
		//Debug.Log ("transform.position :" + transform.position);
		//Debug.Log ("Input.mousePosition :" + Input.mousePosition);
	}

	public void OnEndDrag(PointerEventData eventData){

		//Debug.Log ("transform.parent :" + transform.parent);
		//Debug.Log ("canvasTransform :" + canvasTransform);

		if (transform.parent == canvasTransform) {
			transform.SetParent(parentTransform);
		}

		// どのUI要素でもなくす
		dragObject = null;

		// 再度UI操作を許可
		CanvasGroup.blocksRaycasts = true;

		// UIの色を通常に戻す
		CanvasGroup.alpha = 1.0f;
	}


	public void Update(){
		if (dragObject == null){
			
			// Updateが常に呼ばれているので重くない？
			//Debug.Log ("Updateの処理");

			transform.localPosition -= transform.localPosition / 3.0f;

		}
	}
}