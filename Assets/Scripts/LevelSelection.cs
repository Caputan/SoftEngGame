using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт для управления выбора игроком уровня.
/// </summary>
public class LevelSelection : MonoBehaviour
{
    /// <summary>
    /// Ссылка на UI-элементы
    /// </summary>
    public Button[] lvlButtons;
    
    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2);

        for(int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > levelAt)
                lvlButtons[i].interactable = false;
        }
    }

    /// <summary>
    /// Метод выбора уровня для загрузки
    /// </summary>
    /// <param name="level">
    /// ID уровня
    /// </param>
    public void levelToLoad(int level)
    {
        SceneManager.LoadScene(level);
    }

}
