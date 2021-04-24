using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 movement = Vector2.zero;

        //if(Input.GetKey(KeyCode.UpArrow))
        //{
        //    movement.y += 1;
        //}
        //if(Input.GetKey(KeyCode.DownArrow))
        //{
        //    movement.y -= 1;
        //}
        movement.y = GetComponent<Rigidbody2D>().velocity.y;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x -= 1;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            movement.x += 1;
        }

        GetComponent<Rigidbody2D>().velocity = movement.normalized * speed;
    }
}
