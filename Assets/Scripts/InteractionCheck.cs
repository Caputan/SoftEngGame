using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Скрипт для проверки возможности игрока взаимодействовать с окружающими объектами
/// </summary>
public class InteractionCheck : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Collider2D>().tag == "Player")
        {
            Player.GetComponent<Player>().canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            Player.GetComponent<Player>().canClimb = false;
        }
    }
}
