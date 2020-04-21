/// <summary> 
/// Скрипт для регулирования данных для сохранения 
/// </summary>
[System.Serializable]
public class DataToSave
{
	public int playerHealth;
	public float cooldown;

	public DataToSave(Player player)
	{
		playerHealth = player.currentHealth;
	}
}
