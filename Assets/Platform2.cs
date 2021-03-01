using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2 : MonoBehaviour
{
    private Vector3 leftPoint, rightPoint, upPoint, downPoint;
    private bool movingLeft = true;
    private bool movingUp = true;
    public float movingXSpeed = 2;
    public float movingYSpeed = 2;
    public float movingXDistance = 3;
    public float movingYDistance = 3;
    // Start is called before the first frame update
    void Start()
    {
        leftPoint = new Vector3(gameObject.transform.position.x - movingXDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        rightPoint = new Vector3(gameObject.transform.position.x + movingXDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        upPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + movingYDistance, gameObject.transform.position.z);
        downPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - movingYDistance, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.transform.position += new Vector3(movingXSpeed * (movingLeft ? -1 : 1) * Time.deltaTime, movingYSpeed * (!movingUp ? -1 : 1) * Time.deltaTime, 0);
        if (movingLeft && gameObject.transform.position.x < leftPoint.x)
        {
            movingLeft = false;
        }
        else if (!movingLeft && gameObject.transform.position.x > rightPoint.x)
        {
            movingLeft = true;
        }

        if (movingUp && gameObject.transform.position.y > upPoint.y)
        {
            movingUp = false;
        }
        else if (!movingUp && gameObject.transform.position.y < downPoint.y)
        {
            movingUp = true;
        }
    }
}
