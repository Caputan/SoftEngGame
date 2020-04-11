
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    [SerializeField]
    private float _invisibilityTime;
    private Image _imageCooldown;


    private void Start()
    {
        _imageCooldown = this.GetComponent<Image>();
        _invisibilityTime = GameObject.Find("Player").GetComponent<Player>().invisibilityTime * 3;
    }
    // Update is called once per frame
    void Update()
    {
        //_invisibilityTime = GameObject.Find("Player").GetComponent<Player>().nextInvisibilityTime;
        if (GameObject.Find("Player").GetComponent<Player>().isInvisible || _imageCooldown.fillAmount < 1f)
        {
            _imageCooldown.fillAmount -= 1 / _invisibilityTime * Time.deltaTime;
        } 
        if (_imageCooldown.fillAmount <= 0f)
        {

            _imageCooldown.fillAmount = 1f;
        }
    }
}
