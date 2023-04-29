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
        {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0}
    };

    //starts when the script is first run, kinda like a singleton
    //void Start runs on each object the script is attached to
    private void Awake(){
        GenerateAllTiles(TILE_SIZE, TILE_COUNT_X, TILE_COUNT_Y);
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
        default: // default water
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

        mesh.RecalculateNormals();

        tileObject.AddComponent<BoxCollider>();
        
        return tileObject;
    }
}