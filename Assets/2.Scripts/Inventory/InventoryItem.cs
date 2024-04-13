using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newItemData) //�����ڸ� �̿��� ������ �ο�
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++; //����� �ø��� �޼ҵ�

    public void RemoveStack() => stackSize--; //����� ���̴� �޼ҵ�
}
