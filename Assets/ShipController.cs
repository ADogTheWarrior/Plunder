using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ShipController : MonoBehaviour
{
    public bool IsSelected { get; set; }
    public Vector3 Destination { get; set; }

    public Terrain Terrain;

    private float _squareSize;

    private List<Vector3> Path = new();
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = this.transform.position;
        Terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        _squareSize = Terrain.terrainData.size.x / boardSize;
    }

    private const int boardSize = 20;
    int [,]board = new [,]{
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        },
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        }
    };
    
    Ray ray;
    RaycastHit hit;
    void Update()
    {
        if (Path.Any())
        {
            var pathComponent = Path.First();
            var path = ((Vector3)pathComponent * _squareSize - this.transform.position);
            path.y = 0; 
            if (path.magnitude > .01)
            {
                var move = Vector3.Normalize(path) * (Time.deltaTime * 15);
                this.transform.Translate(move, Space.World);
            }
            else
            {
                Path.RemoveAt(0);
            }
        }
        
        //Handle mouse clicks, did we click the ship? Or the terrain?
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(1) && hit.collider.name == "Terrain")
            {
                if (IsSelected)
                {
                    var destinationSquare = (Vector3)Vector3Int.FloorToInt(hit.point) / _squareSize;
                    Console.WriteLine(destinationSquare);
                    var currentSquare = (Vector3)Vector3Int.FloorToInt(this.transform.position) / _squareSize;
                    Console.WriteLine(currentSquare);
                    
                    Path.Clear();
                    Path.Add(new Vector3(currentSquare.x, 0, destinationSquare.z));
                    Path.Add(new Vector3(destinationSquare.x, 0, destinationSquare.z));
                    
                    // var currentSquare = Vector3Int.FloorToInt(this.transform.position / _squareSize);

                    // var transform = destinationSquare - currentSquare;
                    
                    //TODO: Use djikstra to make a better path
                    // Path.Add(new Vector3Int(0, 0, transform.z));
                    // Path.Add(new Vector3Int(transform.x, 0, 0));
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.GetInstanceID() == this.GetComponent<Collider>().GetInstanceID())
                {
                    IsSelected = true;    
                    // Get the Renderer component from the new cube
                    // var cubeRenderer = this.GetComponent<Renderer>();
                    //
                    // // Call SetColor using the shader property name "_Color" and setting the color to red
                    // cubeRenderer.material.SetColor("_Color", Color.red);
                }
                else
                {
                    IsSelected = false;    
                    // Get the Renderer component from the new cube
                    // var cubeRenderer = this.GetComponent<Renderer>();
                    //
                    // // Call SetColor using the shader property name "_Color" and setting the color to red
                    // cubeRenderer.material.SetColor("_Color", Color.white);       
                }
            }
        }
    }
}
