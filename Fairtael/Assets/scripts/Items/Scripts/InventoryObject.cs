using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemBuffs Buffs;
    public List<Inventoryslot> items = new List<Inventoryslot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == _item) 
            {

                items[i].AddAmount(_amount);
                Buffs = GameObject.FindWithTag("buffs").GetComponent<ItemBuffs>();
                Buffs.ChangeStats(items[i].item.acuraty, items[i].item.damageChange,items[i].item.damageChangeDiv, items[i].item.stunChange, items[i].item.firerateChange, items[i].item.firerateChangeDiv,items[i].item.rangeChange, items[i].item.walkspeed, items[i].item.maxHearts, items[i].item.hearts, items[i].item.bulletSpeed);

                hasItem = true;
                break;
            }
        }
        if (!hasItem) 
        {
            items.Add(new Inventoryslot(_item, _amount));
            Buffs = GameObject.FindWithTag("buffs").GetComponent<ItemBuffs>();
            Buffs.ChangeStats(_item.acuraty, _item.damageChange, _item.damageChangeDiv,_item.stunChange, _item.firerateChange,_item.firerateChangeDiv,_item.rangeChange, _item.walkspeed, _item.maxHearts, _item.hearts, _item.bulletSpeed);
        }

    }

}

[System.Serializable]
public class Inventoryslot
{
    public ItemObject item;
    public int amount;

    public Inventoryslot(ItemObject _item, int _amount) 
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}