﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
	public static void SavePlayer(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player.op";
		FileStream stream = new FileStream(path, FileMode.Create);

		DataToSave data = new DataToSave(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}

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
			Debug.Log("Fuck you!");

			return null;
		}
	}
}