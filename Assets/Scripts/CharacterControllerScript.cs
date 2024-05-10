using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public Rigidbody2D myRb;

    public float jumpForce;
    public bool isGrounded;

    public float secondaryJumpForce;
    public float secondaryJumpTime;
    public bool secondaryJump;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>(); 
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimations();
        UpdateMovement();
        UpdateJump();
    }

    void UpdateAnimations()
    {
        anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x)); 
        anim.SetFloat("jump", myRb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    void UpdateMovement()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && Mathf.Abs(myRb.velocity.x) < maxSpeed) 
        {
            myRb.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration, 0), ForceMode2D.Force); 
        }

        // Animation flip code
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            anim.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            anim.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void UpdateJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump")) 
        {
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); 
            StartCoroutine(SecondaryJump());
        }

        if (!isGrounded && Input.GetButton("Jump") && secondaryJump)
        {
            myRb.AddForce(new Vector2(0, secondaryJumpForce), ForceMode2D.Force); 
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Map"))
        {
            isGrounded = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Map"))
        {
            isGrounded = false;
        }
    }

    IEnumerator SecondaryJump()
    {
        secondaryJump = true;
        yield return new WaitForSeconds(secondaryJumpTime); 
        secondaryJump = false;
    }
}
