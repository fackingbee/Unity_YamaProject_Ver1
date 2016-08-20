using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutInFillAmount : MonoBehaviour {


	private Image    enemyImage; 				// Imageコンポーネント取得用
	private Animator playerCutInStart;			// PlayerSpriteのAnimatorをON・OFF
	private bool     isPlayerCutInSpriteON;     // カットイン時のPlayerSpriteアニメーションONフラグ
	private bool     isPlayerCutInSpriteOFF;    // カットイン時のPlayerSpriteアニメーションOFFフラグ
	public  bool     fadeIn;
	public  bool     fadeOut;
	private bool	 inFadeIn;


	void Start () {
		// Imageコンポーネント取得
		enemyImage            = GetComponent<Image> ();
		// 開始時は何も映ってなくていいので0で初期化
		enemyImage.fillAmount = 0f;
		// 初期値はLeft
		enemyImage.fillOrigin = 0;

		playerCutInStart      = GameObject.Find("PlayerSprite").GetComponent<Animator>();

		fadeIn   = false;
		fadeOut  = false;
		inFadeIn = false;

		isPlayerCutInSpriteON  = true;
		isPlayerCutInSpriteOFF = false;

	}

	
	// このメソッドでは、フェードイン中フラグをONにするだけ
	public void FadeIn() {
		inFadeIn = true;
	}

	void Update () {

		// ScoreCreaterからFadeInフラグをONにする
		if(fadeIn){

			fadeIn = false;
		
			// このスクリプトがオンになってから、1秒後にFadeIn()を実行
			Invoke("FadeIn", 1f);

		}


		// フェードイン中だけ通る
		if (inFadeIn) {
			
			// enemyImage.fillAmountが1を超えるまで毎フレーム加算、超えたらフラグをOFFる
			if (enemyImage.fillAmount < 1 && enemyImage.fillOrigin == 0) {
				
				enemyImage.fillAmount += 0.04f;

			} else {
				
				inFadeIn = false;

			}
		}



		// 目的の数値（max＝1）に達したら、fillOrigin＝1（right）にする
		if(enemyImage.fillAmount >= 1){
			
			// この時点でFadeInが呼ばれても、fillOrifginが0じゃないのでインクリメントが実行されない。
			enemyImage.fillOrigin = 1;

			// fillOrigin＝1になったら、アニメーションさせるスプライトのAnimatorをオンにする
			playerCutInStart.enabled = true;

			// カットインアニメーションへの遷移を許可
			isPlayerCutInSpriteON = true;

			// カットインアニメーションを遷移させる
			playerCutInStart.SetBool ("isPlayerCutInSpriteON", isPlayerCutInSpriteON);

			// 帯を描写しきったら、もうFadeIn関数にはいらないようにする
			//fadeIn = false;

			// FadeOutにはいれるようにする
			fadeOut = true;

		}



		if(fadeOut && enemyImage.fillOrigin == 1 && enemyImage.fillAmount > 0){

			// 3秒後にFadeOut、デクリメントし始める（その間にスプライトのアニメーションを終わらせる）
			Invoke ("FadeOut",3f);

		}
	}




	// FillAmountをデクリメント/この関数が呼ばれても、fillOrigin＝1じゃなければデクリメントしない
	public void FadeOut(){

		// アイドリング状態への遷移を許可
		isPlayerCutInSpriteOFF = true;
		playerCutInStart.SetBool ("isPlayerCutInSpriteOFF", isPlayerCutInSpriteOFF);

		// 一度スプライトがカットインしたらもうアニメーションへ遷移しないようにする
		isPlayerCutInSpriteON = false;
		playerCutInStart.SetBool ("isPlayerCutInSpriteON", isPlayerCutInSpriteON);

		//Debug.Log ("フェードアウト");
		if(enemyImage.fillAmount > 0 && enemyImage.fillOrigin == 1){
			enemyImage.fillAmount -= 0.03f;
		}

		if(enemyImage.fillAmount <= 0){
			enemyImage.fillOrigin = 0;
		}
	}
}
