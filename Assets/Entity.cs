using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Entity : MonoBehaviour
{

    [SerializeField] private  int max_HP ;  // Maximum health points of the entity


    private int _current_HP;

    public int current_HP
    {
        get
        {
            return _current_HP;
        }
    }

    protected void loseHP(int lost_hp)
    {
        _current_HP -= lost_hp;
    }

    public virtual void Awake()
    {
        _current_HP = max_HP;
    }
}



