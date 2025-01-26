using FMODUnity;
using UnityEngine;

public class StrawStirring : MonoBehaviour
{
    public StudioEventEmitter stirringSoundEvent;

    bool isStrawInLiquid = false;
    bool isStirringPlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStrawInLiquid)
        {
            if (!isStirringPlaying)
            {
                isStirringPlaying = true;
                stirringSoundEvent.Play();
            }
        }
        else
        {
            if (isStirringPlaying)
            {
                isStirringPlaying = false;
                stirringSoundEvent.Stop();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Liquid")
            isStrawInLiquid = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Liquid")
            isStrawInLiquid = false;
    }
}
