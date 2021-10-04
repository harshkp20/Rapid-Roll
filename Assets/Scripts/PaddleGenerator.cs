using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleGenerator : MonoBehaviour
{
    public GameObject NormalPaddle,SpikePaddle,JumperPaddle,Freeze,Extended_Life,Normal_Life;
    public GameObject presentPaddle;
    // Value of this variable needs to be updated in Update function from any other script.
    public float speed =10f;
    public AudioSource GameOver;
    public float TimeCount;
    public int PlayerScore;
    public float duration = 10;
    public float ScreenWidth = 2.48f;
    public float ScreenHeight = 5;
    float timeAggregate = 0;
    public Player Player;
    public GameObject MainPlayer;
    public int PlayerLives;
    public GameObject GameOverImage;
    public GameObject[] LifeBar;        //Number of lives 
    bool StartGenerating = false;      //For generation of platforms

    public float normalProb = 0.4f;    //Probability of generation of normal Platform
    public float spikeProb = 0.4f;     //Probability of generation of Spikes Platform
    float jumperProb = 0.2f;           //Probability of generation of Jumpy Platform
    float previousProb=0;
    int Normal_Produced=4;             //Number of normal platforms produced at start
    public float startTime=5f,presentTime;   
    float increasedSpeed=2f;
    public Text Score;
    int N1=5,N2=10;   //N1 - for number of platforms after which Normal life occurs and N2 - for number of platforms after which other occur

    bool toss = false;            
    void Start()
    {
        TimeCount=0;
        jumperProb = 1 - (normalProb + spikeProb);
        Player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
        MainPlayer = GameObject.FindGameObjectWithTag("Player").gameObject;
        PlayerLives=1;
        presentTime=startTime;
    }

    
    void FixedUpdate()
    {
        TimeCount+=Time.deltaTime;
        PlayerScore = (int)(TimeCount*10) ;
        Score.text = PlayerScore.ToString();
        //To check that platforms are not generated simultaneously at start
        if(StartGenerating==false)
        {
            if(presentPaddle.transform.position.y > -ScreenHeight) StartGenerating = true;
        }
        if(StartGenerating==true)
        {
            //Increasing Speed of Platforms
            if(presentTime<=0)
            {
                increasedSpeed+=0.2f;
                speed+=1f;
                if(increasedSpeed>7) increasedSpeed=7;
                presentTime=startTime;
            }
            else presentTime-=Time.deltaTime;

            //Updating LifeBar
            if(PlayerLives> -1)
            {
                for(int i=0;i<PlayerLives;i++)
                {
                    LifeBar[i].SetActive(true);                
                }
                if(PlayerLives!=5)
                {
                    for(int i=PlayerLives;i<5;i++)
                    {
                        LifeBar[i].SetActive(false);
                    }                
                }            
            } 

            //Ending Game
            if(PlayerLives<0)
            {
                if(PlayerScore> PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore",PlayerScore);
                GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<AudioSource>().Stop();
                GameOver.Play();
                GameOverImage.SetActive(true);
                GameOverImage.transform.GetChild(0).gameObject.GetComponent<Text>().text = PlayerScore.ToString();
                Score.enabled=false;
                Time.timeScale = 0f;            
            }

            //Time Counter for Platform Generation
        if (timeAggregate >= duration)
        {
            timeAggregate = 0;
            N1-=1;
            N2-=1;

            //Repositioning of player after death
            if(Player.isDestroyed==true)
            {
                InsPaddle(NormalPaddle);
            }
            else
            {
                if(N2==0)
                {
                    //Generation of Platform with Extended Life
                    if(toss==false)
                    {
                        toss=true;
                        InsPaddle(Extended_Life);
                    } //Generation of Platform with Freeze
                    else
                    {
                        toss=false;
                        InsPaddle(Freeze);                    
                    } 
                    N2 = Random.Range(10,15);
                    N1= Random.Range(7,11);                
                }  //Generation of Platform with Normal Life
            else if(N1==0)
            {
                InsPaddle(Normal_Life);
                N1= Random.Range(7,11);
            }      //Generation of Random Platform 
            else GenerateRandom();
            }            
        }
        else
        {
            timeAggregate += Time.deltaTime*speed;
        }
        }
    }

    private void GenerateRandom()
    {
        float prob = Random.Range(0f, 1f);
        if(previousProb==0) //Making sure that spiked and jumping platform doesn't come simultaneously
        {
            InsPaddle(NormalPaddle);
            Normal_Produced-=1;
            if(Normal_Produced==0) previousProb=normalProb;
        }
        else if( previousProb>normalProb)
        {
            InsPaddle(NormalPaddle);
            previousProb=normalProb;            
        }
        else
        {
            if(prob <= normalProb)  //Generation of Normal Platform
            {
                InsPaddle(NormalPaddle);
            }
            else if((prob <= (normalProb + spikeProb)) && prob > normalProb)  //Generation of Spiked Platform
            {
                InsPaddle(SpikePaddle);
            }
            else  //Generation of Jumpy Platform
            {
                InsPaddle(JumperPaddle);
            }
            previousProb = prob;
        }        
    }

    private void InsPaddle(GameObject paddle)
    {
        GameObject Instantiated = Instantiate(paddle);
        paddle.transform.position =new Vector3(Random.Range(-ScreenWidth, ScreenWidth), -ScreenHeight, 0);
        Instantiated.GetComponent<Platform>().speed= increasedSpeed;
        if(Player.isDestroyed==true)  //Repositioning of Player
        {
            Vector3 NewPosition = new Vector3(Instantiated.transform.position.x -MainPlayer.transform.position.x,Instantiated.transform.position.y-MainPlayer.transform.position.y +1 , 0 );
            if(Instantiated.transform.position.x > ScreenWidth ) NewPosition-= new Vector3(0,1f,0);   //Making sure player is in the main GameScene
            else if(Instantiated.transform.position.x < -ScreenWidth ) NewPosition+= new Vector3(0,1f,0);
            MainPlayer.transform.position += NewPosition;
            MainPlayer.SetActive(true);
            Player.isDestroyed=false;
        }
    }
}
