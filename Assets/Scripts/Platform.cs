using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed; //for speed of upward  motion of platforms
    public float ScreenHeight = 5;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position+= new Vector3(0,speed*Time.deltaTime,0);//moves the platform upwards with a fixed speed 

        if(transform.position.y >= ScreenHeight + 1)
        {
            Destroy(this.gameObject);
        }
    }
}
