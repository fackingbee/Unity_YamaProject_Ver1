using UnityEngine;
using System.Collections;

public class GameDate {

	// どこからでも呼べるようにStaticで

	public static long  score;					// ポイントスコア
	public static float	GagePoint;				// ゲージポイント
	public static float playerPowerGagePoint;	// playerPowerGageアニメーション用
	public static float enemyPowerGagePoint;	// enemyPowerGageアニメーション用

	void Awake(){
		
		score                = 0;
		GagePoint            = 0f;
		playerPowerGagePoint = 0f;

	}
}