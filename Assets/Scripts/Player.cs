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
    public float jumpHeight = 80;
    public float moveSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool canMove, shadowActive;
    public static bool PlayerExists;
    public Interactable insideTrigger;
    public GameManager GameManager;
    public Camera playerCamera;
    public float abilityInterval = 0f;
    private float LastMove = 7.0f;
    public bool jumping = false;
    public GameObject Bullet;
    public float Health = .3f;
    public float MaxHealth = .3f;
    public float bulletSpeed, bulletTime;
    public Slider Healthbar;
    public int UniqueId;
    public Upgrade upgrade;
    public SpriteRenderer spriteRenderer;
    public GameObject feet;
    public AudioManager audioManager;
    public Image sliderImage;
    public Text sliderText;
    public Slider slider;
    public Animator animator;
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
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    GameManager.StartTransition();
        //}
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
                xForce = direction * moveSpeed * Time.deltaTime;// * .02f;
                //rigidBody.AddForce(new Vector2(direction * .02f, 0), ForceMode2D.Force);
                //rigidBody.MovePosition(gameObject.transform.position += new Vector3(direction * Time.deltaTime, 0));
                //rigidBody.velocity = new Vector2(direction, 0);
                LastMove = direction;
                transform.rotation = new Quaternion(transform.rotation.x, direction > 0 ? 0 : 180, transform.rotation.z, transform.rotation.w);
                moving = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                audioManager.PlaySfx(SoundEffect.Jump);
                yForce = jumpHeight;
                rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                jumping = true;
                animator.SetBool("IsJumping", true);
            }

            rigidBody.AddForce(new Vector2(xForce, yForce));
            if (upgrade != Upgrade.None)
            {
                if(shadowActive)
                {
                    if(abilityInterval <= 0)
                    {
                        audioManager.PlaySfx(SoundEffect.ShadowOut);
                        gameObject.layer = 8;
                        feet.gameObject.layer = 8;
                        shadowActive = false;
                        spriteRenderer.color = new Color(1, 1, 1, 1);
                        slider.gameObject.SetActive(true);
                        sliderImage.color = Color.red;
                        sliderText.text = "Recharging";
                        abilityInterval = shadowRecharge;
                    }
                }

                if (abilityInterval > 0)
                {
                    abilityInterval -= Time.deltaTime;
                    if(abilityInterval <= 0)
                    {
                        if (upgrade == Upgrade.Shadowstep && !shadowActive)
                        {
                            sliderImage.color = Color.green;
                            sliderText.text = "Ready!";
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (upgrade == Upgrade.Raygun)
                    {
                        audioManager.PlaySfx(SoundEffect.Shoot);
                        GameObject bulletObject = Instantiate(Bullet, gameObject.transform.position + new Vector3(LastMove > 0 ? 1 : -1, 0, 0), Quaternion.identity);
                        BulletScript bs = bulletObject.GetComponent<BulletScript>();
                        bs.xSpeed = LastMove * 2 * bulletSpeed;
                        bs.ySpeed = 0;
                        bs.TimeAlive = .2f * bulletTime;
                        abilityInterval = raygunRecharge;
                    }
                    else
                    {
                        slider.gameObject.SetActive(false);
                        spriteRenderer.color = new Color(1, 1, 1, .5f);
                        shadowActive = true;
                        gameObject.layer = 10;
                        feet.gameObject.layer = 10;
                        abilityInterval = shadowRecharge;
                        audioManager.PlaySfx(SoundEffect.ShadowIn);
                    }
                }
            }

            animator.SetBool("IsMoving", moving);
            //if (!jumping && !moving)
            //{
            //    //rigidBody.velocity = new Vector2(0, 0);
            //    if (insideTrigger != null)
            //    {
            //        displayInfo = true;
            //        if (Input.GetKey(KeyCode.W))
            //        {
            //            rigidBody.velocity = new Vector2(0, 0);
            //            canMove = false;
            //            insideTrigger.Interact();
            //        }
            //    }
            //}
        }
        else
        {
           // rigidBody.velocity = new Vector3(0, 0);
        }
        GameManager.actionText.gameObject.SetActive(displayInfo);
        playerCamera.transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z);
    }

    public void TakeDamage()
    {
        audioManager.PlaySfx(SoundEffect.Death);
        StartCoroutine(ResetScene(true));

        //if (SceneManager.GetActiveScene().buildIndex == 4 && UniqueId == 2)
        //{
        //    canMove = false;
        //    UniqueId = 0;
        //    ConversationObject conversationObject = new ConversationObject();
        //    conversationObject.DialogueArray.Add("Oh, I guess that jump was too far. Sorry. Hmmm, how should I fix this?");
        //    conversationObject.Options.Add(new OptionObject { OptionText = "Add a platform?", DialogueId = 2 });
        //    conversationObject.Options.Add(new OptionObject { OptionText = "Add a spring?", DialogueId = 3 });
        //    StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
        //}
        //Health -= .1f;
        //Healthbar.value = Health;
        //if(Healthbar.value <= 0)
        //{
        //    ResetScene();
        //}
    }

    public IEnumerator PauseDialogue()
    {
        canMove = false;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        rigidBody.velocity = new Vector2(0, 0);
        yield return StartCoroutine(GameManager.TransitionIn());

        gameObject.transform.position = GameManager.StartingPosition;

        spriteRenderer.color = new Color(1, 1, 1, 1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + addToLevel);
        yield return StartCoroutine(GameManager.TransitionOut());
        canMove = true;
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
            
            //if (UniqueId == 1)
            //{
            //    canMove = false;
            //    gameObject.transform.position = GameManager.StartingPosition;
            //    UniqueId = 0;
                
            //    ConversationObject conversationObject = new ConversationObject();
            //    conversationObject.DialogueArray.Add("Oh, you can’t beat it at all now...");
            //    conversationObject.DialogueArray.Add("I bet I can add something to make it passable.");
            //    conversationObject.DialogueArray.Add("But I'm just not sure what...");
            //    conversationObject.Options.Add(new OptionObject { OptionText = "Add a platform?", DialogueId = 2 });
            //    conversationObject.Options.Add(new OptionObject { OptionText = "Add a spring?", DialogueId = 3 });
            //    StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
            //    //this.gameObject.SetActive(false);
            //}
            //else if(UniqueId == 2)
            //{
            //    ConversationObject conversationObject = new ConversationObject();
           
            //    conversationObject.DialogueArray.Add("Wait, wait, wait.");
            //    conversationObject.DialogueArray.Add("What do you mean...oh, you need some kind of ability to get past that enemy.");
            //    conversationObject.DialogueArray.Add("Sorry, I should’ve thought of that.");
            //    conversationObject.DialogueArray.Add("Let me think. Hm...");
            //    conversationObject.DialogueArray.Add("So I can give you an attack to use against that enemy. Maybe like a little ray gun.");
            //    conversationObject.DialogueArray.Add("Or if you're the stealthy type, we could try an ability that lets you pass through the enemy unharmed. We'll call that shadowstep.");
            //    conversationObject.DialogueArray.Add("So what'll it be? Ray gun or shadowstep?");
            //    conversationObject.Options.Add(new OptionObject { OptionText = "Ray gun", DialogueId = 4 });
            //    conversationObject.Options.Add(new OptionObject { OptionText = "Shadowstep", DialogueId = 5 });
            //    StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
            //}
            //else
            //{

                StartCoroutine(ResetScene(true));
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //}
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

    public IEnumerator ResetScene(bool startMoving = false)
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsJumping", false);
        if (canMove)
        {
            foreach(Enemy enemy in GameManager.enemies)
            {
                enemy.gameObject.SetActive(true);
            }
            canMove = false;
           
            spriteRenderer.color = new Color(1, 1, 1, 0);
            rigidBody.velocity = new Vector2(0, 0);
            yield return StartCoroutine(GameManager.TransitionIn());

            gameObject.transform.position = GameManager.StartingPosition;
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return StartCoroutine(GameManager.TransitionOut());
            if (UniqueId == 3)
            {
                UniqueId = 0;
                ConversationObject conversationObject = new ConversationObject();
                conversationObject.DialogueArray.Add("Wait, wait, wait.");
                conversationObject.Expressions.Add(2);
                conversationObject.DialogueArray.Add("Oh, you need some kind of ability to get past that enemy.");
                conversationObject.Expressions.Add(3);
                conversationObject.DialogueArray.Add("Sorry, I should’ve thought of that.");
                conversationObject.Expressions.Add(3);
                conversationObject.DialogueArray.Add("Let me think. Hm...");
                conversationObject.Expressions.Add(2);
                conversationObject.DialogueArray.Add("So I can give you an attack to use against that enemy. Maybe like a little ray gun.");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("Or if you're the stealthy type, we could try an ability that lets you pass through the enemy unharmed. We'll call that shadowstep.");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("So what'll it be? Ray gun or shadowstep?");
                conversationObject.Expressions.Add(0);
                conversationObject.Options.Add(new OptionObject { OptionText = "Shadowstep", DialogueId = 4 });
                conversationObject.Options.Add(new OptionObject { OptionText = "Ray gun", DialogueId = 5 });
                StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
            }
            else if(UniqueId == 1)
            {
                UniqueId = 0;
                ConversationObject conversationObject = new ConversationObject();
                conversationObject.DialogueArray.Add("Oh, maybe it's too hard now...");
                conversationObject.Expressions.Add(2);
                conversationObject.DialogueArray.Add("I bet I can add some things to the scene to make it more passable.");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("But I'm just not sure what...");
                conversationObject.Expressions.Add(2);
                conversationObject.Options.Add(new OptionObject { OptionText = "Add platforms?", DialogueId = 2 });
                conversationObject.Options.Add(new OptionObject { OptionText = "Add springs?", DialogueId = 3 });
                StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
            
                //StartCoroutine()
            }
            else
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + addToLevel);

                canMove = true;
            }


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
