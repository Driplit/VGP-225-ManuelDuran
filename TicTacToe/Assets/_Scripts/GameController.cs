using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int whoTurn; // 0 == X (Player), 1 == O (AI)
    public int turnCount;
    public GameObject[] turnIcons; // Turn indicators
    public Sprite[] playerIcons; // 0 = X, 1 = O
    public Button[] ticTacToeSpaces;
    public int[] markedSpaces;
    public TextMeshProUGUI winnerText;
    public GameObject[] winningLines;
    public bool vsAI;

    public bool IsAITurn => vsAI && whoTurn == 1;

    void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        whoTurn = 0;
        turnCount = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);

        for (int i = 0; i < ticTacToeSpaces.Length; i++)
        {
            ticTacToeSpaces[i].interactable = true;
            ticTacToeSpaces[i].GetComponent<Image>().sprite = null;
        }

        for (int i = 0; i < markedSpaces.Length; i++)
        {
            markedSpaces[i] = -100;
        }

        winnerText.gameObject.SetActive(false);
        foreach (GameObject line in winningLines)
        {
            line.SetActive(false);
        }
    }

    void Update()
    {
        if (IsAITurn)
        {
            MakeAIMove();
        }
    }

    public void TicTacToeButton(int whichNumber)
    {
        if (!ticTacToeSpaces[whichNumber].interactable) return;

        MakeMove(whichNumber, whoTurn);
        ProcessPostMove();
    }

    void MakeMove(int index, int player)
    {
        ticTacToeSpaces[index].image.sprite = playerIcons[player];
        ticTacToeSpaces[index].interactable = false;
        markedSpaces[index] = player + 1;
        turnCount++;
    }

    void ProcessPostMove()
    {
        if (turnCount > 4)
        {
            bool isWinner = WinnerCheck();
            if (turnCount == 9 && !isWinner)
            {
                Cat();
                return;
            }

            if (isWinner) return;
        }

        SwitchTurn();
    }

    void SwitchTurn()
    {
        whoTurn = 1 - whoTurn;
        turnIcons[0].SetActive(whoTurn == 0);
        turnIcons[1].SetActive(whoTurn == 1);
    }

    void MakeAIMove()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < markedSpaces.Length; i++)
        {
            if (markedSpaces[i] == -100)
            {
                markedSpaces[i] = 2;
                int score = Minimax(markedSpaces, 0, false);
                markedSpaces[i] = -100;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        if (bestMove != -1)
        {
            MakeMove(bestMove, 1);
            ProcessPostMove();
        }
    }

    int Minimax(int[] board, int depth, bool isMaximizing)
    {
        int result = Evaluate(board);
        if (result != 0) return result;
        if (IsBoardFull(board)) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -100)
                {
                    board[i] = 2;
                    int score = Minimax(board, depth + 1, false);
                    board[i] = -100;
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -100)
                {
                    board[i] = 1;
                    int score = Minimax(board, depth + 1, true);
                    board[i] = -100;
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    int Evaluate(int[] board)
    {
        int[,] winPatterns = new int[,]
        {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (board[a] == board[b] && board[b] == board[c])
            {
                if (board[a] == 2) return 10;  // AI wins
                if (board[a] == 1) return -10; // Player wins
            }
        }

        return 0;
    }

    bool IsBoardFull(int[] board)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == -100) return false;
        }
        return true;
    }

    bool WinnerCheck()
    {
        int[,] winPatterns = new int[,]
        {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (markedSpaces[a] == markedSpaces[b] &&
                markedSpaces[b] == markedSpaces[c] &&
                markedSpaces[a] != -100)
            {
                WinnerDisplay(i);
                return true;
            }
        }
        return false;
    }

    void WinnerDisplay(int indexIn)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = whoTurn == 0 ? "Player X Wins!" : "Player O Wins!";
        winningLines[indexIn].SetActive(true);

        foreach (Button space in ticTacToeSpaces)
        {
            space.interactable = false;
        }
    }

    public void ResetGame()
    {
        GameSetup();
    }

    void Cat()
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = "Cat Game!";
        foreach (Button space in ticTacToeSpaces)
        {
            space.interactable = false;
        }
    }
}
