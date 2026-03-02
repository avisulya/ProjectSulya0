using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 5f;
	public bool faceMovementDirection = true;
	public Transform cameraTransform; // optional, used to make movement camera-relative

	[Header("Dash")]
	public float dashSpeed = 20f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 0.5f;

	CharacterController controller;
	float dashCooldownTimer = 0f;
	float dashTimer = 0f;
	Vector3 dashDirection = Vector3.zero;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		if (cameraTransform == null && Camera.main != null)
			cameraTransform = Camera.main.transform;
	}

	public bool IsDashing()
	{
		return dashTimer > 0f;
	}

	void Update()
	{
		// Update dash cooldown timer
		if (dashCooldownTimer > 0f)
			dashCooldownTimer -= Time.deltaTime;
		
		// Update dash timer
		if (dashTimer > 0f)
			dashTimer -= Time.deltaTime;

		HandleMovement();
	}

	void HandleMovement()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Vector3 input = new Vector3(h, 0f, v).normalized;

		// Check for dash input
		if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && input.magnitude > 0.01f)
		{
			dashTimer = dashDuration;
			dashCooldownTimer = dashCooldown;
			
			// Calculate dash direction based on camera if available
			if (cameraTransform != null)
			{
				Vector3 camForward = cameraTransform.forward;
				camForward.y = 0f;
				camForward.Normalize();
				Vector3 camRight = cameraTransform.right;
				camRight.y = 0f;
				camRight.Normalize();
				dashDirection = (camForward * input.z + camRight * input.x).normalized;
			}
			else
			{
				dashDirection = input.normalized;
			}
		}

		Vector3 moveVelocity = Vector3.zero;

		// Apply dash movement if dashing
		if (dashTimer > 0f)
		{
			moveVelocity = dashDirection * dashSpeed;
		}
		// Apply normal movement
		else if (input.magnitude > 0.01f)
		{
			Vector3 move;
			if (cameraTransform != null)
			{
				Vector3 camForward = cameraTransform.forward;
				camForward.y = 0f;
				camForward.Normalize();
				Vector3 camRight = cameraTransform.right;
				camRight.y = 0f;
				camRight.Normalize();
				move = camForward * input.z + camRight * input.x;
			}
			else
			{
				move = input;
			}

			moveVelocity = move * moveSpeed;

			if (faceMovementDirection)
			{
				Quaternion targetRot = Quaternion.LookRotation(move);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
			}
		}

		controller.Move(moveVelocity * Time.deltaTime);

		// Face dash direction during dash
		if (dashTimer > 0f && faceMovementDirection)
		{
			Quaternion targetRot = Quaternion.LookRotation(dashDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
		}
	}
}

