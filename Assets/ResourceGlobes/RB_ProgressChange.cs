using UnityEngine;

public class RB_ProgressChange : MonoBehaviour {
	public SpriteRenderer[] rend;
	public void changeMaterialProgress(float value){
		foreach (SpriteRenderer r in rend)
			r.sharedMaterial.SetFloat("_Progress",value);
	}
}
