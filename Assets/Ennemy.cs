using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

 
    private Camera m_MainCamera;
    [SerializeField] private float m_verticalSpeed = 20.0f;
    [SerializeField] private float m_height = Screen.height;

    // Start is called before the first frame update
    void Awake()
    {
        m_MainCamera = Camera.main;
    }
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
        if (screenPos.y <0 )
            Destroy(gameObject, 1f);
        if (screenPos.y > 0)
            this.transform.position = new Vector3(transform.position.x, transform.position.y  - (m_verticalSpeed * Time.deltaTime), transform.position.z); // Goes toward down direction
    }
}
