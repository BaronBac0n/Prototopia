using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public string actionName;
    public string description;

    public Sprite icon;

    public enum StatCost { STAMINA, MANA, HEALTH, NONE};

    public StatCost statCost;

    public int cost;
    public int damage;

    public string GetInfoText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(cost + " " + statCost.ToString()).AppendLine();
        builder.Append(description);
        return builder.ToString();
    }
}
