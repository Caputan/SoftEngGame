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

    private int _levelAt;
    
    // Start is called before the first frame update
    void Start()
    {
        DataToSave data = SaveSystem.LoadPlayer();
        try
        {
            _levelAt = data.currentLevelIndex;
        }
        catch
        {
            _levelAt = 2;
        }

        for(int i = 0; i < lvlButtons.Length; i++)
        {
            lvlButtons[0].interactable = true;

            if (i + 2 > _levelAt)
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
