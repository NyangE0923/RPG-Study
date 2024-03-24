using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        //만약 현재 크리스탈이 null이라면 CreateCrystal 메서드를 호출한다.
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;


            //지역변수 Vector2 PlayerPos를 플레이어의 위치로 선언한다.
            Vector2 playerPos = player.transform.position;
            //현재 플레이어의 위치를 현재 크리스탈의 위치로 이동시키고 현재 크리스탈 위치를 플레이어의 위치로 변경한다.
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            //cloneInsteadOfCrystal가 true라면
            if (cloneInsteadOfCrystal)
            {
                //싱글톤 스킬매니저의 클론 클래스에서 클론 생성 메서드인 CreateClone메서드를 호출한다.
                //이때 클론 생성을 현재 크리스탈의 위치에서 생성한다.
                //그리고 현재 크리스탈을 파괴한다.
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                //현재크리스탈이 가지고 있는 Crystal_Skill_Controller 컴포넌트의 FinishCrystal메서드를 호출하고
                //FinishCrystal 메서드를 통해 canExplode가 True일 경우 터트리고
                //False일 경우 현재크리스탈을 파괴한다.
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        //크리스탈을 인스턴스화(Instantiate)한다.
        //이때 크리스탈은 crystalPrefab 객체를 받아오고, 플레이어의 위치에서 회전하지 않는 상태로 생성한다.
        //그리고 Crystal_Skill_Controller를 지역변수 'currentCrystalScript'로 선언하고
        //현재 크리스탈의 Crystal_Skill_Controller 컴포넌트를 가져온다.
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        //SetupCrystal메서드를 매개변수 지속시간과 함께 호출한다.
        //이때 해당 float매개변수는 새로 선언한 전역변수 crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy로 한다.
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    //CurrentCrystalChooseRandomTarget메서드는 '람다 표현식'을 사용하여
    //currentCrystal객체의 Crystal_Skill_Controller 스크립트 컴포넌트에 있는 ChooseRandomEnemy메서드를 호출한다.
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    //bool값을 반환하는 CanUseMultiCrystal 메서드를 생성
    private bool CanUseMultiCrystal()
    {
        //만약 canUseMultiStacks가 true라면
        if (canUseMultiStacks)
        {
            //만약 crystalLeft리스트의 Count가 0 초과라면
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                //객체 crystalToSpawn를 crystalLeft리스트의 마지막 항목에 넣는다.
                //crystalToSpawn 객체를 플레이어의 위치에 회전이 없는 상태로 newCrystal으로 생성한다. 
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                //crystalLeft리스트에 있는 crystalSpawn을 제거한다.
                crystalLeft.Remove(crystalToSpawn);

                //이때 생성된 newCrystal은 Crystal_Skill_Controller 컴포넌트를 받고
                //SetupCrystal메서드를 매개변수(크리스탈 지속시간, 폭발 여부, 적들에게 이동, 이동속도, 근처의 적을 찾는 메서드)들과 함께 호출한다.
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                //true를 반환한다.
                return true;
            }
        }
        //false를 반환한다.
        return false;
    }

    private void RefilCrystal()
    {
        //지역변수 amountToAdd를 선언한다
        //이때 amountAdd는 amountOfStacks에서 crystalLeft리스트의 카운트를 뺀 값
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        //반복문 int i = 0이고 i가 amountToAdd 미만일때 i를 더한다.
        //이때 crystalLeft리스트에 crystalPrefab을 생성한다.
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldown = multiStackCooldown;
        RefilCrystal();
    }
}
