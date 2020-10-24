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
        if(screenPos.x < m_MainCamera.pixelWidth && screenPos.x > 0)
        {
            transform.position = transform.position + new Vector3(horizontalInput * m_HorizontalSpeed * Time.deltaTime, 0, verticalInput * m_VerticalSpeed * Time.deltaTime); // we use X and Z cause thats how the camera looks at the ground
            Debug.Log("Out of bounds");
        }
       // transform.position =  new Vector3( Mathf.Clamp(screenPos.x, -Screen.width / 2, Screen.width / 2), transform.position.y, Mathf.Clamp(transform.position.z, 0, m_MainCamera.pixelHeight) );

       
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
