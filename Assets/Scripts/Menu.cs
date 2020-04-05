using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionsDropdown;
    private Resolution[] _resolutions;

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
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void ButtonCharacterPressed()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ButtonSettingsPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonToMenuPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonExitPressed()
    {
        Application.Quit();
    }

    public void SliderVolumeChanged(float value)
    {
        audioMixer.SetFloat("volume", value);
    }

    public void ToggleFullScreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }

    public void DropdownResolutionsChanged(int index)
    {
        var resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
