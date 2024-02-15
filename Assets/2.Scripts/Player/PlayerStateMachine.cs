using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //프로퍼티 public(공용) PlayerState(클래스명)를 currentState(변수명)으로 읽기와 쓰기를 넣어주는 것
    //해당 프로퍼티는 public으로 되어있기 때문에 쓰기가 가능한 set 앞에 private를 부여하는 것
    public PlayerState currentState {  get; private set; }
    //초기화 메서드 : 시작 상태를 설정하고 해당 상태로 진입
    public void Initialize(PlayerState _startState)
    {
        //변수로 지정된 PlayerState를 _startState로 초기값 set
        currentState = _startState;
        //_startState에 진입
        currentState.Enter();
    }

    //상태 변경 메서드 : 새로운 상태로 전환
    public void ChangeState(PlayerState _newState)
    {
        //현재 상태에서 빠져나감
        currentState.Exit();
        //새로운 상태로 변경
        currentState = _newState;
        //새로운 상태에 진입
        currentState.Enter();
    }
}
