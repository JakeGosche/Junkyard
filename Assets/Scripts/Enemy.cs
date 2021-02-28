using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Bullet;
    public Player player;
    public float shootInterval = 0;
    public Vector3 leftPoint, rightPoint;
    private bool movingLeft = true;
    public float movingSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        leftPoint = new Vector3(gameObject.transform.position.x - 3, gameObject.transform.position.y, gameObject.transform.position.z);
        rightPoint = new Vector3(gameObject.transform.position.x + 3, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.transform.position += new Vector3(movingSpeed * (movingLeft ? -1 : 1) * Time.deltaTime, 0, 0);
        if (movingLeft && gameObject.transform.position.x < leftPoint.x)
        {
            movingLeft = false;
        }
        else if (!movingLeft && gameObject.transform.position.x > rightPoint.x)
        {
            movingLeft = true;
        }
        //if (shootInterval > 0)
        //{
        //    shootInterval -= Time.deltaTime;
        //}
        //else
        //{
        //    GameObject bulletObject = Instantiate(Bullet, gameObject.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        //    BulletScript bs = bulletObject.GetComponent<BulletScript>();
        //    bs.Speed = -5;
        //    bs.TimeAlive = 2;
        //    shootInterval = 2f;
        //}

    }

    void OnCollisionEnter2D(Collision2D other)
    {
       
     
            if (other.gameObject.CompareTag("Player") && other.gameObject.name == "Player")
            {
            Debug.Log("Hit");
            Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage();
            }

            //Destroy(this.gameObject);
        
    }
}
