using UnityEngine;

public class StrawController : MonoBehaviour
{
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";

    [SerializeField] private Vector3 strawLimiter;
    [SerializeField] private Vector3 lowerLeft;
    [SerializeField] private Vector3 upperRight;

    [SerializeField] private float strawMoveSpeed = 1.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 movement;
    [SerializeField] public bool connectToWasd = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Resolution res = Screen.currentResolution;
        print(res);

        strawLimiter = GameObject.FindGameObjectWithTag("StrawYLimit").transform.position;
        Vector3 cameraPos = Camera.main.transform.position;

        float distFromCam = strawLimiter.x - cameraPos.x;
        
        lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(50, 0, distFromCam) );
        upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth-50, Camera.main.pixelHeight, distFromCam));

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 resultingMovement = Vector3.zero;

        if (connectToWasd)
        {
            movement.z = Input.GetAxis(horizontalAxis);
            movement.y = Input.GetAxis(verticalAxis);
            movement.Normalize();

            resultingMovement = movement * strawMoveSpeed; ;
        

            if(movement.y < 0.0f && transform.position.y < strawLimiter.y)
            {
                resultingMovement.y = 0.0f;
                transform.position = new Vector3(transform.position.x, strawLimiter.y, transform.position.z);
            }
            else if(movement.y > 0.0f && transform.position.y > upperRight.y)
            {
                resultingMovement.y = 0.0f;
                transform.position = new Vector3(transform.position.x, upperRight.y, transform.position.z);
            }
            else if (movement.z < 0.0f && transform.position.z > lowerLeft.z)
            {
                resultingMovement.z = 0.0f;
                transform.position = new Vector3(transform.position.x, transform.position.y, lowerLeft.z);
            }
            else if (movement.z > 0.0f && transform.position.z < upperRight.z)
            {
                resultingMovement.z = 0.0f;
                transform.position = new Vector3(transform.position.x, transform.position.y, upperRight.z);
            }

            //Debug.Log("pos  " + transform.position.z + " left " + lowerLeft.z);

        }

        //Debug.Log("move " + resultingMovement);

        resultingMovement.z *= -1;

        //Debug.Log("after move " + resultingMovement);
        rb.linearVelocity = resultingMovement;
    }
}
