using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 5f;
	public bool faceMovementDirection = true;
	public Transform cameraTransform; // optional, used to make movement camera-relative

	CharacterController controller;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		if (cameraTransform == null && Camera.main != null)
			cameraTransform = Camera.main.transform;
	}

	void Update()
	{
		HandleMovement();
	}

	void HandleMovement()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Vector3 input = new Vector3(h, 0f, v).normalized;

		if (input.magnitude < 0.01f)
			return;

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

		controller.Move(move * moveSpeed * Time.deltaTime);

		if (faceMovementDirection)
		{
			Quaternion targetRot = Quaternion.LookRotation(move);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
		}
	}
}

