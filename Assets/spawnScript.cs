using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour
{
    public GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        spawnObject.transform.position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
