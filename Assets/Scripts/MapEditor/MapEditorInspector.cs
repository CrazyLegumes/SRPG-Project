using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;





[CustomEditor(typeof(MapEditor))]
public class MapEditorInspector : Editor
{

    [MenuItem("Create/Tileset")]
    private static void createTileset()
    {
        var asset = CreateInstance<TileSet>();
        string path = "Assets/Resources/Prefabs/Tiles/";
        AssetDatabase.CreateAsset(asset, path + "Tileset.asset");
        EditorUtility.FocusProjectWindow();
    }


    private MapEditor editor;
    private List<Tile[]> map = new List<Tile[]>();



    private List<string> prefabs;
    string tilepath;
    int oldindex = 0;

    void OnEnable()
    {
        editor = (MapEditor)target;
        editor.tempTile = Resources.Load<GameObject>("Prefabs/Tiles/TempTile");
        if (map.Count == 0)
        {
            editor.map = new Tile[editor.mapWidth, editor.mapLength];
        }
        else
        {
            
        }
            


        // GetTiles();


    }


    void GetTiles()
    {

        tilepath = Application.dataPath + "/Resources/Prefabs/Tiles/";
        string[] files = Directory.GetFiles(tilepath);
        prefabs = new List<string>();
        foreach (string x in files)
        {
            if (!x.Contains(".meta"))
                prefabs.Add(Path.GetFileName(x));

        }

        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i] = prefabs[i].Replace(".prefab", "");

        }

    }

    public override void OnInspectorGUI()
    {


        //Grid Lines and Stuff
        editor.mapLength = Mathf.Clamp(editor.mapLength, 5, 40);
        editor.mapWidth = Mathf.Clamp(editor.mapWidth, 5, 40);
        editor.mapLength = EditorGUILayout.IntField("Map Length", editor.mapLength);
        editor.mapWidth = EditorGUILayout.IntField("Map Width", editor.mapWidth);

        TileSet set = (TileSet)EditorGUILayout.ObjectField("Tile Set", editor.tileSet, typeof(TileSet), false);


        GameObject currentTile = (GameObject)EditorGUILayout.ObjectField("Current Tile", editor.currentTile, typeof(GameObject), false);


        if (set != null)
        {
            string[] tiles = new string[editor.tileSet.tiles.Length];
            int[] values = new int[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = editor.tileSet.tiles[i].name;
                values[i] = i;
            }

            int index = EditorGUILayout.IntPopup("Select Tile", oldindex, tiles, values);
            if (oldindex != index)
                oldindex = index;
            editor.currentTile = editor.tileSet.tiles[oldindex];

        }

        if (GUILayout.Button("Place Temp Map"))
        {
            editor.InstantiateTempTile();
            Debug.Log("Instantiate Button Clicked");
        }









    }

    void OnSceneGUI()
    {
        Event e = Event.current;
        int controllid = GUIUtility.GetControlID(FocusType.Passive);

        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

       // Debug.Log(HandleUtility.WorldToGUIPoint(e.mousePosition));
        
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100))
        {
            
          //  Debug.Log("Raycast hit : " + hit.collider.gameObject.name);
          if(hit.transform.GetComponent<TempTile>() != null)
            {
                
                editor.tileToRepalce = hit.transform.gameObject;
            }
         //   Debug.Log(hit.point);


        }
        // Debug.Log(ray.origin);
        // Ray point = Camera.current.ScreenPointToRay();
        // Vector3 mousepos = Camera.current.ScreenToViewportPoint(new Vector3(e.mousePosition.x , 1 -e.mousePosition.y + Camera.current.pixelHeight / editor.mapLength)*10);
        //mousepos.x = Mathf.Clamp(mousepos.x, 0, editor.mapWidth);
        // mousepos.y = Mathf.Clamp(mousepos.y, 0, editor.mapLength);
        // Debug.Log(mousepos);
        if (e.isMouse && e.type == EventType.MouseDown && hit.transform != null && e.button == 0)
        {
            GUIUtility.hotControl = controllid;
            e.Use();
            Vector3 allign = hit.transform.position;
            GameObject prefab = PrefabUtility.InstantiatePrefab(editor.currentTile) as GameObject;

            prefab.transform.position = allign;
            prefab.transform.parent = editor.transform;
            editor.map[(int)allign.x, (int)allign.z] = prefab.GetComponent<Tile>();

            DestroyImmediate(hit.transform.gameObject);

        }
        else if (e.isMouse && e.type == EventType.mouseDown && hit.transform != null && e.button == 1)
        {
            GUIUtility.hotControl = controllid;
            e.Use();
            Vector3 allign = hit.transform.position;
            GameObject prefab = PrefabUtility.InstantiatePrefab(editor.tempTile) as GameObject;

            prefab.transform.position = allign;
            prefab.transform.parent = editor.transform;
            editor.map[(int)allign.x, (int)allign.z] = null;

            DestroyImmediate(hit.transform.gameObject);

        }

        if (e.isMouse && e.type == EventType.MouseUp)
        {
            GUIUtility.hotControl = 0;
        }
    }



}



