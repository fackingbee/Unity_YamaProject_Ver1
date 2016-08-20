using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnBack00 : MonoBehaviour {


	/// <summary>
	/// 一度isOnがtrueになったら、isCheckがfalseになるので上の条件文には入らず
	/// 尚且つ、まだ下記には入らず、isOnがfalseになるまで待機することで、むやみにUpdateに入らない 
	/// 又、開始直後はHomeから始まるが、最初だけはSEを再生しない
	/// </summary>


	private Toggle      toggle00;		// isOnのON・OFF 
	private GameObject  onBackImage00;	// 背景画のON・OFF
	private GameObject  tabShadow00;	// アイコン影のON・OFF
	private bool        isChecked;		// Update進入フラグ管理
	private bool        isTouch;		// SEは一度再生したらもう鳴らさない
	private Image       image00;		// 背景画のON・OFF
	private Image       imageShadow00;	// アイコン影のON・OFF
	public  AudioClip   onButton00;		// SE格納
	private AudioSource audioSource;	// SE取得

	// 例外：OnBack00.csだけの処理（開始直後に再生しない為、カウントを設ける）
	private int count = 0;

	// Tab00の背景のON・OFF用
	private Color onBackColor00  = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color offBackColor00 = new Color (1.0f, 1.0f, 1.0f, 0.5f);


	void Awake(){

		// まず探す。但し、ゲームオブジェクト表示・非表示方式だとNullになるので、今回はImageコンポーネントを切り替える
		onBackImage00 = GameObject.Find ("HomeBack");
		image00       = onBackImage00.GetComponent<Image> ();
		tabShadow00   = GameObject.Find ("TabShadow00");
		imageShadow00 = tabShadow00.GetComponent<Image> ();
		audioSource   = GetComponent<AudioSource> ();

		// スタート時はHomeからON状態で始まる
		image00.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

	}

	void Start () {

		// 取得し、フラグの初期状態を決める。スタート時はHomeがON状態で始めるので、01~05と少し変わる
		toggle00      = GetComponent<Toggle> ();
		toggle00.isOn = true;
		isChecked     = true;

		// 開始直後は触っていないので鳴らさない
		isTouch = false;

	}


	void Update () {

		// isCheckedが真とはつまり、イメージがOFFの状態で、isOnがtrueになったら、イメージをON
		if(isChecked == true && toggle00.isOn == true){

			// ONにしたら、isCheckedはfalseになるので、ここには用はない
			OnImage00 ();

			// 一度呼ばれたらむやみにUpdateに入らないようにする為に確認
			//Debug.Log ("hoge_0_1");	

		}else if(isChecked == false && toggle00.isOn == false){

			// OFFにしたら、isCheckedはtrueになるので、ここには用はない
			OffImage00 ();

			// 一度呼ばれたらむやみにUpdateに入らないようにする為に確認
			//Debug.Log ("hoge_0_2");

		} 

		// 開始直後にSEが再生しないように、countを設ける。最低でも一回、isOnがOFFになったらOK
		if(isTouch && toggle00.isOn == true && count > 0){
			
			// 再生
			audioSource.PlayOneShot (onButton00,0.6f);

			// 一度再生したら、もう一度isOnがOFFからONに鳴らない限り、条件式に入らない
			isTouch = false;

		}
	}


	public void OnImage00(){
		if(toggle00.isOn == true){
			
			// 選択時は背景はOn
			image00.color         = onBackColor00;

			// 選択時は影はON
			imageShadow00.enabled = true;

			// 無駄にUpdateのifに入らない
			isChecked             = false;

			// タッチされたら、SEを鳴らす（但し開始時は鳴らさない）
			isTouch = true;

		}
	}
		

	public void OffImage00(){
		if(toggle00.isOn == false){
			
			// 他のTabが選択されたら、背景はOff
			image00.color         = offBackColor00;

			// 他のTabが選択されたら、影はOff
			imageShadow00.enabled = false;

			// 無駄にUpdateのelse ifに入らない
			isChecked             = true;

			//一度、OFFにしたら、いつでもSEを再生出来る
			count = 1;

		}
	}
}
