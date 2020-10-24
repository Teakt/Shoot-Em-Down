using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField]
    private Camera m_MainCamera;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    private float m_width = Screen.width;
    private float m_height = Screen.height;

    public float gravity = 20.0f;


    private float objectWidth;
    private float objectHeight;
    private Vector2 screenBounds;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerControl();

       
    }


    void PlayerControl()
    {
        // Pos of the player to the camera
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        
        


        //get the Input from Horizontal axis
        float horizontalInput = Input.GetAxis("Horizontal");
        //get the Input from Vertical axis
        float verticalInput = Input.GetAxis("Vertical");

        //if (characterController.isGrounded)
        //{
        // We are grounded, so recalculate
        // move direction directly from axes
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            if (screenPos.x > m_width)
                return;
            this.transform.position = new Vector3(transform.position.x + (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            if (screenPos.x < 0)
                return;
            this.transform.position = new Vector3(transform.position.x - (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            if (screenPos.y > m_height)
                return;
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            if (screenPos.y < 0)
                return;
            this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        Debug.Log("target is " + screenPos.x + " pixels from the left" + Screen.width);
            Debug.Log("target is " + screenPos.y + " pixels from the bottom");

        //}

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        //moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller

       
        

    }
}
