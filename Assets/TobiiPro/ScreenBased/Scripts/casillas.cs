using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class casillas : MonoBehaviour
{
    public Material material;

    void Start()
    {
        this.GetComponent<Renderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
