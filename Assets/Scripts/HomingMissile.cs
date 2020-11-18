using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{

    public enum MissileState
    {
        IN_RANGE,
        NOT_INRANGE,
       
    }

    public float speed = 25f; 
    
    private Transform target;

    [SerializeField] private float countdownBeforeDestruction;
    [SerializeField] private float timenBeforeDestruction = 5f;
    [SerializeField] private float in_range_distance = 10f;
    private Rigidbody rb;

    Vector3 fixed_direction; 

    MissileState missile_state = MissileState.NOT_INRANGE;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        countdownBeforeDestruction = timenBeforeDestruction;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnnemyBullet"), LayerMask.NameToLayer("EnnemyBullet"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        
        if (FindObjectOfType<Player>() != null)
            target = FindObjectOfType<Player>().transform;
        if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist >= in_range_distance)
            {
                
                missile_state = MissileState.NOT_INRANGE;
            }
            else
            {
                missile_state = MissileState.IN_RANGE;
            }
           
            if(missile_state == MissileState.NOT_INRANGE)
            {
                CrashToPlayer();
            }
            if (missile_state == MissileState.IN_RANGE)
            {
                InRangeBehaviour();
            }
            
        }
        
    }

    void CrashToPlayer()
    {
      
        Vector3 direction = target.position - rb.position ;
       
        this.transform.LookAt(target);
    
        rb.velocity = direction * speed * Time.deltaTime;

        fixed_direction = direction; 
    }

    void InRangeBehaviour()
    {
        if (countdownBeforeDestruction <= 0)
        {
            Vector3 direction = target.position - rb.position;

            this.transform.LookAt(target);

            rb.AddForce(fixed_direction * speed * 2 * Time.deltaTime);
            Destroy(this.gameObject, 2f);
        }
        else
        {
            countdownBeforeDestruction -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.rigidbody.tag == "Player")
        {

            Destroy(this.gameObject);
        }





    }


}
