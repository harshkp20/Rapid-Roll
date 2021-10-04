using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuImage;
    public GameObject HighScoreImage;
    public GameObject GameOverImage;
    public GameObject Score;
    
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Restarted") !=0)
        {
            Play();
            PlayerPrefs.SetInt("Restarted",0);
        }
        else Time.timeScale = 0f; 
        int HighScore = PlayerPrefs.GetInt("HighScore",0);
    }

    public void Play()
    {
        MainMenuImage.SetActive(false);
        Score.SetActive(true);
        this.gameObject.GetComponent<AudioSource>().Play();
        Time.timeScale = 1f;
        if(PlayerPrefs.GetInt("Restarted") ==1) PlayerPrefs.SetInt("Restarted",0);
    }

    public void HighScore()
    {
        HighScoreImage.transform.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore").ToString();
        if(HighScoreImage.activeSelf)
        {
            HighScoreImage.SetActive(false);
            MainMenuImage.SetActive(true);
        }
        else
        {
            HighScoreImage.SetActive(true);
            MainMenuImage.SetActive(false);
        }
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("HighScore",0);
        HighScoreImage.transform.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore").ToString();
    } 

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        int Restarted = PlayerPrefs.GetInt("Restarted",0);
        PlayerPrefs.SetInt("Restarted",1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }
}
