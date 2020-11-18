using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet Property and The Size of the Screen


    private float shooting_power = 20.0f;
    private float m_height = Screen.height;
    private float m_width = Screen.width;

    private Camera m_MainCamera;

    public delegate void OnBulletHitEvent(int score_amount);
    public event OnBulletHitEvent OnBulletHit;

    public int score { get; set; } // score to update with Susribe / Listener Scripts system 

    Vector3 direction;

    //bool token = false;

    void Awake()
    {
        m_MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        m_width = Screen.width;
        m_height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Move();  
    }

    void Move()
    {
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(this.transform.position);
        this.transform.position = new Vector3(transform.position.x + direction.x * (shooting_power * Time.deltaTime), transform.position.y +  direction.y * (shooting_power * Time.deltaTime) , 0);
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
            Destroy(this.gameObject);
        }

        if (collision.rigidbody.tag == "CubeBoss")
        {
            //UnityEngine.Debug.Log(collision.rigidbody.tag);
            score += 1000;
            if (OnBulletHit != null)
            {
                OnBulletHit(score);
            }
            Destroy(this.gameObject);
        }
    }
}