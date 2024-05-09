using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    string _tag = "Obstacle";
    void OnTriggerEnter(Collider other){
        if(other.tag == _tag){
            GameController.Instance.GameOver();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
