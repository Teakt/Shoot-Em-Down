using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{

    //private Slider HP_bar_UI;
    // Start is called before the first frame update
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Button playAgain_button;
    [SerializeField] private Text score_text;
    [SerializeField] private Slider hp_bar;
    [SerializeField] private Player player;
   
    void Awake()
    {
        /*
        HP_bar_UI = GetComponentInChildren<Slider>();
        Player player = FindObjectOfType<Player>();
        player.OnHPChange += HandlingHPChange;
        HP_bar_UI.value = player.GetHP(); // Set the slider bar value to the Player max hp current
        */

        
        player.OnHPChange += HandlingHPChange;
        player.OnHPChange += HandlingGameOver;
        player.OnScoreChange += HandlingScoreChange;

        playAgain_button.onClick.AddListener(GameManager.Instance.ResetLevel);

    }
    void Start()
    {
        hp_bar.maxValue = player.GetMaxHP() ;
        hp_bar.value = player.GetMaxHP();
        //float test = 3f;
        // bar.localScale = new Vector3(test, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandlingHPChange(int hp)
    {
        //HP_bar_UI.value -= hp;
        hp_bar.value = player.GetHP() - hp;
        Debug.Log("Bar value : " + hp_bar.value  + "HP PLAYER : " + player.GetHP() + "MAX HP : " + player.GetMaxHP());
    }

    private void HandlingScoreChange(int score)
    {
        //HP_bar_UI.value -= hp;
        score_text.text = $"Score:  ({player.GetScore()})";
    }

    private void HandlingGameOver(int hp)
    {
        if(player.GetHP() - hp == 0)
        {
            gameOver.SetActive(true);
        }
    }


    /*
    public void ResetLevel()
    {
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
        GameManager.Instance.ResetLevel();
    }
    */
}
