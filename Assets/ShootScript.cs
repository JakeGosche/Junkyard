using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject Bullet;
    private float activeShootInterval;
    public float shootInterval = 0;
    public float bulletSpeed;
    public float timeAlive;
    public Enums.Direction shootDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeShootInterval > 0)
        {
            activeShootInterval -= Time.deltaTime;
        }
        else
        {
            float startingX = 0;
            float startingY = 0;
            switch (shootDirection)
            {
                case Enums.Direction.Left:
                    startingX = -.75f;
                    break;
                case Enums.Direction.Right:
                    startingX = .75f;
                    break;
                case Enums.Direction.Up:
                    startingY = .75f;
                    break;
                default:
                    startingY = .75f;
                    break;
            }
            GameObject bulletObject = Instantiate(Bullet, gameObject.transform.position + Vector3.forward, Quaternion.identity);
            BulletScript bs = bulletObject.GetComponent<BulletScript>();
            bs.xSpeed = 7 * startingX;
            bs.ySpeed = 7 * startingY;
            bs.TimeAlive = 2;
            bs.Friendly = false;
            activeShootInterval = shootInterval;
        }
    }
}
