using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class PlayerPathing : MonoBehaviour
{


    public Tile start;
    bool pathing;
    public Tile hitTile;
    Vector3 pos;
    bool canmove;


    List<Tile> closed;
    List<Tile> inRange;
    List<Vector3> mypath;
    List<GameObject> inRangeObjs;
    List<GameObject> pathObjs;

    [SerializeField]
    GameObject path, cursor, myRange;

    [SerializeField]
    Transform player, cursortran;

    [SerializeField]
    Vector2 cursorpos;

    [SerializeField]
    int range;

    bool MapCreated;

    public MapGen mapobj;

    // Use this for initialization
    void Start()
    {
        MapCreated = false;
        mypath = new List<Vector3>();
        inRangeObjs = new List<GameObject>();
        pathObjs = new List<GameObject>();


        ThreadQueue.StartThreadFunction(WaitForMapGen);



    }

    void WaitForMapGen()
    {
        while (!MapGen.mapCreated) ;

        Debug.Log("Done");

        MapCreated = true;
        Action find = () =>
        {
            mapobj = FindObjectOfType<MapGen>();
            player = transform;
            pos = player.position;
            cursortran = Instantiate(cursor, new Vector3(pos.x, .3f, pos.z), Quaternion.Euler(90, 0, 0)).transform;
            cursorpos = new Vector2(cursortran.position.x, cursortran.position.z);
            Debug.Log("Player Assigned");
        };
        ThreadQueue.QueueAction(find);

        Thread.Sleep(10);

        ThreadQueue.StartThreadFunction(FindRange);




    }

    void CheckTile()
    {
        hitTile = mapobj.map[(int)cursorpos.x, (int)cursorpos.y];

        if (hitTile.walkable && inRange.Contains(hitTile))
        {

            ThreadQueue.StartThreadFunction(AStarFind);
        }
    }

    void CursorInput()
    {
        if (mapobj != null)
        {




            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cursorpos.y += 1;
                if (!pathing)
                    CheckTile();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cursorpos.y -= 1;

                if (!pathing)
                    CheckTile();

            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cursorpos.x += 1;
                if (!pathing)
                    CheckTile();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cursorpos.x -= 1;
                if (!pathing)
                    CheckTile();
            }


            cursorpos.x = Mathf.Clamp(cursorpos.x, 0, mapobj.mapX - 1);
            cursorpos.y = Mathf.Clamp(cursorpos.y, 0, mapobj.mapY - 1);
            cursortran.position = new Vector3(cursorpos.x, .5f, cursorpos.y);
            player.position = pos;



        }
    }
    // Update is called once per frame
    void Update()
    {


        CursorInput();

        canmove = !pathing;


        if (Input.GetKeyDown(KeyCode.Z) && closed != null && !pathing)
        {
            if (closed.Count > 0 && inRange.Contains(hitTile))
            {
                ThreadQueue.StartThreadFunction(Path);
            }
        }
        /*
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

    void FindRange()
    {
        while (mapobj == null) ;

        inRange = new List<Tile>();



        int totalR = RecursiveRange(range);

        List<Tile> open = new List<Tile>();
        bool done = false;
        int tileCount = 0;

        Tile center = mapobj.map[(int)pos.x, (int)pos.z];
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




            try { right = mapobj.map[curr.x + 1, curr.y]; } catch (Exception e) { right = null; }
            try { left = mapobj.map[curr.x - 1, curr.y]; } catch (Exception e) { left = null; }
            try { up = mapobj.map[curr.x, curr.y + 1]; } catch (Exception e) { up = null; }
            try { down = mapobj.map[curr.x, curr.y - 1]; } catch (Exception e) { down = null; }



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
                inRangeObjs.Add(Instantiate(myRange, new Vector3(curr.x, .03f, curr.y), Quaternion.identity));

            };

            ThreadQueue.QueueAction(func);
        }
    }


    void Path()
    {
        Thread.Sleep(100);
        Debug.Log("Walking..");
        ThreadQueue.StartThreadFunction(DestroyPath);
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



            }
            pos = new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z));




        }

        mypath.Clear();
        ThreadQueue.StartThreadFunction(ClearParents);
        pathing = false;
        ThreadQueue.StartThreadFunction(FindRange);
        // start = mapobj.map[(int)pos.x, (int)pos.z];
    }

    public void ClearParents()
    {
        for (int i = 0; i < mapobj.mapX; i++)
            for (int j = 0; j < mapobj.mapY; j++)
                mapobj.map[i, j].parent = null;
    }

    public void AStarFind()
    {


        Tile endTile = hitTile;

        start = mapobj.map[(int)pos.x, (int)pos.z];




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
            if (mapobj.map[curr.x + 1, curr.y].walkable)
                adjacents.Add(mapobj.map[curr.x + 1, curr.y]);
            //Tile to Left;
            if (mapobj.map[curr.x - 1, curr.y].walkable)
                adjacents.Add(mapobj.map[curr.x - 1, curr.y]);
            //Tile to North
            if (mapobj.map[curr.x, curr.y + 1].walkable)
                adjacents.Add(mapobj.map[curr.x, curr.y + 1]);
            //Tile to South
            if (mapobj.map[curr.x, curr.y - 1].walkable)
                adjacents.Add(mapobj.map[curr.x, curr.y - 1]);

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

    void DestroyPath()
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

    void DestroyRange()
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
                pathObjs.Add(Instantiate(path, new Vector3(xpos, .05f, ypos), Quaternion.identity));
            };

            ThreadQueue.QueueAction(func);
        }
    }
}
