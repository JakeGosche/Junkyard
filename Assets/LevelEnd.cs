using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManager GameManager;

    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null && !active) //&& jumping)
        {
            active = true;
            Player player = other.gameObject.GetComponent<Player>();
            player.canMove = false;
            GameManager.audioManager.PlaySfx(Enums.SoundEffect.Goal);
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameManager.StartTransition(2);
                //ConversationObject conversationObject = new ConversationObject();
                //conversationObject.DialogueArray.Add("That was pretty easy, wasn't it?");
                //conversationObject.Options.Add(new OptionObject { OptionText = "Yes", DialogueId = 0 });
                //conversationObject.Options.Add(new OptionObject { OptionText = "No", DialogueId = 1});
                //StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
            }
            else
            {
                GameManager.StartTransition();
            }
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
