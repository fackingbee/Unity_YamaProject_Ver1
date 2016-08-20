using UnityEngine;

/// <summary>
/// 様々なUIビューのベースクラスとして使用
/// UIに共通する処理はこのクラスで実装する
/// </summary>

// Rect Transformコンポーネントを必須にする
// スクリプトをアタッチすると自動でコンポーネントも追加してくれるようにしている
[RequireComponent(typeof(RectTransform))]

public class ViewController : MonoBehaviour {

	// Rect Transformコンポーネントをキャッシュ
	private RectTransform cachedRectTransform;

	public RectTransform CachedRectTransform{
		
		// プロパティの設定
		get{ 
			
			//Rectのコンポーネントがなければ
			if(cachedRectTransform == null){
				
				// 取得して
				cachedRectTransform = GetComponent<RectTransform> ();

			}

			//取得したものを返す
			return cachedRectTransform;

		}
	}


	// ビューのタイトルを取得、設定するプロパティ
	public virtual string Title { get { return ""; } set{} }


}
