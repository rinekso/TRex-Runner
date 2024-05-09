using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float _speed = .01f;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(0,0,-1)*_speed*Time.deltaTime;
    }
}
