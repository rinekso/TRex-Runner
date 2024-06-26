using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyDetectObstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.transform.parent.GetComponent<ObstacleScript>()){
            GameController.Instance.CurrentObstacle = null;
            Destroy(other.transform.parent.gameObject);
        }
    }
}
