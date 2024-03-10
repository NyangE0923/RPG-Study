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

    [SerializeField] private bool createCloneOnDashStart; //Dash�� ���۵� �� ����� Ŭ�� ���� bool��
    [SerializeField] private bool createCloneOnDashOver;  //Dash�� ���� �� ����� Ŭ�� ���� bool��
    [SerializeField] private bool canCreateCloneOnCounterAttack; //�ݰ��� �Ҷ� Ŭ���� ���������� ���� bool��
    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceDuplicate;
    [Header("Crystal instead of clone")]
    [SerializeField] private bool crystalInseadOfClone;

    //�÷��̾� ���� ��ġ�� ������ ��ü�� ����� �޼���
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        //���ο� ������Ʈ ������ 'Ŭ��'�� Instantiate�޼���� �����.
        GameObject newClone = Instantiate(clonePrefab);
        //���� ������� newClone������Ʈ ������Ʈ�� Clone_Skill_Controller ��ũ��Ʈ�� �����´�.
        //()(�Ұ�ȣ)�� �̿��Ͽ� SetupClone�޼��带 ȣ���Ѵ�.
        //_clonePosition�� Ŭ���� ������ ��ġ�� ��Ÿ���� Transform ��ü
        //cloneDuration�� Ŭ���� ���� �ð� ����
        //���� : ���ο� Ŭ�п� �߰��� Clone_Skill_Controller ������Ʈ�� SetupClone �޼ҵ带 ȣ���Ͽ�
        //Ŭ���� ��ġ�� ���ӽð��� �����ϴ� �޼���
        //FindClosestEnemy�� �Ű������� Transform�� ������ ������ newClone�� �ƴ� newClone�� Transform���� �Է��Ѵ�.
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            //�÷��̾��� ��ġ�� ��ġ ���Ծ��� Ŭ�� ���� �޼��� ȣ��
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            //�÷��̾��� ��ġ�� ��ġ ���Ծ��� Ŭ�� ���� �޼��� ȣ��
            CreateClone(player.transform, Vector3.zero);
        }
    }
    //�ݰ��� ������ Ŭ���� �����Ǵ� �޼��� ����
    //�Ű������� Tranform�� _enemyTransform���� ����
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        //���� canCreateCloneOnCounterAttack�� true���
        //CreateCloneWithDelay �ڷ�ƾ�� �����Ѵ�.
        //�̶� CreateCloneWithDelay�� Transform �Ű������� CreateCloneOnCounterAttack�� �����
        //_enemyTransform���� ȣ���Ѵ�.
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    //Transform, Vector3 �Ű������� ���� CreateCloneWithDelay �ڷ�ƾ�� �����Ѵ�.
    //�ڷ�ƾ�� 0.4�� �Ŀ� ����Ǹ�, CreateClone�޼��带 ȣ���Ѵ�.
    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offSet)
    {
        yield return new WaitForSeconds(cloneDelay);
        CreateClone(_transform, _offSet);
    }
}
