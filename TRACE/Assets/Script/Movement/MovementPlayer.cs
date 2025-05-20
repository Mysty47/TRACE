using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float crouchSpeed = 3.5f; // Speed while crouching
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public float crouchHeight = 1.0f; // Height of the character while crouching
    public float standingHeight = 2.0f; // Default height of the character
    public float crouchTransitionSpeed = 5.0f; // Speed of height transition

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    private bool isCrouching = false; // To track crouching state
    private Vector3 cameraOriginalPosition;
    private Vector3 crouchCameraPosition;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store the camera's default position
        cameraOriginalPosition = playerCamera.transform.localPosition;
        crouchCameraPosition = new Vector3(cameraOriginalPosition.x, crouchHeight / 2, cameraOriginalPosition.z);
    }

    void Update()
    {
        // Handle crouch input
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        // Smoothly transition between crouching and standing
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Adjust camera position for crouching
        Vector3 targetCameraPosition = isCrouching ? crouchCameraPosition : cameraOriginalPosition;
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetCameraPosition, crouchTransitionSpeed * Time.deltaTime);

        // Adjust speed based on movement state
        float speed = canMove ? (isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runningSpeed : walkingSpeed)) : 0;

        // Movement calculations
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY = speed * Input.GetAxis("Horizontal");
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
