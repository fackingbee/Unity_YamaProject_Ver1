using UnityEngine;
using System.Collections;

public class Shadow2 : MonoBehaviour {

	// Player&Enemy用影エフェクト（基本的にShadow.csと同じ）
	public bool    shadowEnabled    = true;
	public bool    updateEnabled    = true;
	public Vector3 offsetPosition   = new Vector3 (-0.2f, 0.0f, 0.1f);
	public string  sortingLayerName = "Water";
	public int     sortingOrder     = 0;
	public Color   ShadowColor      = new Color (0.0f, 0.0f, 0.0f, 0.5f);
	SpriteRenderer spriteSrc;
	SpriteRenderer spriteCopy;


	void Start () {

		spriteSrc          = GetComponent<SpriteRenderer> ();
		GameObject goEmpty = new GameObject ("Shadow2");

		// 子であるShadow2のレイヤー名を変更する
		goEmpty.layer      = LayerMask.NameToLayer("Enemy");
		spriteCopy         = goEmpty.AddComponent<SpriteRenderer> ();
		spriteCopy         = goEmpty.GetComponent<SpriteRenderer> ();

		goEmpty.transform.parent     = transform;
		goEmpty.transform.localScale = Vector3.one * 1.08f;

//		spriteCopy.tag               = "Shadow";
		spriteCopy.sortingLayerName  = sortingLayerName;
		spriteCopy.sortingOrder      = sortingOrder;
		spriteCopy.color             = ShadowColor;

		UpdateShadow ();

	}

	// Update is called once per frame
	void Update () {
		if(updateEnabled){
			UpdateShadow ();
		}
	}
		
	void UpdateShadow(){
		spriteCopy.transform.position = spriteSrc.transform.position;
		spriteCopy.transform.Translate (-0.2f,0.0f,0.1f,Space.Self);
		spriteCopy.sprite = spriteSrc.sprite;
	}
}