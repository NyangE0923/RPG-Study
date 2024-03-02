using System;
using UnityEngine;


public enum SwordType //SwordType이라는 Enum을 생성해서 타입을 고를 수 있도록 한다.
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Peirce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenereateDots();

        SetupGravity();
    }
    private void SetupGravity() //검 스킬의 중력 전용 메서드
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
            }
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }
    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        //int i를 0으로 초기화 하고 i가 dots.Length보다 작다면 i를 더한다
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenereateDots()
    {
        //dots는 새로운 오브젝트 배열을 생성, 배열의 크기는 numberOfDots 변수에 저장된 값
        dots = new GameObject[numberOfDots];
        //반복문을 사용하여 dots 배열의 각 요소를 초기화
        //초기화 되는 요소의 수는 numberOfDots와 같다.
        //int i는 0이고 i가 numberOfDots보다 작다면 i에 1을 더한다.
        for (int i = 0; i < numberOfDots; i++)
        {
            //'dots' i번째 요소에 dotPrefab을 복제하여 새로운 GameObject 인스턴스를 생성
            //Player의 transform.position(위치)에 생성되며 회전은 적용되지 않은 상태로 생성된다.
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    //점의 위치를 계산하는 함수
    //t 매개변수는 시간을 나타내며 해당시간에 따른 점의 위치를 계산한다.
    private Vector2 DotsPosition(float t)
    {
        //점의 위치를 저장할 Vector2 변수를 선언하고
        //이 변수를 player.transform.position(플레이어의 위치)로 초기화한다.
        //AimDirection()메서드가 반환한 벡터를 정규화(normalized)하고 launchForce의 x와 y에 곱한다.
        //런치 방향과 런치 힘의 크기를 고려하여 점의 초기 속도를 설정하는 것
        //중력을 고려하여 t에 따른 중력의 영향을 더한다.
        //t * t는 시간의 제곱을 의미하며, 중력과 추가적인 속도 변화를 고려한다.
        Vector2 position = (Vector2)player.transform.position +
            new Vector2(AimDirection().normalized.x * launchForce.x,
                        AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        //계산된 위치를 반환한다.
        return position;
    }
    #endregion
}
