using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataToSave
{
	public int playerHealth;
	public float[] playerPosition;

	public DataToSave(Player player)
	{
		playerHealth = player.currentHealth;
		playerPosition = new float[2];

		playerPosition[0] = player.transform.position.x;
		playerPosition[1] = player.transform.position.y;
	}
}
