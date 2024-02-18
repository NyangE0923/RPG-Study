using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //PlayerStateMachine 인스턴스를 저장하는 변수
    //protected = 상속 받은 자식 클래스만 사용 가능
    protected PlayerStateMachine stateMachine;
    protected float stateTimer;
    //Player 인스턴스를 저장하는 변수
    protected Player player;
    protected Rigidbody2D rb;

    [Header("Input")]
    protected float xInput;
    protected float yInput;
    protected bool triggerCalled;

    //현재 상태에서 사용될 애니메이션 bool 변수의 이름
    private string animBoolName;


    //PlayerState의 생성자
    //플레이어, 플레이어의 상태 머신, 애니메이션 bool 변수의 이름을 매개변수로 받는다.
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        //받은 매개변수로 변수 초기화
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }

    //상태에 진입했을 때 호출되는 가상 메서드
    //virtual 다른 클래스에서도 override(재정의)를 통해 수정할 수 있도록 허용해주는 것
    public virtual void Enter()
    {
        //상태에 진입할 때 실행될 코드 작성 가능
        //PlayerState의 rigidbody2D의 값에 player클래스의 rigidbody2D의 값을 상속한다.(원래라면 getcomponent 알지?
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;

        //연속 공격 기본값
        triggerCalled = false;
    }

    //매 프레임마다 호출되는 가상 메서드
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        //매 프레임마다 실행될 코드 작성 가능
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // 플레이어 캐릭터의 수직 속도를 애니메이션에 전달하여 점프 애니메이션을 조절합니다.
        // player: 플레이어 객체
        // anim: 플레이어의 애니메이터 컴포넌트
        // rb: 플레이어의 Rigidbody 컴포넌트
        // "yVelocity": 애니메이션 블렌드 트리에서 사용될 파라미터 이름
        // rb.velocity.y: 플레이어의 수직 속도
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    //상태를 빠져나갈 때 호출되는 가상 메서드
    public virtual void Exit()
    {
        //상태를 빠져나갈 때 실행될 코드 작성 가능
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
