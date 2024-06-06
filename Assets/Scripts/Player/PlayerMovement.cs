using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody rigidBody;

    [SerializeField] private float movementSpeed;  //10000
    [SerializeField] private int sensitivity;      //5
    private int camAngleXLimit = 85;
    private float playerXRotation;

    public bool isGrounded = true;
    [SerializeField] private float jumpHeight;     //400

    private Vector3 currentForce;
    [SerializeField] private float customDrag;     //idk yet (in %)

    void Start()
    {
        mainCam = Camera.main;
        rigidBody = GetComponent<Rigidbody>();

        //locks the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        currentForce = rigidBody.GetAccumulatedForce();

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
        }
        
        if (!isGrounded)
        {
            rigidBody.AddForce(transform.right * (Input.GetAxis("Horizontal") * (movementSpeed / 20f) * Time.deltaTime));
            rigidBody.AddForce(transform.forward * (Input.GetAxis("Vertical") * (movementSpeed / 20f) * Time.deltaTime));
        }

        //Allows the player to jump with [SPACE]
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(transform.up * jumpHeight);
        }

        //Adds drag only on the X or Z axis
        if (Input.GetAxis("Horizontal") == 0 && currentForce.x != 0)
            rigidBody.AddForce(transform.right * (-currentForce.x / 100) * customDrag);
        if (Input.GetAxis("Vertical") == 0 && currentForce.z != 0)
            rigidBody.AddForce(transform.forward * (-currentForce.x / 100) * customDrag);
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
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
// Don't forget to add drag!