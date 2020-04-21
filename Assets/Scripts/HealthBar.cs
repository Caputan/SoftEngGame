using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// Скрипт для регулирования UI-эелемента со здоровьем 
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Ссылка на UI-слайдер.
    /// </summary>
    public Slider slider;

    /// <summary> 
    /// Установка максимального значения здоровья
    /// </summary>
    /// <param name="health">
    /// Максимальное значение здоровья
    /// </param>
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary> 
    /// Установка определенного значения уровня здоровья
    /// </summary>
    /// <param name="health">
    /// Значение здоровья
    /// </param>
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
