using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using static Enums;

public class DialogueManager : MonoBehaviour
{
    public const int LineLength = 40;
    public GameObject dialogueBox, optionPointer, speakerBox, optionsObject, button1, button2, playButton;
    public Text dialogueText, speakerText, title;
    public Image selectArrow;
    private List<string> dialogueLines;
    private bool typing = false;
    public Text dialogueInstruction;
    private bool optionsDisplayed;
    private int selectedOption, optionsCount;
    public List<Text> options;
    private bool dialogueRunning = false;
    private List<OptionObject> optionObjs;
    public DialogueRepository DialogueRepository;
    public Player Player;
    public GameManager GameManager;
    public AudioSource bgMusic;
    public AudioManager audioManager;
    public static int Option1Value, Option2Value;
    public List<Sprite> devySprites;
    public List<int> expressions;
    public Image image;

    void Update()
    {
       
    }

    public IEnumerator StartDialogueLoop(ConversationObject dialogueToDisplay, bool moveAgain = false)
    {
        dialogueText.text = "";
        dialogueBox.SetActive(true);
        optionObjs = dialogueToDisplay.Options;
        optionsCount = optionObjs.Count;
        dialogueLines = dialogueToDisplay.DialogueArray;
        expressions.Clear();
        expressions = dialogueToDisplay.Expressions;
            
        yield return StartCoroutine(StartDisplayDialogue());

        if (dialogueToDisplay.Options.Count == 2)
        {
            button1.GetComponentInChildren<Text>().text = dialogueToDisplay.Options[0].OptionText;
            button2.GetComponentInChildren<Text>().text = dialogueToDisplay.Options[1].OptionText;
            button1.SetActive(true);
            button2.SetActive(true);
            Option1Value = dialogueToDisplay.Options[0].DialogueId;
            Option2Value = dialogueToDisplay.Options[1].DialogueId;
        }
        else if(dialogueToDisplay.Options.Count == 1)
        {
            playButton.GetComponentInChildren<Text>().text = "Play";
            playButton.SetActive(true);
            //button1.GetComponentInChildren<Text>().text = "Play";
            //button1.SetActive(true);
            Option1Value = dialogueToDisplay.Options[0].DialogueId;
        }
        else
        {
            dialogueText.text = "";
            dialogueBox.SetActive(false);
        }

        //dialogueBox.SetActive(false);
        //speakerBox.SetActive(false);
        
        if (moveAgain)
        {
            Player.canMove = true;
        }

    }

    IEnumerator StartDisplayDialogue()
    {
        //dialogueInstruction.text = "";
        dialogueRunning = true;
        int dialogueNumber = 0;
        int portraitNumber = 0;
        bool showingOptions = false;
        if(expressions.Count > portraitNumber)
        {
            image.sprite = devySprites[expressions[portraitNumber]];
        }
        yield return StartCoroutine(TypeOutDialogue(dialogueLines[dialogueNumber]));
     
        yield return new WaitForSeconds(0.02f);
        while (dialogueRunning)
        {
            if (dialogueNumber >= dialogueLines.Count)
            {
                dialogueRunning = false;
                yield break;
            }

            if ((dialogueNumber == (dialogueLines.Count - 1)) && (optionObjs.Count > 0) && !optionsDisplayed)
            {
                optionsDisplayed = true;
                //button1.GetComponentInChildren<Text>().text = optionObjs[0].OptionText;
                //button2.GetComponentInChildren<Text>().text = optionObjs[1].OptionText;
                //button1.SetActive(true);
                //button2.SetActive(true);
                //Option1Value = optionObjs[0].DialogueId;
                //Option2Value = optionObjs[1].DialogueId;
                showingOptions = true;
            }

            while (!Input.GetKeyDown(KeyCode.Space) && !showingOptions)
            {
                yield return null;
            }

            //if (optionsDisplayed)
            //{
            //    optionsDisplayed = false;
            //}
            dialogueInstruction.text = "";
            dialogueNumber++;
            portraitNumber++;
            if (dialogueNumber < dialogueLines.Count)
            {
                if (expressions.Count > portraitNumber)
                {
                    image.sprite = devySprites[expressions[portraitNumber]];
                }
                yield return StartCoroutine(TypeOutDialogue(dialogueLines[dialogueNumber]));
            }
            else
            {
                dialogueRunning = false;
            }


            yield return null;
        }
    }

