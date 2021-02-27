using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class NPC : Interactable
{
    public Npc Name;
    public DialogueManager DialogueManager;
    public GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager = FindObjectOfType<DialogueManager>();
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public override void Interact()
    {
        ConversationObject conversationObject = new ConversationObject();
        //conversationObject.Speaker = Name;
        
        switch (Name)
        {
            //case Npc.Priest:
            //    Objective relevantObjective = GameManager.ObjectiveList.Find(x => x.ObjectiveName == Enums.Objective.FindCircle);
            //    int currentLevel = relevantObjective.LevelCurrent;
                
            //    if(currentLevel == 1)
            //    {
            //        conversationObject.Options.Add(new OptionObject { OptionText = "I'll look for it. (End Conversation)", ActionId = 0, DialogueId = 0 });
            //    }
            //    else if(currentLevel == 2 || currentLevel == 3)
            //    {
            //        conversationObject.Options.Add(new OptionObject { OptionText = "I have it here.", ActionId = 1, DialogueId = 1 });
            //        conversationObject.Options.Add(new OptionObject { OptionText = "I'll look for it. (End Conversation)", ActionId = 0, DialogueId = 0 });
            //    }

            //    switch (currentLevel)
            //    {
            //        case 0:
            //        case 2:
            //            GameManager.CompleteObjective(Enums.Objective.FindCircle);
            //            conversationObject.DialogueArray.Add("Ah, hello. It seems that I've misplaced my lucky circle.");
            //            conversationObject.DialogueArray.Add("Would you be so kind as to retrieve it for me? It's all the way in the next room and that's much too far for me.");
            //            break;
            //        case 1:
            //        case 3:
            //            conversationObject.DialogueArray.Add("Any luck finding my circle?");
            //            break;
            //        default:
            //            conversationObject.DialogueArray.Add("Thank you for returning that for me.");
            //            break;

            //    }
            //    break;
           
            default:
                break;
        }
        StartCoroutine(DialogueManager.StartDialogueLoop(conversationObject));
    }

}
