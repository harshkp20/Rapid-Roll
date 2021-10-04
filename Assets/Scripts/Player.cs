using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float translationSpeed;
    public float JumpForce;
    public PaddleGenerator Generator;
    public int Lives;
    public GameObject NormalSpikes,FreezedSpikes;
    public float Stopped_for_time =5f;
    public bool isDestroyed=false;
    public int maxLifes = 5;
    public AudioSource[] Audios;

    void Start()
    {
        NormalSpikes = GameObject.FindGameObjectWithTag("MainSpikes").gameObject.transform.GetChild(0).transform.gameObject;
        FreezedSpikes = GameObject.FindGameObjectWithTag("MainSpikes").gameObject.transform.GetChild(1).transform.gameObject;
        Generator = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.GetChild(0).transform.gameObject.GetComponent<PaddleGenerator>();        
    }
    // Update is called once per frame
    void Update()
    {

        if (transform.position.x < -Generator.ScreenWidth)
        {
            transform.position = new Vector3(-Generator.ScreenWidth, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > Generator.ScreenWidth)
        {
            transform.position = new Vector3(Generator.ScreenWidth, transform.position.y, transform.position.z);
        }

        //Implementing Freeze Powerup
        if(FreezedSpikes.activeSelf)
        {
            Stopped_for_time-=Time.deltaTime;
        }
        if(Stopped_for_time<=0)
        {
            NormalSpikes.SetActive(true);
            FreezedSpikes.SetActive(false);
            Stopped_for_time = 5f;
        }
        //Player Movement
        float InputX;

        InputX = Input.GetAxis("Horizontal");  //Taking input
        transform.position += new Vector3(translationSpeed*InputX*Time.deltaTime,0,0);

        if(transform.position.y < -Generator.ScreenHeight )
        {
            Generator.PlayerLives-=1;
            Audios[0].Play();
            gameObject.SetActive(false);
            isDestroyed=true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Spikes"))
        {
            Generator.PlayerLives-=1;
            Audios[0].Play();
            gameObject.SetActive(false);
            isDestroyed=true;
        }
        else if(other.gameObject.CompareTag("Jumpy_Platform"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,JumpForce));
        }
        else if(other.gameObject.CompareTag("Normal_Life"))
        {
            Audios[1].Play();
            other.gameObject.SetActive(false);
            if(Generator.PlayerLives< maxLifes) Generator.PlayerLives+=1;
            else Generator.PlayerLives= maxLifes;
        }
        else if(other.gameObject.CompareTag("Extended_Life"))
        {
            Audios[2].Play();
            other.gameObject.SetActive(false);
            Generator.PlayerLives+=3;
            if(Generator.PlayerLives>maxLifes) Generator.PlayerLives=maxLifes;
        }
        else if(other.gameObject.CompareTag("Freeze"))
        {
            Audios[3].Play();
            other.gameObject.SetActive(false);
            NormalSpikes.SetActive(false);
            FreezedSpikes.SetActive(true);
        }      
    }
}
