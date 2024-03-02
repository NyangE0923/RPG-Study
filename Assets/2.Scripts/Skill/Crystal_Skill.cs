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

        //���� ���� ũ����Ż�� null�̶�� ũ����Ż�� �ν��Ͻ�ȭ(Instantiate)�Ѵ�.
        //�̶� ũ����Ż�� crystalPrefab ��ü�� �޾ƿ���, �÷��̾��� ��ġ���� ȸ������ �ʴ� ���·� �����Ѵ�.
        //�׸��� Crystal_Skill_Controller�� �������� 'currentCrystalScript'�� �����ϰ�
        //���� ũ����Ż�� Crystal_Skill_Controller ������Ʈ�� �����´�.
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

            //SetupCrystal�޼��带 �Ű����� ���ӽð��� �Բ� ȣ���Ѵ�.
            //�̶� �ش� float�Ű������� ���� ������ �������� crystalDuration�� �Ѵ�.
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed);
        }
        else
        {
            //�������� Vector2 PlayerPos�� �÷��̾��� ��ġ�� �����Ѵ�.
            Vector2 playerPos = player.transform.position;

            //���� �÷��̾��� ��ġ�� ���� ũ����Ż�� ��ġ�� �̵���Ű�� ���� ũ����Ż ��ġ�� �÷��̾��� ��ġ�� �����Ѵ�.
            //����ũ����Ż�� ������ �ִ� Crystal_Skill_Controller ������Ʈ�� FinishCrystal�޼��带 ȣ���ϰ�
            //FinishCrystal �޼��带 ���� canExplode�� True�� ��� ��Ʈ����
            //False�� ��� ����ũ����Ż�� �ı��Ѵ�.
            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }
}
