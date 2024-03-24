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

        //���� ���� ũ����Ż�� null�̶�� CreateCrystal �޼��带 ȣ���Ѵ�.
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;


            //�������� Vector2 PlayerPos�� �÷��̾��� ��ġ�� �����Ѵ�.
            Vector2 playerPos = player.transform.position;
            //���� �÷��̾��� ��ġ�� ���� ũ����Ż�� ��ġ�� �̵���Ű�� ���� ũ����Ż ��ġ�� �÷��̾��� ��ġ�� �����Ѵ�.
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            //cloneInsteadOfCrystal�� true���
            if (cloneInsteadOfCrystal)
            {
                //�̱��� ��ų�Ŵ����� Ŭ�� Ŭ�������� Ŭ�� ���� �޼����� CreateClone�޼��带 ȣ���Ѵ�.
                //�̶� Ŭ�� ������ ���� ũ����Ż�� ��ġ���� �����Ѵ�.
                //�׸��� ���� ũ����Ż�� �ı��Ѵ�.
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                //����ũ����Ż�� ������ �ִ� Crystal_Skill_Controller ������Ʈ�� FinishCrystal�޼��带 ȣ���ϰ�
                //FinishCrystal �޼��带 ���� canExplode�� True�� ��� ��Ʈ����
                //False�� ��� ����ũ����Ż�� �ı��Ѵ�.
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        //ũ����Ż�� �ν��Ͻ�ȭ(Instantiate)�Ѵ�.
        //�̶� ũ����Ż�� crystalPrefab ��ü�� �޾ƿ���, �÷��̾��� ��ġ���� ȸ������ �ʴ� ���·� �����Ѵ�.
        //�׸��� Crystal_Skill_Controller�� �������� 'currentCrystalScript'�� �����ϰ�
        //���� ũ����Ż�� Crystal_Skill_Controller ������Ʈ�� �����´�.
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        //SetupCrystal�޼��带 �Ű����� ���ӽð��� �Բ� ȣ���Ѵ�.
        //�̶� �ش� float�Ű������� ���� ������ �������� crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy�� �Ѵ�.
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    //CurrentCrystalChooseRandomTarget�޼���� '���� ǥ����'�� ����Ͽ�
    //currentCrystal��ü�� Crystal_Skill_Controller ��ũ��Ʈ ������Ʈ�� �ִ� ChooseRandomEnemy�޼��带 ȣ���Ѵ�.
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    //bool���� ��ȯ�ϴ� CanUseMultiCrystal �޼��带 ����
    private bool CanUseMultiCrystal()
    {
        //���� canUseMultiStacks�� true���
        if (canUseMultiStacks)
        {
            //���� crystalLeft����Ʈ�� Count�� 0 �ʰ����
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                //��ü crystalToSpawn�� crystalLeft����Ʈ�� ������ �׸� �ִ´�.
                //crystalToSpawn ��ü�� �÷��̾��� ��ġ�� ȸ���� ���� ���·� newCrystal���� �����Ѵ�. 
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                //crystalLeft����Ʈ�� �ִ� crystalSpawn�� �����Ѵ�.
                crystalLeft.Remove(crystalToSpawn);

                //�̶� ������ newCrystal�� Crystal_Skill_Controller ������Ʈ�� �ް�
                //SetupCrystal�޼��带 �Ű�����(ũ����Ż ���ӽð�, ���� ����, ���鿡�� �̵�, �̵��ӵ�, ��ó�� ���� ã�� �޼���)��� �Բ� ȣ���Ѵ�.
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                //true�� ��ȯ�Ѵ�.
                return true;
            }
        }
        //false�� ��ȯ�Ѵ�.
        return false;
    }

    private void RefilCrystal()
    {
        //�������� amountToAdd�� �����Ѵ�
        //�̶� amountAdd�� amountOfStacks���� crystalLeft����Ʈ�� ī��Ʈ�� �� ��
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        //�ݺ��� int i = 0�̰� i�� amountToAdd �̸��϶� i�� ���Ѵ�.
        //�̶� crystalLeft����Ʈ�� crystalPrefab�� �����Ѵ�.
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
