using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : BattleState
{
    BaseUnit unit;

    

    public override void Enter()
    {
        base.Enter();
        unit = GameMachine.instance.selectedUnit;
        GameMachine.instance.cursor.canMove = false;
    }
    protected override void OnFire(object sender, InputEvents<int> e)
    {
        base.OnFire(sender, e);
        switch (e.info)
        {

            case 0:
                if (!unit.unitPathing.pathing)
                {
                    ThreadQueue.StartThreadFunction(unit.unitPathing.DestroyPath);
                    unit.active = false;
                    GameMachine.instance.selectedUnit.selected = false;
                    GameMachine.instance.selectedUnit = null;
                    GameMachine.instance.ChangeState<SelectUnit>();

                }
                break;
            case 1:
                if (!unit.unitPathing.pathing)
                {
                    unit.transform.position = unit.cancelPos;
                    ThreadQueue.StartThreadFunction(unit.unitPathing.DestroyPath);
                    GameMachine.instance.ChangeState<SelectMovement>();
                }
                break;

            default:
                break;
                
        }
        
    }

    //Display attack Range and a list of Actions to be taken


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