    //public void ShowOptions()
    //{
    //    optionsDisplayed = true;
    //    ToggleOptions(true);
    //    foreach (Text t in options)
    //    {
    //        t.enabled = false;
    //    }
    //    for (int i = 0; i < optionObjs.Count; i++)
    //    {
    //        Text t = options[i];
    //        t.enabled = true;
    //        t.text = optionObjs[i].OptionText;
    //    }
    //    dialogueInstruction.text = "Press Space to Select";
    //    selectArrow.enabled = true;
    //    selectedOption = 0;
    //    MoveArrow();
    //}

    IEnumerator TypeOutDialogue(string line)
    {
        typing = true;
        dialogueText.text = "";

        int currentSubstring = 0;
        string finishedString = string.Empty;
        while (currentSubstring + LineLength <= line.Length)
        {
            int lastSpace = 0;
            if (line.Substring(currentSubstring, LineLength).Contains(Environment.NewLine))
            {
                lastSpace = line.Substring(currentSubstring, LineLength).LastIndexOf(Environment.NewLine);
                finishedString = finishedString + line.Substring(currentSubstring, lastSpace + 1);
            }
            else
            {
                lastSpace = line.Substring(currentSubstring, LineLength).LastIndexOf(' ');
                finishedString = finishedString + line.Substring(currentSubstring, lastSpace) + Environment.NewLine;

            }
            currentSubstring = currentSubstring + lastSpace + 1;

        }
        finishedString = finishedString + line.Substring(currentSubstring);

        int i = 0;
        int noiseCounter = 7;
        while (i < finishedString.Length)
        {

            yield return new WaitForSecondsRealtime(.02f);
            if (typing == false)
            {
                dialogueInstruction.text = "Press space to Continue";
                dialogueText.text = finishedString;
                typing = false;
                yield break;
            }
            dialogueText.text = dialogueText.text + finishedString[i];
            if (noiseCounter >= 7)
            {
                noiseCounter = 0;

                audioManager.PlaySfx(SoundEffect.Bloop);
            }
            else
            {
                noiseCounter++;
            }
            i++;

        }
        dialogueInstruction.text = "Press space to Continue";
        typing = false;
        dialogueText.text = finishedString;
        yield break;
    }



