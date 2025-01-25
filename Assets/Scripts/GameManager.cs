using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public StockSimulation stockSim { get; private set; }
    public StockChaosSim chaosSim { get; private set; }
    public BobaStockManager bobaStockManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        stockSim = GetComponent<StockSimulation>();
        chaosSim = GetComponent<StockChaosSim>();
        bobaStockManager = GetComponent<BobaStockManager>();
    }
}
