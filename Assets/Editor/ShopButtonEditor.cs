using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[UnityEditor.CustomEditor(typeof(ShopItemImageButton))]
public class ShopButtonEditor : UnityEditor.Editor
{

     public override void OnInspectorGUI()
     {
         ShopItemImageButton targetMenuButton = (ShopItemImageButton)target;

        targetMenuButton.costText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("CostText:",targetMenuButton.costText, typeof(Text), true);
        targetMenuButton.pokemonNameText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("PokemonText:", targetMenuButton.pokemonNameText, typeof(Text), true);
        targetMenuButton.classText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("ClassText:", targetMenuButton.classText, typeof(Text), true);
        targetMenuButton.typeText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("TypeText:", targetMenuButton.typeText, typeof(Text), true);
        // Show default inspector property editor
        DrawDefaultInspector();
     }
 }
