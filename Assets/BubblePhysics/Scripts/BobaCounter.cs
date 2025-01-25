using UnityEngine;

public class BobaCounter : MonoBehaviour
{
    [SerializeField] float totalScore = 0.0f;

    public float getScore() { return totalScore; }

    private void OnTriggerEnter(Collider other)
    {
        BobaEntity bobaComp = other.GetComponent<BobaEntity>();

        if(bobaComp != null)
        {
            totalScore += bobaComp.getScore();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BobaEntity bobaComp = other.GetComponent<BobaEntity>();

        if (bobaComp != null)
        {
            totalScore -= bobaComp.getScore();
        }
    }
}
