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
    [SerializeField] private float rate_of_fire_type2;
    [SerializeField] private float shooting_power ;
    [SerializeField] private float shooting_power_type2;

    public GameObject bullets_prefab;
    public GameObject bullet_type2_prefab;
    public AudioSource Bang;
    public AudioSource Laser;

    Stopwatch stopWatch = new Stopwatch();



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
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-90.0f, 0f, 0.0f));
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
        if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true )
        {
            //UnityEngine.Debug.Log("Width max:" + m_width);
            if (screenPos.x > m_width)
                return;
            this.transform.position = new Vector3(transform.position.x + (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.Q) == true )
        {
            if (screenPos.x < 0)
                return;
            this.transform.position = new Vector3(transform.position.x - (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.Z) == true )
        {
            if (screenPos.y > m_height)
                return;
            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true )
        {
            if (screenPos.y < 0)
                return;
            this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }

        Vector3 pos = Input.mousePosition;
        Vector3 screenToWorldPoint_player = m_MainCamera.WorldToScreenPoint(this.transform.position);
        Vector3 direction = pos - screenToWorldPoint_player;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.Rotate(new Vector3(0, -angle + 90, 0));

        if (Input.GetMouseButtonDown(0))
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;



            double bullet_time = ts.TotalSeconds;
            screenToWorldPoint_player.z = 0.0f;
            //UnityEngine.Debug.Log("Player pos  : " + this.transform.position + " screentoworldplayer :" + screenToWorldPoint_player);
            //UnityEngine.Debug.Log("mouse : " + Input.mousePosition );
            shootDirection = Input.mousePosition - screenToWorldPoint_player;
            //UnityEngine.Debug.Log("ShootDirection : " + shootDirection);
            shootDirection = shootDirection.normalized;
            //UnityEngine.Debug.Log("ShootDirectionNormalized : " + shootDirection);

            if (bullet_time > rate_of_fire)
            {
                stopWatch.Reset();
                stopWatch.Start();
                Bang.Play();
                GameObject instanciated_prefab = Instantiate(bullets_prefab, new Vector3(transform.position.x, transform.position.y , transform.position.z), Quaternion.Euler(new Vector3(0.0f, 0f, angle - 90.0f)));
                instanciated_prefab.GetComponent<Bullet>().ShootWithDirection(shootDirection, shooting_power);
                instanciated_prefab.GetComponent<Bullet>().OnBulletHit += OnBulletHitPlayer; // suscribe to Bullet event OnHitBulelt
            }
            else
            {
                stopWatch.Start();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            secondaryShoot();
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

        if (collision.rigidbody.tag == "CubeBoss")
        {
            //UnityEngine.Debug.Log(collision.rigidbody.tag);
            // Destroy(collision.gameObject); // Ennmey dies while crashing into the player
            //current_HP -= 1;
            if (OnHPChange != null)
            {
                OnHPChange(5);
            }
            this.loseHP(5);

            //UnityEngine.Debug.Log("Player Current HP : " + current_HP);
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

            //UnityEngine.Debug.Log("Player Current HP : " + current_HP);
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
        //UnityEngine.Debug.Log("Total score : " + score_Player + "!");
    }

    public int GetHP() // Egalement, il sera nécessaire de créer une propriété public qui permet de récupérer la valeur max des PVs du vaisseau. 
    {
        return current_HP;
    }

    public int GetScore() //  et de même pour OnScoreChange avec le score. 
    {
        return score_Player;
    }

    public void secondaryShoot()
    {
        //UnityEngine.Debug.Log("Secondary shoot");
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        double bullet_time = ts.TotalSeconds;
        Vector3 screenToWorldPoint_player = m_MainCamera.WorldToScreenPoint(this.transform.position);
        screenToWorldPoint_player.z = 0.0f;
        shootDirection = Input.mousePosition - screenToWorldPoint_player;
        shootDirection = shootDirection.normalized;   

        if (bullet_time > rate_of_fire_type2)
        {
            stopWatch.Reset();
            stopWatch.Start();
            Laser.Play();
            GameObject instanciated_prefab = Instantiate(bullet_type2_prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            instanciated_prefab.GetComponent<Bullet_Type2>().ShootWithDirection(shootDirection, shooting_power_type2);
            instanciated_prefab.GetComponent<Bullet_Type2>().OnBulletHit += OnBulletHitPlayer; // suscribe to Bullet event OnHitBulelt
        }
        else
        {
            stopWatch.Start();
        }
    }

}
