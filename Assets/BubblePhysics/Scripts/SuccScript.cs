using FMODUnity;
using UnityEngine;

public class SuccScript : MonoBehaviour
{
    [SerializeField] float succForce = 100.0f;
    [SerializeField] string succButton = "succ";

    public StudioEventEmitter succSoundEvent;

    bool isSuccing = false;

    private void FixedUpdate()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<BobaEntity>() != null && Input.GetButton(succButton))
        {
            if (!isSuccing)
            {
                isSuccing = true;
                BeginSucc();
            }
            
            Debug.Log("Succing");
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(transform.up * succForce);
            }
        }
        else if (!Input.GetButton(succButton))
        {
            if (isSuccing)
            {
                isSuccing = false;
                EndSucc();
            }
        }
    }

    private void BeginSucc()
    {
        succSoundEvent.Play();
    }

    private void EndSucc()
    {
        succSoundEvent.Stop();
    }
}
