using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using static Enums;

public class DialogueManager : MonoBehaviour
{
    public const int LineLength = 65;
    public GameObject dialogueBox, optionPointer, speakerBox, optionsObject, button1, button2;
    public Text dialogueText, speakerText;
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
    public static int Option1Value, Option2Value;
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
            
        yield return StartCoroutine(StartDisplayDialogue());

        if (dialogueToDisplay.Options.Count == 2)
        {
            button1.GetComponentInChildren<Text>().text = dialogueToDisplay.Options[0].OptionText;
            button2.GetComponentInChildren<Text>().text = dialogueToDisplay.Options[1].OptionText;
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);
            Option1Value = dialogueToDisplay.Options[0].DialogueId;
            Option2Value = dialogueToDisplay.Options[1].DialogueId;
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
        yield return StartCoroutine(TypeOutDialogue(dialogueLines[0]));
        dialogueRunning = true;
    }

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
                //dialogueInstruction.text = "Press space to Continue";
                dialogueText.text = finishedString;
                typing = false;
                yield break;
            }
            dialogueText.text = dialogueText.text + finishedString[i];
            if (noiseCounter >= 7)
            {
                noiseCounter = 0;
                //Play Sound?
            }
            else
            {
                noiseCounter++;
            }
            i++;

        }
        //dialogueInstruction.text = "Press space to Continue";
        typing = false;
        dialogueText.text = finishedString;
        yield break;
    }



    public void ButtonSelected(int selected)
    {
        int choice = selected == 0 ? Option1Value : Option2Value;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        switch (choice)
        {
            case 0:
                GameManager.StartTransition(2);
                break;
            case 1:
                GameManager.StartTransition();
                break;
            case 2:
                //Add a platform
                GameObject invisibleTile = GameObject.Find("InvisibleTile");
                invisibleTile.transform.position = new Vector3(30, -1.5f, 0);
                Player.canMove = true;
             
               
                break;
            case 3:
                //Adds a spring
                GameObject spring = GameObject.Find("Spring");
                spring.transform.position = new Vector3(19.5f, .5f, 0);
                Player.canMove = true;

                break;
        }
    }
}
