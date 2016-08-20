using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	// アニメーション終了時のイベント
	public void OnAnimationStop(){ Destroy(gameObject); }
		
	// アニメーション終了時のイベント(親を消す処理)
	public void OnAnimationStopOnParent() { Destroy(transform.parent.gameObject); }

}
