using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class FurnitureItem : Interactable
{
    public int Id;
    public DialogueManager DialogueManager;
    public GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager = FindObjectOfType<DialogueManager>();
        GameManager = FindObjectOfType<GameManager>();
        SetActive();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActive()
    {
        switch (Id)
        {
            case 1:
                //Objective relevantObjective = GameManager.ObjectiveList.Find(x => x.ObjectiveName == Enums.Objective.FindCircle);
                //gameObject.SetActive(relevantObjective.LevelCurrent < 2);
                break;
        }
        
    }

    public override void Interact()
    {
        switch (Id)
        {
            case 1:
               // Objective relevantObjective = GameManager.ObjectiveList.Find(x => x.ObjectiveName == Enums.Objective.FindCircle);
               //// GameManager.CompleteObjective(Enums.Objective.FindCircle, 2);
               // GameManager.Player.insideTrigger = null;
               // gameObject.SetActive(false);
               // GameManager.Player.canMove = true;
               break;
        }
    }

}
