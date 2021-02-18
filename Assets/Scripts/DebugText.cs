using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{

    public float velocity = 0f;
    public float force = 0f;
    // Start is called before the first frame update
    private Vector3 rel_position;
    private Transform parent_transform;
    TextMesh textMesh;
    // Update is called once per frame
    void Start() {
        textMesh = GetComponent<TextMesh>();
        parent_transform=GetComponentInParent<Transform>();
        rel_position = transform.position - parent_transform.position;

    }
    
    void FixedUpdate()
    {
        textMesh.text = string.Format("Velocity: {0}\nForce: {1}",velocity, force);
        transform.position=parent_transform.position+rel_position;
    }
}
