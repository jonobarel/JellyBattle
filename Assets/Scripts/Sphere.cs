using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetComponent<MeshRenderer>().material.GetTexturePropertyNames().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
