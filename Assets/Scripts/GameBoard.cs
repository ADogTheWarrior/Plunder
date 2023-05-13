using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Art stuff")]
    [SerializeField] private Material coveTileMaterial;
    [SerializeField] private Material landTileMaterial;
    [SerializeField] private Material waterTileMaterial;

    //Logic
    private const int TILE_COUNT_X = 18;
    private const int TILE_COUNT_Y = 12;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover = -Vector2Int.one;

    public const int TILE_SIZE = 50;
    //0 is water
    //1 is land
    //2 is cove
    public int[,] BOARDSTATE = {
        {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,1,0,0,0,1,1,0,0,0,0,0,0,2,0,0,0},
        {0,2,1,0,0,0,1,2,0,0,0,0,0,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {1,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,2,1,0,0,2,0,0,0,0,1,2,0,0,0,0},
        {0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0},
        {0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0},
        {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,1,0,0,2,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0}
    };

    //starts when the script is first run, kinda like a singleton
    //void Start runs on each object the script is attached to
    private void Awake(){
        Debug.Log("test awake");
        GenerateAllTiles(TILE_SIZE, TILE_COUNT_X, TILE_COUNT_Y);
    }

    private void Update(){
        if (!currentCamera) {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile"))){
            //get the indexes of the tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //If we're hovering a tile after not hovering any tiles
            if (currentHover == -Vector2Int.one) {
                Debug.Log("currentHover == -Vector2Int.one");
                Debug.Log("hitPosition= "+hitPosition);
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //If we were already hovering a tile, change the previous one
            if (currentHover != hitPosition)
            {
                Debug.Log("currentHover != hitPosition");
                Debug.Log("currentHover= "+currentHover);
                Debug.Log("hitPosition= "+hitPosition);
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
        }
        else {
            if(currentHover != -Vector2Int.one) {
                Debug.Log("currentHover != -Vector2Int.one");
                Debug.Log("currentHover= "+currentHover);
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }
        }
    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY){
        tiles = new GameObject[tileCountX, tileCountY];
        for(int x = 0; x < tileCountX; x++){
            for(int y = 0; y < tileCountY; y++){
                tiles[x,y] = GenerateSingleTile(tileSize, x, y);
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y){
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;

        switch(BOARDSTATE[y,x]) 
        {
        case 0: // water
            tileObject.AddComponent<MeshRenderer>().material = waterTileMaterial;
            break;
        case 1: // land
            tileObject.AddComponent<MeshRenderer>().material = landTileMaterial;
            break;
        case 2: // cove
            tileObject.AddComponent<MeshRenderer>().material = coveTileMaterial;
            break;
        default: // default to water
            tileObject.AddComponent<MeshRenderer>().material = waterTileMaterial;
            break;
        }

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 1, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 1, (y+1) * tileSize);
        vertices[2] = new Vector3((x+1) * tileSize, 1, y * tileSize);
        vertices[3] = new Vector3((x+1) * tileSize, 1, (y+1) * tileSize);

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;

        tileObject.layer = LayerMask.NameToLayer("Tile");
        mesh.RecalculateNormals();

        tileObject.AddComponent<BoxCollider>();
        
        return tileObject;
    }

    private Vector2Int LookupTileIndex(GameObject hitInfo){
        for (int x = 0; x < TILE_COUNT_X; x++){
            for (int y = 0; y < TILE_COUNT_Y; y++){
                if(hitInfo == null){
                    Debug.Log("hitInfo == null");
                }
                if(tiles[x,y] == hitInfo){
                    return new Vector2Int(x, y);
                }
            }
        }
        return -Vector2Int.one; //Invalid
    }
}