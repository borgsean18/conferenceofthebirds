using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_pOwner;
    private State<T> m_pCurrentState;

    public StateMachine(T owner)
    {
        m_pOwner = owner;
    }

    public void SetCurrentState(State<T> state)
    {
        m_pCurrentState = state;
    }

    public void StateMachineUpdate()
    {

        if (m_pCurrentState != null)
        {
            m_pCurrentState.Execute(m_pOwner);
        }
    }

    public void ChangeState(State<T> newState)
    {
        m_pCurrentState.Exit(m_pOwner);
        m_pCurrentState = newState;
        m_pCurrentState.Enter(m_pOwner);
    }

    /// <summary>
    /// 返回之前的状态
    /// </summary>


    public State<T> CurrentState()
    {
        return m_pCurrentState;
    }

    public bool IsInState(State<T> state)
    {
        return m_pCurrentState == state;
    }
}