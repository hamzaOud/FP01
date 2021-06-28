using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthbarSlider;

    private UnitStats stats;
    public GameObject healthbarPositionObject;

    private void Start()
    {
        stats = this.gameObject.GetComponent<UnitStats>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionVector = Camera.main.WorldToScreenPoint(healthbarPositionObject.transform.position);
        healthbarSlider.transform.position = positionVector;
        float sliderValue = (float)stats.currentHP / (float)stats.maxHP;

        healthbarSlider.value = sliderValue;
    }
}
