using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public abstract class AIState<T>
{
    public string stateName="";
    public T owner;

    public IStateAI<T> stateAI;
    public GameObject ownerGo;
    protected AIState(string stateName, T owner)
    {
        this.stateName = stateName;
        this.owner = owner;
        stateAI = this.owner as IStateAI<T>;
        //Debug.Log((owner as Customer).gameObject.name+"切换状态到"+stateName);
        ownerGo = stateAI.GetObj();
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnExit()
    {
        
    }
}