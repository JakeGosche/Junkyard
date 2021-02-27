using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class Player : MonoBehaviour
{
    
    public float shadowRecharge, shadowLength, raygunRecharge;
    public Rigidbody2D rigidBody;
    public static float jumpHeight = 20;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool canMove, shadowActive;
    public static bool PlayerExists;
    public Interactable insideTrigger;
    public GameManager GameManager;
    public Camera playerCamera;
    private float abilityInterval = 0f;
    private float LastMove = 7.0f;
    public bool jumping = false;
    public GameObject Bullet;
    public float Health = .3f;
    public float MaxHealth = .3f;

    public Slider Healthbar;
    public int UniqueId;
    public Upgrade upgrade;
    public SpriteRenderer spriteRenderer;
    public GameObject feet;
    private void Awake()
    {
        if (!PlayerExists)
        {
            PlayerExists = true;
            DontDestroyOnLoad(transform.gameObject);
            DontDestroyOnLoad(playerCamera);
        }
        else
        {
            Destroy(playerCamera);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = FindObjectOfType<Camera>();
        GameManager = FindObjectOfType<GameManager>();
        //shadowinterval = shadowLength;
       // upgrade = Upgrade.None;
        shadowActive = false;
    }

    void Update()
    {
        bool displayInfo = false;
        if (canMove)
        {
            float xForce = 0;
            float yForce = 0;
            bool moving = false;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                //left or right
                GameManager.started = true;
                float direction = Input.GetKey(KeyCode.D) ? 7f : -7f;
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                {
                    direction = LastMove;
                }
                //gameObject.transform.position += new Vector3(dire * Time.deltaTime, 0, 0);
                xForce = direction;// * .02f;
                //rigidBody.AddForce(new Vector2(direction * .02f, 0), ForceMode2D.Force);
                //rigidBody.MovePosition(gameObject.transform.position += new Vector3(direction * Time.deltaTime, 0));
                //rigidBody.velocity = new Vector2(direction, 0);
                LastMove = direction;
                transform.rotation = new Quaternion(transform.rotation.x, direction > 0 ? 0 : 180, transform.rotation.z, transform.rotation.w);
                moving = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                yForce = jumpHeight;
                rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                jumping = true;
            }

            rigidBody.AddForce(new Vector2(xForce, yForce));
            if (upgrade != Upgrade.None)
            {
                if(shadowActive)
                {
                    shadowLength -= Time.deltaTime;
                    if(shadowLength <= 0)
                    {
                        gameObject.layer = 8;
                        feet.gameObject.layer = 8;
                        shadowActive = false;
                        spriteRenderer.color = new Color(1, 1, 1, 1);
                        
                        abilityInterval = shadowRecharge;
                    }
                }

                if (abilityInterval > 0)
                {
                    abilityInterval -= Time.deltaTime;
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (upgrade == Upgrade.Raygun)
                    {
                        GameObject bulletObject = Instantiate(Bullet, gameObject.transform.position + new Vector3(LastMove > 0 ? 1 : -1, 0, 0), Quaternion.identity);
                        BulletScript bs = bulletObject.GetComponent<BulletScript>();
                        bs.Speed = LastMove * 2;
                        bs.TimeAlive = .2f;
                        abilityInterval = raygunRecharge;
                    }
                    else
                    {
                        spriteRenderer.color = new Color(1, 1, 1, .5f);
                        shadowActive = true;
                        gameObject.layer = 10;
                        feet.gameObject.layer = 10;
                    }
                }
            }
            

            if (!jumping && !moving)
            {
                //rigidBody.velocity = new Vector2(0, 0);
                if (insideTrigger != null)
                {
                    displayInfo = true;
                    if (Input.GetKey(KeyCode.W))
                    {
                        rigidBody.velocity = new Vector2(0, 0);
                        canMove = false;
                        insideTrigger.Interact();
                    }
                }
            }
        }
        //else
        //{
        //    rigidBody.velocity = new Vector3(0, 0);
        //}
        GameManager.actionText.gameObject.SetActive(displayInfo);
        playerCamera.transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z);
    }

    public void TakeDamage()
    {
        StartCoroutine(ResetScene());
        //Health -= .1f;
        //Healthbar.value = Health;
        //if(Healthbar.value <= 0)
        //{
        //    ResetScene();
        //}
    }

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("Floor") && (gameObject.transform.position.y  - .9f > other.transform.position.y)) //&& jumping)
    //    {
    //        jumping = false;
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pit"))
        {
            
            if (UniqueId == 1)
            {
                canMove = false;
                gameObject.transform.position = GameManager.StartingPosition;
                UniqueId = 0;
                
                ConversationObject conversationObject = new ConversationObject();
                conversationObject.DialogueArray.Add("Oh, I guess that jump was too far. Sorry. Hmmm, how should I fix this?");
                conversationObject.Options.Add(new OptionObject { OptionText = "Add a platform?", DialogueId = 2 });
                conversationObject.Options.Add(new OptionObject { OptionText = "Higher jumping?", DialogueId = 3 });
                StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
                //this.gameObject.SetActive(false);
            }
            else
            {

                StartCoroutine(ResetScene());
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        //else
        //{
        //    Interactable changeScene = other.GetComponent<Interactable>();
        //    if (changeScene != null)
        //    {
        //        insideTrigger = changeScene;
        //    }
        //}
    }

    public IEnumerator ResetScene()
    {
        if (canMove)
        {
            canMove = false;
            if (UniqueId == 2)
            {
            }
            spriteRenderer.color = new Color(1, 1, 1, 0);
            rigidBody.velocity = new Vector2(0, 0);
            yield return StartCoroutine(GameManager.TransitionIn());

            gameObject.transform.position = GameManager.StartingPosition;

            spriteRenderer.color = new Color(1, 1, 1, 1);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + addToLevel);
            yield return StartCoroutine(GameManager.TransitionOut());
            canMove = true;
            //GameManager.StartTransition(0);
        }
       //     gameObject.transform.position = GameManager.StartingPosition;
        
            //Health = MaxHealth;
        //Healthbar.value = MaxHealth;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        insideTrigger = null;
    }
}
