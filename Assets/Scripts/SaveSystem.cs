using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary> 
/// Скрипт для сохранения/загрузки данных
/// </summary>
public static class SaveSystem 
{
	/// <summary> 
	/// Сохранение прогресса игрока
	/// </summary>
	/// <param name="player">
	/// Игровой персонаж, прогресс которого необходимо сохранить
	/// </param>
	public static void SavePlayer(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player" + player.activeSaveIndex.ToString() + ".save";
		FileStream stream = new FileStream(path, FileMode.Create);

		DataToSave data = new DataToSave(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}
	
	/// <summary> 
	/// Загрузка прогресса игрока
	/// </summary>
	public static DataToSave LoadPlayer()
	{
		string path = Application.persistentDataPath + "/player" + Slot.activeIndex.ToString() + ".save"; ;
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			if (stream.Length != 0)
			{
				DataToSave data = formatter.Deserialize(stream) as DataToSave;
				stream.Close();


				return data;
			} else
			{
				return null;
			}
		}
		else
		{
			return null;
		}
	}

	public static void ClearData()
	{
		string path = Application.persistentDataPath + "/player" + Slot.activeIndex.ToString() + ".save";
		if (File.Exists(path))
		{
			FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			stream.SetLength(0);
		}
	}

	public static void DeleteData()
	{
		string path = Application.persistentDataPath + "/player" + Slot.activeIndex.ToString() + ".save";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}
}
