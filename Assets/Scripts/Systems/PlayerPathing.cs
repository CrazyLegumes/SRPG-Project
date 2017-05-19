using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class PlayerPathing : MonoBehaviour
{


    public Tile start;
    public bool pathing;
    bool cancel;
    public Tile hitTile;
    [SerializeField]
    Vector3 pos;
    bool canmove;

    int moveR;
    int AttR;

    List<Tile> closed;
    List<Tile> inRange;
    [HideInInspector]
    public List<Vector3> mypath;
    List<GameObject> inRangeObjs;
    List<GameObject> pathObjs;

    [SerializeField]
    GameObject path, cursor, myRange;

    [SerializeField]
    Transform player, cursortran;
    Vector3 cursorpos;

    public MapGen mapobj;

    // Use this for initialization
    void Start()
    {
        pos = transform.position;
        mypath = new List<Vector3>();
        inRangeObjs = new List<GameObject>();
        pathObjs = new List<GameObject>();
        moveR = GetComponent<BaseUnit>().moveRange;
        AttR = GetComponent<BaseUnit>().attackRange;
        cancel = false;

        InputController.fireEvent += CancelAction;





    }

    private void CancelAction(object sender, InputEvents<int> e)
    {
        if (pathing && e.info == 1)
        {
            cancel = true;
        }
    }

    public void CheckTile()
    {
        Thread.Sleep(10);
        hitTile = GameMachine.instance.mapobj.map[(int)cursorpos.x, (int)cursorpos.z];

        if (hitTile.walkable && inRange.Contains(hitTile))
        {

            ThreadQueue.StartThreadFunction(AStarFind);

        }
    }





    // Update is called once per frame
    void Update()
    {
        if (pathing)
            transform.position = pos;
        else
            pos = transform.position;
        if (GameMachine.instance.cursor != null)
            cursorpos = GameMachine.instance.cursor.transform.position;

        canmove = !pathing;

        /*
        if (Input.GetKeyDown(KeyCode.Z) && closed != null && !pathing)
        {
            if (closed.Count > 0 && inRange.Contains(hitTile))
            {
                ThreadQueue.StartThreadFunction(Path);
            }
        }
        
        
                if (Input.GetKeyDown(KeyCode.X) && MapCreated && !pathing)
                {

                    hitTile = mapobj.map[(int)cursorpos.x, (int)cursorpos.y];
                    if (hitTile.walkable && inRange.Contains(hitTile))
                    {
                        ThreadQueue.StartThreadFunction(AStarFind);
                    }
                }
        */




    }
    int RecursiveRange(int n)
    {

        if (n == 1)
            return 4;

        return (4 * n) + RecursiveRange(n - 1);

    }

    public void FindMoveRange()
    {
        Thread.Sleep(10);

        Action funx = () =>
        {
            Debug.Log(gameObject.name);

        };

        ThreadQueue.QueueAction(funx);

        inRange = new List<Tile>();


        int range = moveR;

        int totalR = RecursiveRange(range);

        List<Tile> open = new List<Tile>();
        bool done = false;
        int tileCount = 0;

        Tile center = GameMachine.instance.mapobj.map[(int)pos.x, (int)pos.z];
        open.Add(center);
        inRange.Add(center);//doo
        int repcount = 0;
        while (open.Count > 0)
        {
            repcount++;

            if (repcount >= totalR)
                break;
            Tile curr = open[0];
            open.RemoveAt(0);







            Tile right = null;
            Tile left = null;
            Tile up = null;
            Tile down = null;




            try { right = GameMachine.instance.mapobj.map[curr.x + 1, curr.y]; } catch (Exception e) { right = null; }
            try { left = GameMachine.instance.mapobj.map[curr.x - 1, curr.y]; } catch (Exception e) { left = null; }
            try { up = GameMachine.instance.mapobj.map[curr.x, curr.y + 1]; } catch (Exception e) { up = null; }
            try { down = GameMachine.instance.mapobj.map[curr.x, curr.y - 1]; } catch (Exception e) { down = null; }



            if (!open.Contains(right) && right != null && Tile.Distance(center, right) * right.cost <= range)
            {

                open.Add(right);
                if (!inRange.Contains(right))
                {
                    tileCount += right.cost;
                    if (right.walkable)
                        inRange.Add(right);
                }
            }

            if (!open.Contains(left) && left != null && Tile.Distance(center, left) * left.cost <= range)
            {

                open.Add(left);
                if (!inRange.Contains(left))
                {
                    tileCount += left.cost;
                    if (left.walkable)
                        inRange.Add(left);
                }
            }

            if (!open.Contains(up) && up != null && Tile.Distance(center, up) * up.cost <= range)
            {

                open.Add(up);
                if (!inRange.Contains(up))
                {
                    tileCount += up.cost;
                    if (up.walkable)
                        inRange.Add(up);
                }
            }

            if (!open.Contains(down) && down != null && Tile.Distance(center, down) * down.cost <= range)
            {
                //
                open.Add(down);
                if (!inRange.Contains(down))
                {
                    tileCount += down.cost;
                    if (down.walkable)
                        inRange.Add(down);
                }
            }


            if (tileCount >= totalR || open.Count == 0)
                done = true;
            if (done)
                break;






        }

       
        

        foreach (Tile curr in inRange)
        {
            Action func = () =>
            {
                inRangeObjs.Add(Instantiate(myRange, new Vector3(curr.x, .001f, curr.y), Quaternion.identity));

            };

            ThreadQueue.QueueAction(func);
        }
    }


    public void Path()
    {
        Thread.Sleep(100);
        Debug.Log("Walking..");
        // ThreadQueue.StartThreadFunction(DestroyPath);
        ThreadQueue.StartThreadFunction(DestroyRange);
        pathing = true;


        foreach (Vector3 c in mypath)
        {

            Vector3 dest = c;
            dest.y = 0;


            Vector3 desire = Vector3.Normalize(dest - pos) * 45 * .002f;




            while (Vector3.Distance(pos, dest) > .075f)
            {

                pos += desire;
                Thread.Sleep(10);

                if (cancel)
                {
                    break;
                }



            }
            pos = new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z));
            Debug.Log(GameMachine.instance.CurrentState);
            if (cancel)
                break;





        }

        if (cancel)
        {
            ThreadQueue.StartThreadFunction(DestroyPath);
            Action func = () =>
            {
                pathing = false;
                transform.position = GetComponent<BaseUnit>().cancelPos;
                cancel = false;
                GameMachine.instance.ChangeState<SelectMovement>();


            };

            ThreadQueue.QueueAction(func);
        }

        mypath.Clear();
        ThreadQueue.StartThreadFunction(ClearParents);
        pathing = false;

        //ThreadQueue.StartThreadFunction(FindRange);
        // start = mapobj.map[(int)pos.x, (int)pos.z];
    }

    public void ClearParents()
    {
        for (int i = 0; i < GameMachine.instance.mapobj.mapWidth; i++)
            for (int j = 0; j < GameMachine.instance.mapobj.mapLength; j++)
                GameMachine.instance.mapobj.map[i, j].parent = null;
    }

    public void AStarFind()
    {


        Tile endTile = hitTile;

        start = GameMachine.instance.mapobj.map[(int)pos.x, (int)pos.z];




        closed = new List<Tile>();
        mypath.Clear();


        List<Tile> open = new List<Tile>();



        start.f = 100;
        start.h = Mathf.Abs(start.x - endTile.x) + Mathf.Abs(start.y - endTile.y);
        bool done = false;


        open.Add(start);



        Tile curr = start;


        while (open.Count > 0)
        {
            foreach (Tile e in open)
            {

                if (e.h < curr.h)
                    curr = e;
                if (curr == endTile)
                {
                    done = true;
                    break;
                }
            }

            if (!open.Remove(curr))
            {
                Debug.Log("Failed TO Remove");
                Debug.Log("Tile that Failed: " + curr.position);

                break;
            }

            closed.Add(curr);
            if (done)
                break;


            List<Tile> adjacents = new List<Tile>();


            //Tile to Right;
            if (GameMachine.instance.mapobj.map[curr.x + 1, curr.y].walkable)
                adjacents.Add(GameMachine.instance.mapobj.map[curr.x + 1, curr.y]);
            //Tile to Left;
            if (GameMachine.instance.mapobj.map[curr.x - 1, curr.y].walkable)
                adjacents.Add(GameMachine.instance.mapobj.map[curr.x - 1, curr.y]);
            //Tile to North
            if (GameMachine.instance.mapobj.map[curr.x, curr.y + 1].walkable)
                adjacents.Add(GameMachine.instance.mapobj.map[curr.x, curr.y + 1]);
            //Tile to South
            if (GameMachine.instance.mapobj.map[curr.x, curr.y - 1].walkable)
                adjacents.Add(GameMachine.instance.mapobj.map[curr.x, curr.y - 1]);

            /*

                Diaganol Directions

                if (mapobj.map[curr.x + 1, curr.y + 1].walkable)
                    adjacents.Add(mapobj.map[curr.x + 1, curr.y + 1]);
                //Tile to Left;
                if (mapobj.map[curr.x - 1, curr.y + 1].walkable)
                    adjacents.Add(mapobj.map[curr.x - 1, curr.y + 1]);
                //Tile to North
                if (mapobj.map[curr.x + 1, curr.y - 1].walkable)
                    adjacents.Add(mapobj.map[curr.x + 1, curr.y - 1]);
                //Tile to South
                if (mapobj.map[curr.x - 1, curr.y - 1].walkable)
                    adjacents.Add(mapobj.map[curr.x - 1, curr.y - 1]);

                */



            foreach (Tile a in adjacents)
            {
                if (closed.Contains(a))
                {

                    continue;
                }


                else if (open.Contains(a))
                {
                    Tile m = null;
                    foreach (Tile x in open)
                    {
                        if (a == x)
                        {
                            m = x;
                            break;
                        }
                    }


                    if (a.g < m.g)
                    {

                        m.parent = curr;
                        m.g = a.g;
                        m.f = m.g + m.h;
                        continue;
                    }
                    else {

                        continue;

                    }

                }

                else
                {

                    a.parent = curr;
                    a.g = Mathf.Abs(a.x - start.x) + Mathf.Abs(a.y - start.y);
                    a.h = Mathf.Abs(a.x - endTile.x) + Mathf.Abs(a.y - endTile.y);
                    a.f = a.h + a.g;
                    open.Add(a);
                }
            }




        }






        if (done)
        {

            closed.Reverse();
            Tile me = closed[0];
            while (me.parent != null)
            {
                int xpos = me.x;
                int ypos = me.y;
                mypath.Add(new Vector3(xpos, 0, ypos));

                me = me.parent;

            }
            mypath.Reverse();
        }

        ThreadQueue.StartThreadFunction(DrawPath);

    }

    public void DestroyPath()
    {
        foreach (GameObject a in pathObjs.ToArray())
        {
            Action destroy = () =>
            {
                Destroy(a);
            };
            ThreadQueue.QueueAction(destroy);
        }
    }

    public void DestroyRange()
    {
        foreach (GameObject a in inRangeObjs.ToArray())
        {
            Action destroy = () =>
            {
                Destroy(a);
            };
            ThreadQueue.QueueAction(destroy);
        }
    }

    void DrawPath()
    {
        ThreadQueue.StartThreadFunction(DestroyPath);


        foreach (Vector3 a in mypath)
        {
            float xpos = a.x;
            float ypos = a.z;
            Action func = () =>
            {
                pathObjs.Add(Instantiate(path, new Vector3(xpos, .002f, ypos), Quaternion.identity));
            };

            ThreadQueue.QueueAction(func);
        }
    }
}
