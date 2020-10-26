using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet Property and The Size of the Screen
    [SerializeField]
    
    private float m_verticalSpeed = 20.0f;
    private float m_height = Screen.height;
    //private float m_width = Screen.width;

    private Camera m_MainCamera;

    public delegate void OnBulletHitEvent(int score_amount);
    public event OnBulletHitEvent OnBulletHit;

    public int score { get; set; } // score to update with Susribe / Listener Scripts system 

    void Awake()
    {
        m_MainCamera = Camera.main;
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
    }

    void Move()
    {

        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);

        //System.Console.WriteLine("balle.y = ", screenPos.y, ", position max_ecran = ", m_height);
        if (screenPos.y >= m_height)
            Destroy(gameObject, 0);
        if (screenPos.y < m_height)
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_verticalSpeed  * Time.deltaTime), transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
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
            Destroy(this.gameObject);
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