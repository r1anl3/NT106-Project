using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class NetworkMovement : NetworkBehaviour 
{
    #region Variables
    private Rigidbody2D playerRb;
	private BoxCollider2D _collider;
	private Animator _animation;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float speed = 5f;
	[SerializeField] private float jumpForce = 10f;
	private float leftBoundary = -6.27f;
	private float rightBoundary = 6f;
    private float bottomBoundary = -5f;
	
	private float input;
	private bool isGrounded;
	private bool isPressingSpace;
	private bool isHoldingSpace;
	private bool isReleasingSpace;

	private float jumpTime = 1;
    [SerializeField] private float jumpDelta = 15;
    [SerializeField] private float jumpTimeCounter;
	[SerializeField] private float jumpHeight;

	[SerializeField] private AudioSource playerBump;
	[SerializeField] private AudioSource playerJump;

	private enum MovementState { idle, run, hold, jump, fall}
	[SerializeField] private MovementState state;

	#endregion

	// Start is called before the first frame update
	private void Start()
	{
		playerRb = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
		_animation = GetComponent<Animator>();
	}
	// Update is called once per frame
    private void Update()
	{
        if (!IsOwner)
		{
			return;
		}
		CheckStatus();
		Walk();
		Jump();
		UpdateAnimationState();
	}

    #region Movement
	private void CheckStatus()
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
		//TODO use space key to jump
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

	private void UpdateAnimationState()
	{
		//Update running animation
		Vector2 theScale = transform.localScale;

		if (input < 0)
		{
			state = MovementState.run;
			theScale.x = -1;
			transform.localScale = theScale;
		}

		else if (input > 0)
		{
			state = MovementState.run;
			theScale.x = 1;
			transform.localScale = theScale;
		}

		else 
		{
			state = MovementState.idle;
		}

		//Update holding animation
		if (isGrounded && isHoldingSpace)
		{
			state = MovementState.hold;
		}

		//Update jumping and falling animation
		if (playerRb.velocity.y > .1f)
		{
			state = MovementState.jump;
		}

		else if (playerRb.velocity.y < -1f)
		{
			state = MovementState.fall;
		}

		//Set enum value to state
		_animation.SetInteger("state", (int)state);
	}
}
