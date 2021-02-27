using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Enums;
using System;

public class DialogueRepository : MonoBehaviour
{


    public ConversationObject FetchDialogue(int Id)
    {
        ConversationObject conversationObject = new ConversationObject();
        switch (Id)
        {
            case 0:
                conversationObject.DialogueArray.Add("That was pretty easy, wasn't it?");
                conversationObject.Options.Add(new OptionObject { OptionText = "Yes", DialogueId = 0 });
                conversationObject.Options.Add(new OptionObject { OptionText = "No", DialogueId = 1 });
                break;
            
        }
        return conversationObject;
    }
}
