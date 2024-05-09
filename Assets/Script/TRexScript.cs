using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexScript : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSourceStep, audioSourceRoar;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void PlayStep(){
        audioSourceStep.Stop();
        audioSourceStep.Play();
    }
    public void PlayRoar(){
        audioSourceRoar.Stop();
        audioSourceRoar.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
