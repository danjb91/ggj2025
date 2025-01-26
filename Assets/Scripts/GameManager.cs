using Unity.VisualScripting;
using TMPro;
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

    public string CurrentStateStr;

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

    public int player1Wins { get; private set; } = 0;
    public int player2Wins { get; private set; } = 0;

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
                gameState = GameState.GAME_END;
                timeLeft = resultLength;

                int p1Value = stockSim.GetTotalPortfolio(1);
                int p2Value = stockSim.GetTotalPortfolio(2);
                playerWinner = p1Value > p2Value ? 1 : 2;
                if(p1Value == p2Value)
                    playerWinner = 3;
                switch (playerWinner)
                {
                    case 1:
                        player1Wins++;
                        break;
                    case 2:
                        player2Wins++;
                        break;
                }
                //gameState = GameState.QUARTER_END;
                //timeLeft = quarterBreakLength;
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
                stockSim.ResetGame();
                bobaStockManager.ResetGame();
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

 
        string text = "";
        switch(gameState){
            case GameState.PREP:
                text = "Get Ready!";
                break;
            case GameState.PLAY:
                text = "";
                break;
            case GameState.GAME_END:
                switch(playerWinner)
                {
                    case 1:
                        text = "Player 1 Won!";
                        break;
                    case 2:
                        text = "Player 2 Won!";
                        break;
                    case 3:
                        text = "Tie!";
                        break;
                }
                break;

        }
        text += $"\n{timeLeft:0.0}";

        CurrentStateStr = text;
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
