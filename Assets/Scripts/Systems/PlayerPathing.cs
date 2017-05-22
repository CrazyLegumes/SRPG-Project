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
    Vector3 trackedPos;

    public Tile[] map;
    public Tile[] rangeMap;

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
        Thread.Sleep(20);

        ClearParents();
        hitTile = GameMachine.instance.mapobj.map[(int)cursorpos.z * GameMachine.instance.mapobj.mapWidth + (int)cursorpos.x];


        if (hitTile.walkable && inRange.Contains(hitTile))
        {


            ThreadQueue.StartThreadFunction(AStarFind);

        }
    }





    // Update is called once per frame
    void Update()
    {

        trackedPos = transform.position;
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
        Thread.Sleep(20);



        inRange = new List<Tile>();


        int range = moveR;

        int totalR = RecursiveRange(range);

        List<Tile> open = new List<Tile>();
        bool done = false;
        int tileCount = 0;

        Tile center = GameMachine.instance.mapobj.map[(int)trackedPos.z * GameMachine.instance.mapobj.mapWidth + (int)trackedPos.x];
        open.Add(center);
        inRange.Add(center);//doo
        int repcount = 0;
        while (open.Count > 0)
        {
            repcount++;

            if (repcount >= GameMachine.instance.mapobj.mapdata.count)
                break;

            Tile curr = open[0];
            open.RemoveAt(0);







            Tile right = null;
            Tile left = null;
            Tile up = null;
            Tile down = null;




            try { right = GameMachine.instance.mapobj.map[(curr.y) * GameMachine.instance.mapobj.mapWidth + (curr.x + 1)]; }
            catch (Exception e)
            {
                right = null;

            }
            try { left = GameMachine.instance.mapobj.map[(curr.y) * GameMachine.instance.mapobj.mapWidth + (curr.x - 1)]; }
            catch (Exception e)
            {
                left = null;

            }
            try { up = GameMachine.instance.mapobj.map[(curr.y + 1) * GameMachine.instance.mapobj.mapWidth + (curr.x)]; }
            catch (Exception e)
            {
                up = null;

            }
            try { down = GameMachine.instance.mapobj.map[(curr.y - 1) * GameMachine.instance.mapobj.mapWidth + (curr.x - 1)]; }
            catch (Exception e)
            {
                down = null;

            }





            if (!open.Contains(right) && right != null && Tile.Distance(center, right) * right.cost <= range)
            {

                open.Insert(0, right);

                open.Add(right);
                if (!inRange.Contains(right))
                {
                    tileCount += right.cost;
                    if (right.walkable && Range_AStar(center, right, range))
                        inRange.Add(right);
                }
                //ClearParents();


            }

            if (!open.Contains(left) && left != null && Tile.Distance(center, left) * left.cost <= range)
            {

                open.Insert(0, left);

                if (!inRange.Contains(left))
                {
                    tileCount += left.cost;
                    if (left.walkable && Range_AStar(center, left, range))
                        inRange.Add(left);
                }
                //ClearParents();

            }

            if (!open.Contains(up) && up != null && Tile.Distance(center, up) * up.cost <= range)
            {

                open.Insert(0, up);
                if (!inRange.Contains(up))
                {
                    tileCount += up.cost;
                    if (up.walkable && Range_AStar(center, up, range))
                        inRange.Add(up);
                }
                //ClearParents();

            }

            if (!open.Contains(down) && down != null && Tile.Distance(center, down) * down.cost <= range)
            {


                //

                open.Insert(0, down);
                if (!inRange.Contains(down))
                {
                    tileCount += down.cost;
                    if (down.walkable && Range_AStar(center, down, range))
                        inRange.Add(down);
                }
               // ClearParents();

            }



            if (done)
                break;






        }
     //   ClearParents();


        rangeMap = new Tile[GameMachine.instance.mapobj.mapdata.count];

        foreach (Tile curr in inRange)
        {
            rangeMap[curr.y * GameMachine.instance.mapobj.mapWidth + curr.x] = curr;
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
        ClearParents();
        pathing = false;

        //ThreadQueue.StartThreadFunction(FindRange);
        // start = mapobj.map[(int)pos.x, (int)pos.z];
    }

    public void ClearParents()
    {
        for (int i = 0; i < GameMachine.instance.mapobj.mapWidth; i++)
            for (int j = 0; j < GameMachine.instance.mapobj.mapLength; j++)
            {
                Tile reset = GameMachine.instance.mapobj.map[j * GameMachine.instance.mapobj.mapWidth + i];
                reset.parent = null;
                reset.g = reset.f = reset.h = 0;

            }
    }


    bool Range_AStar(Tile begin, Tile endTile, int range, bool data = false)
    {
        Debug.Log("Started");

       // ClearParents();

        bool stop = false;

        if (endTile.x == 3 && endTile.y == 2)
            data = true;



        List<Tile> pathing = new List<Tile>();
        List<Tile> check = new List<Tile>();
        begin.f = 100;
        begin.h = Tile.Distance(begin, endTile);


        Tile current = begin;
        check.Add(begin);

        while (check.Count > 0)
        {

            current = check[0];
            foreach (Tile e in check)
            {


                // Debug.LogFormat("E's F score {0} currents F Score {1}", e.f, current.f);
                if (e.f < current.f || e.f == current.f && e.h < current.h)
                    current = e;
                if (current == endTile)
                {
                    stop = true;
                    break;
                }
            }
            
            /*
            if (data)
                Debug.LogFormat("Lowest Tile Current Stats X: {0} Y: {1} G: {2} H: {3} F: {4}", current.x, current.y, current.g, current.h, current.f);
*/
            if (!check.Remove(current))
            {
                Debug.LogFormat("Failed to Remove Tile at {0} , {1}, End Tile Was: {2} , {3} ", current.x, current.y, endTile.x, endTile.y);

                return false;

            }
            pathing.Add(current);



            if (stop)
                break;

            List<Tile> adj = new List<Tile>();

            Tile right = null;
            Tile left = null;
            Tile up = null;
            Tile down = null;




            try { right = GameMachine.instance.mapobj.map[(current.y) * GameMachine.instance.mapobj.mapWidth + (current.x + 1)]; }
            catch (Exception e)
            {
                right = null;

            }
            try { left = GameMachine.instance.mapobj.map[(current.y) * GameMachine.instance.mapobj.mapWidth + (current.x - 1)]; }
            catch (Exception e)
            {
                left = null;

            }
            try { up = GameMachine.instance.mapobj.map[(current.y + 1) * GameMachine.instance.mapobj.mapWidth + (current.x)]; }
            catch (Exception e)
            {
                up = null;

            }
            try { down = GameMachine.instance.mapobj.map[(current.y - 1) * GameMachine.instance.mapobj.mapWidth + (current.x)]; }
            catch (Exception e)
            {
                down = null;

            }

            if (right != null && right.walkable)
                adj.Add(right);
            if (left != null && left.walkable)
                adj.Add(left);
            if (up != null && up.walkable)
                adj.Add(up);
            if (down != null && down.walkable)
                adj.Add(down);


            foreach (Tile e in adj)
            {
                if (pathing.Contains(e))
                    continue;

                int newCost = current.g + Tile.Distance(current, e) + e.cost;
                if (newCost < e.g || !check.Contains(e))
                {
                    e.g = newCost;
                    e.h = Tile.Distance(e, endTile);
                    e.f = e.g + e.h;
                    e.parent = current;

                    if (!check.Contains(e))
                        check.Add(e);

                }
            }

        }

        // ClearParents();

        List<Tile> trueList = new List<Tile>();

        pathing.Reverse();
        Tile me = pathing[0];
        int totalcost = 0;

        while (me.parent != null)
        {
           
            trueList.Add(me);
            totalcost += me.cost;
            me = me.parent;
        }
        


        if (trueList.Count > range || totalcost > range)
            return false;
        else
            return true;
    }

    public void AStarFind()
    {
        Thread.Sleep(10);

        Tile endTile = hitTile;

        start = GameMachine.instance.mapobj.map[(int)trackedPos.z * GameMachine.instance.mapobj.mapWidth + (int)trackedPos.x];




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
            curr = open[0];
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

                Debug.Log("Tile that Failed: " + curr.position);

                break;
            }


            closed.Add(curr);
            if (done)
                break;


            List<Tile> adjacents = new List<Tile>();


            Tile right = null;
            Tile left = null;
            Tile up = null;
            Tile down = null;




            try { right = rangeMap[(curr.y) * GameMachine.instance.mapobj.mapWidth + (curr.x + 1)]; }
            catch (Exception e)
            {
                right = null;

            }
            try { left = rangeMap[(curr.y) * GameMachine.instance.mapobj.mapWidth + (curr.x - 1)]; }
            catch (Exception e)
            {
                left = null;

            }
            try { up = rangeMap[(curr.y + 1) * GameMachine.instance.mapobj.mapWidth + (curr.x)]; }
            catch (Exception e)
            {
                up = null;

            }
            try { down = rangeMap[(curr.y - 1) * GameMachine.instance.mapobj.mapWidth + (curr.x)]; }
            catch (Exception e)
            {
                down = null;

            }


            //Tile to Right;
            if (right != null && right.walkable)
                adjacents.Add(right);
            //Tile to Left;
            if (left != null && left.walkable)
                adjacents.Add(left);
            //Tile to North
            if (up != null && up.walkable)
                adjacents.Add(up);
            //Tile to South
            if (down != null && down.walkable)
                adjacents.Add(down);

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



            foreach (Tile e in adjacents)
            {
                if (closed.Contains(e))
                    continue;

                int newCost = curr.g + Tile.Distance(curr, e) + e.cost;
                if (newCost < e.g || !open.Contains(e))
                {
                    e.g = newCost;
                    e.h = Tile.Distance(e, endTile);
                    e.f = e.g + e.h;
                    e.parent = curr;

                    if (!open.Contains(e))
                        open.Add(e);

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
        Thread.Sleep(5);
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

        Thread.Sleep(5);
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
