using UnityEngine;

public class BobaEntity : MonoBehaviour
{
    [SerializeField] float shares = 1.0f;
    [SerializeField] string stock;
    int owner = 0;

    public float getShares() { return shares; }
    public void setShares(float newShares) { shares = newShares; }
    public string getStock() { return stock; }
    public void setStock(string newStock) { stock = newStock; }

    public void setOwner(int newOwner) { owner = newOwner; }
    public int getOwner() { return owner; }
    public void clearOwner() { owner = 0; }

    bool isDestroyed = false;

    public FMODUnity.EventReference bubbleSpawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pitch_Change", 10f / shares);
        FMODUnity.RuntimeManager.PlayOneShot(bubbleSpawn);    
    }

    public void SetupScale()
    {
        transform.localScale *= shares / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50f && !isDestroyed)
        {
            GameManager.Instance.bobaStockManager.RemoveBobaFromPlay(this);
            isDestroyed = true;
        }
    }
}
