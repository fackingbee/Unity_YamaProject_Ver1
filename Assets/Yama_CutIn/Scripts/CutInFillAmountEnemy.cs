using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutInFillAmountEnemy : MonoBehaviour {

	private Image    enemyImage; 				// Imageコンポーネント取得用
	private Animator enemyCutInStart;			// PlayerSpriteのAnimatorをON・OFF
	private bool     isEnemyCutInSpriteON;     // カットイン時のPlayerSpriteアニメーションONフラグ
	private bool     isEnemyCutInSpriteOFF;    // カットイン時のPlayerSpriteアニメーションOFFフラグ
	public  bool     fadeIn;
	public  bool     fadeOut;


	void Start () {
		// Imageコンポーネント取得
		enemyImage            = GetComponent<Image> ();
		// 開始時は何も映ってなくていいので0で初期化
		enemyImage.fillAmount = 0f;
		// 初期値はRight
		enemyImage.fillOrigin = 1;
		//
		enemyCutInStart       = GameObject.Find("EnemySprite").GetComponent<Animator>();

		fadeIn  = true;
		fadeOut = false;

		isEnemyCutInSpriteON  = true;
		isEnemyCutInSpriteOFF = false;

	}

	void Update () {


		if(fadeIn){

			// このスクリプトがオンになってから、2秒後にFadeIn
			Invoke ("FadeIn",1f);

		}



		// 目的の数値（max＝1）に達したら、fillOrigin＝1（right）にする
		if(enemyImage.fillAmount >= 1){

			// この時点でFadeInが呼ばれても、fillOrifginが0じゃないのでインクリメントが実行されない。
			enemyImage.fillOrigin = 0;

			// fillOrigin＝1になったら、アニメーションさせるスプライトのAnimatorをオンにする
			enemyCutInStart.enabled = true;

			// カットインアニメーションへの遷移を許可
			isEnemyCutInSpriteON = true;

			// カットインアニメーションを遷移させる
			enemyCutInStart.SetBool ("isEnemyCutInSpriteON", isEnemyCutInSpriteON);

			// 帯を描写しきったら、もうFadeIn関数にはいらないようにする
			fadeIn = false;

			// FadeOutにはいれるようにする
			fadeOut = true;

		}



		if(fadeOut && enemyImage.fillOrigin == 0 && enemyImage.fillAmount > 0){

			// 3秒後にFadeOut、デクリメントし始める（その間にスプライトのアニメーションを終わらせる）
			Invoke ("FadeOut",3.1f);

		}
	}



	// FillAmountをインクリメント/この関数が呼ばれてもfillOrigin = 0じゃなければインクリメントしない
	public void FadeIn(){

		//Debug.Log ("フェードイン");
		if(enemyImage.fillAmount < 1 && enemyImage.fillOrigin == 1){
			enemyImage.fillAmount += 0.04f;
		}
	}


	// FillAmountをデクリメント/この関数が呼ばれても、fillOrigin＝1じゃなければデクリメントしない
	public void FadeOut(){

		// アイドリング状態への遷移を許可
		isEnemyCutInSpriteOFF = true;
		enemyCutInStart.SetBool ("isEnemyCutInSpriteOFF", isEnemyCutInSpriteOFF);

		// 一度スプライトがカットインしたらもうアニメーションへ遷移しないようにする
		isEnemyCutInSpriteON = false;
		enemyCutInStart.SetBool ("isEnemyCutInSpriteON", isEnemyCutInSpriteON);


		//Debug.Log ("フェードアウト");
		if(enemyImage.fillAmount > 0 && enemyImage.fillOrigin == 0){
			enemyImage.fillAmount -= 0.027f;

		}

		if(enemyImage.fillAmount <= 0){
			enemyImage.fillOrigin = 1;
		}
	}
}
