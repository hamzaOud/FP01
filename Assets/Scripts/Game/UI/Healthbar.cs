using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthbarSlider;
    public Slider manaSlider;

    public UnitStats stats;
    public GameObject healthbarPositionObject;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionVector = Camera.main.WorldToScreenPoint(healthbarPositionObject.transform.position);
        this.transform.position = positionVector;
        float sliderValue = (float)stats.currentHP / (float)stats.maxHP;
        
        healthbarSlider.value = sliderValue;
        manaSlider.value = (float)stats.currentMana / (float)stats.maxMana;
    }
}
