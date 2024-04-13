using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ȭ ��Ʈ����Ʈ
//�޸𸮳� ���� ���� ��ġ�� ������ ������ �������� ��ȯ��Ų��.
//����ȭ : ��ü�� ����(��ü�� �ʵ忡 ����� ����)�� �޸𸮳� ���� ���� ��ġ�� ������ ������ 0�� 1�� ������ �ٲٴ� ��
[System.Serializable]
public class Stat
{
    //int�� ���� baseValue�� �����ϰ�
    //int�� ������ ��ȯ�ؾ� �ϴ� GetValue�޼ҵ带 ����
    //baseValue�� ��ȯ�Ѵ�.
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        //int�� ���� finalValue�� baseValue���� �ʱ�ȭ�Ѵ�.
        int finalValue = baseValue;

        //modifiers����Ʈ�� �ִ� �� ��ҿ� ���� �ݺ����� �����Ѵ�.
        //finalValue�� ���� modifier���� ���Ѵ�.
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        //���������� finalValue���� ��ȯ�ϴ� ������
        //GetValue()�޼ҵ带 ȣ���ϸ� baseValue�� modifiers����Ʈ�� ������ �ջ�� ����� ���� �� �ִ�.
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        //Value�� �⺻���� �����ϴ� �޼ҵ�
        //baseValue�� �Ű����� _value�� ������ �ʱ�ȭ�Ѵ�.
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
