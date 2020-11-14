using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;



public class Player : Entity
{

    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;


    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    [SerializeField] private float m_width = Screen.width;
    [SerializeField] private float m_height = Screen.height;
    [SerializeField] private float rate_of_fire;

    public GameObject bullets_prefab;

    Stopwatch stopWatch = new Stopwatch();


    public AudioSource audioSource;

    Vector3 shootDirection;



    public int score_Player { get; set; }


    /*-----------------------------------------------------------------------------*/
    public delegate void OnHPChangeEvent(int hp);
    public event OnHPChangeEvent OnHPChange;

    //public int HP { get; set; } // Health Points of the player 

    /*-----------------------------------------------------------------------------*/
    public delegate void OnScoreChangeEvent(int score);
    public event OnScoreChangeEvent OnScoreChange;

    public override void Awake()
    {
        base.Awake();
        stopWatch.Start();
        m_MainCamera = Camera.main;
        score_Player = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_width = Screen.width;
        m_height = Screen.height;
        
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

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
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            UnityEngine.Debug.Log("Width max:" + m_width);
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
        /*
        if (Input.GetKey(KeyCode.Space) == true)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            //print(ts.TotalSeconds);
            double bullet_time = ts.TotalSeconds;

            if (bullet_time > rate_of_fire)
            {
                stopWatch.Reset();
                stopWatch.Start();
                GameObject instanciated_prefab = Instantiate(bullets_prefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
                instanciated_prefab.GetComponent<Bullet>().OnBulletHit += OnBulletHitPlayer; // suscribe to Bullet event OnHitBulelt
            }
            else
                stopWatch.Start();
            //this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        */
        if (Input.GetMouseButtonDown(0))
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;



            double bullet_time = ts.TotalSeconds;
            Vector3 screenToWorldPoint_player = m_MainCamera.WorldToScreenPoint(this.transform.position);
            screenToWorldPoint_player.z = 0.0f;
            //UnityEngine.Debug.Log("Player pos  : " + this.transform.position + " screentoworldplayer :" + screenToWorldPoint_player);
            //UnityEngine.Debug.Log("mouse : " + Input.mousePosition );
            shootDirection = Input.mousePosition - screenToWorldPoint_player;
            //UnityEngine.Debug.Log("ShootDirection : " + shootDirection);
            shootDirection = shootDirection.normalized;
            UnityEngine.Debug.Log("ShootDirectionNormalized : " + shootDirection);

            if (bullet_time > rate_of_fire)
            {
                stopWatch.Reset();
                stopWatch.Start();
                GameObject instanciated_prefab = Instantiate(bullets_prefab, new Vector3(transform.position.x, transform.position.y , transform.position.z), Quaternion.identity);
                instanciated_prefab.GetComponent<Bullet>().ShootWithDirection(shootDirection);
                instanciated_prefab.GetComponent<Bullet>().OnBulletHit += OnBulletHitPlayer; // suscribe to Bullet event OnHitBulelt
            }
            else
            {
                stopWatch.Start();
            }
        }
          






    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Ennemy")
        {
            //UnityEngine.Debug.Log(collision.rigidbody.tag);
            // Destroy(collision.gameObject); // Ennmey dies while crashing into the player
            //current_HP -= 1;
            if (OnHPChange != null)
            {
                OnHPChange(1);
            }
            this.loseHP(1) ;

            UnityEngine.Debug.Log("Player Current HP : " + current_HP);
        }

        if (collision.rigidbody.tag == "EnnemyBullet")
        {
            //UnityEngine.Debug.Log(collision.rigidbody.tag);
            // Destroy(collision.gameObject); // Ennmey dies while crashing into the player
            //current_HP -= 1;
            if (OnHPChange != null)
            {
                OnHPChange(1);
            }
            this.loseHP(1);

            UnityEngine.Debug.Log("Player Current HP : " + current_HP);
        }

        if (current_HP <= 0)  // If the player has nno HP , he dies 
        {
            this.gameObject.SetActive(false);
        }


       /* // Play a sound if the colliding objects had a big impact.
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();
            */
    }

    private void OnBulletHitPlayer(int score)
    {
        score_Player += score;
        if (OnScoreChange != null)
        {
            OnScoreChange(score);
        }
        UnityEngine.Debug.Log("Total score : " + score_Player + "!");
    }

    public int GetHP() // Egalement, il sera nécessaire de créer une propriété public qui permet de récupérer la valeur max des PVs du vaisseau. 
    {
        return current_HP;
    }

    public int GetScore() //  et de même pour OnScoreChange avec le score. 
    {
        return score_Player;
    }

}
