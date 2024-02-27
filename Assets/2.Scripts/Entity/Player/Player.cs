using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    public bool isBusy {  get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float swordReturnImpact;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }

    [Header("Wall Jump info")]
    public float xWallJumpForce;
    public float yWallJumpForce;


    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword {  get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        //PlayerState���� ������ ������ Player��� ������Ʈ�� StateMachine�� �ִ� Idle �ִϸ��̼��� �����´�.
        //idleState�� bool���� Idle�� �ش��Ѵ�.
        idleState = new PlayerIdleState(this, StateMachine, "Idle");
        //moveState�� bool���� Move�� �ش��Ѵ�.
        moveState = new PlayerMoveState(this, StateMachine, "Move");
        dashState = new PlayerDashState(this, StateMachine, "Dash");
        jumpState = new PlayerJumpState(this, StateMachine, "Jump");
        airState  = new PlayerAirState(this, StateMachine, "Jump");
        wallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, StateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, StateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
    }

    protected override void Start()
    {
        base.Start();

        //SkillManager.instance�� ���� ������ skill�̶�� ġ�� �ȴ� ��ȣ!
        skill = SkillManager.instance;

        StateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.currentState.Update();
        CheckForDashInput();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    //�־��� �ð� ���� isBusy�� true�� ������ ��, �ش� �ð��� ����ϸ� �ٽ� false�� �����Ѵ�.
    public IEnumerator BusyFor(float _seconds)
    {
        //isBusy ������ true�� �����Ͽ� ���� �۾� ������ ��Ÿ���ϴ�.
        isBusy = true;

        //waitForSecounds(_seconds)�� ����Ͽ� �־��� �ð�(_seconds)��ŭ ����Ѵ�.
        //�̸� ���� ������ �ð���ŭ �ڷ�ƾ�� ����ϰ�, ��Ⱑ �Ϸ�Ǹ� ���� �ڵ��� isBusy = false;�� ������
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //���� ǥ������ ȭ��ǥ �����ڸ� ����� PlayerStateMachineŬ������ CurrentStage�� �����ϰ� AnimationFinishTrigger�޼��带 ȣ���Ѵ�.
    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        //dashUsageTimer -= Time.deltaTime; ��� ��Ÿ���� ��Ÿ Ÿ�Ӹ�ŭ ����.
        //dashUsageTimer = dashCooldown; �ٽ� �ν�����â���� ���ص� ��Ÿ������ �ǵ�����

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()) //��ų ��Ÿ���� 0�̸��� �Ǿ��� �� ��ø� ������
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            StateMachine.ChangeState(dashState); //dashState�� �̵�
        }
    }
}
