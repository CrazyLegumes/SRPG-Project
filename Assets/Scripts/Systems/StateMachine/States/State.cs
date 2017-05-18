using UnityEngine;
using System.Collections;
[System.Serializable]
public abstract class State : MonoBehaviour {

    public virtual void Enter()
    {
        AddListeners();
    }

    public virtual void Exit()
    {
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners()
    {

    }

    protected virtual void RemoveListeners()
    {

    }

    public override string ToString()
    {
        return this.GetType().Name;
    }

}
