using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public int currentHP = 100;
    public int maxHP = 100;
    public int currentMana = 0;
    public int maxMana = 100;


    public void Reset()
    {
        currentHP = maxHP;
        currentMana = 0;
    }

}
