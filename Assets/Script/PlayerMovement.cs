using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour 
{
    #region Variables
    private Rigidbody2D playerRb;
	private BoxCollider2D _collider;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float speed = 5;
	[SerializeField] private float jumpForce = 20f;
	private float leftBoundary = -6.27f;
	private float rightBoundary = 6f;
	private float bottomBoundary = -5f;
	
	[SerializeField] private float input;
	private bool isGrounded;
	private bool isPressingSpace;
	private bool isHoldingSpace;
	private bool isReleasingSpace;

	private float jumpTime = 1;
    [SerializeField] private float jumpDelta = 15;
    private float jumpTimeCounter = 0;
	private float jumpHeight = 0;

	[SerializeField] private AudioSource playerBump;
	[SerializeField] private AudioSource playerJump;
    //[SerializeField] private AudioSource playerLand;
    #endregion

    // Start is called before the first frame update
    private void Start()
	{
		playerRb = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
    private void Update()
	{
        Checkstatus();
		Walk();
		Jump();		
	}

    #region Movement
	private void Checkstatus()
	{
		isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
	}
    private void Walk()
	{

		//TODO moving character horizontal, create horizontal boundary
		if (isGrounded && !isHoldingSpace)
		{
            playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
            input = Input.GetAxisRaw("Horizontal");

            if (transform.position.x <= leftBoundary)
            {
                transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
            }

            else if (transform.position.x >= rightBoundary)
            {
                transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
            }

			else if (transform.position.y < bottomBoundary)
			{
				transform.position = new Vector3(transform.position.x, bottomBoundary, transform.position.z);
			}
		}
	}
	private void Jump()
	{
		//TODO :
		/*
			trên mặt đất giữ space: time giảm, height tăng
			nếu time <= 0 nhảy k cần đk
			nhả space: nhảy theo height
			chạm colision từ trái, phải: văng ra
			nhảy lệch trái phải
			k ở mặt đất thì k dc nhảy
		*/
		isPressingSpace = Input.GetButtonDown("Jump");
		isHoldingSpace = Input.GetButton("Jump");
		isReleasingSpace = Input.GetButtonUp("Jump");

		if (isGrounded)
		{
            if (isPressingSpace)
            {
				//Reset jumptimecounter and jumpheight
                playerBump.Play();
                jumpTimeCounter = jumpTime;
                jumpHeight = jumpForce;
            }

            if (isHoldingSpace)
            {
                //Increase JumpHeight and decrease JumpTimeCounter by deltatime

                if (jumpTimeCounter > 0)
                { 
                    jumpTimeCounter -= Time.deltaTime;
                    jumpHeight += Time.deltaTime * jumpDelta;
                }

				else if (jumpTimeCounter <= 0)
				{
					playerJump.Play();
					float tmpX = input * speed;
					float tmpY = jumpHeight; 
					playerRb.velocity = new Vector2(tmpX, tmpY);
				}
            }

            else if (isReleasingSpace)	
            {
                playerJump.Play();
                float tmpX = input * speed;
                float tmpY = jumpHeight; 
                playerRb.velocity = new Vector2(tmpX, tmpY);
            }
		}	
	}

    #endregion

}
