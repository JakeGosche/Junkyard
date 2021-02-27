using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float TimeAlive;
    public float Speed;
    public bool Friendly;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3((Speed * Time.deltaTime), 0, 0);
        TimeAlive -= Time.deltaTime;
        if(TimeAlive <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        if (Friendly)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }

        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage();
            }

            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hit");
        if (Friendly)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }

            Destroy(this.gameObject);
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage();
            }

            Destroy(this.gameObject);
        }
    }
}
