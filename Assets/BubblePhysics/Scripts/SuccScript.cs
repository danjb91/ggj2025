using UnityEngine;

public class SuccScript : MonoBehaviour
{
    [SerializeField] float succForce = 100.0f;
    [SerializeField] string succButton = "succ";
    private void FixedUpdate()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<BobaEntity>() != null && Input.GetButton(succButton))
        {
            Debug.Log("Succing");
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(transform.up * succForce);
            }
        }
    }
}
