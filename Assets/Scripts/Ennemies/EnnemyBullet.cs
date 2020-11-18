using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    [SerializeField] private float m_verticalSpeed = 20.0f;
    private float m_height = Screen.height;
    private float m_width = Screen.width;

    private Camera m_MainCamera;

    Vector3 direction;

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
        this.transform.position = new Vector3(transform.position.x + direction.x * (m_verticalSpeed * Time.deltaTime), transform.position.y + direction.y * (m_verticalSpeed * Time.deltaTime), 0);

        if (screenPos.y >= m_height)
            Destroy(gameObject, 0);
        if (screenPos.y  < 0 )
            Destroy(gameObject, 0);
        if (screenPos.x >= m_width)
            Destroy(gameObject, 0);
        if (screenPos.x < 0)
            Destroy(gameObject, 0);


    }


    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Player")
        {
            
            Destroy(this.gameObject);
        }




       
    }

    public void ShootWithDirection(Vector3 direction)
    {
        this.direction = direction;

    }



}
