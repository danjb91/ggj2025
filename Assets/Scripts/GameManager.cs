using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    START,
    PREP,
    PLAY,
    QUARTER_END,
    GAME_END
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState _gameState = GameState.START;
    public GameState gameState
    {
        get => _gameState;
        private set
        {
            _gameState = value;
        }
    }
    public const float quarterLength = 60f;
    public const float quarterBreakLength = 10f;
    public const float prepLength = 5f;
    public const float resultLength = 10f;
    public int currentQuarter { get; private set; }

    public int playerWinner { get; private set; } = 1;

    public float timeLeft { get; private set; } = 0f;

    public StockSimulation stockSim { get; set; }
    public StockChaosSim chaosSim { get; set; }
    public BobaStockManager bobaStockManager { get; set; }

    public void GoToNextState()
    {
        if (timeLeft > 0f)
        {
            return;
        }
        switch (gameState)
        {
            case GameState.START:
                gameState = GameState.PREP;
                timeLeft = prepLength;
                break;
            case GameState.PREP:
                gameState = GameState.PLAY;
                timeLeft = quarterLength;
                break;
            case GameState.PLAY:
                gameState = GameState.QUARTER_END;
                timeLeft = quarterBreakLength;
                break;
            case GameState.QUARTER_END:
                {
                    if (currentQuarter == 4)
                    {
                        gameState = GameState.GAME_END;
                        timeLeft = resultLength;
                        return;
                    }
                    gameState = GameState.PLAY;
                    break;
                }
            case GameState.GAME_END:
                gameState = GameState.START;
                break;
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
        }
        else
        {
            GoToNextState();
        }
    }

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
    }
}
