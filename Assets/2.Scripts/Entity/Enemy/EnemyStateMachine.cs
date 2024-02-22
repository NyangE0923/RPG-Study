using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    //���� ���� ���¸� ��Ÿ���� �Ӽ� (�б� ����)
    public EnemyState currentState {  get; private set; }

    //���� �ӽ��� �ʱ�ȭ�ϴ� �޼���
    public void Initialize(EnemyState _startState)
    {
        //���� ���¸� _startState�� �����ϰ�, �ش� ������ Enter�Լ� ȣ��
        currentState = _startState;
        currentState.Enter();
    }

    //���¸� �����ϴ� �޼���
    public void ChangeState(EnemyState _newState)
    {
        //���� ������ Exit�Լ��� ȣ���ϰ�, ���ο� ���·� ���� �� Enter�Լ��� ȣ���Ѵ�.
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
