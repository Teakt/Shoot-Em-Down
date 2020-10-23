using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;


//This Class describes the player and his attributes. A simple ship script that allows to move in 2d and shoot bullets.

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 m_position;


    // The ship speed values, Width and Height values of the Screen
    [SerializeField]
    private Camera m_MainCamera;
    private float m_VerticalSpeed = 5.0f;
    private float m_HorizontalSpeed = 5.0f;
    private float m_width = Screen.width;
    private float m_height = Screen.height;


    //The value that holds the firing_rate
    private int m_firing_rate = 3;

    public GameObject Bullet;
    Stopwatch stopWatch = new Stopwatch();


    //This function is the first called, even before the Start method, but once in all the program execution.
    void Awake()
    {
        stopWatch.Start();
        m_MainCamera = Camera.main;
    }

    void Start()
    {
        //m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        PlayerControl();
        
    }


    //Method that defines the way the control of the ship is handled
    void PlayerControl()
    {
        Vector3 screenPos;
        screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);
      
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
        if (Input.GetKey(KeyCode.Space) == true)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            print(ts.TotalSeconds);
            double bullet_time = ts.TotalSeconds;

            if (bullet_time > 1)
            {
                stopWatch.Reset();
                stopWatch.Start();
                Instantiate(Bullet, new Vector3(transform.position.x, transform.position.y+2, transform.position.z), Quaternion.identity);
            }
            else
                stopWatch.Start();
            //this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }

    }

}
