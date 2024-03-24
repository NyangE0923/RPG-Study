using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;     //�ִ�ũ��
    public float growSpeed;   //Ŀ���� �ӵ�
    public float shrinkSpeed; //�۾����� �ӵ�
    private float blackholeTimer;

    private bool canGrow = true;      //Ŀ����
    private bool canShrink;    //�۾�����
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

        if (canGrow && !canShrink) //Ŀ���� �����̸鼭 �۾����� �ʴ� �����϶�
        {
            //canGrow�� true�̸鼭 canShrink�� false�϶� ���� ũ��(tranform.localScale)�� Vector2.Lerp�޼��带 ȣ���Ѵ�.
            //Vector2.Lerp�� ������ �ӵ��� ���������� ��ǥ �������� �̵��ϴµ� ���Ǵ� �޼���
            //�� ũ�⸦ �⺻ ũ�⿡��, maxSize��ŭ growSpeed * Time.deltaTime�� �ӵ��� �ε巴�� Ŀ����.
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            //canShrink�� true��� Ŀ���ִ� ���� ũ�⿡�� Vector2.Lerp�޼��带 ȣ���Ѵ�.
            //���������� Vector2.Lerp�� ������ �ӵ��� ���������� ��ǥ �������� �̵��ϴµ� ���Ǵ� �޼����̹Ƿ�
            //���� ũ�⿡�� (x.-1, y.-1)�� �ɶ����� shrinkSpeed�� �ε巴�� �۾�����.
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            //���� ���� x�� ũ�Ⱑ 0�̸��� �ȴٸ� �ش� ��ü�� �ı��Ѵ�.
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) //�ƹ��͵� Ÿ������ ���� �ʾ��� �� ����� �������� (���� Ÿ���� ���� 0 ���϶�� ReleaseCloneAttack�� ��ȯ�Ѵ�.
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
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0) //cloneAttackTimer�� 0�̸��̰� canAttack�� true�̰� amountOfAttack�� 0�ʰ����
        {
            //cloneAttackCooldown�� ���� cloneAttackTimer���� �ְ�
            cloneAttackTimer = cloneAttackCooldown;
            //0���� targets����Ʈ�� Count���� Random.Range�޼��带 ����� randomIndex ���������� �����Ѵ�.
            int randomIndex = Random.Range(0, targets.Count);
            //��ų�Ŵ����� instance.clone.CreateClone(Ŭ�� ����)�� ����Ѵ�.
            //�̶� CreateClone�� �Ű������� ���� ������ �������� randomIndex�� ����Ѵ�.

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            //���� �̱��� ��ų �Ŵ����� ����� Clone_Skill ������Ʈ�� crystalInseadOfClone�� true���
            //Crystal_SKill ������Ʈ�� CreateCrystal �޼ҵ带 ȣ���Ѵ�.
            //Crystal_SKill ������Ʈ�� CurrentCrystalChooseRandomTarget �޼ҵ带 ȣ���Ѵ�.
            //crystalInseadOfClone�� true�� �ƴ϶�� (else���)
            //Clone_Skill ������Ʈ�� CreateClone �޼ҵ带 ȣ���Ѵ�.
            //�̶� Transform, Vector3 �Ű������� �����´�.
            //targets�� ���� ���� ����Ʈ, �����ϰ� 50%�� Ȯ���� 2�� -2��ŭ x��ġ�� �̵��� ���¿��� Ŭ���� �����Ѵ�.

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
    private void CreateHotKey(Collider2D collision) //HotKey ���� �޼��� 
    {
        if (keyCodeList.Count <= 0) //���� ����Ʈ ī��Ʈ�� 0�� ���ų� �۴ٸ� ��ȯ�Ѵ�.
        {
            Debug.LogWarning("Not enough hot keys in a key code list!");
            return;
        }
        if (!canCreateHotKeys)
            return;

        //hotKeyPrefab�� �����Ͽ� ���ο� ���� ������Ʈ newHotKey ����
        //��ġ�� �ݶ��̴��� ��ġ���� (0, 3, 0)��ŭ �̵��� ���� �����ϰ�
        //ȸ���� ������� ���� ���·� �����Ѵ�.
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        //�������� 0���� List�� ��� ���� �����ϰ� �ش� ���õ� �׸��� �����Ѵ�.
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }
    //���� ǥ���� => �� ����ؼ� �Ű����� _enemyTransform�� �޾Ƽ� tragets����Ʈ�� �߰��Ѵٴ� ��
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
