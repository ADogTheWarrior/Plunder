using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    public GameObject shipPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; ++i)
        {
            var new_location = new Vector3(i*30, 10, 20);
            Instantiate(shipPrefab, new_location, Quaternion.identity); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
