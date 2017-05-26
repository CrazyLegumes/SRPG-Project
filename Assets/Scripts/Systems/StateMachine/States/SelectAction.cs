using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : BattleState
{
    BaseUnit unit;
    UnitUI ui;
    bool canselect;



    public override void Enter()
    {
        base.Enter();
        canselect = false;
        unit = GameMachine.instance.selectedUnit;
        ui = unit.GetComponent<UnitUI>();
        GameMachine.instance.cursor.canMove = false;

        StartCoroutine(WaitForPathing());
    }


    IEnumerator WaitForPathing()
    {
        yield return new WaitForSeconds(.5f);
        while (unit.unitPathing.pathing)
        {
            yield return null;
        }

        ui.ShowOptions();
        canselect = true;
    }

    protected override void OnMove(object sender, InputEvents<Vector3> e)
    {

        base.OnMove(sender, e);
        if (canselect)
            ui.MoveCursorOptions(e.info);

    }
    protected override void OnFire(object sender, InputEvents<int> e)
    {
        base.OnFire(sender, e);
        if (canselect)
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
                        ThreadQueue.StartThreadFunction(unit.unitPathing.DestroyAttackTile);
                        ui.HideAll();
                        GameMachine.instance.ChangeState<SelectMovement>();
                    }
                    break;

                default:
                    break;

            }

    }

    //Display attack Range and a list of Actions to be taken


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
