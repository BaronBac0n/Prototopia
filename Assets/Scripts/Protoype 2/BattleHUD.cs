using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Slider hpSlider;
    public Slider staminaSlider;
    public Slider manaSlider;

    public void SetHUD(UnitScript unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.stats.maxHP;
        hpSlider.value = unit.stats.currHP;

        staminaSlider.maxValue = unit.stats.maxStamina;
        staminaSlider.value = unit.stats.currStamina;

        manaSlider.maxValue = unit.stats.maxMana;
        manaSlider.value = unit.stats.currMana;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetStamina(int stam)
    {
        staminaSlider.value = stam;
    }

    public void SetMana(int mana)
    {
        manaSlider.value = mana;
    }
}
