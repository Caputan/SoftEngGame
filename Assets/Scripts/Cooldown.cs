using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// Скрипт для управления UI-элемента перезарядки "невидимости" 
/// </summary>
public class Cooldown : MonoBehaviour
{
    [SerializeField]
    private float _invisibilityTime;
    public Image imageCooldown;

    private void Start()
    {
        imageCooldown = this.GetComponent<Image>();
        _invisibilityTime = GameObject.Find("Player").GetComponent<Player>().invisibilityTime * 3;
    }
    // Update is called once per frame
    
    void Update()
    {
        //_invisibilityTime = GameObject.Find("Player").GetComponent<Player>().nextInvisibilityTime;
        if (GameObject.Find("Player").GetComponent<Player>().isInvisible || imageCooldown.fillAmount < 1f)
        {
            imageCooldown.fillAmount -= 1 / _invisibilityTime * Time.deltaTime;
        } 
        if (imageCooldown.fillAmount <= 0f)
        {

            imageCooldown.fillAmount = 1f;
        }
    }
}
