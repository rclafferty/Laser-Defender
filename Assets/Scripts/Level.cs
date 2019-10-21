using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("title");
        GameObject.Find("Music Player").GetComponent<MusicPlayer>().PlayLoadingScreenMusic();
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("game");
        GameObject.Find("Music Player").GetComponent<MusicPlayer>().PlayGameMusic();

        GameObject.Find("Game Session").GetComponent<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverScreen());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("game_over");
        GameObject.Find("Music Player").GetComponent<MusicPlayer>().PlayGameOverMusic();
    }
}
