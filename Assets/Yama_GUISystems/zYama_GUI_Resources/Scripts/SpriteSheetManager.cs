using UnityEngine;
using System.Collections.Generic;

public class SpriteSheetManager {


	// スプライトシートに含まれるスプライトをキャッシュするディクショナリー
	// この場合値のDictionaryにはスプライトの名前とスプライト自体を格納する
	private static Dictionary<string, Dictionary<string, Sprite>> spriteSheets = 
		new Dictionary<string, Dictionary<string, Sprite>>();


	// スプライトシートに含まれるスプライトを読み込んでキャッシュするメソッド
	// このLoad関数はShopItemTableViewControllerで呼ばれる
	// pathには"IconAtlas"が入って来ている
	public static void Load(string path) {

		// 最初は無いので追加
		if(!spriteSheets.ContainsKey(path)) {

			// "IconAtlas"をキーとして、値である<スプライト名、実際のスプライトの絵>を追加する
			spriteSheets.Add(path, new Dictionary<string, Sprite>());

		}


		// スプライトを読み込んで、名前と紐付けてキャッシュする
		// 実際この右辺の書き方が見慣れないが、Unityで用意されているっぽいし、覚えるしかない。
		Sprite[] sprites = Resources.LoadAll<Sprite>(path);

		foreach(Sprite sprite in sprites) {

			// 最初はなにも入っていないのでまずはAddする
			if(!spriteSheets[path].ContainsKey(sprite.name)) {

				// 
				spriteSheets[path].Add(sprite.name, sprite);

				// ここでデバッグすると、Resourcesフォルダに格納されているアトラスが一旦全部読み込まれている事が分かる
				//Debug.Log (sprite.name);

			}
		}
	}

	// スプライト名からスプライトシートに含まれるスプライトを返すメソッド
	public static Sprite GetSpriteByName(string path, string name) {

		if(spriteSheets.ContainsKey(path) && spriteSheets[path].ContainsKey(name)){

			// Resource直下の"IconAtlas"というPathとそのスプライト名であるnameが返る
			return spriteSheets[path][name];

		}
		return null;
	}
}
