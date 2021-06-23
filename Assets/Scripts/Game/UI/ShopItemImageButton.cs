using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemImageButton : Image
{
    [SerializeField]
    public Text pokemonNameText;
    public Text costText;
    public Text classText;
    public Text typeText;

    public Pokemon pokemon;
}