using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    //현재 적의 상태를 나타내는 속성 (읽기 전용)
    public EnemyState currentState {  get; private set; }

    //상태 머신을 초기화하는 메서드
    public void Initialize(EnemyState _startState)
    {
        //현재 상태를 _startState로 설정하고, 해당 상태의 Enter함수 호출
        currentState = _startState;
        currentState.Enter();
    }

    //상태를 변경하는 메서드
    public void ChangeState(EnemyState _newState)
    {
        //현재 상태의 Exit함수를 호출하고, 새로운 상태로 변경 후 Enter함수를 호출한다.
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
