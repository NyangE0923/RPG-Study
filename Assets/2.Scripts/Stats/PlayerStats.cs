using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    
    //CharacterStats 클래스의 Die메소드를 재정의 하고, Player 클래스의 Die 메소드를 호출한다.
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
