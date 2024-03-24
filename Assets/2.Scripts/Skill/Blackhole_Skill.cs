using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks; //공격 관련
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab; //블랙홀 관련
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

    //bool값을 반환해야하는 메서드이기 때문에 true나 false를 반환해야함
    //이때 currentBlackhole이 false라면 동일하게 false를 반환하고
    //true라면 똑같이 true를 반환하며 더이상 해당 currentBlackhole을 참조하지 않는다.
    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;

        //Blackhole_Skill_Controller의 playerCanExitState가 true라면 현재 블랙홀을 null로 설정하고 true를 반환한다.
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
