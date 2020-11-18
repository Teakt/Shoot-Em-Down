using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy : Entity
{

    public abstract void Move();
    public abstract void Shoot();
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
            //Debug.Log("Ennemy HP : " + current_HP);
        }
        if (collision.rigidbody.tag == "Bullet_type2") // the ennemy crashes if he collides with player
        {
            this.loseHP(5);
        }
        if (current_HP <= 0)  // If the player has nno HP , he dies 
        {
            Destroy(this.gameObject);
        }
    }
}
