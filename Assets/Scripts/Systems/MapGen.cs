using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;



public class MapGen : MonoBehaviour
{



    [SerializeField]
    public int mapX, mapY;

    public static int MAPX, MAPY;

    [SerializeField]
    GameObject tile, startTile, wall, player1, player2;


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
                Instantiate(player1, new Vector3(l, .5f, m), Quaternion.Euler(90,0,0));
                int randx = UnityEngine.Random.Range(1, MAPX);
                int randy = UnityEngine.Random.Range(1, MAPY);
                Debug.LogFormat("The X: {0}, The Y: {1}", randx, randy);
                GameObject x = Instantiate(player2, new Vector3(randx, .5f, randy), Quaternion.Euler(90, 0, 0));
                x.name = "Second Box";
                mapCreated = true;
                Debug.Log("Finished Generating Map");


            }



        };

        ThreadQueue.QueueAction(spawn);
        
        


    }
}
