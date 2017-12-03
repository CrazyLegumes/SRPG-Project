using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : BattleState
{

    BaseUnit unit;
    UnitUI ui;
    List<BaseUnit> attackables;
    BaseUnit currentTarget;
    int index;
    public override void Enter()
    {

        base.Enter();
        attackables = new List<BaseUnit>();
        index = 0;

        unit = GameMachine.instance.selectedUnit;
        ui = unit.GetComponent<UnitUI>();
        foreach (Tile e in unit.unitPathing.attRange)
        {
            if (e.occupant != null && e.occupant.team == BaseUnit.Team.enemies)
                attackables.Add(e.occupant);
        }


        currentTarget = attackables[index];
        //Change Camera Target


    }

    void CheckLeft()
    {

        BaseUnit farthest = currentTarget;
        foreach (BaseUnit a in attackables)
        {
            if (a.transform.position.x < farthest.transform.position.x)
                farthest = a;
        }
        if (farthest == currentTarget)
        {
            foreach (BaseUnit a in attackables)
            {
                if (a.transform.position.x > farthest.transform.position.x)
                    farthest = a;
            }
        }
        currentTarget = farthest;
    }

    void CheckRight()
    {
        BaseUnit farthest = currentTarget;
        foreach (BaseUnit a in attackables)
        {
            if (a.transform.position.x > farthest.transform.position.x)
                farthest = a;
        }
        if (farthest == currentTarget)
        {
            foreach (BaseUnit a in attackables)
            {
                if (a.transform.position.x < farthest.transform.position.x)
                    farthest = a;
            }
        }
        currentTarget = farthest;
    }

    void CheckUp()
    {
        BaseUnit farthest = currentTarget;
        foreach (BaseUnit a in attackables)
        {
            if (a.transform.position.z > farthest.transform.position.z)
                farthest = a;
        }
        if (farthest == currentTarget)
        {
            foreach (BaseUnit a in attackables)
            {
                if (a.transform.position.z < farthest.transform.position.z)
                    farthest = a;
            }
        }
        currentTarget = farthest;
    }


    void CheckDown()
    {
        BaseUnit farthest = currentTarget;
        foreach (BaseUnit a in attackables)
        {
            if (a.transform.position.z < farthest.transform.position.z)
                farthest = a;
        }
        if (farthest == currentTarget)
        {
            foreach (BaseUnit a in attackables)
            {
                if (a.transform.position.z > farthest.transform.position.z)
                    farthest = a;
            }
        }
        currentTarget = farthest;
    }

    protected override void OnMove(object sender, InputEvents<Vector3> e)
    {
        base.OnMove(sender, e);
        if (e.info.x != 0 & e.info.z == 0)
            switch ((int)e.info.x)
            {
                case 1:
                    CheckRight();

                    break;

                case -1:
                    CheckLeft();

                    break;
            }

        else if (e.info.x == 0 && e.info.z != 0)
            switch ((int)e.info.z)
            {
                case 1:
                    CheckUp();
                    break;
                case -1:
                    CheckDown();
                    break;
            }

    }


    protected override void OnFire(object sender, InputEvents<int> e)
    {
        base.OnFire(sender, e);
        Debug.Log(e.info);
        switch (e.info)
        {
            case 0:
                break;
            case 1:
                currentTarget = null;
                GameMachine.instance.cam.target = GameMachine.instance.cursor.transform;
                GameMachine.instance.ChangeState<SelectAction>();
                break;
        }
    }

    void UpdateTarget()
    {
        GameMachine.instance.cam.target = currentTarget.transform;
    }
    void Update()
    {
       
        if (currentTarget != null)
            UpdateTarget();
    }
}
