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
    public int heal;
    public int healStam;
    public int healMana;

    public string GetInfoText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(cost + " " + statCost.ToString()).AppendLine();
        builder.Append(description).AppendLine();

        if (damage > 0)
        {
            builder.Append("Deals " + damage + " damage").AppendLine();
        }

        if (heal > 0)
        {
            builder.Append("Heals " + heal + " damage").AppendLine();
        }

        if (healStam > 0)
        {
            builder.Append("Restores " + healStam + " stamina").AppendLine();
        }

        if (healMana > 0)
        {
            builder.Append("Restores " + healMana + " mana").AppendLine();
        }

        return builder.ToString();
    }
}
