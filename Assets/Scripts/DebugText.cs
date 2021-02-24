using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    /// <summary>
    /// debugging class created for showing on screen data about the players.
    /// </summary>
public class DebugText : MonoBehaviour
{

    public float velocity = 0f;
    public float force = 0f;
    public TextAnchor textPosition;
    // Start is called before the first frame update
    Text textBox;
    // Update is called once per frame
    void Start() {
        textBox = GetComponent<Text>();
    }
    
    void FixedUpdate()
    {
        textBox.alignment = textPosition;
        textBox.text = string.Format("Velocity: {0}\nForce: {1}",velocity, force);
        //transform.position=parent_transform.position+rel_position;
    }
}
