using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{

    private Slider HP_bar_UI;
    // Start is called before the first frame update

   
    void Awake()
    {
        HP_bar_UI = GetComponentInChildren<Slider>();
        Player player = FindObjectOfType<Player>();
        player.OnHPChange += HandlingHPChange;
        HP_bar_UI.value = player.GetHP(); // Set the slider bar value to the Player max hp current
    }
    void Start()
    {
        
        float test = 3f;
       // bar.localScale = new Vector3(test, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandlingHPChange(int hp)
    {
        HP_bar_UI.value -= hp;

    }
}
