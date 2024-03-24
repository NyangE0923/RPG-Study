using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;     //최대크기
    public float growSpeed;   //커지는 속도
    public float shrinkSpeed; //작아지는 속도
    private float blackholeTimer;

    private bool canGrow = true;      //커지기
    private bool canShrink;    //작아지기
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisapear = true;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuratin)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuratin;

        if (SkillManager.instance.clone.crystalInseadOfClone)
            playerCanDisapear = false;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink) //커지는 상태이면서 작아지지 않는 상태일때
        {
            //canGrow가 true이면서 canShrink가 false일때 현재 크기(tranform.localScale)는 Vector2.Lerp메서드를 호출한다.
            //Vector2.Lerp는 정해진 속도로 시작점에서 목표 지점까지 이동하는데 사용되는 메서드
            //즉 크기를 기본 크기에서, maxSize만큼 growSpeed * Time.deltaTime의 속도로 부드럽게 커진다.
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            //canShrink가 true라면 커져있는 현재 크기에서 Vector2.Lerp메서드를 호출한다.
            //마찬가지로 Vector2.Lerp는 정해진 속도로 시작점에서 목표 지점까지 이동하는데 사용되는 메서드이므로
            //현재 크기에서 (x.-1, y.-1)이 될때까지 shrinkSpeed로 부드럽게 작아진다.
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            //만약 현재 x의 크기가 0미만이 된다면 해당 객체를 파괴한다.
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) //아무것도 타겟으로 삼지 않았을 때 생기는 오류방지 (만약 타겟의 수가 0 이하라면 ReleaseCloneAttack을 반환한다.
            return;

        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0) //cloneAttackTimer가 0미만이고 canAttack이 true이고 amountOfAttack이 0초과라면
        {
            //cloneAttackCooldown의 값을 cloneAttackTimer으로 주고
            cloneAttackTimer = cloneAttackCooldown;
            //0부터 targets리스트의 Count값을 Random.Range메서드를 사용해 randomIndex 지역변수로 선언한다.
            int randomIndex = Random.Range(0, targets.Count);
            //스킬매니저의 instance.clone.CreateClone(클론 생성)를 사용한다.
            //이때 CreateClone의 매개변수로 위에 생성한 지역변수 randomIndex를 사용한다.

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            //만약 싱글톤 스킬 매니저에 선언된 Clone_Skill 컴포넌트의 crystalInseadOfClone이 true라면
            //Crystal_SKill 컴포넌트의 CreateCrystal 메소드를 호출한다.
            //Crystal_SKill 컴포넌트의 CurrentCrystalChooseRandomTarget 메소드를 호출한다.
            //crystalInseadOfClone이 true가 아니라면 (else라면)
            //Clone_Skill 컴포넌트의 CreateClone 메소드를 호출한다.
            //이때 Transform, Vector3 매개변수를 가져온다.
            //targets는 적을 담을 리스트, 랜덤하게 50%의 확률로 2와 -2만큼 x위치가 이동한 상태에서 클론을 생성한다.

            if (SkillManager.instance.clone.crystalInseadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }
    private void CreateHotKey(Collider2D collision) //HotKey 생성 메서드 
    {
        if (keyCodeList.Count <= 0) //만약 리스트 카운트가 0과 같거나 작다면 반환한다.
        {
            Debug.LogWarning("Not enough hot keys in a key code list!");
            return;
        }
        if (!canCreateHotKeys)
            return;

        //hotKeyPrefab을 복제하여 새로운 게임 오브젝트 newHotKey 생성
        //위치는 콜라이더의 위치에서 (0, 3, 0)만큼 이동한 곳에 생성하고
        //회전이 적용되지 않은 상태로 생성한다.
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        //랜덤으로 0부터 List에 담긴 값을 선택하고 해당 선택된 항목을 삭제한다.
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }
    //람다 표현식 => 을 사용해서 매개변수 _enemyTransform을 받아서 tragets리스트에 추가한다는 뜻
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
