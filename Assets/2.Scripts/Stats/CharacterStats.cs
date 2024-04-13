using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CharacterStats : MonoBehaviour
{
    //EntityFx ������ private �����ڷ� fx��� �̸����� ���� �����Ѵ�.
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // ��ġ 1�� ������ 1����, ũ��Ƽ�� ���ݷ� 1% ����
    public Stat agility;  // ��ġ 1�� ȸ���� 1% ����, ũ��Ƽ�� Ȯ�� 1% ����
    public Stat intelligence; // ��ġ 1�� ���� ������ 1����, ���� ���׷� 3 ����
    public Stat vitality;     // ��ġ 1�� ü�� 3 - 5 ����

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion; //ȸ��
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;
    [Space(10)]
    public bool isIgnited; //���� ����
    public bool isChilled; //���� ���� 20%
    public bool isShocked; //���ݷ� 20% ���� (�Ǹ�)
    [Space(10)]

    [SerializeField] private float ailmentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCoolDown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public System.Action onHealthChanged;
    protected bool isDead; 

    protected virtual void Start()
    {
        //Start������ ����ü���� �ִ�ü�� ������ �����Ѵ�.
        //maxHealth�� GetValue�޼ҵ带 ȣ���ϴ� ������ baseValue�� ��ȯ�Ѵ�.
        //�̷��� �ϸ� maxHealth�� �ִ� ü�� ���� StatŬ������ ���� ȹ���ϰ�
        //�� ���� currentHelath�� �Ҵ��ϰ� �ȴ�.
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();
    }


    //DoDamage�޼ҵ�� CharacterStatsŬ������ �Ű������� ȣ���Ѵ�.
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        //int�� �������� totalDamage�� �����Ѵ�.
        //�̶� totalDamage�� ���� StatŬ������ GetValue�� ���� ����� damage�� ���� strength�� ���� ���� ������ �Ѵ�.
        //���� CharacterStatsŬ������ TakeDamage�޼ҵ带 ȣ���Ͽ�
        //�������� �ְų� ü���� 0���ϰ� �Ǹ� Die�޼ҵ带 ȣ���Ѵ�.
        int totalDamage = damage.GetValue() + strength.GetValue();

        //CanCrit�޼ҵ忡 ���� ũ��Ƽ���� true��� �Ʒ� �Լ��� �����Ѵ�.
        //�� �������� ���� CalculateCriticalDamage�޼ҵ��� �Ű����� ���� �Ҵ��Ѵ�.
        //�� ��µ� �������� ũ��Ƽ�� �������� �ٽ� ����ϰ� �� ���� �ٽ� ��µ� �������� �Ҵ����ִ� ������
        //ũ��Ƽ�� �������� �����Ѵ�.
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        //if inventory current weapon has fire effect
        // then DoMagicalDamage(_targetStats);
    }
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue(); //�� ����������

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        //�� �Ӽ��� �������� 0�� ���϶�� ��ȯ�Ѵ�.
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    #region Magical damage and ailements
    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        //�� �Ӽ� ���� �ٸ� �Ӽ����� ũ�ٸ� bool���� Ȱ��ȭ �Ѵ�.
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //���� ���� �������� �ִ� ��� Ȱ��ȭ�� ���� �����Ƿ� while���� ����Ѵ�.
        //��� �Ӽ� bool���� false �� ���
        //0���� 1������ �Ҽ��� �������� ���� ����ϰ�, �ش� ��� ���� 0.5f �̸��̰� �� �Ӽ� �������� 0���� ũ�ٸ�
        //�ش� �Ӽ��� true�� �����ϰ�, ApplyAilments�޼ҵ带 ȣ���Ѵ�.
        //�̶� ApplyAilments�޼ҵ�� �Ű����� �� ������� ��, ����, ������ �����ϰ�
        //�ش� �Ӽ��� true�� �����Ұ�� ApplyAilments �޼ҵ忡���� true�� �ȴ�.

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            //Mathf.RoundToInt = �ݿø� ��Ʈ�� ��ȯ
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); //ȭ�� ���� �������� 20% ��ŭ ��������
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);

            if(currentHealth <= 0)
            {
                isIgnited = false;
            }
        }
        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float _slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(_slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                //���� ����� ���� ã�� �޼ҵ�
                HitNearestTargetWithShockStrike();

            }
        }
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }
    private void HitNearestTargetWithShockStrike()
    {
        //��ü�� ��ġ���� ����25 �ȿ� �ִ� Collider2D ��ü���� colliders �迭�� �����Ѵ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        //���� ��ġ �ֺ��� ��� Collider2D ��ü�� ���� �ݺ��ϴ� foreach��
        foreach (var hit in colliders)
        {
            //hit ��ü�� Enemy ������Ʈ�� ������ �ִٸ�(null üũ)
            //���� Boss��� ������Ʈ�� ���� ����� �ȴٸ� �� ��ų�� ��ȿȭ ��ų �� ���� ������ ����
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                //float distanceToEnemy ���������� ���� ��ü�� ��ġ�� Enemy��ũ��Ʈ ������Ʈ�� ������ �ִ� Collider2D ��ü���� �Ÿ��� ����Ѵ�.
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                //����� ���� �Ÿ��� closestDistance(������ ����� ���� ����� ������ �Ÿ�)�������� ª����
                if (distanceToEnemy < closestDistance)
                {
                    //���� ���� ���� ����� ������ �����ϰ�,
                    //closestDistance���� distanceToEnemy�� �����Ѵ�.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        //nullüũ
        if (closestEnemy != null)
        {
            //��ü�� ��ġ���� ȸ���� ���� ���·� ������ ����
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            //�ش� ��ü�� ShockStrike_Controller�� �ִ� Setup�޼ҵ� ȣ��
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCoolDown;
        }
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage; //���ٽ� �Ű����� ���� �� igniteDamage = _damage�� ����.
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    public virtual void TakeDamage(int _damage)
    {
        //�������� �ִ� �޼ҵ� int�� �Ű����� _damege
        //TakeDamage �޼ҵ尡 ȣ��Ǹ� ����ü�¿� �ش��ϴ� currentHealth�� ������ _damage �� ��ŭ ����.
        //�̶� ���� ���� ü���� 0���϶�� Die�޼ҵ带 ȣ���Ѵ�.

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        //�Ű����� int�� ������ ����޼ҵ�
        //����ü�¿��� ��������ŭ ����.

        currentHealth -= _damage;

        if(onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    #region Stat calculation
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        //�̶� �� �������� 0����, int.MaxValue���� �����θ� �����ȴ�.
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3); //�������׷�, ���� ��ġ�� ���� ������ ����
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue); //�������� ������ ���� �ʵ��� ����
        return totalMagicalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        //bool���� ��ȯ�ϴ� �޼ҵ�

        //int�� ���� totalEvasion�� �� ȸ������ ��Ÿ����.
        //�� �� ȸ������ evasion, armor�� ���� ������ �Ѵ�.
        //�������� 0���� 99���� ���� �����ϰ�, ���� �� ������ ���� totalEvasion �̸��̶�� retrun�� true�� �Ѵ�.

        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.armor.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        
        return false;
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    private int CalculateCriticalDamage(int _damage)
    {
        //�Ű����� int _damage�� ����ϴ� int�� ��ȯ �޼ҵ�
        //ũ��Ƽ�� Ȯ��, ������ �޼ҵ�

        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        Debug.Log("total crit power %" + totalCritPower);
        
        //���� ���Ŀ� �Ű����� _damage�� �����ִ� ������ ���� ũ��Ƽ�� �������� ���� ���Ѵ�.
        //�׸��� �� ���� ��ȯ���ش�.

        float critDamage = _damage * totalCritPower;
        Debug.Log("crit damage before round up" + critDamage);

        //�ݿø� int����ȯ �Լ�
        //critDamage ���� �ݿø��Ͽ� int������ ����ȯ�Ѵ�.
        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion
}
