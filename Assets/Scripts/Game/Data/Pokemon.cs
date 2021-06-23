using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokeClass { Grass, Fire, Water, Normal, Electric, Fighting, Psychic, Dark, Metal, Ground, Poison, Dragon }
public enum PokeType { Field, Monster, Humanoid, Aquatic, Bug, Flying, Flora, Fairy, Mineral, Amorphous }


[CreateAssetMenu(menuName ="Pokemon")]
public class Pokemon : ScriptableObject
{ 
    public Sprite image;
    public int price;
    public int sellPrice;
    public new string name;
    public GameObject model;
    public Pokemon evolution;
    public PokeType[] type;
    public PokeClass[] pokeClass;
}
