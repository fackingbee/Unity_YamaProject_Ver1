using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Text))]
public class EvaluationHandler : MonoBehaviour {

	private RectTransform cachedRectTransform;
	private Text          cachedText;

	// RectTransformをキャッシュするプロパティ
	public RectTransform CachedRectTransform{
		get{ 
			if(cachedRectTransform == null){
				cachedRectTransform = GetComponent<RectTransform> ();
			}
			return cachedRectTransform;
		}
	}

	// Textをキャッシュするプロパティ
	public Text CachedText{
		get{ 
			if(cachedText == null){
				cachedText = GetComponent<Text> ();
			}
			return cachedText;
		}
	}
		
	// カウント宣言
	private int currentPerfect;		//現在のPerfect数
	private int currentGreat;		//現在のGreat数
	private int currentGood;		//現在のGood数
	private int currentCool;		//現在のCool数
	private int currentMiss;		//現在のMiss数

	// Use this for initialization
	void Start () {

		// 開始時に初期化する（0を代入してもいいし、GameDateの値も初期値は0なのでこちらでもよい）
		currentPerfect = 0;
		currentGreat   = 0;
		currentGood    = 0;
		currentCool    = 0;
		currentMiss    = 0;

		// スタート時に初期値をセット
		CachedText.text = currentPerfect.ToString();
		CachedText.text = currentGreat.ToString();
		CachedText.text = currentGood.ToString();
		CachedText.text = currentCool.ToString();
		CachedText.text = currentMiss.ToString();

	}
	
	// Update is called once per frame
	void Update () {

		if(currentPerfect < GameDate.perfectNum){
			if(gameObject.tag == "Perfect"){
				currentPerfect  = GameDate.perfectNum;
				CachedText.text = currentPerfect.ToString();
			}
		}

		if(currentGreat < GameDate.greatNum){
			if(gameObject.tag == "Great"){
				currentGreat  = GameDate.greatNum;
				CachedText.text = currentGreat.ToString();
			}
		}

		if(currentGood < GameDate.goodNum){
			if(gameObject.tag == "Good"){
				currentGood = GameDate.goodNum;
				CachedText.text = currentGood.ToString();
			}
		}

		if(currentCool < GameDate.badNum){
			if(gameObject.tag == "Cool"){
				currentCool = GameDate.badNum;
				CachedText.text = currentCool.ToString();
			}
		}

		if(currentMiss < GameDate.missNum){
			if(gameObject.tag == "Miss"){
				currentMiss = GameDate.missNum;
				CachedText.text = currentMiss.ToString();
			}
		}



	}
}
