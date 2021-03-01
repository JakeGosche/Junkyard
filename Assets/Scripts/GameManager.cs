using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class GameManager : MonoBehaviour
{
    public Canvas MenuBackground;
    public int DoorId = 0;
    public Player Player;
    public Image Transition;
    public static bool GmExists;
    public DialogueManager DialogueManager;
    public bool objectivesCompleted = false;
    public Text objectiveText, actionText;
    public bool started;
    public bool testing;
    public AudioManager audioManager;
    public static Vector3 StartingPosition = new Vector3(3, 0.5f, 0);
    public List<Enemy> enemies = new List<Enemy>();
    private void Awake()
    {
        if (!GmExists)
        {
            GmExists = true;
            DontDestroyOnLoad(transform.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Player = FindObjectOfType<Player>();
        //DialogueManager = FindObjectOfType<DialogueManager>();

        ConversationObject conversationObject = new ConversationObject();
        Player.gameObject.transform.position = StartingPosition;

        Player.spriteRenderer.color = new Color(1, 1, 1, 1);
        Player.canMove = false;
        bool startMoving = true;
        enemies.Clear();
        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemies.Add(enemy);
        }
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                //conversationObject.DialogueArray.Add("Hey there!");
                //conversationObject.DialogueArray.Add("I’m Devy! Thanks so much for playing my game!");
                //conversationObject.DialogueArray.Add("This is actually the first game I've ever made.");
                //conversationObject.DialogueArray.Add("I'm having a lot of fun making it...but I'm not too confident yet.");
                //conversationObject.DialogueArray.Add("Do you think you could help me out?");
                //conversationObject.DialogueArray.Add("I made this game for the Wowie jam 3.0.");
                //conversationObject.DialogueArray.Add("It's a bit rough around the edges, but I think you will enjoy.");
                //conversationObject.DialogueArray.Add("Can you playtest it and tell me what you think?");
                //conversationObject.DialogueArray.Add("You will? Thanks! I’ll def buy you lunch!");
                //conversationObject.DialogueArray.Add("Controls are simple. A moves left. D moves right. Use space to jump.");
                //conversationObject.DialogueArray.Add("Alright, let's do this. Just click the play button!");

                //conversationObject.Options.Add(new OptionObject { OptionText = "Play", DialogueId = -1 });
                //startMoving = false;
                //LEVEL 1.0(Game Screens 1 + 2)
                //conversationObject.DialogueArray.Add("Okay, this is level 1. Pretty neat, right ?");
                //conversationObject.DialogueArray.Add("It’s just the beginning, but I feel good about it.");



                //LEVEL 1.1(Game Screen 3, 4, 5)
                //conversationObject.DialogueArray.Add("I updated the level and made it harder.");
                //conversationObject.DialogueArray.Add("Bet you can’t beat it so fast now!");



                //LEVEL 2.0

                //conversationObject.DialogueArray.Add("Ray Gun lets you fire a projectile at enemies.");
                //conversationObject.DialogueArray.Add("Press Left Shirt to fire your LAZER CANNON!!!");
                //conversationObject.DialogueArray.Add("Ah--do people still know that joke?");


                break;
            //case 1:
             //   conversationObject.DialogueArray.Add("I couldn't really think of how to make it any easier...so I just removed all the obstacles.");

               // break;
            case 1:
                Player.UniqueId = 1;
                conversationObject.DialogueArray.Add("Wow, you...you did that pretty fast.");
                conversationObject.Expressions.Add(3);
                conversationObject.DialogueArray.Add("Maybe that level was too easy.");
                                    conversationObject.Expressions.Add(2);
                conversationObject.DialogueArray.Add("Let’s make it a little more challenging, shall we?");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("I bet you can’t beat this level so fast!");
                conversationObject.Expressions.Add(0);


                break;
            case 2:
            case 3:
                Player.canMove = true;
                break;
            case 4:
                conversationObject.DialogueArray.Add("I'm starting to feel confident about this! Nothing can go wrong now!");
                conversationObject.Expressions.Add(0);
                Player.UniqueId = 2;
                    break;
            case 5:
                conversationObject.DialogueArray.Add("Well, this is it!");
                conversationObject.Expressions.Add(3);
                conversationObject.DialogueArray.Add("This is the last level! It needs to be super hard!");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("And believe me! It would have been!");
                conversationObject.Expressions.Add(1);
                conversationObject.DialogueArray.Add("If I hadn't run out of time while making the level.");
                conversationObject.Expressions.Add(0);
                break;
            case 6:
                StartCoroutine(DialogueManager.StartCutscene(3));
                //conversationObject.DialogueArray.Add("You actually made it to the end!");
                //conversationObject.DialogueArray.Add("GASP, games need to have a satisfying ending...!");
                //conversationObject.DialogueArray.Add("Uh...!The real treasure is the friends we made along the way!");
                //conversationObject.DialogueArray.Add("Meh, that’s pretty cliche…");
                //conversationObject.DialogueArray.Add("OH, AND TREASURE!");
                break;
            default:
                break;
        }
        if (conversationObject.DialogueArray.Count > 0)
        {
            StartCoroutine(DialogueManager.StartDialogueLoop(conversationObject, startMoving));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransition(int addToLevel = 1)
    {
        StartCoroutine(SceneTransition(addToLevel));
    }

    public IEnumerator SceneTransition(int addToLevel)
    {
        Player.spriteRenderer.color = new Color(1, 1, 1, 0);
        Player.rigidBody.velocity = new Vector2(0, 0);
        yield return StartCoroutine(TransitionIn());
        
        Player.gameObject.transform.position = StartingPosition;

        Player.spriteRenderer.color = new Color(1, 1, 1, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + addToLevel);

        yield return StartCoroutine(TransitionOut());
       // Player.canMove = true;
    }

    public IEnumerator TransitionOut()
    {
        while (Transition.fillAmount > 0)
        {
            Transition.fillAmount = Mathf.Max(0, Transition.fillAmount - (3 * Time.deltaTime));

            yield return null;
        }

        yield break;
    }

    public IEnumerator TransitionIn()
    {

            while (Transition.fillAmount < 1)
            {
                Transition.fillAmount = Mathf.Min(1, Transition.fillAmount + (3 * Time.deltaTime));
                yield return null;
            }

        yield break;
    }


}
