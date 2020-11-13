using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTypeB : Ennemy
{
    [SerializeField]
    private GameObject target;


    private Camera m_MainCamera;
    [SerializeField] private float m_verticalSpeed;
    [SerializeField] private float m_height = Screen.height;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

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

    public override void Move()
    {
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);

        //System.Console.WriteLine("balle.y = ", screenPos.y, ", position max_ecran = ", m_height);
        if (screenPos.y < 0)
            Destroy(gameObject, 0f);
        if (screenPos.y > 0)
            this.transform.position = new Vector3(transform.position.x + (m_verticalSpeed * Time.deltaTime * 0.2f), transform.position.y - (m_verticalSpeed * Time.deltaTime), transform.position.z); // Goes toward down direction
    }

    public override void Shoot()
    {
        

    }
}
