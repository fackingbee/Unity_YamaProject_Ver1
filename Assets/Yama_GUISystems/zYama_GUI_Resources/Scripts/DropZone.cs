using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler {


//	void Start(){
//		
//	}

//	void Update(){
//
//	}


	public GameObject Item {
		get {
			// ドロップエリアに既にオブジェクトがある場合（つまりchildCount = 1）
			if (transform.childCount > 0){
				
				// ドラッグ中のオブジェクトを反映させる
				return transform.GetChild(0).gameObject;
			}
			// ドロップ先が空の場合はそのままドラッグ中のオブジェクトを返す
			return null;
		}
	}


	public void OnDrop(PointerEventData eventData) {

		//Debug.Log ("ドラッグ中のタグ : " + DraggableAdvance.dragObject.tag);
		//Debug.Log ("ドロップゾーンのタグ : " + gameObject.tag);

		// ドラッグ中のtagがドロップゾーンのタグと一致するか
		if(gameObject.tag == DraggableAdvance.dragObject.tag){

			// ドロップ先が空の場合
			if (Item == null){
				
				// ドラッグ中のオブジェクトをそのままセット
				DraggableAdvance.dragObject.transform.SetParent(transform);


			// ドロップ先に既にオブジェクトがある場合は配置交換 
			} else {
				
				// ドラッグが開始された元のBoxに既にあるオブジェクトを移動
				Item.transform.SetParent(DraggableAdvance.dragObject.parentTransform);

				// ドロップ先のBoxにドラッグ中のオブジェクトを配置
				DraggableAdvance.dragObject.transform.SetParent(transform);

			}
		}
	}
}