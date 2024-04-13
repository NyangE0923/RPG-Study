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

    //시각적 충돌 체크
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌된 객체가 Player를 가지고 있다면 Log를 출력하고, 객체를 파괴한다.
        if(collision.GetComponent<Player> () != null)
        {
            Inventory.Instance.AddItem(itemData); //싱글톤 Inventory의 AddItem메소드 호출 (itemData에 해당하는 Data를 추가)
            Destroy(gameObject);
        }
    }
}
