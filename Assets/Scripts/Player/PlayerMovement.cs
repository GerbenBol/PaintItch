using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody rigidBody;

    [SerializeField] private float movementSpeed;  //10000
    [SerializeField] private int sensitivity;      //5
    private readonly int camAngleXLimit = 85;
    private float playerXRotation;

    public bool isGrounded = true;
    [SerializeField] private int jumpHeight;       //600
    [SerializeField] private int trampJumpMod;     //700
    private bool onTramp;
    [SerializeField] private int drag;             //20

    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider playerTrigger;
    [SerializeField] private AudioSource walkingSound;

    private bool crouched;

    void Start()
    {
        mainCam = Camera.main;
        rigidBody = GetComponent<Rigidbody>();

        //locks the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;

        rigidBody.drag = drag;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            LookAround();


        //Makes the player move with [W,A,S,D]
        if (isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {
                rigidBody.AddForce(transform.right * (Input.GetAxis("Horizontal") * (movementSpeed / 1.3f) * Time.deltaTime));
                rigidBody.AddForce(transform.forward * (Input.GetAxis("Vertical") * (movementSpeed / 1.3f) * Time.deltaTime));
            }
            else
            {
                rigidBody.AddForce(transform.right * (Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime));
                rigidBody.AddForce(transform.forward * (Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));
            }

            // SFX
            if (rigidBody.velocity != Vector3.zero && !walkingSound.isPlaying)
                walkingSound.Play();
            else if (rigidBody.velocity == Vector3.zero && walkingSound.isPlaying)
                walkingSound.Stop();
        }

        if (!isGrounded && !crouched)
        {
            rigidBody.AddForce(transform.right * (Input.GetAxis("Horizontal") * (movementSpeed / 25f) * Time.deltaTime));
            rigidBody.AddForce(transform.forward * (Input.GetAxis("Vertical") * (movementSpeed / 25f) * Time.deltaTime));

            if (walkingSound.isPlaying)
                walkingSound.Stop();
        }

        //Allows the player to jump with [SPACE]
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !crouched)
        {
            if (onTramp)
                jumpHeight += trampJumpMod;

            rigidBody.AddForce(transform.up * jumpHeight);

            if (jumpHeight > trampJumpMod)
                jumpHeight -= trampJumpMod;
            //rigidBody.drag = 0;
        }

        // Crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!crouched)
            {
                crouched = true;
                playerCollider.enabled = false;
                playerTrigger.enabled = false;
                isGrounded = false;
                rigidBody.drag = 0;
            }
            else
            {
                crouched = false;
                playerCollider.enabled = true;
                playerTrigger.enabled = true;
            }
        }
    }

    private void LookAround()
    {
        //Rotates the Camera with a set limit on the X Axis and rotates the player model around the Y Axis
        playerXRotation += -Input.GetAxis("Mouse Y") * sensitivity;
        playerXRotation = Mathf.Clamp(playerXRotation, -camAngleXLimit, camAngleXLimit);
        mainCam.transform.localRotation = Quaternion.Euler(playerXRotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    //checks if the player is standing on the ground or not
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Arrows"))
        {
            rigidBody.drag = drag;

            if (other.CompareTag("Trampoline"))
                onTramp = true;
            else
                onTramp = false;

            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        rigidBody.drag = 0;
    }
}
