using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : Entity
{

    public enum BossState
    {
        WAITING,
        ATTACKING,
        APPEARING,
    }

    public enum BossPhases
    {
        PHASE1,
        PHASE2,
        PHASE3,
    }

    private Player target;



    GameObject cube;
    float angleZ = 0;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_width = Screen.width;
    [SerializeField] private float m_height = Screen.height;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;

    [SerializeField] private float rate_of_fire;

    [SerializeField] private float shooting_power;

    [SerializeField] private GameObject ennemybullet_prefab;
    [SerializeField] private GameObject ennemytype2_prefab;

    public float timeBetweenEachShot = 1f;
    [SerializeField] private float timeBetweenShotCountdown;

    float cd = 5f;
    float speed;

    public float timeBetweenEachState = 3f;
    [SerializeField] private float timeBetweenStateCountdown ;

    // Start is called before the first frame update
    [SerializeField] private BossState boss_state = BossState.APPEARING;
    [SerializeField] private BossPhases boss_phase = BossPhases.PHASE1;

    [SerializeField] private float MovSpeed = 10f;
    /*-----------------------------------------------------------------------------*/
    public delegate void OnHPChangeEvent(int hp);
    public event OnHPChangeEvent OnHPChange;


    public Transform[] corners;


    void Start()
    {
        
        m_MainCamera = Camera.main;
        m_width = Screen.width;
        m_height = Screen.height;

       speed = 15f;

        timeBetweenStateCountdown = timeBetweenEachState;
        timeBetweenShotCountdown = timeBetweenEachShot;
    }

    // Update is called once per frame
    void Update()
    {

        if (FindObjectOfType<Player>() != null)
            target = FindObjectOfType<Player>();


        if (boss_state == BossState.APPEARING)
        {
            MoveToInitPos();
            
        }
        /*
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        angleZ += 10f;
        this.transform.Rotate(angleZ , 0.0f, angleZ, Space.World);
        if (screenPos.x > m_width)
            return;
        this.transform.position = new Vector3(transform.position.x + (m_HorizontalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        */

        if(boss_phase == BossPhases.PHASE1)
        {

            if (this.current_HP <= this.GetMaxHP() / 2)
            {
                boss_phase = BossPhases.PHASE2; 
            }

            if (timeBetweenStateCountdown <= 0)
            {
                if (boss_state == BossState.WAITING)
                {
                    StartCoroutine(MoveRandom());

                    // Start spawning wave
                    StartCoroutine(WaitingState());
                }


                if (boss_state == BossState.ATTACKING)
                {

                    if( timeBetweenShotCountdown <= 0)
                    {
                        Corner_Shoot();
                        timeBetweenShotCountdown = timeBetweenEachShot;
                    }
                    else
                    {
                        timeBetweenShotCountdown -= Time.deltaTime;
                    }
                        
                }
            }
            else
            {
                timeBetweenStateCountdown -= Time.deltaTime;
            }
        }

        if(boss_phase == BossPhases.PHASE2)
        {
            transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            if (boss_state == BossState.WAITING)
            {
                //this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(speed * Time.deltaTime, -speed * Time.deltaTime), transform.position.y, transform.position.z);
                MoveRandom();
                // Start spawning wave
                StartCoroutine(WaitingState());
            }


            if (boss_state == BossState.ATTACKING)
            {

                if (timeBetweenShotCountdown <= 0)
                {
                    Corner_Shoot();
                    Corner_Spawn();
                    timeBetweenShotCountdown = timeBetweenEachShot;
                }
                else
                {
                    timeBetweenShotCountdown -= Time.deltaTime;
                }

            }


            if (current_HP <= 0)  // If the player has nno HP , he dies 
            {
                this.setHP(GetMaxHP());
                boss_state = BossState.APPEARING;
                boss_phase = BossPhases.PHASE3;
            }



        }

        if (boss_phase == BossPhases.PHASE3)
        {
            transform.Rotate(Vector3.forward * 80 * Time.deltaTime, Space.Self);
            transform.localScale.Set(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z * 1.5f);
            if (boss_state == BossState.WAITING)
            {
                //this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(speed * Time.deltaTime, -speed * Time.deltaTime), transform.position.y, transform.position.z);
                MoveRandom();
                // Start spawning wave
                StartCoroutine(WaitingState());
            }


            if (boss_state == BossState.ATTACKING)
            {

                if (timeBetweenShotCountdown <= 0)
                {
                    Corner_Shoot();
                    Corner_Spawn();
                    timeBetweenShotCountdown = timeBetweenEachShot;
                }
                else
                {
                    timeBetweenShotCountdown -= Time.deltaTime;
                }

            }


            if (current_HP <= 0)  // If the player has nno HP , he dies 
            {
                this.setHP(GetMaxHP());
                boss_state = BossState.APPEARING;
                boss_phase = BossPhases.PHASE3;
            }



        }
    }


    void MoveToInitPos()
    {
        // Pos of the player to the camera
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y > m_height - 0.25*m_height)
        {
            //Debug.Log("IF");
            if (screenPos.x < m_width / 2)
            {
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            this.transform.position = new Vector3(transform.position.x, transform.position.y - (m_VerticalSpeed * Time.deltaTime), transform.position.z);

        }
        else
        {
            //Debug.Log("ELSE");
            if (screenPos.x < m_width/2)
            {
                //Debug.Log("ELSE1" + screenPos.x + " width / 2 : " + m_width /2);
                this.transform.position = new Vector3(transform.position.x + (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }
            else
            {
                //Debug.Log("ELS2E2");
                this.transform.position = new Vector3(transform.position.x - (m_VerticalSpeed * Time.deltaTime), transform.position.y, transform.position.z);
            }


            this.transform.position = new Vector3(transform.position.x, transform.position.y + (m_VerticalSpeed * Time.deltaTime), transform.position.z);
        }

        if(screenPos.x > (m_width / 2) - 1 && screenPos.x <= (m_width / 2) + 1 && screenPos.y > m_height - 0.25 * m_height - 1 && screenPos.y <= m_height - 0.25 * m_height + 1)
        {
            boss_state = BossState.WAITING;
        }
        
       
        
    }

    IEnumerator WaitingState()
    {
       
       


        yield return new WaitForSeconds(timeBetweenEachState);
        boss_state = BossState.ATTACKING;

        timeBetweenStateCountdown = timeBetweenEachState;

       
        
    }


    private void Corner_Shoot()
    {
        if (corners.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        for (int i = 0; i < corners.Length; i++)
        {
            //Debug.Log("Shoot : " + corners[i].name);
            GameObject instanciated_prefab = Instantiate(ennemybullet_prefab, new Vector3(corners[i].position.x, corners[i].position.y, corners[i].position.z), Quaternion.identity);
           
            
        }

        boss_state = BossState.WAITING;


    }

    private void Corner_Spawn()
    {
        if (corners.Length == 0)
        {
            Debug.LogError("No spawn points referenced .");
        }
        for (int i = 0; i < corners.Length; i++)
        {
            //Debug.Log("Shoot : " + corners[i].name);
            GameObject instanciated_prefab = Instantiate(ennemytype2_prefab, new Vector3(corners[i].position.x, corners[i].position.y, 0), Quaternion.identity);


        }

        boss_state = BossState.WAITING;


    }


    IEnumerator  MoveRandom() {

        int rand = (int)Random.Range(0, 2);
        Vector3 screenPos = m_MainCamera.WorldToScreenPoint(transform.position);
        yield return new WaitForSeconds(1f);

        if (screenPos.x < m_width  && screenPos.x > 0 )
        {
            if (rand == 0)
                GoLeft();
            else
                GoRight();
        }
        

    }
    private void GoRight()
    {
        this.transform.position = new Vector3(transform.position.x + (MovSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }
    private void GoLeft()
    {
        this.transform.position = new Vector3(transform.position.x - (MovSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Player") // the ennemy crashes if he collides with player
        {
            
        }

        if (collision.rigidbody.tag == "Bullet") // the ennemy crashes if he collides with player
        {
            if (OnHPChange != null)
            {
                OnHPChange(1);
            }
            this.loseHP(1);
            
            //Debug.Log("Ennemy HP : " + current_HP);
        }

        if (collision.rigidbody.tag == "Bullet_type2") // the ennemy crashes if he collides with player
        {
            if (OnHPChange != null)
            {
                OnHPChange(5);
            }
            this.loseHP(5);

        }


        /*
        if (current_HP <= 0)  // If the player has nno HP , he dies 
        {
            Destroy(this.gameObject);
        }
        */


        /* // Play a sound if the colliding objects had a big impact.
         if (collision.relativeVelocity.magnitude > 2)
             audioSource.Play();
             */
    }
}