    public void ButtonSelected(int selected)
    {
        audioManager.PlaySfx(SoundEffect.Click);
        optionsDisplayed = false;
        dialogueText.text = "";
        int choice;
        if (selected == 0) {
            choice = Option1Value; }
        else if (selected == 1) { 
            choice = Option2Value;
        }
        else{
                choice = -1;
            //button1.gameObject.SetActive(false);
             playButton.gameObject.SetActive(false);
        }
        //if (choice >= 0)
        //{
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
        //}
        switch (choice)
        {
            case -1:
                StartCoroutine(StartCutscene(-1));
                break;
            case 0:
                GameManager.StartTransition();
                break;
            case 1:
                GameManager.StartTransition();
                break;
            case 2:
                //Add a platform
                StartCoroutine(StartCutscene(4));
              //  GameObject invisibleTile = GameObject.Find("Platform");

              ////  invisibleTile.transform.position = new Vector3(27.5f, -1.5f, 0);
              //  ConversationObject conversationObject3 = new ConversationObject();
              //  conversationObject3.DialogueArray.Add("Okay, let's use some platforms!");
              //  conversationObject3.DialogueArray.Add("I like it. It feels...well, like a game!");

              //  StartCoroutine(StartDialogueLoop(conversationObject3, true));
                //Player.canMove = true;
             
               
                break;
            case 3:
                //Adds a spring
                StartCoroutine(StartCutscene(5));
                //ConversationObject conversationObject4 = new ConversationObject();
                //conversationObject4.DialogueArray.Add("Spring it is!");
                //conversationObject4.DialogueArray.Add("Alright, it took a bit of coding, but you should be able to bounce across now!");
                //GameObject spring = GameObject.Find("Spring");
                //spring.transform.position = new Vector3(19.5f, .5f, 0);
                ////Player.canMove = true;

                //StartCoroutine(StartDialogueLoop(conversationObject4, true));
                break;
            case 4:
                //Adds attack;
                Player.upgrade = Upgrade.Shadowstep;
                ConversationObject conversationObject = new ConversationObject();
                //enable meter;
                conversationObject.DialogueArray.Add("Shadowstep lets you phase past enemies.");
                conversationObject.Expressions.Add(2);
                conversationObject.DialogueArray.Add("If you press left shift you'll be able to move through enemies for a couple seconds. Try it out!");
                conversationObject.Expressions.Add(0);
                Player.slider.gameObject.SetActive(true);
                //StartCoroutine(GameManager.TransitionOut());
                //Player.canMove = true;
                StartCoroutine(StartDialogueLoop(conversationObject, true));
                break;
            case 5:
                //Adds shadowstep;
                Player.upgrade = Upgrade.Raygun;
                ConversationObject conversationObject2 = new ConversationObject();
                //enable meter;
                conversationObject2.DialogueArray.Add("The ray gun lets you fire a projectile at enemies.");
                conversationObject2.Expressions.Add(2);
                conversationObject2.DialogueArray.Add("Press left shift to fire your LAZER CANNON!!!");
                conversationObject2.Expressions.Add(0);
                conversationObject2.DialogueArray.Add("Ah--do people still know that joke?");
                conversationObject2.Expressions.Add(2);
                //StartCoroutine(GameManager.TransitionOut());
                //Player.canMove = true;
                StartCoroutine(StartDialogueLoop(conversationObject2, true));
                break;
        }
    }

