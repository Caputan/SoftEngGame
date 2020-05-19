﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт для работы с главным меню и меню настроек
/// </summary>
public class Menu : MonoBehaviour
{
    /// <summary>
    /// Ссылка на регулировщик громкости проекта.
    /// </summary>
    public AudioMixer audioMixer;
    
    /// <summary>
    /// Ссылка на выпадающий список доступных разрешений экрана.
    /// </summary>
    public TMP_Dropdown resolutionsDropdown;

    private Resolution[] _resolutions;
    

    public static bool GameIsPaused;
    public GameObject pauseMenuUI;


    private void Start()
    {
        Screen.fullScreen = true;
        _resolutions = Screen.resolutions;
        var options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            options.Add(_resolutions[i].width + " x " + _resolutions[i].height);
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        try
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(options);
            resolutionsDropdown.value = currentResolutionIndex;
            resolutionsDropdown.RefreshShownValue();
        } catch (Exception e)
        {
            Debug.Log(e);
        }

        pauseMenuUI = GameObject.Find("PauseMenu");

        GameIsPaused = false;
        pauseMenuUI.SetActive(GameIsPaused);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveSystem.DeleteData();
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
                Resume();
			} else
			{
                Pause();
			}
		}
    }

    public void ButtonCharacterPressed()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        SceneManager.LoadScene(6);
    }

    public void Resume()
	{
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
	{
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;
	}

    /// <summary>
    /// Обработка нажатия кнопки "Настройки". Переадресует пользователя на экран настроек.
    /// </summary>
    public void ButtonSettingsPressed()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        SceneManager.LoadScene(5);
    }
    
    /// <summary>
    /// Обработка нажатия кнопки "Вернуться в меню". Переадресует пользователя на экран главного меню.
    /// </summary>
    public void ButtonToMenuPressed()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    /// <summary>
    /// Обработка нажатия кнопки "Выйти из игры". Закрывает окно программы и прекращает процесс.
    /// </summary>
    public void ButtonExitPressed()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        Application.Quit();
    }

    /// <summary>
    /// Обработка изменения значения слайдера "Громкость". Меняет громкость звука в программе.
    /// </summary>
    /// <param name="value">
    /// Значение громкости.
    /// </param>
    public void SliderVolumeChanged(float value)
    {
        audioMixer.SetFloat("volume", value);
    }

    /// <summary>
    /// Обработка переключателя "Полный экран". Переключает между режимами "Полный экран" и "Оконный режим".
    /// </summary>
    /// <param name="value">
    /// True - режим "Полный экран".
    /// False - "Оконный режим".
    /// </param>
    public void ToggleFullScreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }

    /// <summary>
    /// Обработка выбора в выпадном меню "Разрешение экрана". Устанавливает выбранное разрешение для приложения.
    /// </summary>
    /// <param name="index">
    /// Индекс значения разрешения в выпадном списке.
    /// </param>
    public void DropdownResolutionsChanged(int index)
    {
        var resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ButtonPlayPressed()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
