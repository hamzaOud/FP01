using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellPokemon : MonoBehaviour
{
    private bool isHovering;
    public Pokemon pokemon;
    private ShopController shopController;


    // Start is called before the first frame update
    void Start()
    {
        shopController = GameObject.Find("ShopCanvas").GetComponent<ShopController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sell champ
        if (isHovering && Input.GetKeyDown(KeyCode.E))
        {
            Data.Instance.trainer.balance += pokemon.sellPrice;
            shopController.updateUI();
            RemoveObjectFromData();
            Destroy(this.gameObject);
        }
    }

    void RemoveObjectFromData()
    {
        Data.Instance.trainer.pokedex[pokemon].Remove(this.gameObject);

        //If the player no longer has one of these pokemons, remove the gameobject list of this pokemon from the pokedex
        if(Data.Instance.trainer.pokedex[pokemon].Count == 0)
        {
            Data.Instance.trainer.pokedex.Remove(pokemon);
        }
    }

    private void OnMouseEnter()
    {
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}
