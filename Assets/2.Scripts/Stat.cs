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
