using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

/// <summary> 
/// Скрипт для регулирования данных для сохранения 
/// </summary>
public class DataToSave
{
	public int playerHealth;
	public float cooldown;

	public DataToSave(Player player)
	{
		playerHealth = player.currentHealth;

		cooldown = GameObject.Find("Invisibility").GetComponent<Cooldown>().imageCooldown.fillAmount;
	}
}
