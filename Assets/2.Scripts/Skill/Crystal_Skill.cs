using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    public override void UseSkill()
    {
        base.UseSkill();

        //만약 현재 크리스탈이 null이라면 크리스탈을 인스턴스화(Instantiate)한다.
        //이때 크리스탈은 crystalPrefab 객체를 받아오고, 플레이어의 위치에서 회전하지 않는 상태로 생성한다.
        //그리고 Crystal_Skill_Controller를 지역변수 'currentCrystalScript'로 선언하고
        //현재 크리스탈의 Crystal_Skill_Controller 컴포넌트를 가져온다.
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

            //SetupCrystal메서드를 매개변수 지속시간과 함께 호출한다.
            //이때 해당 float매개변수는 새로 선언한 전역변수 crystalDuration로 한다.
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed);
        }
        else
        {
            //지역변수 Vector2 PlayerPos를 플레이어의 위치로 선언한다.
            Vector2 playerPos = player.transform.position;

            //현재 플레이어의 위치를 현재 크리스탈의 위치로 이동시키고 현재 크리스탈 위치를 플레이어의 위치로 변경한다.
            //현재크리스탈이 가지고 있는 Crystal_Skill_Controller 컴포넌트의 FinishCrystal메서드를 호출하고
            //FinishCrystal 메서드를 통해 canExplode가 True일 경우 터트리고
            //False일 경우 현재크리스탈을 파괴한다.
            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }
}
