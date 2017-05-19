using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class GameMachine : StateMachine
{
    /*
        States Wanted:
        Player Start
        Select Unit
        Select Movement Target
        Move Unit
        Select Action
        Execute Action
        Rinse Repeat



        Transitions Allowed:
        PS -> SU
        SU -> SMT
        SMT -> SU
        SMT -> MU
        MU -> SMT
        MU -> SA
        SA -> SMT
        SA -> EA
        EA -> PS



    */


    public static GameMachine instance;
    public List<BaseUnit> units;
    public BaseUnit selectedUnit;
    public CameraFollow cam;

    [SerializeField]
    GameObject cursorObj;

    public Cursor cursor;
    public MapEditor mapobj;




    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        ThreadQueue.StartThreadFunction(WaitForMapGen);
        
        
        

        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public override void ChangeState<T>()
    {

        if (typeof(BattleState).IsAssignableFrom(typeof(T)))
            base.ChangeState<T>();
    }




    void WaitForMapGen()
    {
        //while (!MapGen.mapCreated) ;

        Debug.Log("Done");
        

        
        Action find = () =>
        {
            mapobj = FindObjectOfType<MapEditor>();

            foreach (Tile x in mapobj.map)
            {
                Debug.LogFormat("Xpos: {0}, Ypos: {1}", x.x, x.y);
            }
            cam = FindObjectOfType<CameraFollow>();
            Debug.LogFormat("MapX {0}, MapY {1}", mapobj.mapLength, mapobj.mapWidth); 

            Instantiate(cursorObj, new Vector3(1, .01f, 1), Quaternion.Euler(90,0,0));
            cursor = FindObjectOfType<Cursor>();
            cam.target = cursor.transform;
            cam.following = true;
            Debug.Log("Player Assigned");
            ChangeState<PlayerEnter>();
        };
        ThreadQueue.QueueAction(find);


        Thread.Sleep(10);
        

        //ThreadQueue.StartThreadFunction(FindRange);




    }


}
