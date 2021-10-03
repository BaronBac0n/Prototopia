using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Tooltip tooltip;
    public enum tooltipType { ACTION1, ACTION2, ACTION3, ACTION4 };
    public tooltipType type;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type.ToString() == "ACTION1")
        {
            tooltip.DisplayActionInfo(BattleSystem.instance.playerUnit.actions[0]);
        }
        else if (type.ToString() == "ACTION2")
        {
            tooltip.DisplayActionInfo(BattleSystem.instance.playerUnit.actions[1]);
        }
        else if (type.ToString() == "ACTION3")
        {
            tooltip.DisplayActionInfo(BattleSystem.instance.playerUnit.actions[2]);
        }
        else if (type.ToString() == "ACTION4")
        {
            tooltip.DisplayActionInfo(BattleSystem.instance.playerUnit.actions[3]);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideInfo();
    }
}
