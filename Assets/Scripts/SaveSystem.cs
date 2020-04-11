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
		string path = Application.persistentDataPath + "/player.op";
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
		string path = Application.persistentDataPath + "/player.op";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			DataToSave data = formatter.Deserialize(stream) as DataToSave;
			stream.Close();

			return data;
		}
		else
		{
			return null;
		}
	}
}
