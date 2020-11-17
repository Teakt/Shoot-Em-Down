using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Type2 : MonoBehaviour
{
    //Bullet Property and The Size of the Screen


    private float shooting_power ;
    private float m_height = Screen.height;
    private float m_width = Screen.width;

    private Camera m_MainCamera;

    public delegate void OnBulletHitEvent(int score_amount);
    public event OnBulletHitEvent OnBulletHit;

    public int score { get; set; } // score to update with Susribe / Listener Scripts system 

    Vector3 direction;



    private Vector3 scaleChange;

    [SerializeField] private float scaling_rate = 1f ; 

    //bool token = false;

    void Awake()
    {
        m_MainCamera = Camera.main;
        scaleChange = new Vector3(0, scaling_rate, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
       
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        transform.LookAt(new Vector3(direction.x * 200, direction.y * 200, 0));
        transform.localScale += scaleChange; 
    }

    void Move()
    {

        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);
        
        this.transform.position = new Vector3(transform.position.x + direction.x * (shooting_power * Time.deltaTime), transform.position.y + direction.y * (shooting_power * Time.deltaTime), 0);
        //System.Console.WriteLine("balle.y = ", screenPos.y, ", position max_ecran = ", m_height);
        if (screenPos.y >= m_height)
            Destroy(gameObject, 0);
        if (screenPos.y >= m_height)
            Destroy(gameObject, 0);
        if (screenPos.y < 0)
            Destroy(gameObject, 0);
        if (screenPos.x >= m_width)
            Destroy(gameObject, 0);
        if (screenPos.x < 0)
            Destroy(gameObject, 0);

        //if (screenPos.y < m_height)
        //this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_verticalSpeed  * Time.deltaTime), transform.position.z);

    }

    public void ShootWithDirection(Vector3 direction, float shooting_power)
    {
        this.direction = direction;
        this.shooting_power = shooting_power;

    }

    public void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Ennemy")
        {
            //UnityEngine.Debug.Log(collision.rigidbody.tag);
            score += 100;
            if (OnBulletHit != null)
            {
                OnBulletHit(score);
            }
           
        }




        /* // Play a sound if the colliding objects had a big impact.
         if (collision.relativeVelocity.magnitude > 2)
             audioSource.Play();
             */
    }
    /*
    public void BulletHit()
    {
        FindObjectOfType<Player>().score+= 50;
        UnityEngine.Debug.Log("Ennemy Hit SCORE GOES UP : " + FindObjectOfType<Player>().score + "!");
    }
    */

}