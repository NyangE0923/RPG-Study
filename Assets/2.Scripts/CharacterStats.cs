using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; // 수치 1당 데미지 1증가, 크리티컬 공격력 1% 증가
    public Stat agility;  // 수치 1당 회피율 1% 증가, 크리티컬 확률 1% 증가
    public Stat intelligence; // 수치 1당 마법 데미지 1증가, 마법 저항력 3 증가
    public Stat vitality;     // 수치 1당 체력 3 - 5 증가


    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        //Start에서는 현재체력을 최대체력 값으로 설정한다.
        //maxHealth에 GetValue메소드를 호출하는 것으로 baseValue를 반환한다.
        //이렇게 하면 maxHealth는 최대 체력 값을 Stat클래스로 부터 획득하고
        //이 값을 currentHelath에 할당하게 된다.
        currentHealth = maxHealth.GetValue();
    }

    //DoDamage메소드는 CharacterStats클래스를 매개변수로 호출한다.
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //int형 지역변수 totalDamage를 선언한다.
        //이때 totalDamage의 값은 Stat클래스의 GetValue의 값이 적용된 damage의 값과 strength의 값을 합한 값으로 한다.
        //이후 CharacterStats클래스의 TakeDamage메소드를 호출하여
        //데미지를 주거나 체력이 0이하가 되면 Die메소드를 호출한다.
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    //데미지를 주는 메소드 int형 매개변수 _damege
    //TakeDamage 메소드가 호출되면 현재체력에 해당하는 currentHealth의 값에서 _damage 값 만큼 뺀다.
    //이때 만약 현재 체력이 0이하라면 Die메소드를 호출한다.
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
