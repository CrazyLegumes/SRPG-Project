﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMovement : BattleState 
{

    BaseUnit unit;

    public override void Enter()
    {
        base.Enter();
        unit = GameMachine.instance.selectedUnit;
        ThreadQueue.StartThreadFunction(unit.unitPathing.FindMoveRange);
        //ThreadQueue.StartThreadFunction(unit.unitPathing.CheckTile);
    }


    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void OnFire(object sender, InputEvents<int> e)
    {
        base.OnFire(sender, e);

        switch (e.info)
        {
            case 0:
                unit.cancelPos = unit.transform.position;
                if (unit.unitPathing.mypath.Count != 0)
                {
                    
                   
                    ThreadQueue.StartThreadFunction(unit.unitPathing.Path);
                    GameMachine.instance.ChangeState<SelectAction>();

                }
                break;

            case 1:
                ThreadQueue.StartThreadFunction(unit.unitPathing.DestroyRange);
                ThreadQueue.StartThreadFunction(unit.unitPathing.DestroyPath);
                GameMachine.instance.selectedUnit.selected = false;
                GameMachine.instance.selectedUnit = null;
                GameMachine.instance.ChangeState<SelectUnit>();
                break;
            default:
                break;
        }
    }

    protected override void OnMove(object sender, InputEvents<Vector3> e)
    {
        base.OnMove(sender, e);
        ThreadQueue.StartThreadFunction(unit.unitPathing.CheckTile);
    }
}
