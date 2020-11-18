using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviourSingleton<UserInterface>
{

    //private Slider HP_bar_UI;
    // Start is called before the first frame update
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWin;
    [SerializeField] private Button playAgain_button;
    [SerializeField] private Button playAgain_button2;
    [SerializeField] private Text score_text;
    [SerializeField] private Slider hp_bar;

    [SerializeField] private Slider cubeboss_hp_bar;

    [SerializeField] private Player player;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pause_button;


    [SerializeField] private CubeBoss cube_boss;

    [SerializeField] private Button special_shot;
    [SerializeField] private Text special_shot_text;
    [SerializeField] private Text special_shot_cd;
    [SerializeField] private GameObject special_shot_cross;
    [SerializeField] private GameObject warningPanel;

    [SerializeField] private Text finalScore;

    float timer = 3f;
    float cd_timer;

    [SerializeField] private EnnemyManager ennemy_manager;
    protected override void Awake()
    {
        player.OnHPChange += HandlingHPChange;
        player.OnHPChange += HandlingGameOver;

        cube_boss.OnHPChange += HandlingBossHPChange;
        cube_boss.OnHPChange += HandlingGameWin;

        player.OnScoreChange += HandlingScoreChange;

        playAgain_button.onClick.AddListener(GameManager.Instance.ResetLevel);
        playAgain_button2.onClick.AddListener(GameManager.Instance.ResetLevel);
        pause_button.onClick.AddListener(GameManager.Instance.Play);


    }
    void Start()
    {
        hp_bar.maxValue = player.GetMaxHP() ;
        hp_bar.value = player.GetMaxHP();

        cubeboss_hp_bar.maxValue = cube_boss.GetMaxHP();
        cubeboss_hp_bar.value = cube_boss.GetMaxHP();

        cd_timer = timer; 
    }

    // Update is called once per frame
    void Update()
    {
        special_shot_cd.text = "" + (int)player.GetSpecialShootCd();
        if(special_shot_cd.text == "0")
        {
            special_shot_cross.SetActive(false);
        }
        else
        {
            special_shot_cross.SetActive(true);
        }


        if (cube_boss != null)
        {
            if (cube_boss.isActiveAndEnabled)
            {
                cubeboss_hp_bar.gameObject.SetActive(true);
            }
            else
            {
                cubeboss_hp_bar.gameObject.SetActive(false);
            }

        }
        if (cube_boss.GetHP()  == 0)
        {
            if (cube_boss.GetStatus()==false)
            {
                gameWin.SetActive(true);
                finalScore.text = " " + player.GetScore();
            }
        }

        if(ennemy_manager.GetSpawnState() == EnnemyManager.SpawnState.BOSS)
        {
            warningPanel.SetActive(true);
            if(cd_timer <= 0)
            {
                warningPanel.SetActive(false);
            }
            else
            {
                cd_timer -= Time.deltaTime;
            }
        }

        
    }

    private void HandlingHPChange(int hp)
    {
        hp_bar.value = player.GetHP() - hp;
    }

    private void HandlingBossHPChange(int hp)
    {
        cubeboss_hp_bar.value = cube_boss.GetHP() - hp;
        //Debug.Log("Bar value : " + hp_bar.value  + "HP PLAYER : " + player.GetHP() + "MAX HP : " + player.GetMaxHP());
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

    private void HandlingGameWin(int hp)
    {
       
    }

    public void TogglePausePanel(bool pause)
    {
        //if(GameManager.Instance.gameIsNotPaused)
        pausePanel.SetActive(pause);
    }
}
