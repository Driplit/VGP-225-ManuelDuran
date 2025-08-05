using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private float aiDelay = 0.5f;

    private void Update()
    {
        if (gameController.whoTurn == 1 && !gameController.winnerText.gameObject.activeSelf)
        {
            Invoke(nameof(PlayBestMove), aiDelay);
        }
    }

    private void PlayBestMove()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (gameController.markedSpaces[i] == -100)
            {
                gameController.markedSpaces[i] = 2; // AI = O = player 1
                int score = Minimax(gameController.markedSpaces, 0, false);
                gameController.markedSpaces[i] = -100;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        if (bestMove != -1)
        {
            gameController.TicTacToeButton(bestMove);
        }
    }

    private int Minimax(int[] board, int depth, bool isMaximizing)
    {
        int result = CheckWinner(board);
        if (result != 0)
        {
            return result;
        }

        if (IsBoardFull(board))
        {
            return 0; // Tie
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
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
            for (int i = 0; i < 9; i++)
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

    private int CheckWinner(int[] board)
    {
        int[][] winPatterns = new int[][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 0, 3, 6 },
            new int[] { 1, 4, 7 },
            new int[] { 2, 5, 8 },
            new int[] { 0, 4, 8 },
            new int[] { 2, 4, 6 },
        };

        foreach (int[] pattern in winPatterns)
        {
            int sum = board[pattern[0]] + board[pattern[1]] + board[pattern[2]];
            if (sum == 3) return -10; // Human (1) wins
            if (sum == 6) return 10;  // AI (2) wins
        }

        return 0; // No winner
    }

    private bool IsBoardFull(int[] board)
    {
        foreach (int spot in board)
        {
            if (spot == -100)
                return false;
        }
        return true;
    }
}
