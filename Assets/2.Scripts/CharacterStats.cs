using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; // ��ġ 1�� ������ 1����, ũ��Ƽ�� ���ݷ� 1% ����
    public Stat agility;  // ��ġ 1�� ȸ���� 1% ����, ũ��Ƽ�� Ȯ�� 1% ����
    public Stat intelligence; // ��ġ 1�� ���� ������ 1����, ���� ���׷� 3 ����
    public Stat vitality;     // ��ġ 1�� ü�� 3 - 5 ����


    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        //Start������ ����ü���� �ִ�ü�� ������ �����Ѵ�.
        //maxHealth�� GetValue�޼ҵ带 ȣ���ϴ� ������ baseValue�� ��ȯ�Ѵ�.
        //�̷��� �ϸ� maxHealth�� �ִ� ü�� ���� StatŬ������ ���� ȹ���ϰ�
        //�� ���� currentHelath�� �Ҵ��ϰ� �ȴ�.
        currentHealth = maxHealth.GetValue();
    }

    //DoDamage�޼ҵ�� CharacterStatsŬ������ �Ű������� ȣ���Ѵ�.
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //int�� �������� totalDamage�� �����Ѵ�.
        //�̶� totalDamage�� ���� StatŬ������ GetValue�� ���� ����� damage�� ���� strength�� ���� ���� ������ �Ѵ�.
        //���� CharacterStatsŬ������ TakeDamage�޼ҵ带 ȣ���Ͽ�
        //�������� �ְų� ü���� 0���ϰ� �Ǹ� Die�޼ҵ带 ȣ���Ѵ�.
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    //�������� �ִ� �޼ҵ� int�� �Ű����� _damege
    //TakeDamage �޼ҵ尡 ȣ��Ǹ� ����ü�¿� �ش��ϴ� currentHealth�� ������ _damage �� ��ŭ ����.
    //�̶� ���� ���� ü���� 0���϶�� Die�޼ҵ带 ȣ���Ѵ�.
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()
    {
    }
}
