using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody rigidBody;

    [SerializeField] private float movementSpeed;  //15000
    [SerializeField] private int sensitivity;      //5
    private int camAngleXLimit = 85;
    private float playerXRotation;

    public bool isGrounded = true;
    [SerializeField] private float jumpHeight;     //500
    [SerializeField] private int drag;             //30

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
            rigidBody.AddForce(transform.right * (Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime));
            rigidBody.AddForce(transform.forward * (Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));
        }

        //Allows the player to jump with [SPACE]
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(transform.up * jumpHeight);
            rigidBody.drag = 0;
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
        rigidBody.drag = drag;
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        rigidBody.drag = 0;
    }
}
// Don't forget to add drag!