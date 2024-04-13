using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//직렬화 어트리뷰트
//메모리나 영구 저장 장치에 저장이 가능한 형식으로 변환시킨다.
//직렬화 : 객체의 상태(객체의 필드에 저장된 값들)를 메모리나 영구 저장 장치에 저장이 가능한 0과 1의 순서로 바꾸는 것
[System.Serializable]
public class Stat
{
    //int형 변수 baseValue를 선언하고
    //int형 변수를 반환해야 하는 GetValue메소드를 만들어서
    //baseValue를 반환한다.
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        //int형 변수 finalValue에 baseValue값을 초기화한다.
        int finalValue = baseValue;

        //modifiers리스트에 있는 각 요소에 대해 반복문을 실행한다.
        //finalValue의 값에 modifier값을 더한다.
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        //최종적으로 finalValue값을 반환하는 것으로
        //GetValue()메소드를 호출하면 baseValue와 modifiers리스트의 값들이 합산된 결과를 얻을 수 있다.
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        //Value의 기본값을 설정하는 메소드
        //baseValue는 매개변수 _value의 값으로 초기화한다.
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.RemoveAt(_modifier);
    }
}
