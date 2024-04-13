using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; //Ŭ���� �Ǵ� ��ü�� ������ �ν��Ͻ�, ���� ����

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //ItemData Key, InventoryItem Value�� ��ųʸ� ����

    private void Awake()
    {
        if (Instance == null) //�갡 ��� ������ �긦 Instance ������ �Ҵ��Ѵ�.
            Instance = this;
        else
            Destroy(Instance); //�ƴϸ� �ı��ϼ�
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>(); //�ʱ�ȭ
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>(); //�ʱ�ȭ
    }

    public void AddItem(ItemData _item) //������ �����͸� �Ű������� �ϴ� �������� ����Ʈ�� �����ϴ� �޼ҵ�
    {
        //��ųʸ����� _item�� �ش��ϴ� Ű�� ã�ƺ���
        //TryGetValue �޼ҵ带 ����Ͽ� ��ųʸ����� ���� ���������� �Ѵ�.
        //���� �ش��ϴ� Ű�� �ִٸ� �� Ű�� ���� ���� �������� Value ������ �Ҵ��Ѵ�.
        //�̶� out Ű���带 ����Ͽ� Value������ ���� ��ȯ�Ѵ�.
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem Value))
        {
            Value.AddStack();
        }
        else
        {
            //���� �κ��丮�� �������� �ʴ� �������̶��
            //����Ʈ�� �����ϴ� ������ ���ο� ĭ�� ��� ���� �ȴ�.
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    //�������� �����ϴ� �޼ҵ� (�Ű������� ItemData�� ����Ѵ�)
    public void RemoveItem(ItemData _item)
    {
        //��ųʸ� TryGetValue�޼ҵ带 ����ؼ� _item�� �ش��ϴ� Value�� �޾ƿ���
        //�ش� Value�� 1���� ��� ����Ʈ�� ��ųʸ����� �ش� Value, Key�� �����Ѵ�.
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem Value))
        {
            if (Value.stackSize <= 1)
            {
                inventoryItems.Remove(Value);
                inventoryDictionary.Remove(_item);
            }
            else
                //RemoveStack�� �̿��ؼ� ������ ������Ų��.
                Value.RemoveStack();
        }
    }
}
