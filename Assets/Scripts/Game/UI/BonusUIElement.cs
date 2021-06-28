using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusUIElement : MonoBehaviour
{
    public Text bonusName;
    public Text bonusCount;
    public Image bonusIcon;
    public GameObject parentCanvas;

    private void Start()
    {
        parentCanvas = GameObject.Find("BonusPanel");
    }

    public void DeleteElement()
    {
        Destroy(this.gameObject);
    }
}
