using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnter : BattleState{

    //Refresh All Units and Play some Anim
    public override void Enter()
    {
        base.Enter();
        foreach (BaseUnit b in GameMachine.instance.units)
            b.active = true;


        StartCoroutine(waiter());
        
    }



    IEnumerator waiter()
    {
        yield return new WaitForSeconds(.05f);
        GameMachine.instance.ChangeState<SelectUnit>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}
}
