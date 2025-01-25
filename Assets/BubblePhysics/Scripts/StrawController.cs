using UnityEngine;

public class StrawController : MonoBehaviour
{
    [SerializeField] private Transform strawLimiter;
    [SerializeField] private float strawMoveSpeed = 1.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 movement;
    [SerializeField] public bool connectToWasd = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        strawLimiter = GameObject.FindGameObjectWithTag("StrawYLimit").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 resultingMovement = Vector3.zero;

        if (connectToWasd)
        {
            movement.z = -Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            movement.Normalize();

            //Debug.Log("move " + movement);

            resultingMovement = movement * strawMoveSpeed; ;
        

            if(movement.y < 0.0f && transform.position.y < strawLimiter.transform.position.y)
            {
                resultingMovement.y = 0.0f;
                transform.position = new Vector3(transform.position.x, strawLimiter.transform.position.y, transform.position.z);
            }


       
        }

        rb.linearVelocity = resultingMovement;
    }
}
