using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text healthTracker;
    public Text stamTracker;
    public Text manaTracker;
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

        healthTracker.text = hpSlider.maxValue + "/" + hpSlider.maxValue;
        stamTracker.text = staminaSlider.maxValue + "/" + hpSlider.maxValue;
        manaTracker.text = manaSlider.maxValue + "/" + manaSlider.maxValue;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        healthTracker.text = hp + "/" + hpSlider.maxValue;
    }

    public void SetStamina(int stam)
    {
        staminaSlider.value = stam;
        stamTracker.text = stam + "/" + staminaSlider.maxValue;
    }

    public void SetMana(int mana)
    {
        manaSlider.value = mana;
        manaTracker.text = mana + "/" + manaSlider.maxValue;
    }

    public IEnumerator FlashText(Text text, Color originalColor, Color newColor, int flashTimes)
    {
        for (int i = 0; i < flashTimes; i++)
        {
            text.color = newColor;
            yield return new WaitForSeconds(.1f);
            text.color = originalColor;
            yield return new WaitForSeconds(.1f);

        }
        text.color = originalColor;
    }
}
