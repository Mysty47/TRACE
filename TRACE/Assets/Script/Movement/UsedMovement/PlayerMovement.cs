using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEditor;


public class PlayerMovement : MonoBehaviour
{
	[Header("References")]
	public Transform playerCam;
	public Transform orientation;
	private Collider playerCollider;
	public Rigidbody rb;
	public EscapeMenuController escapeMenuController;
	[NonSerialized] public GrapplingGun gg;
	public static PlayerMovement Instance { get; private set; }
	
	[Header("Layers")] 
	public LayerMask whatIsGround;
	public LayerMask whatIsWallrunnable;
	
	[Header("Particle Effects")]
	public ParticleSystem fastMovementParticles;
	public float velocityThreshold = 20f;

	[Header("MovementSettings")]
	public static float sensitivity = 50f;

	public float moveSpeed = 450f;
	public float runSpeed = 10f;
	public bool onWall;
	public float maxYSpeed;

	[Header("Private Floats")]
	public float wallRunGravity = 0.3f;
	public float maxSlopeAngle = 35f;
	private float wallRunRotation;
	public float slideSlowdown = 0.1f;
	private float actualWallRotation;
	private float wallRotationVel;
	private float desiredX;
	private float xRotation;
	private float sensMultiplier = 1f;
	private float jumpCooldown = 0.25f;
	private float jumpForce = 700f;
	private float inputX;
	private float inputY;
	private float vel;

	[Header("State Bools")]
	public bool surfing;
	public bool jumping;
	public bool crouching;
	public bool wallRunning;
	public bool grounded;
	public bool dashing;
	public bool climbing;
	
	[Header("Private Bools")]
	private bool cancelling;
	private bool readyToWallrun = true;
	private bool airborne;
	private bool readyToJump;
	private bool cancellingGrounded;
	private bool cancellingWall;
	private bool cancellingSurf;

	[Header("Private Vector3's")]
	private Vector3 grapplePoint;
	private Vector3 normalVector;
	private Vector3 wallNormalVector;
	private Vector3 wallRunPos;
	private Vector3 previousLookdir;

	[Header("Private ints")]
	private int nw;

	bool isCrouched = false;
	float crouchSpeed = 2f;
	float normalSpeed = 5f;
	Vector3 originalScale;
	Vector3 crouchScale;
	
	[Header("States")]
	public MovementState state;
	public enum MovementState
	{
		idle,
		running,
		crouching,
		wallrunning,
		climbing,
		dashing,
		swinging,
		sliding,
		air
	}

	[Header("Audio Settings")]
	public AudioSource runningSound;
	public AudioSource wallrunSound;
	public AudioSource jumpSound;


