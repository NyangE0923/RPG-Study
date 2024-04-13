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
    
    //CharacterStats Ŭ������ Die�޼ҵ带 ������ �ϰ�, Player Ŭ������ Die �޼ҵ带 ȣ���Ѵ�.
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
