using UnityEngine;

public class StrawController : MonoBehaviour
{
    [SerializeField] private float strawMoveSpeed = 1.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 movement;
    [SerializeField] public bool connectToWasd = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(connectToWasd)
        {
            movement.z = -Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            movement.Normalize();

            Debug.Log("move " + movement);

            rb.linearVelocity = movement * strawMoveSpeed;
        }
    }
}
