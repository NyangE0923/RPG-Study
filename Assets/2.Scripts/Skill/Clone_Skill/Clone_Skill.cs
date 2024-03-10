using System.Collections;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float cloneDelay;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart; //Dash가 시작될 때 사용할 클론 생성 bool값
    [SerializeField] private bool createCloneOnDashOver;  //Dash가 끝날 때 사용할 클론 생성 bool값
    [SerializeField] private bool canCreateCloneOnCounterAttack; //반격을 할때 클론이 생성될지에 대한 bool값
    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceDuplicate;
    [Header("Crystal instead of clone")]
    [SerializeField] private bool crystalInseadOfClone;

    //플레이어 현재 위치에 프리팹 객체를 만드는 메서드
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        //새로운 오브젝트 프리팹 '클론'을 Instantiate메서드로 만든다.
        GameObject newClone = Instantiate(clonePrefab);
        //새로 만들어진 newClone오브젝트 컴포넌트에 Clone_Skill_Controller 스크립트를 가져온다.
        //()(소괄호)를 이용하여 SetupClone메서드를 호출한다.
        //_clonePosition은 클론을 생성한 위치를 나타내는 Transform 객체
        //cloneDuration은 클론의 지속 시간 변수
        //정리 : 새로운 클론에 추가된 Clone_Skill_Controller 컴포넌트의 SetupClone 메소드를 호출하여
        //클론의 위치와 지속시간을 설정하는 메서드
        //FindClosestEnemy는 매개변수로 Transform을 가지기 때문에 newClone이 아닌 newClone의 Transform값을 입력한다.
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            //플레이어의 위치에 위치 변함없이 클론 생성 메서드 호출
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            //플레이어의 위치에 위치 변함없이 클론 생성 메서드 호출
            CreateClone(player.transform, Vector3.zero);
        }
    }
    //반격을 했을때 클론이 생성되는 메서드 생성
    //매개변수로 Tranform을 _enemyTransform으로 선언
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        //만약 canCreateCloneOnCounterAttack이 true라면
        //CreateCloneWithDelay 코루틴을 실행한다.
        //이때 CreateCloneWithDelay의 Transform 매개변수를 CreateCloneOnCounterAttack에 선언된
        //_enemyTransform으로 호출한다.
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    //Transform, Vector3 매개변수를 가진 CreateCloneWithDelay 코루틴을 생성한다.
    //코루틴은 0.4초 후에 실행되며, CreateClone메서드를 호출한다.
    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offSet)
    {
        yield return new WaitForSeconds(cloneDelay);
        CreateClone(_transform, _offSet);
    }
}