    public IEnumerator StartCutscene(int id)
    {
        switch (id)
        {
            case -1:
                //hide title screen

                title.text = "";
                title.gameObject.SetActive(false);
                if (GameManager.testing)
                {
                    Player.canMove = true;
                }
                else { 
                    yield return new WaitForSecondsRealtime(1);
                    bgMusic.Stop();
                //cut sound
                yield return new WaitForSecondsRealtime(1);
                ConversationObject conversationObject1 = new ConversationObject();
                conversationObject1.DialogueArray.Add("...");
                    conversationObject1.Expressions.Add(3);
                    conversationObject1.DialogueArray.Add("Um...");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("Hey there!");
                    conversationObject1.Expressions.Add(0);
                    conversationObject1.DialogueArray.Add("I’m Devy! I'm the developer for this game. Thank you so much for playing!");
                    conversationObject1.Expressions.Add(0);
                    conversationObject1.DialogueArray.Add("This is a little embarrassing. When you clicked that button it was supposed to start the game.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("But, um, it looks like it's not working like it's supposed to.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("To be perfectly honest this is actually the first time I've ever made a game. I made it for Wowie jam 3.0.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("I had a lot of fun making it...but I'm still running into some issues.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("Do you think you could help me out by playtesting and telling me what you think?");
                    conversationObject1.Expressions.Add(3);
                    conversationObject1.DialogueArray.Add("You will? Thanks! My first ever playtester! I’ll def buy you lunch!");
                    conversationObject1.Expressions.Add(0);
                    conversationObject1.DialogueArray.Add("So the controls are simple. A moves left. D moves right. Use space to jump.");
                    conversationObject1.Expressions.Add(0);
                    conversationObject1.DialogueArray.Add("Oh, right. I forgot the button that starts the game isn't working.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("I'll just go ahead and start it manually. I'll fix the button later.");
                    conversationObject1.Expressions.Add(2);
                    conversationObject1.DialogueArray.Add("Have fun!");
                    conversationObject1.Expressions.Add(0);
                    yield return StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject1, true));
                    bgMusic.Play();
                }
                break;
            case 1:
                break;
            case 2:
                ConversationObject conversationObject = new ConversationObject();
                conversationObject.DialogueArray.Add("How would you like to get past the enemy?");
                conversationObject.Options.Add(new OptionObject { OptionText = "Attack", DialogueId = 4 });
                conversationObject.Options.Add(new OptionObject { OptionText = "Stealth", DialogueId = 5 });
                StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObject));
                break;
            case 3:
                ConversationObject conversationObjectEnd = new ConversationObject();
                conversationObjectEnd.DialogueArray.Add("You actually made it to the end!");
                conversationObjectEnd.Expressions.Add(0);
                conversationObjectEnd.DialogueArray.Add("GASP, games need to have a satisfying ending...!");
                conversationObjectEnd.Expressions.Add(3);
                conversationObjectEnd.DialogueArray.Add("Uh...here!");
                conversationObjectEnd.Expressions.Add(2);
                yield return StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObjectEnd));
                title.text = "The real treasure is the friends we made along the way!";
                title.gameObject.SetActive(true);
                audioManager.PlaySfx(SoundEffect.Goal);
                ConversationObject conversationObjectEnd2 = new ConversationObject();
                conversationObjectEnd2.DialogueArray.Add("Meh, that’s pretty cliche.");
                conversationObjectEnd2.Expressions.Add(2);
                conversationObjectEnd2.DialogueArray.Add("How about...");
                conversationObjectEnd2.Expressions.Add(2);
                yield return StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObjectEnd2));
                title.text = "The real treasure is the friends we made along the way! And also the treasure!";

                GameObject treasure = GameObject.Find("Treasure");
                treasure.transform.position = new Vector3(3, .5f, -1);
                audioManager.PlaySfx(SoundEffect.Goal);
                yield return new WaitForSecondsRealtime(2);
                ConversationObject conversationObjectEnd3 = new ConversationObject();
                conversationObjectEnd3.DialogueArray.Add("Much better!");
                conversationObjectEnd3.Expressions.Add(0);
                yield return StartCoroutine(GameManager.DialogueManager.StartDialogueLoop(conversationObjectEnd3));
                yield return new WaitForSecondsRealtime(2);
                title.text = "";
                title.color = Color.white;
                yield return StartCoroutine(GameManager.TransitionIn());
                title.text = "Thank you for playing!" + Environment.NewLine + Environment.NewLine + "Amanda Swiger (Aswagger) - Producer" + Environment.NewLine + "Emily Woods - Art" + Environment.NewLine + "Giordano Nin - Game Design" + Environment.NewLine + "Jake Gosche (Goatcheese) - Programming" + Environment.NewLine + "Ostrich2401 - Sound Design";
                break;
            case 4:
                //Adds attack;
                ConversationObject conversationObject4 = new ConversationObject();
                //enable meter;
                conversationObject4.DialogueArray.Add("Okay, let's add some platforms!");
                conversationObject4.Expressions.Add(0);
                yield return StartCoroutine(StartDialogueLoop(conversationObject4));
                GameManager.StartTransition();
               
                break;
            case 5:
                //Adds attack;
                ConversationObject conversationObject5 = new ConversationObject();
                //enable meter;
                conversationObject5.DialogueArray.Add("Okay, let's add some springs!");
                conversationObject5.Expressions.Add(0);
                yield return StartCoroutine(StartDialogueLoop(conversationObject5));
                GameManager.StartTransition(2);
                break;
        }
        yield return null;
    }
}
