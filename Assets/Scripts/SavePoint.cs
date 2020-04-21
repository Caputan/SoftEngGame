using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// Скрипт для управления точкой созранения прогресса
/// </summary>
/// 
public class SavePoint : MonoBehaviour
{
	/// <summary>
	/// ID загружмаемой зоны
	/// </summary>
	public int nextSceneLoad;

	/// <summary>
	/// Ссылка на объект игрока
	/// </summary>
	public Player player;
	
	private void Start()
	{
		nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			player.SaveProgress();

			SceneManager.LoadScene(nextSceneLoad);
			if(nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
			{
				PlayerPrefs.SetInt("levelAt", nextSceneLoad);
			}
		}
	}

}
