using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Entity
{
    [SerializeField]
    private GameObject target;

 
    private Camera m_MainCamera;
    [SerializeField] private float m_verticalSpeed = 20.0f;
    [SerializeField] private float m_height = Screen.height;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
        m_MainCamera = Camera.main;
    }
    void Start()
    {
        //Debug.Log("Ennemy current HP : " + current_HP);
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
            Destroy(gameObject, 0f);
        if (screenPos.y > 0)
            this.transform.position = new Vector3(transform.position.x, transform.position.y  - (m_verticalSpeed * Time.deltaTime), transform.position.z); // Goes toward down direction
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Player") // the ennemy crashes if he collides with player
        {
            Destroy(this.gameObject);
        }

        if (collision.rigidbody.tag == "Bullet") // the ennemy crashes if he collides with player
        {
            this.loseHP(1);
            Debug.Log("Ennemy HP : " + current_HP);
        }

        if (current_HP <= 0)  // If the player has nno HP , he dies 
        {
            Destroy(this.gameObject);
        }


        /* // Play a sound if the colliding objects had a big impact.
         if (collision.relativeVelocity.magnitude > 2)
             audioSource.Play();
             */
    }
}
