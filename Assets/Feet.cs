using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public Player Player;
    public int springHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
             Player.transform.parent = other.transform;
            Player.jumping = false;
            Player.animator.SetBool("IsJumping", false);
        }
        else if (other.gameObject.CompareTag("Floor")) //&& jumping)
        {
            Player.jumping = false;
            Player.animator.SetBool("IsJumping", false);
        }
        else if (other.gameObject.CompareTag("Spring"))
        {
            Player.jumping = true;
            Player.rigidBody.AddForce(new Vector2(0, springHeight), ForceMode2D.Impulse);
            Player.animator.SetBool("IsJumping", true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Player.transform.parent = null;
            DontDestroyOnLoad(Player.transform.gameObject);
            DontDestroyOnLoad(Player.playerCamera);
          //  Player.jumping = false;
        }
    }
}
