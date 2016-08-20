using UnityEngine;
using System.Collections;

public class SpriteShadow : MonoBehaviour {


	//// スコアの擬似的影エフェクトとして、ここまでずっと「Shadow.cs」というスクリプトを使用して来た    		   ////
	//// しかし、どうやらある一定以上のバージョンで、標準でShadowエフェクトが実装された模様            		   ////
	//// それにより、公式で『Shadow』という名前空間が存在し始めた 								  		   ////
	//// では何故これまでぶつからなかったかというと、恐らくusingにUIを使用していなかったからだと思われる		   ////
	//// よって、この段階で『SpriteShadow』という名前に変更して新たにスクリプトを作成し、スコアにアタッチし直した  ////
	//// 貼り忘れている部分がありそうで今後エラーの恐れがあるので、もしもの時の為に記載しておく 		           ////
	//// 尚、Character関連はサイズの違いから『Shadow2』というスクリプトで分けていたのでそちらはそのままにしておく ////


	// MusicBar用影エフェクト
	public bool    shadowEnabled    = true;
	public bool    updateEnabled    = false;
	public Vector3 offsetPosition   = new Vector3 (-0.2f, 0.0f, 0.1f);
	public string  sortingLayerName = "UI";
	public int     sortingOrder     = 0;
	public Color   ShadowColor      = new Color (0.0f, 0.0f, 0.0f, 0.5f);

	SpriteRenderer spriteSrc;
	SpriteRenderer spriteCopy;


	// Use this for initialization
	void Start () {
		spriteSrc = GetComponent<SpriteRenderer> ();

		// 影をSpriteRendererとして生成
		GameObject goEmpty = new GameObject ("Shadow");
		spriteCopy         = goEmpty.AddComponent<SpriteRenderer> ();
		spriteCopy         = goEmpty.GetComponent<SpriteRenderer> ();

		// "Shadow"を子に設定
		goEmpty.transform.parent     = transform;

		// 影の大きさ
		goEmpty.transform.localScale = Vector3.one * 1.1f;

		// 影の角度
		goEmpty.transform.localRotation = Quaternion.Euler(0,0,0);

		// 名前をつけたり色をつけたり
		//		spriteCopy.tag              = "Shadow";
		spriteCopy.sortingLayerName = sortingLayerName;
		spriteCopy.sortingOrder     = sortingOrder;
		spriteCopy.color            = ShadowColor;

		updateEnabled = true;

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
