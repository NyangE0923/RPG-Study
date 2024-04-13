using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.icon;
    }

    //�ð��� �浹 üũ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹�� ��ü�� Player�� ������ �ִٸ� Log�� ����ϰ�, ��ü�� �ı��Ѵ�.
        if(collision.GetComponent<Player> () != null)
        {
            Inventory.Instance.AddItem(itemData); //�̱��� Inventory�� AddItem�޼ҵ� ȣ�� (itemData�� �ش��ϴ� Data�� �߰�)
            Destroy(gameObject);
        }
    }
}
