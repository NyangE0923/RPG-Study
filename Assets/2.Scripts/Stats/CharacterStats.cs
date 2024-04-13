using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CharacterStats : MonoBehaviour
{
    //EntityFx 형식을 private 한정자로 fx라는 이름으로 변수 정의한다.
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // 수치 1당 데미지 1증가, 크리티컬 공격력 1% 증가
    public Stat agility;  // 수치 1당 회피율 1% 증가, 크리티컬 확률 1% 증가
    public Stat intelligence; // 수치 1당 마법 데미지 1증가, 마법 저항력 3 증가
    public Stat vitality;     // 수치 1당 체력 3 - 5 증가

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion; //회피
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;
    [Space(10)]
    public bool isIgnited; //지속 피해
    public bool isChilled; //방어력 감소 20%
    public bool isShocked; //공격률 20% 감소 (실명)
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
        //Start에서는 현재체력을 최대체력 값으로 설정한다.
        //maxHealth에 GetValue메소드를 호출하는 것으로 baseValue를 반환한다.
        //이렇게 하면 maxHealth는 최대 체력 값을 Stat클래스로 부터 획득하고
        //이 값을 currentHelath에 할당하게 된다.
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


    //DoDamage메소드는 CharacterStats클래스를 매개변수로 호출한다.
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        //int형 지역변수 totalDamage를 선언한다.
        //이때 totalDamage의 값은 Stat클래스의 GetValue의 값이 적용된 damage의 값과 strength의 값을 합한 값으로 한다.
        //이후 CharacterStats클래스의 TakeDamage메소드를 호출하여
        //데미지를 주거나 체력이 0이하가 되면 Die메소드를 호출한다.
        int totalDamage = damage.GetValue() + strength.GetValue();

        //CanCrit메소드에 의해 크리티컬이 true라면 아래 함수를 실행한다.
        //총 데미지의 값에 CalculateCriticalDamage메소드의 매개변수 값을 할당한다.
        //즉 출력된 데미지를 크리티컬 공식으로 다시 계산하고 그 값을 다시 출력된 데미지에 할당해주는 것으로
        //크리티컬 데미지를 구현한다.
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

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue(); //총 마법데미지

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        //각 속성의 데미지가 0과 이하라면 반환한다.
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    #region Magical damage and ailements
    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        //각 속성 별로 다른 속성보다 크다면 bool값을 활성화 한다.
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //서로 같은 데미지를 주는 경우 활성화가 되지 않으므로 while문을 사용한다.
        //모든 속성 bool값이 false 일 경우
        //0부터 1사이의 소수를 랜덤으로 값을 출력하고, 해당 출력 값이 0.5f 미만이고 각 속성 데미지가 0보다 크다면
        //해당 속성을 true로 설정하고, ApplyAilments메소드를 호출한다.
        //이때 ApplyAilments메소드는 매개변수 각 순서대로 불, 얼음, 번개로 선언하고
        //해당 속성을 true로 설정할경우 ApplyAilments 메소드에서도 true가 된다.

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
            //Mathf.RoundToInt = 반올림 인트형 변환
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); //화염 마법 데미지의 20% 만큼 지속피해
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
                //가장 가까운 적을 찾는 메소드
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
        //객체의 위치에서 범위25 안에 있는 Collider2D 객체들을 colliders 배열에 저장한다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        //현재 위치 주변의 모든 Collider2D 객체에 대해 반복하는 foreach문
        foreach (var hit in colliders)
        {
            //hit 객체가 Enemy 컴포넌트를 가지고 있다면(null 체크)
            //만약 Boss라는 컴포넌트를 따로 만들게 된다면 이 스킬을 무효화 시킬 수 있을 것으로 보임
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                //float distanceToEnemy 지역변수를 만들어서 객체의 위치와 Enemy스크립트 컴포넌트를 가지고 있는 Collider2D 객체와의 거리를 계산한다.
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                //계산한 적의 거리가 closestDistance(이전에 저장된 가장 가까운 적과의 거리)변수보다 짧으면
                if (distanceToEnemy < closestDistance)
                {
                    //현재 적을 가장 가까운 적으로 설정하고,
                    //closestDistance값에 distanceToEnemy를 갱신한다.
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        //null체크
        if (closestEnemy != null)
        {
            //객체의 위치에서 회전이 없는 상태로 프리팹 생성
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            //해당 객체의 ShockStrike_Controller에 있는 Setup메소드 호출
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
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage; //람다식 매개변수 선언 후 igniteDamage = _damage와 같음.
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    public virtual void TakeDamage(int _damage)
    {
        //데미지를 주는 메소드 int형 매개변수 _damege
        //TakeDamage 메소드가 호출되면 현재체력에 해당하는 currentHealth의 값에서 _damage 값 만큼 뺀다.
        //이때 만약 현재 체력이 0이하라면 Die메소드를 호출한다.

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        //매개변수 int를 포함한 가상메소드
        //현재체력에서 데미지만큼 뺀다.

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

        //이때 총 데미지는 0이하, int.MaxValue안의 값으로만 설정된다.
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3); //마법저항력, 지능 수치에 따른 데미지 감소
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue); //데미지가 음수가 되지 않도록 방지
        return totalMagicalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        //bool값을 반환하는 메소드

        //int형 변수 totalEvasion은 총 회피율을 나타낸다.
        //이 총 회피율은 evasion, armor를 합한 값으로 한다.
        //랜덤으로 0부터 99중의 값을 생성하고, 만약 그 랜덤한 값이 totalEvasion 미만이라면 retrun을 true로 한다.

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
        //매개변수 int _damage를 사용하는 int형 반환 메소드
        //크리티컬 확률, 데미지 메소드

        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        Debug.Log("total crit power %" + totalCritPower);
        
        //위의 공식에 매개변수 _damage를 곱해주는 것으로 실제 크리티컬 데미지의 값을 구한다.
        //그리고 그 값을 반환해준다.

        float critDamage = _damage * totalCritPower;
        Debug.Log("crit damage before round up" + critDamage);

        //반올림 int형변환 함수
        //critDamage 값을 반올림하여 int형으로 형변환한다.
        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion
}
