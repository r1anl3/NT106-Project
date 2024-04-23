using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AnimationPlay : MonoBehaviour
{
    private Rigidbody2D playerRb;
	private Animator _animation;
	private SpriteRenderer spriteRenderer;
	private BoxCollider2D _collider;

	private float input;
	private bool isGrounded;
	[SerializeField] private LayerMask groundLayer;
	private bool isHoldingSpace;
	private enum MovementState { idle, run, hold, jump, fall}
	[SerializeField] private MovementState state;
    // Start is called before the first frame update
    void Start()
    {
		playerRb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<BoxCollider2D>();
		_animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		UpdateAnimationState(); 
    }
	private void UpdateAnimationState()
	{

		input = Input.GetAxisRaw("Horizontal");
		isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
		isHoldingSpace = Input.GetButton("Jump");

		//Update running animation
		if (input < 0)
		{
			state = MovementState.run;
			spriteRenderer.flipX = true;
		}

		else if (input > 0)
		{
			state = MovementState.run;
			spriteRenderer.flipX = false;
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
