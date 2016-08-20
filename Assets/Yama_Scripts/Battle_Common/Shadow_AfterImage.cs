using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shadow_AfterImage : MonoBehaviour {

	public SpriteRenderer SpriteSrc;
	public bool afterImageEnabled;

	float span   = 0f;
	float margin = -10f;

	public  float longTapEndPosY;
	public  float longTapEndImgPosY;
	private float nextImgPosY;

	void Start () {

		span = 120f;

		afterImageEnabled = true;

		longTapEndImgPosY = longTapEndPosY + SpriteSrc.transform.localPosition.y + margin;

		nextImgPosY = transform.localPosition.y + SpriteSrc.transform.localPosition.y + span;

	}


	void Update() {

		// シャドウを生成する間隔は、ボードが120移動したら
		if (afterImageEnabled && nextImgPosY < longTapEndImgPosY){

			SpriteRenderer spriteCopy = Instantiate(SpriteSrc) as SpriteRenderer;

			spriteCopy.GetComponent<SpriteShadow>().enabled = false;

			spriteCopy.transform.SetParent(transform);
			spriteCopy.transform.SetAsLastSibling();

			spriteCopy.transform.localPosition = new Vector3(SpriteSrc.transform.localPosition.x,
															 SpriteSrc.transform.localPosition.y + span,
															 SpriteSrc.transform.localPosition.z);
			
			// スコアの間隔が480（boardY換算）の倍数だったので、シャドウの間隔は１／４にしてみた
			span += 120f;

			nextImgPosY = transform.localPosition.y + SpriteSrc.transform.localPosition.y + span;

			spriteCopy.transform.localScale = SpriteSrc.transform.localScale * 0.5f;

			spriteCopy.color            = new Color(1.0f, 0.0f, 0.0f, 0.5f);
			spriteCopy.sortingLayerName = "UI";
			spriteCopy.sortingOrder     = 1;

			SpriteRenderer[] spList = spriteCopy.GetComponentsInChildren<SpriteRenderer>();

			// Shadowスクリプトの項目と一致してなかったら反映されないので注意
			foreach (SpriteRenderer sp in spList) {
				if (sp.name == "Shadow") {
					sp.enabled = false;
				}
			}
		}
	}
}