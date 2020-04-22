using System;
using UnityEngine.SceneManagement;


/// <summary> 
/// Скрипт для регулирования данных для сохранения 
/// </summary>
[System.Serializable]
public class DataToSave
{
	public string nickname;

	public string date;

	public int playerHealth;
	public int currentLevelIndex;


	public DataToSave(Player player)
	{
		date = DateTime.Now.ToString();

		playerHealth = player.currentHealth;

		if (SceneManager.GetActiveScene().buildIndex < 1 || SceneManager.GetActiveScene().buildIndex > 4)
		{
			currentLevelIndex = 2;
		} else
		{
			currentLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
		}

		nickname = player.nickname;
	}
}
