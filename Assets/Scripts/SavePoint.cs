using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Скрипт для управления точкой созранения прогресса
/// </summary>
public class SavePoint : MonoBehaviour
{

	public Player player;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		player.SaveProgress();
	}

}
