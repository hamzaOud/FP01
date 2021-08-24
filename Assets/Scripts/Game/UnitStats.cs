using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{


    private int currentHP = 100;
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public int maxHP = 100;
    
    private int currentMana = 0;
    public int CurrentMana
    {
        get { return currentMana; }
        set { currentMana = value; }
    }
    public int maxMana = 100;

    public int Attack;
    public int Defense;
    public int SpecialAttack;
    public int SpecialDefense;
    public int AttackSpeed;


    public void Reset()
    {
        currentHP = maxHP;
        currentMana = 0;
    }

}
