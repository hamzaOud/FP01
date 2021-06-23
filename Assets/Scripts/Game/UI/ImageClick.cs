using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageClick : MonoBehaviour , IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{

    public GameController gameController;
    public ShopController shopController;
     
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameController.data.trainer.balance >= GetComponent<ShopItemImageButton>().pokemon.price)
        {
            gameController.spawnChampion(GetComponent<ShopItemImageButton>().pokemon);
            gameController.data.trainer.balance -= GetComponent<ShopItemImageButton>().pokemon.price;
            shopController.updateUI();
            this.gameObject.SetActive(false);
        }    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        shopController = GameObject.Find("ShopCanvas").GetComponent<ShopController>();
    }

}