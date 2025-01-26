using FMODUnity;
using UnityEngine;

public class SuccScript : MonoBehaviour
{
    [SerializeField] float succForce = 100.0f;
    [SerializeField] string succButton = "succ";
    StrawScaleManager strawParent;

    int owner = 0;
    public StudioEventEmitter succSoundEvent;

    bool isSuccing = false;

    private void FixedUpdate()
    {
        
    }

    private void Start()
    {
        strawParent = gameObject.GetComponentInParent<StrawScaleManager>();
    }
    private void OnTriggerStay(Collider other)
    {
        var owner = strawParent.owner;
        if(other.GetComponent<BobaEntity>() != null && Input.GetButton(succButton))
        {
            var boba = other.GetComponent<BobaEntity>();
            if (!isSuccing)
            {
                isSuccing = true;
                BeginSucc();
            }
            
            Rigidbody rb = other.GetComponent<Rigidbody>();
            float money = Mathf.Max(StrawScaleManager.minMoneyRange, (float)GameManager.Instance.stockSim.currentMoney[owner]);
            float moneyRatio = money / StrawScaleManager.maxMoneyRange;
            float scale = StrawScaleManager.minScale + (StrawScaleManager.maxScale - StrawScaleManager.minScale) * moneyRatio;

            if (rb != null && boba != null)
            {
                var bobaScale = (boba.getShares() / 10f);
                float scaleDamping = 1.0f;
                if (bobaScale > scale)
                {
                    scaleDamping = Mathf.Max(1f, scale / bobaScale);
                }
                rb.AddForce(transform.up * succForce * scaleDamping);
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
