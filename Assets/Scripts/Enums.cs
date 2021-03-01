using UnityEngine;
using System.Collections;

public class Enums
{

    public enum SoundEffect
    {
        Jump,
        Death,
        ShadowIn,
        ShadowOut,
        Shoot,
        Goal,
        Bloop,
        Click,
        Spring,
        EnemyHit
    }



    public enum Objective
    {
        FindCircle
    }

    public enum Upgrade
    {
        None,
        Shadowstep,
        Raygun
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
}
