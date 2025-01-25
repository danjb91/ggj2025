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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
