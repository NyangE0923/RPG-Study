using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; //클래스 또는 객체의 유일한 인스턴스, 정적 변수

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //ItemData Key, InventoryItem Value인 딕셔너리 생성

    private void Awake()
    {
        if (Instance == null) //얘가 비어 있으면 얘를 Instance 변수에 할당한다.
            Instance = this;
        else
            Destroy(Instance); //아니면 파괴하셈
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>(); //초기화
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>(); //초기화
    }

    public void AddItem(ItemData _item) //아이템 데이터를 매개변수로 하는 아이템을 리스트에 생성하는 메소드
    {
        //딕셔너리에서 _item에 해당하는 키를 찾아보고
        //TryGetValue 메소드를 사용하여 딕셔너리에서 값을 가져오려고 한다.
        //만약 해당하는 키가 있다면 이 키에 대한 값을 가져오고 Value 변수에 할당한다.
        //이때 out 키워드를 사용하여 Value변수에 값을 반환한다.
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem Value))
        {
            Value.AddStack();
        }
        else
        {
            //만약 인벤토리에 존재하지 않는 아이템이라면
            //리스트에 생성하는 것으로 새로운 칸에 담는 것이 된다.
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    //아이템을 삭제하는 메소드 (매개변수로 ItemData를 사용한다)
    public void RemoveItem(ItemData _item)
    {
        //딕셔너리 TryGetValue메소드를 사용해서 _item에 해당하는 Value를 받아오고
        //해당 Value가 1이하 라면 리스트와 딕셔너리에서 해당 Value, Key를 삭제한다.
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem Value))
        {
            if (Value.stackSize <= 1)
            {
                inventoryItems.Remove(Value);
                inventoryDictionary.Remove(_item);
            }
            else
                //RemoveStack을 이용해서 개수를 차감시킨다.
                Value.RemoveStack();
        }
    }
}
