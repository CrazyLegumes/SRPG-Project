using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : BattleState
{
    BaseUnit unit;
    UnitUI ui;


    public override void Enter()
    {
        base.Enter();
        unit = GameMachine.instance.selectedUnit;
        ui = unit.GetComponent<UnitUI>();
        ui.ShowInventory();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }


    protected override void OnFire(object sender, InputEvents<int> e)
    {
        base.OnFire(sender, e);
        switch (e.info)
        {
            case 0:
                if (!ui.selectingItem)
                    ui.ShowItemOptions();
                else
                    ui.ExecuteAction();

                break;
            case 1:
                if (ui.selectingItem)
                {
                    ui.DestroyItemInfo();
                    ui.ResetCursorInventory();
                }
                else
                    GameMachine.instance.ChangeState<SelectAction>();
                break;
        }
    }

    protected override void OnMove(object sender, InputEvents<Vector3> e)
    {
        base.OnMove(sender, e);
        if (!ui.selectingItem)
            ui.MoveCursorInventory(e.info);
        else
            ui.MoveCursorInfo(e.info);
    }


}
