using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public bool gameIsNotPaused = true ;

   
    
    private bool _gameIsNotPaused // ecriture prive pour _pause_status
    {
        get
        {
            return gameIsNotPaused;
        }
        set
        {
            gameIsNotPaused = _gameIsNotPaused;
        }
    }
    // Start is called before the first frame update
    protected override void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Play() // puts the game on pause or not
    {
        Debug.Log("PAUSE!");
        UserInterface.Instance.TogglePausePanel(gameIsNotPaused);
        if (gameIsNotPaused)
        {
            gameIsNotPaused = false; // Game is paused 
            Time.timeScale = 0f;
        }
        else
        {
            gameIsNotPaused = true; // Game is paused 
            Time.timeScale = 1f;
        }
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
