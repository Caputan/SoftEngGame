﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Скрипт для проверки нахождения игрока на земле
/// </summary>
public class GroundCheck : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
		Player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground"){
            Player.GetComponent<Player>().isGrounded = true;
        }
    }

	private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground"){
			Player.GetComponent<Player>().isGrounded = false;
        }
    }
}
