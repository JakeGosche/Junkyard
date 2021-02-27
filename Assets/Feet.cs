using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public Player Player;
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
        Debug.Log("Hit floor!");
        if (other.gameObject.CompareTag("Floor")) //&& jumping)
        {
            Player.jumping = false;
        }
    }
}
