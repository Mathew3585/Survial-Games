using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item" , menuName ="Items/NewItem")]
public class ItemData : ScriptableObject
{
    public string name;
    public string desciption;
    public Sprite visual;
    public GameObject prefabs;

    public ItemType ItemType;
    public EquipmentType equipementType;
}

public enum ItemType
{
    Ressource,
    Equipement,
    Consuamble,
}

public enum EquipmentType
{
    Head,
    Chest,
    Weapon,
    Legs,
    BagPack,

}