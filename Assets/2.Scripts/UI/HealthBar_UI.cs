using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private Slider slider;
    private RectTransform myTransform;
    private CharacterStats myStats;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        //entity ��ü�� onFlipped �̺�Ʈ�� FlipUI �޼ҵ带 �߰��Ѵ�.
        //�� onFlipped �̺�Ʈ�� Ʈ���� �ɶ����� FlipUI �޼ҵ尡 ȣ��� ������ �ǹ��Ѵ�.
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        //slider�� �ִ밪�� ĳ���� ���� ������Ʈ�� maxHealth�� ������ �ִ� Value���� vistality Value������ �Ѵ�.
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;     //slider�� current���� ĳ���� ���� ������Ʈ�� currentHealth������ �Ѵ�.
    }



    private void FlipUI() => myTransform.Rotate(0, 180, 0);    //RectTranform ��ü�� 180�� ȸ���Ѵ�.
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        //entity ��ü�� onFlipped �̺�Ʈ�� �ִ� FlipUI �޼ҵ带 �����Ѵ�. ȣ��Ǿ��ִ� �޼ҵ带 ���ٴ� ���� �ǹ��Ѵ�.
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
