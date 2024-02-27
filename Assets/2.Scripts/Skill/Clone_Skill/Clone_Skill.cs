using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    //플레이어 현재 위치에 프리팹 객체를 만드는 메서드
    public void CreateClone(Transform _clonePosition)
    {
        //새로운 오브젝트 프리팹 '클론'을 Instantiate메서드로 만든다.
        GameObject newClone = Instantiate(clonePrefab);
        //새로 만들어진 newClone오브젝트 컴포넌트에 Clone_Skill_Controller 스크립트를 가져온다.
        //()(소괄호)를 이용하여 SetupClone메서드를 호출한다.
        //_clonePosition은 클론을 생성한 위치를 나타내는 Transform 객체
        //cloneDuration은 클론의 지속 시간 변수
        //정리 : 새로운 클론에 추가된 Clone_Skill_Controller 컴포넌트의 SetupClone 메소드를 호출하여
        //클론의 위치와 지속시간을 설정하는 메서드
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack);
    }
}
