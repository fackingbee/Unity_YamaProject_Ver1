 using UnityEngine;
using System.Collections;

// 単独で使うのでMonoBehaviorはいらない
public class MusicDate  {

	// 音楽譜面データを格納しておく構造体 //

//	public float tick;
	public long tick;		//Tick
	public int  value;		//value(note_onなら音程でset_tempoならBPMを司る)
	public bool isCreated;	//すでに譜面を生成したかどうかを判別するフラグ
	public int  tapType;	//タップなのかフリックなどの特殊な操作なのか
	public long nextTick;	//次のスコアのティック

	// コンストラクタ
	public MusicDate(long tick, int value){

		// 値を格納
		this.tick  =  tick;
		this.value =  value;

		// 初期化
		this.isCreated = false;

		//タップのタイプを識別
		if (value == 84 || value == 85) {
			
			// 1はフリック
			this.tapType = 1;

		} else if (value == 96 || value == 98 || value == 100 || value == 102) {
			
			// 3はロングタップ・ダウン（同時に４レーンに出せるようにしておく）
			this.tapType = 3;

		} else if (value == 97 || value == 99 || value == 101 || value == 103) {
			
			// 4はロングタップ・アップ（同時に４レーンに出せるようにしておく）
			this.tapType = 4;

		} else if (value == 36) {

			// 5はPlayerTurnStartフラグ
			this.tapType = 5;

		} else if (value == 35) {

			// 6はPlayerTurnEndフラグ
			this.tapType = 6;

		// それ以外は今のところ通常のスコアを使用
		} else {
			
			// 2は通常のタップ
			this.tapType = 2;	

		}
	}
}
