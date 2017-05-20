using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TileSet : ScriptableObject
{
    public GameObject[] tiles = new GameObject[0];
}

[System.Serializable]
public class MapEditor : MonoBehaviour
{


    public int mapLength = 0;
    public int mapWidth = 0;
    public int tilesInEditor;
    public Texture2D myTexture;
    public TileSet tileSet;
    public GameObject currentTile;
    public GameObject tileToRepalce;
    public GameObject tempTile;
    public bool spawned;
    public Tile[] map;
    public int index;
    public DataMap mapdata;
    public bool mapset;
    public List<Tile> TileList;
    

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {


    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        float xpos;
        for (int i = 0; i < mapLength + 1; i++)
        {
            xpos = i - .5f;

            Gizmos.DrawLine(new Vector3(-.5f, 0, xpos), new Vector3(mapWidth - .5f, 0, xpos));


        }
        float ypos;
        for (int i = 0; i < mapWidth + 1; i++)
        {
            ypos = i - .5f;

            Gizmos.DrawLine(new Vector3(ypos, 0, -.5f), new Vector3(ypos, 0, mapLength - .5f));
        }

    }

    public void InstantiateTempTile()
    {


        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                GameObject spawned = UnityEditor.PrefabUtility.InstantiatePrefab(tempTile) as GameObject;
                spawned.transform.position = new Vector3(j, 0, i);
                spawned.transform.parent = transform;
            }
        }

        spawned = true;
    }

}
