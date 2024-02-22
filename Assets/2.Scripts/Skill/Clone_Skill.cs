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

    //�÷��̾� ���� ��ġ�� ������ ��ü�� ����� �޼���
    public void CreateClone(Transform _clonePosition)
    {
        //���ο� ������Ʈ ������ 'Ŭ��'�� Instantiate�޼���� �����.
        GameObject newClone = Instantiate(clonePrefab);
        //���� ������� newClone������Ʈ ������Ʈ�� Clone_Skill_Controller ��ũ��Ʈ�� �����´�.
        //()(�Ұ�ȣ)�� �̿��Ͽ� SetupClone�޼��带 ȣ���Ѵ�.
        //_clonePosition�� Ŭ���� ������ ��ġ�� ��Ÿ���� Transform ��ü
        //cloneDuration�� Ŭ���� ���� �ð� ����
        //���� : ���ο� Ŭ�п� �߰��� Clone_Skill_Controller ������Ʈ�� SetupClone �޼ҵ带 ȣ���Ͽ�
        //Ŭ���� ��ġ�� ���ӽð��� �����ϴ� �޼���
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack);
    }
}
