using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks; //���� ����
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab; //��Ȧ ����
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller currentBlackhole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    //bool���� ��ȯ�ؾ��ϴ� �޼����̱� ������ true�� false�� ��ȯ�ؾ���
    //�̶� currentBlackhole�� false��� �����ϰ� false�� ��ȯ�ϰ�
    //true��� �Ȱ��� true�� ��ȯ�ϸ� ���̻� �ش� currentBlackhole�� �������� �ʴ´�.
    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;

        //Blackhole_Skill_Controller�� playerCanExitState�� true��� ���� ��Ȧ�� null�� �����ϰ� true�� ��ȯ�Ѵ�.
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
