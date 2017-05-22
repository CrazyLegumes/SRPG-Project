using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : BattleState
{

    public override void Enter()
    {
        base.Enter();
        if (GameMachine.instance.cursor.highlighted != null)
        {
            GameMachine.instance.cursor.highlighted.selected = false;
            GameMachine.instance.cursor.highlighted = null;
        }
        GameMachine.instance.selectedUnit = null;
        Debug.Log(GameMachine.instance.CurrentState);
        GameMachine.instance.cursor.canMove = true;



    }


    //Move Cursor Around and Press button to select unit



    // Use this for initialization
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
                if(GameMachine.instance.cursor.highlighted != null)
                {
                    GameMachine.instance.selectedUnit = GameMachine.instance.cursor.highlighted;
                    if(GameMachine.instance.selectedUnit.active && GameMachine.instance.selectedUnit.team == BaseUnit.Team.allies)
                        GameMachine.instance.ChangeState<SelectMovement>();
                }
                break;
            default:
                break;
        }
    }
}
