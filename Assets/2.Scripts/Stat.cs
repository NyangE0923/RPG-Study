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
        return baseValue;
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
