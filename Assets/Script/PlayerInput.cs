using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    private bool IsGrounded=false;
    private bool BouncePad = false;
    
    private float gravity = -9.8f;
    private float scale = 1.0f;
    private int count = 0;
    private int MaxCount = 1;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;

    private Vector2 direction = Vector2.zero;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    
    void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        Debug.Log(direction);
        this.direction = direction;

    }

    void Update()
    {
        Move(direction.x, direction.y);
    }

    private void Move(float x, float z)
    {
        rb.velocity = new Vector3(x * speed, rb.velocity.y, z* speed);
        
    }

    void OnJump()
    {
        if (IsGrounded)
        {
            if (BouncePad)
            {
                rb.AddForce(0,jumpHeight*1.5F,0,ForceMode.VelocityChange);
                
            }
            Jump();
                
            
        }

        
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        
    }

    private void UpsideDown()
    {
            gravity = -1*gravity;
            Physics.gravity = new Vector3(0, gravity, 0);
      
    }

    private void OnShrink()
    {
        if (count > -1)
        {
            count--;
            scale -= 0.5f;
            Shrink();
        }
    }

    private void Shrink()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        rb.mass -= 5;
        rb.AddForce(speed+2.0f,0,speed+2.0f,ForceMode.VelocityChange);
    }
    
    private void OnEnlarge()
    {
        if (count < 1)
        {
            scale+=0.5f;
            count++;
            Enlarge();
        }
        
    }

    private void Enlarge()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        rb.mass += 5;
        rb.AddForce(speed-2.0f,0,speed-2.0f,ForceMode.VelocityChange);
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "JumpPad":
                BouncePad = true;
                break;
            case "Grav":
                UpsideDown();
                break;
        }
        
    }
    
    void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            BouncePad = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(collision.GetContact(0).normal, Vector3.up) < 45f)
        {
            IsGrounded = true;
            
        }
        else
        {
            IsGrounded = false;
        }
    }
}
