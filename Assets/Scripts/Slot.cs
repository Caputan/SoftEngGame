using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public TMP_InputField nicknameText;
    public static string nickname;

    public TextMeshProUGUI dateText;

    public HealthBar healthBar;
    public int health;

    public static int activeIndex;

    private void Start()
    {
        nickname = nicknameText.text;
    }

    private void Update()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(100);
            healthBar.SetHealth(health);
        }

    }

    public void SaveNickname()
    {
        nickname = nicknameText.text;
    }

    public void SetActiveIndex(int index)
    {
        activeIndex = index;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
