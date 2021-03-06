//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// インポートした全シナリオデータ
	/// </summary>
	public class AdvImportScenarios : ScriptableObject
	{
		const int Version = 1;
		
		[SerializeField]
		int importVersion = 0;

		public bool CheckVersion()
		{
			return importVersion == Version;
		}

		//インポートされた章データ
		[SerializeField]
		List<AdvChapterData> chapters = new List<AdvChapterData>();
		public List<AdvChapterData> Chapters { get { return chapters; } }

#if UNITY_EDITOR
		//インポート時のクリア処理
		public void ClearOnImport()
		{
			importVersion = Version;
			this.Chapters.Clear();
		}

		/// <summary>
		/// 設定データのエクセルシートを読み込む
		/// </summary>
		/// <param name="sheetName">シート名</param>
		/// <param name="grid">シートのStringGridデータ</param>
		public void AddImportData( string chapterName, List<AdvImportBook> importDataList)
		{
			importVersion = Version;
			AdvChapterData chapter = this.Chapters.Find(x => x.ChapterName == chapterName);
			if(chapter==null)
			{
				chapter = new AdvChapterData(chapterName);
				this.Chapters.Add(chapter);
			}
			chapter.InitImportData(importDataList);
		}

		/*
		/// <summary>
		/// 設定データのエクセルシートを読み込む
		/// </summary>
		/// <param name="sheetName">シート名</param>
		/// <param name="grid">シートのStringGridデータ</param>
		public void ImportFromExcel(string name, StringGrid grid)
		{
			importVersion = Version;
			importedGridList.AddGrid(grid);
		}
*/

#endif

	}
}