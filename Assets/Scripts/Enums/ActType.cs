using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActType
{
    public static ArrayList ArrayList = new(
        new ActTypeEnum[]
        {
            ActTypeEnum.MOVE, 
            ActTypeEnum.ATTACK
        }
    );
}

public enum ActTypeEnum
{
    MOVE, ATTACK
}