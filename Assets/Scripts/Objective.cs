using UnityEngine;
using System.Collections;
using static Enums;

public class Objective
{
    public bool Completed = false;
    public Enums.Objective ObjectiveName;
    public int LevelCurrent = 0;
    public int LevelNeeded = 0;
    public string[] DisplayString;
}
