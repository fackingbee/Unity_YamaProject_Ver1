using UnityEngine;
using System.Collections;

public class ParticleSize : MonoBehaviour {

	private float   scaleRate;			// ロングタップ時のエフェクト用で、長さに合わせて縮小率を変える
	private float   baseTick;			// 
	private Vector3 baseLocalScale;		//
	public  float   startTick;			// ロングタップ時のエフェクト用で、ダウン時のTick
	public  float   endTick;			// ロングタップ時のエフェクト用で、アップ時のTick

	void Start () {
		baseTick = TimeManager.tick;
		baseLocalScale = this.transform.localScale;
	}
	
	void Update () {

		// （構文）縮小率を決める
		scaleRate = (TimeManager.tick - baseTick) / (endTick - startTick);

		// 0.2以下になるまで縮小する
		if (this.transform.localScale.x > 0.2f) {
			this.transform.localScale = baseLocalScale * (1f - scaleRate * 0.7f);
		}

	}
}
