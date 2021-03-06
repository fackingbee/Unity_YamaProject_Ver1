//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// サウンドのタイプ
	/// </summary>
	public enum SoundType
	{	
		/// <summary>BGM</summary>
		Bgm,
		/// <summary>SE</summary>
		Se,
		/// <summary>環境音</summary>
		Ambience,
		/// <summary>ボイス</summary>
		Voice,
		/// <summary>タイプの数</summary>
		Max,
	};


	/// <summary>
	/// サウンドファイル設定（ラベルとファイルの対応）
	/// </summary>
	public class AdvSoundSettingData : AdvSettingDataDictinoayItemBase
	{
		/// <summary>
		/// サウンドのタイプ
		/// </summary>
		public SoundType Type { get { return this.type; } }
		SoundType type;

		/// <summary>
		/// 表示タイトル
		/// </summary>
		public string Title { get { return this.title; } }
		string title;

		/// <summary>
		/// ファイル名
		/// </summary>
		string fileName;

		/// <summary>
		/// ファイル名
		/// </summary>
		public string FilePath { get { return this.filePath; } }
		string filePath;

		/// <summary>
		/// ストリーミングするか
		/// </summary>
		public bool IsStreaming { get { return this.isStreaming; } }
		bool isStreaming;

		/// <summary>
		/// バージョン
		/// </summary>
		public int Version { get { return this.version; } }
		int version;

		public StringGridRow RowData { get; protected set; }


		/// <summary>
		/// StringGridの一行からデータ初期化
		/// </summary>
		/// <param name="row">初期化するためのデータ</param>
		/// <returns>成否</returns>
		public override bool InitFromStringGridRow(StringGridRow row, AdvBootSetting bootSetting)
		{
			if (row.IsEmptyOrCommantOut) return false;

			this.RowData = row;
			string key = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			else
			{
				InitKey(key);
				this.type = AdvParser.ParseCell<SoundType>(row, AdvColumnName.Type);
				this.fileName = AdvParser.ParseCell<string>(row, AdvColumnName.FileName);
				this.isStreaming = AdvParser.ParseCellOptional<bool>(row, AdvColumnName.Streaming, false);
				this.version = AdvParser.ParseCellOptional<int>(row, AdvColumnName.Version, 0);
				this.title = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Title, "");

				this.filePath = FileNameToPath(fileName, bootSetting);
				AssetFile file = AssetFileManager.GetFileCreateIfMissing(this.FilePath, this.RowData);
				if (file != null)
				{
					file.Version = this.Version;
					//ロードタイプをストリーミングにする場合、
					if (this.IsStreaming)
					{
						file.AddLoadFlag(AssetFileLoadFlags.Streaming);
					}
				}

				return true;
			}
		}

		string FileNameToPath(string fileName, AdvBootSetting settingData)
		{
			switch (type)
			{
				case SoundType.Se:
					return settingData.SeDirInfo.FileNameToPath(fileName);
				case SoundType.Ambience:
					return settingData.AmbienceDirInfo.FileNameToPath(fileName);
				case SoundType.Bgm:
				default:
					return settingData.BgmDirInfo.FileNameToPath(fileName);
			}
		}
	}


	/// <summary>
	/// サウンドの設定
	/// </summary>
	public class AdvSoundSetting : AdvSettingDataDictinoayBase<AdvSoundSettingData>
	{
		/// <summary>
		/// 全てのリソースをダウンロード
		/// </summary>
		public override void DownloadAll()
		{
			//ファイルマネージャーにバージョンの登録
			foreach (AdvSoundSettingData data in List)
			{
				AssetFileManager.Download(data.FilePath);
			}
		}


		/// <summary>
		/// ラベルが登録されているか
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <param name="type">サウンドのタイプ</param>
		/// <returns>ファイルパス</returns>
		public bool Contains(string label, SoundType type)
		{
			//既に絶対URLならそのまま
			if (FilePathUtil.IsAbsoluteUri(label))
			{
				return true;
			}
			else
			{
				AdvSoundSettingData data = FindData(label);
				if (data == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// ラベルからファイルパスを取得
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <param name="type">サウンドのタイプ</param>
		/// <returns>ファイルパス</returns>
		public string LabelToFilePath(string label, SoundType type)
		{
			//既に絶対URLならそのまま
			if (FilePathUtil.IsAbsoluteUri(label))
			{
				//プラットフォームが対応する拡張子にする
				return ExtensionUtil.ChangeSoundExt(label);
			}
			else
			{
				AdvSoundSettingData data = FindData(label);
				if (data == null)
				{
					//ラベルをそのままファイル名扱いに
					return label;
				}
				else
				{
					return data.FilePath;
				}
			}
		}

		//ラベルからデータを取得
		AdvSoundSettingData FindData(string label)
		{
			AdvSoundSettingData data;
			if (!Dictionary.TryGetValue(label, out data))
			{
				return null;
			}
			else
			{
				return data;
			}
		}

		//元となるデータを取得（拡張性のために）
		public StringGridRow FindRowData(string label)
		{
			AdvSoundSettingData data = FindData(label);
			if (data == null)
			{
				return null;
			}
			else
			{
				return data.RowData;
			}
		}


		/// <summary>
		/// サウンドルームに表示するデータのリスト
		/// </summary>
		/// <returns></returns>
		public List<AdvSoundSettingData> GetSoundRoomList()
		{
			List<AdvSoundSettingData> list = new List<AdvSoundSettingData>();
			foreach (AdvSoundSettingData item in List)
			{
				if (!string.IsNullOrEmpty(item.Title))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
}