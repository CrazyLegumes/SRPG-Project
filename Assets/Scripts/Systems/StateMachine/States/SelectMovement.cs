using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMovement : BattleState 
{

    BaseUnit unit;

    public override void Enter()
    {
        GameMachine.instance.cursor.canMove = true;
        base.Enter();
        unit = GameMachine.instance.selectedUnit;
        unit.unitPathing.mypath.Clear();
        GameMachine.instance.cursor.transform.position = new Vector3(unit.transform.position.x, 0.01f, unit.transform.position.z);
        Debug.Log(unit.name);
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
                if (unit.unitPathing.mypath.Count != 0 && GameMachine.instance.mapobj.map[(int) GameMachine.instance.cursor.transform.position.z  *
                    GameMachine.instance.mapobj.mapWidth + (int)GameMachine.instance.cursor.transform.position.x].occupant == null)
                {


                    ThreadQueue.StartThreadFunction(unit.unitPathing.Path);
                    GameMachine.instance.ChangeState<SelectAction>();

                }
                if(unit.unitPathing.mypath.Count == 0)
                {
                    unit.unitPathing.DestroyRange();

                    ThreadQueue.StartThreadFunction(unit.unitPathing.FindAttackRange);
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
