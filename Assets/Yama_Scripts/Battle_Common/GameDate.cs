using UnityEngine;
using System.Collections;

public class GameDate {

	// どこからでも呼べるようにStaticで

	public static long  score;					// ポイントスコア
	public static float	GagePoint;				// ゲージポイント
	public static float playerPowerGagePoint;	// playerPowerGageアニメーション用
	public static float enemyPowerGagePoint;	// enemyPowerGageアニメーション用
	public static int   perfectNum;				// perfec総数
	public static int   greatNum;				// great総数
	public static int   goodNum;				// good総数
	public static int   badNum;					// bad総数
	public static int   missNum;				// miss総数
	public static int   totalScoreNum;			// 総スコア数格納変数

	void Awake(){

		// バトル開始時に明示的に初期化
		score                = 0;
		GagePoint            = 0f;
		playerPowerGagePoint = 0f;
		totalScoreNum        = 0;
		perfectNum           = 0;
		greatNum             = 0;
		goodNum              = 0;
		badNum               = 0;
		missNum              = 0;

	}
}