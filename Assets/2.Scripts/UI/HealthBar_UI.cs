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

        //entity 객체의 onFlipped 이벤트에 FlipUI 메소드를 추가한다.
        //즉 onFlipped 이벤트가 트리거 될때마다 FlipUI 메소드가 호출될 것임을 의미한다.
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
        //slider의 최대값은 캐릭터 스탯 컴포넌트의 maxHealth가 가지고 있는 Value값과 vistality Value값으로 한다.
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;     //slider의 current값은 캐릭터 스탯 컴포넌트의 currentHealth값으로 한다.
    }



    private void FlipUI() => myTransform.Rotate(0, 180, 0);    //RectTranform 객체를 180도 회전한다.
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        //entity 객체의 onFlipped 이벤트에 있는 FlipUI 메소드를 제거한다. 호출되어있는 메소드를 뺀다는 것을 의미한다.
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
