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
    public static Vector3 StartingPosition = new Vector3(3, 0.5f, 0);
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

        Player.canMove = true;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                conversationObject.DialogueArray.Add("Ah, hello. You must be the new game tester.");

                break;
            case 1:
                conversationObject.DialogueArray.Add("I couldn't really think of how to make it any easier...so I just removed all the obstacles.");

                break;
            case 2:
                Player.UniqueId = 1;
                conversationObject.DialogueArray.Add("Okay this one should be a littler harder.");
                

                break;
            case 3:
                conversationObject.DialogueArray.Add("Keep going!");


                break;
            case 4:
                conversationObject.DialogueArray.Add("I'm starting to feel confident about this! Nothing can go wrong now!");
                Player.UniqueId = 2;
                    break;
            case 5:
                conversationObject.DialogueArray.Add("I'll make this one even more difficult.");
                break;
            case 6:
                conversationObject.DialogueArray.Add("You win!");
                break;
            default:
                break;
        }
        if (conversationObject.DialogueArray.Count > 0)
        {
            StartCoroutine(DialogueManager.StartDialogueLoop(conversationObject));
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
        Player.canMove = true;
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
