using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public bool IsSelected { get; set; }
    public Vector3 Destination { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Destination = this.transform.position;
    }

    Ray ray;
    RaycastHit hit;
    void Update()
    {
        var path = (Destination - this.transform.position);
        if (path.magnitude > .01)
        {
            Vectorthis.transform.orientation 
            // var move = Vector3.Normalize(path) * Time.deltaTime * 5;
            // this.transform.Translate(move, Space.World);
        }
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(1) && hit.collider.name == "Terrain")
            {
                if (IsSelected)
                {
                    Destination = hit.point;
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
