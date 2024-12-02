using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fondo : MonoBehaviour
{
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material = material;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