	private void Awake()
	{
		Instance = this;
		rb = GetComponent<Rigidbody>();
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	private void Start()
	{
		playerCollider = GetComponent<Collider>();
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		readyToJump = true;
		
		wallNormalVector = Vector3.up;
		originalScale = transform.localScale;
		crouchScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
		
		wallrunSound.Stop();
		
		// Ensure the particle system is disabled at the start
		if (fastMovementParticles != null)
		{
			fastMovementParticles.Stop();
		}
	}

	private void LateUpdate()
	{
		//For wallRunning
		WallRunning();
	}

	private void FixedUpdate()
	{
		//For moving
		Movement();
		
		// Play particles if moving fast
	}
	
	IEnumerator FadeOutSound(AudioSource audioSource)
	{
		while (audioSource.volume > 0)
		{
			audioSource.volume -= Time.deltaTime * 2f; // Adjust fade speed
			yield return null;
		}
		audioSource.Stop();
		audioSource.volume = 0.5f; // Reset volume
	}
	
	private void Update()
	{
		StateHandler();

		if (rb.linearVelocity.magnitude > velocityThreshold || !grounded)
		{
			if (fastMovementParticles != null && !fastMovementParticles.isPlaying)
			{
				fastMovementParticles.Play();
			}
		}
		else
		{
			if (fastMovementParticles != null && fastMovementParticles.isPlaying)
			{
				fastMovementParticles.Stop();
			}
		}
		
		MyInput();

		Look();
		if (Input.GetKeyDown(KeyCode.C))
		{
			StartCrouch();
		}
		else if (Input.GetKeyUp(KeyCode.C))
		{
			StopCrouch();
		}

		HandleMovement();
	}

	void HandleMovement()
	{
		float speed = isCrouched ? crouchSpeed : normalSpeed;

		if (Input.GetKey(KeyCode.W))
		{
			rb.AddForce(orientation.transform.forward * speed * Time.fixedDeltaTime);
		}

		if (Input.GetKey(KeyCode.S)) // Move backward
		{
			rb.AddForce(-orientation.transform.forward * speed * Time.fixedDeltaTime);
		}

		if (Input.GetKey(KeyCode.A)) // Move left
		{
			rb.AddForce(-orientation.transform.right * speed * Time.fixedDeltaTime);
		}

		if (Input.GetKey(KeyCode.D)) // Move right
		{
			rb.AddForce(orientation.transform.right * speed * Time.fixedDeltaTime);
		}
	}

	private MovementState lastState;

	private void StateHandler()
	{
		MovementState newState;

		if (climbing)
			newState =  MovementState.climbing;
		else if (gg != null && gg.swinging)
			newState = MovementState.swinging;
		else if (dashing)
			newState = MovementState.dashing;
		else if (wallRunning)
			newState = MovementState.wallrunning;
		else if (surfing)
			newState = MovementState.sliding;
		else if (crouching)
			newState = MovementState.crouching;
		else if (!grounded)
			newState = MovementState.air;
		else
		{
			float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

			if (horizontalSpeed > 0.5f)
				newState = MovementState.running;
			else
				newState = MovementState.idle;
		}

		if (newState != lastState)
		{
			StopAllMovementSounds();

			switch (newState)
			{
				case MovementState.running:
					if (!runningSound.isPlaying) runningSound.Play();
					break;
				case MovementState.wallrunning:
					if (!wallrunSound.isPlaying) wallrunSound.Play();
					break;
				case MovementState.idle:
					break;
				case MovementState.air:
					break;
			}
			lastState = newState;
		}

		state = newState;
	}


	private void StopAllMovementSounds()
	{
		if (runningSound.isPlaying) runningSound.Stop();
		if (wallrunSound.isPlaying) wallrunSound.Stop();
	}


	//Player input
	public void MyInput()
	{
		inputX = Input.GetAxisRaw("Horizontal");
		inputY = Input.GetAxisRaw("Vertical");

		jumping = Input.GetButton("Jump");

		crouching = Input.GetKey(KeyCode.LeftControl);
		
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			StartCrouch();
		}

		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			StopCrouch();
		}
	}

	//Scale player down
	void StartCrouch()
	{
		if (!isCrouched)
		{
			isCrouched = true;
			transform.localScale = crouchScale;
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
		}
	}

	//Scale player to original size
	void StopCrouch()
	{
		if (isCrouched)
		{
			isCrouched = false;
			transform.localScale = originalScale;
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
		}
	}

	//Moving around with WASD
	private void Movement()
	{
		rb.AddForce(Vector3.down * Time.fixedDeltaTime * 10f);
		
		// We get the speed of the player in the direction he is moving
		Vector2 relativeVelocity = FindVelRelativeToLook();
		
		float lateralVelocity = relativeVelocity.x; // side speed
		float forwardVelocity = relativeVelocity.y; // forward and backward speed
		
		CounterMovement(inputX, inputY, relativeVelocity);
		
		if (readyToJump && jumping)
		{
			Jump();
		}

		float currentRunSpeed = runSpeed;

		// Applying force on the player so he can stick to the ground
		if (crouching && grounded && readyToJump)
		{
			rb.AddForce(Vector3.down * Time.fixedDeltaTime * 3000f);
			return;
		}
		
		// All of these limit the player from going over the maxSpeed
		if (inputX > 0f && lateralVelocity > currentRunSpeed)
		{
			inputX = 0f;
		}

		if (inputX < 0f && lateralVelocity < 0f - currentRunSpeed)
		{
			inputX = 0f;
		}

		if (inputY > 0f && forwardVelocity > currentRunSpeed)
		{
			inputY = 0f;
		}

		if (inputY < 0f && forwardVelocity < 0f - currentRunSpeed)
		{
			inputY = 0f;
		}

		float airControlMultiplier = 1f;
		float movementMultiplier = 1f;
		
		if (!grounded)
		{
			airControlMultiplier = 0.5f;
			movementMultiplier = 0.5f;
		}

		if (grounded && crouching)
		{
			movementMultiplier = 0f;
		}

		if (wallRunning)
		{
			movementMultiplier = 0.3f;
			airControlMultiplier = 0.3f;
		}

		if (surfing)
		{
			airControlMultiplier = 0.7f;
			movementMultiplier = 0.3f;
		}

		rb.AddForce(orientation.transform.forward * inputY * moveSpeed * Time.fixedDeltaTime * airControlMultiplier * movementMultiplier); // forward and backward movement
		rb.AddForce(orientation.transform.right * inputX * moveSpeed * Time.fixedDeltaTime * airControlMultiplier); // side movement
	}

	//Ready to jump again
	private void ResetJump()
	{
		readyToJump = true;
	}

	//Player go fly
	private void Jump()
	{
		if ((grounded || wallRunning || surfing) && readyToJump)
		{
			Vector3 velocity = rb.linearVelocity;
			readyToJump = false;
			rb.AddForce(Vector2.up * jumpForce * 1.5f);
			rb.AddForce(normalVector * jumpForce * 0.5f);
			if (rb.linearVelocity.y < 0.5f)
			{
				rb.linearVelocity = new Vector3(velocity.x, 0f, velocity.z);
			}
			else if (rb.linearVelocity.y > 0f)
			{
				rb.linearVelocity = new Vector3(velocity.x, velocity.y / 2f, velocity.z);
			}

			if (wallRunning)
			{
				rb.AddForce(wallNormalVector * jumpForce * 3f);
			}

			if (!jumpSound.isPlaying)
			{
				jumpSound.Play();
			}

			Invoke("ResetJump", jumpCooldown);
			if (wallRunning)
			{
				wallRunning = false;
			}
		}
	}

	//Looking around by using your mouse
	private void Look()
	{
		if (EscapeMenuController.isPaused == false)
		{
			float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
			float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
			
			desiredX = playerCam.transform.localRotation.eulerAngles.y + mouseX;
			
			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			
			if(!climbing) FindWallRunRotation();
			
			actualWallRotation = Mathf.SmoothDamp(actualWallRotation, wallRunRotation, ref wallRotationVel, 0.2f);
			
			playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, actualWallRotation);
			orientation.transform.localRotation = Quaternion.Euler(0f, desiredX, 0f);
		}
	}

	//Make the player movement feel good 
	private void CounterMovement(float inputX, float inputY, Vector2 mag)
	{
		if (!grounded || jumping)
		{
			return;
		}

		float counterStrength = 0.16f;
		float stopThreshold = 0.01f;
		
		// applies force while crouching
		if (crouching)
		{
			rb.AddForce(moveSpeed * Time.fixedDeltaTime * -rb.linearVelocity.normalized * slideSlowdown);
			return;
		}

		// side movement counter force
		if ((Math.Abs(mag.x) > stopThreshold && Math.Abs(inputX) < 0.05f) || (mag.x < 0f - stopThreshold && inputX > 0f) ||
		    (mag.x > stopThreshold && inputX < 0f))
		{
			rb.AddForce(moveSpeed * orientation.transform.right * Time.fixedDeltaTime * (0f - mag.x) * counterStrength);
		}

		// front and back movement counter force
		if ((Math.Abs(mag.y) > stopThreshold && Math.Abs(inputY) < 0.05f) || (mag.y < 0f - stopThreshold && inputY > 0f) ||
		    (mag.y > stopThreshold && inputY < 0f))
		{
			rb.AddForce(moveSpeed * orientation.transform.forward * Time.fixedDeltaTime * (0f - mag.y) * counterStrength);
		}

		// limits max speed
		if (Mathf.Sqrt(Mathf.Pow(rb.linearVelocity.x, 2f) + Mathf.Pow(rb.linearVelocity.z, 2f)) > runSpeed)
		{
			float originalYVelocity = rb.linearVelocity.y;
			Vector3 limitedVelocity = rb.linearVelocity.normalized * runSpeed;
			rb.linearVelocity = new Vector3(limitedVelocity.x, originalYVelocity, limitedVelocity.z);
		}
	}

	private Vector2 FindVelRelativeToLook()
	{
		// where the player looks
		float playerYaw = orientation.transform.eulerAngles.y;
		
		// where the player goes
		float velocityYaw = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;
		
		// the difference between where the player looks and where the player goes
		float deltaYaw = Mathf.DeltaAngle(playerYaw, velocityYaw);
		float sideAngle = 90f - deltaYaw; // if deltaYaw = 0 -> front; if deltaYaw = 90 -> sideways; if deltaYaw = 180 -> back
		
		float speed = rb.linearVelocity.magnitude;
		
		// speed relative to the direction the player is looking
		return new Vector2(y: speed * Mathf.Cos(deltaYaw * ((float)Math.PI / 180f)),
			x: speed * Mathf.Cos(sideAngle * ((float)Math.PI / 180f)));
	}

	private void FindWallRunRotation()
	{
		if (!wallRunning)
		{
			wallRunRotation = 0f;
			return;
		}
		// the angle of the wall
		float wallAngle = 0f;
		
		// rotation of the playerCam
		float camYaw = playerCam.transform.rotation.eulerAngles.y;
		
		// finds the approximate angle
		if (Math.Abs(wallNormalVector.x - 1f) < 0.1f)
		{
			wallAngle = 90f;
		}
		else if (Math.Abs(wallNormalVector.x - -1f) < 0.1f)
		{
			wallAngle = 270f;
		}
		else if (Math.Abs(wallNormalVector.z - 1f) < 0.1f)
		{
			wallAngle = 0f;
		}
		else if (Math.Abs(wallNormalVector.z - -1f) < 0.1f)
		{
			wallAngle = 180f;
		}

		wallAngle = Vector3.SignedAngle(new Vector3(0f, 0f, 1f), wallNormalVector, Vector3.up);
		
		// the difference between the camera and the wall
		float deltaAngle = Mathf.DeltaAngle(camYaw, wallAngle);
		
		// calculates how much should the camera rotate for the wallrun
		wallRunRotation = (0f - deltaAngle / 90f) * 15f;
		
		if (!readyToWallrun)
		{
			return;
		}

		//  Automatically ends wallRun in certain conditions
		if ((Mathf.Abs(wallRunRotation) < 4f && inputY > 0f && Math.Abs(inputX) < 0.1f) ||
		    (Mathf.Abs(wallRunRotation) > 22f && inputY < 0f && Math.Abs(inputX) < 0.1f))
		{
			if (!cancelling)
			{
				cancelling = true;
				CancelInvoke("CancelWallrun");
				Invoke("CancelWallrun", 0.2f); // little delay before ending wallRun
			}
		}
		else
		{
			cancelling = false;
			CancelInvoke("CancelWallrun");
		}
	}

	private void CancelWallrun()
	{
		MonoBehaviour.print("cancelled");
		Invoke("GetReadyToWallrun", 0.1f);
		rb.AddForce(wallNormalVector * 600f);
		readyToWallrun = false;
	}

	private void GetReadyToWallrun()
	{
		readyToWallrun = true;
	}

	private void WallRunning()
	{
		if (wallRunning && !climbing)
		{
			rb.AddForce(-wallNormalVector * Time.deltaTime * moveSpeed);
			rb.AddForce(Vector3.up * Time.deltaTime * rb.mass * 100f * wallRunGravity);
		}
	}

	private bool IsFloor(Vector3 v)
	{
		return Vector3.Angle(Vector3.up, v) < maxSlopeAngle;
	}

	private bool IsSurf(Vector3 v)
	{
		float surfaceAngle = Vector3.Angle(Vector3.up, v);
		if (surfaceAngle < 89f)
		{
			return surfaceAngle > maxSlopeAngle;
		}

		return false;
	}

	private bool IsWall(Vector3 v)
	{
		return Math.Abs(90f - Vector3.Angle(Vector3.up, v)) < 0.1f;
	}

	private bool IsRoof(Vector3 v)
	{
		return v.y == -1f;
	}

	private void StartWallRun(Vector3 normal)
	{
		if (!grounded && readyToWallrun)
		{
			// used for camera control and canceling wallRun
			wallNormalVector = normal;
			float initialVerticalBoost = 20f;
			
			// small boost if the wallRun just starts
			if (!wallRunning)
			{
				rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
				rb.AddForce(Vector3.up * initialVerticalBoost, ForceMode.Impulse);
			}

			wallRunning = true;
			
			TryVaultOverWall(normal);
		}
	}
	
	private void TryVaultOverWall(Vector3 normal)
	{
		// direction of the player
		Vector3 dir = rb.linearVelocity.normalized;
		
		Vector3 maxVaultPos = transform.position + Vector3.up * 1.5f;

		// checks if there is space above
		if (Physics.Raycast(maxVaultPos, dir, out RaycastHit forwardHit, 2f, whatIsGround))
		{
			return;
		}

		// finding the ground under the player
		if (Physics.Raycast(maxVaultPos + dir * 1.5f, Vector3.down, out RaycastHit downHit, 3f, whatIsGround))
		{
			Vector3 landPos = downHit.point + Vector3.up * 0.5f;

			// we move the player smoothly
			StartCoroutine(VaultToPosition(landPos));
		}
	}

	private IEnumerator VaultToPosition(Vector3 targetPos)
	{
		float duration = 0.15f;
		Vector3 startPos = transform.position;
		float elapsed = 0f;

		Vector3 storedVelocity = rb.linearVelocity;

		// temporal
		rb.useGravity = false;

		while (elapsed < duration)
		{
			transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.position = targetPos;
		rb.useGravity = true;
		rb.linearVelocity = storedVelocity;
	}


	private void OnCollisionStay(Collision other)
	{
		int layer = other.gameObject.layer;

		for (int i = 0; i < other.contactCount; i++)
		{
			Vector3 normal = other.contacts[i].normal;

			// Ground check: only if layer is Ground
			if (((1 << layer) & whatIsGround.value) != 0 && IsFloor(normal))
			{
				grounded = true;
				normalVector = normal;
				cancellingGrounded = false;
				CancelInvoke(nameof(StopGrounded));

				if (wallRunning)
					wallRunning = false;
			}

			// Wallrun check: only if layer is WallRunnable AND not floor
			if (((1 << layer) & whatIsWallrunnable.value) != 0 && IsWall(normal) && !IsFloor(normal))
			{
				grounded = false;
				StartWallRun(normal);
				onWall = true;
				cancellingWall = false;
				CancelInvoke(nameof(StopWall));
			}

			// Surf check
			if (IsSurf(normal))
			{
				surfing = true;
				cancellingSurf = false;
				CancelInvoke(nameof(StopSurf));
			}
		}

		float delayMultiplier = 3f;

		if (!cancellingGrounded)
		{
			cancellingGrounded = true;
			Invoke(nameof(StopGrounded), Time.deltaTime * delayMultiplier);
		}

		if (!cancellingWall)
		{
			cancellingWall = true;
			Invoke(nameof(StopWall), Time.deltaTime * delayMultiplier);
		}

		if (!cancellingSurf)
		{
			cancellingSurf = true;
			Invoke(nameof(StopSurf), Time.fixedDeltaTime * delayMultiplier);
		}
	}

	private void StopGrounded()
	{
		grounded = false;
	}

	private void StopWall()
	{
		onWall = false;
		wallRunning = false;
	}

	private void StopSurf()
	{
		surfing = false;
	}

	public Vector3 GetVelocity()
	{
		return rb.linearVelocity;
	}

	public float GetFallSpeed()
	{
		return rb.linearVelocity.y;
	}

	public Collider GetPlayerCollider()
	{
		return playerCollider;
	}

	public Transform GetPlayerCamTransform()
	{
		return playerCam.transform;
	}

	public bool IsCrouching()
	{
		return crouching;
	}

	public Rigidbody GetRb()
	{
		return rb;
	}
}