using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public string actionName;

    public enum StatCost { STAMINA, MANA, HEALTH, NONE};

    public StatCost statCost;

    public int cost;
}
