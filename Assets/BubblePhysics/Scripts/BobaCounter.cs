using UnityEngine;

public class BobaCounter : MonoBehaviour
{
    [SerializeField] int player = 1;
    [SerializeField] float totalScore = 0.0f;

    public float getScore() { return totalScore; }

    private void OnTriggerEnter(Collider other)
    {
        BobaEntity bobaComp = other.GetComponent<BobaEntity>();

        if(bobaComp != null)
        {
            GameManager.Instance.stockSim.BuyStock(bobaComp.getStock(), player);
            bobaComp.setOwner(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BobaEntity bobaComp = other.GetComponent<BobaEntity>();

        if (bobaComp != null && bobaComp.getOwner() == player)
        {
            GameManager.Instance.stockSim.SellStock(bobaComp.getStock(), player);
            bobaComp.clearOwner();
        }
    }
}
