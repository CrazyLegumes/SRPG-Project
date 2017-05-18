using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void AddListeners()
    {
        base.AddListeners();
        InputController.fireEvent += OnFire;
        InputController.moveEvent += OnMove;
        
    }


    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        InputController.fireEvent -= OnFire;
        InputController.moveEvent -= OnMove;
    }
    protected virtual void OnFire(object sender, InputEvents<int> e)
    {
        Debug.Log(e.info);
    }
    protected virtual void OnMove(object sender, InputEvents<Vector3> e)
    {

    }
    

   
}
