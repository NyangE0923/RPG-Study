using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newItemData) //생성자를 이용해 독립성 부여
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++; //사이즈를 늘리는 메소드

    public void RemoveStack() => stackSize--; //사이즈를 줄이는 메소드
}
