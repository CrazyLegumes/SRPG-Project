using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[System.Serializable]
public class Tile
{
    public bool walkable;
    public bool start;
    public int x, y, f, g, h,cost;
    public Vector2 position;
    public Tile parent;



    public static int Distance(Tile a, Tile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    public Tile(int a, int b, bool walk = true)
    {

        walkable = walk;
        x = a;
        y = b;
        cost = 1;


        position = new Vector2(x, y);
        parent = null;
        f = g = h = 0;

    }

    public void ResetParent()
    {
        this.parent = null;
    }


}

public class MapGen : MonoBehaviour
{



    [SerializeField]
    public int mapX, mapY;

    public static int MAPX, MAPY;

    [SerializeField]
    GameObject tile, startTile, wall, cursor;


    public Tile start;
    bool startSpawned;


    public Tile[,] map;
    //Doot
    public static bool mapCreated;

    [SerializeField]
    int tilecount;





    // Use this for initialization
    void Start()
    {
        tilecount = 0;


        map = new Tile[mapX, mapY];
        
        ThreadQueue.StartThreadFunction(GenerateMap);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Tile GetStart()
    {
        return start;
    }

    void GenerateMap()
    {

       
        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {

                int x = i;
                int y = j;
                if (x == 0 || y == 0 || mapX - 1 == x || mapY - 1 == y)
                    map[x, y] = new Tile(x, y, false);
                else
                    map[x, y] = new Tile(x, y);


                Action func = () =>
                {



                    if (!startSpawned && UnityEngine.Random.Range(1, 100) > 98 && map[x, y].walkable)
                    {
                        map[x, y].start = true;
                        start = map[x, y];
                        Debug.LogFormat("Start X is {0} and Start Y is {1} ", start.x, start.y);
                        startSpawned = true;
                    }


                    if (map[x, y].start)
                    {
                        Instantiate(startTile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), transform);

                    }
                    else
                    {
                        if (map[x, y].walkable)
                            Instantiate(tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), transform);
                        else
                            Instantiate(wall, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), transform);

                    }

                    tilecount++;
                };
                
                ThreadQueue.QueueAction(func);
            }
        }



        Action spawn = () =>
        {
            if (start != null)
            {
                Debug.LogFormat("X is {0}, Y is {1}", start.x, start.y);
                int l = start.x; int m = start.y;
                Instantiate(cursor, new Vector3(l, 0, m), Quaternion.Euler(90,0,0));
                mapCreated = true;
                Debug.Log("Finished Generating Map");


            }



        };

        ThreadQueue.QueueAction(spawn);
        
        


    }
}
