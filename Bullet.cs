using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet Property and The Size of the Screen
    [SerializeField]
    private Camera m_MainCamera;
    private float m_verticalSpeed = 5.0f;
    private float m_height = Screen.height;
    //private float m_width = Screen.width;

    void Awake()
    {
        m_MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);

        //System.Console.WriteLine("balle.y = ", screenPos.y, ", position max_ecran = ", m_height);
        if (screenPos.y >= m_height)
            Destroy(gameObject, 0);
        if (screenPos.y < m_height)
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_verticalSpeed * Time.deltaTime), transform.position.z);
    }
}
