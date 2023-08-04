using ChessChallenge.API;
using System;
using System.Linq;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    int positionCount;
    int[][] pawnEvalWhite = new int[][] {
        new int[] { 0,   0,   0,   0,   0,   0,   0,   0 },
        new int[] { 78,  83,  86,  73, 102,  82,  85,  90 },
        new int[] { 7,  29,  21,  44,  40,  31,  44,   7 },
        new int[] { -17,  16,  -2,  15,  14,   0,  15, -13 },
        new int[] { -26,   3,  10,   9,   6,   1,   0, -23 },
        new int[] { -22,   9,   5, -11, -10,  -2,   3, -19 },
        new int[] { -31,   8,  -7, -37, -36, -14,   3, -31 },
        new int[] { 0,   0,   0,   0,   0,   0,   0,   0 }
    };

    int[][] knightEval = new int[][] {
        new int[] { -66, -53, -75, -75, -10, -55, -58, -70 },
        new int[] { -3,  -6, 100, -36,   4,  62,  -4, -14 },
        new int[] { 10,  67,   1,  74,  73,  27,  62,  -2 },
        new int[] { 24,  24,  45,  37,  33,  41,  25,  17 },
        new int[] { -1,   5,  31,  21,  22,  35,   2,   0 },
        new int[] { -18,  10,  13,  22,  18,  15,  11, -14 },
        new int[] { -23, -15,   2,   0,   2,   0, -23, -20 },
        new int[] { -74, -23, -26, -24, -19, -35, -22, -69 }
    };

    int[][] bishopEvalWhite = new int[][] {
        new int[] { -59, -78, -82, -76, -23, -107, -37, -50 },
        new int[] { -11,  20,  35, -42, -39,  31,   2, -22 },
        new int[] { -9,  39, -32,  41,  52, -10,  28, -14 },
        new int[] { 25,  17,  20,  34,  26,  25,  15,  10 },
        new int[] { 13,  10,  17,  23,  17,  16,   0,   7 },
        new int[] { 14,  25,  24,  15,   8,  25,  20,  15 },
        new int[] { 19,  20,  11,   6,   7,   6,  20,  16 },
        new int[] { -7,   2, -15, -12, -14, -15, -10, -10 }
    };


    int[][] rookEvalWhite = new int[][] {
        new int[] { 35,  29,  33,   4,  37,  33,  56,  50 },
        new int[] { 55,  29,  56,  67,  55,  62,  34,  60 },
        new int[] { 19,  35,  28,  33,  45,  27,  25,  15 },
        new int[] { 0,   5,  16,  13,  18,  -4,  -9,  -6 },
        new int[] { -28, -35, -16, -21, -13, -29, -46, -30 },
        new int[] { -42, -28, -42, -25, -25, -35, -26, -46 },
        new int[] { -53, -38, -31, -26, -29, -43, -44, -53 },
        new int[] { -30, -24, -18,   5,  -2, -18, -31, -32 }
    };


    int[][] evalQueen = new int[][] {
        new int[] { 6,   1,  -8,-104,  69,  24,  88,  26 },
        new int[] { 14,  32,  60, -10,  20,  76,  57,  24 },
        new int[] { -2,  43,  32,  60,  72,  63,  43,   2 },
        new int[] { 1, -16,  22,  17,  25,  20, -13,  -6 },
        new int[] { -14, -15,  -2,  -5,  -1, -10, -20, -22 },
        new int[] { -30,  -6, -13, -11, -16, -11, -16, -27 },
        new int[] { -36, -18,   0, -19, -15, -15, -21, -38 },
        new int[] { -39, -30, -31, -13, -31, -36, -34, -42 }
    };

    int[][] kingEvalWhite = new int[][] {
        new int[] { 4,  54,  47, -99, -99,  60,  83, -62 },
        new int[] { -32,  10,  55,  56,  56,  55,  10,   3 },
        new int[] { -62,  12, -57,  44, -67,  28,  37, -31 },
        new int[] { -55,  50,  11,  -4, -19,  13,   0, -49 },
        new int[] { -55, -43, -52, -28, -51, -47,  -8, -50 },
        new int[] { -47, -42, -43, -79, -64, -32, -29, -32 },
        new int[] { -4,   3, -14, -50, -57, -18,  13,   4 },
        new int[] { 17,  30,  -3, -14,   6,  -1,  40,  18 }
    };

    int[][] pawnEvalBlack;
    int[][] bishopEvalBlack;
    int[][] rookEvalBlack;
    int[][] kingEvalBlack;
    
    public MyBot() {
        // Reverse arrays
        pawnEvalBlack = ReverseArray(pawnEvalWhite);
        bishopEvalBlack = ReverseArray(bishopEvalWhite);
        rookEvalBlack = ReverseArray(rookEvalWhite);
        kingEvalBlack = ReverseArray(kingEvalWhite);
    }

    private static int[][] ReverseArray(int[][] input)
    {
        var rows = input.Length;
        var columns = input[0].Length;
        var reversed = new int[rows][]; 

        for (var row = 0; row < rows; row++)
        {
            reversed[row] = new int[columns];
            for (var column = 0; column < columns; column++)
                reversed[row][column] = input[rows - row - 1][column];
        }

        return reversed;
    }

    public T[] Shuffle<T>(T[] array) 
    {
        int currentIndex = array.Length;
        int randomIndex;
        T temporaryValue;
        Random rnd = new Random();
    
        while (0 != currentIndex) 
        {
        
            randomIndex = rnd.Next(currentIndex);
            currentIndex -= 1;
    
            
            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }
    
        return array;
    }

    public Move Think(Board board, Timer timer)
    {
        return MinimaxRoot(5, board, true);
    }

    public Move MinimaxRoot(int depth, Board game, bool isMaximisingPlayer)
    {
        var newGameMoves = game.GetLegalMoves(false);
        int bestMove = -9999;
        Move bestMoveFound = new Move();

        foreach(var newGameMove in newGameMoves)
        {
            game.MakeMove(newGameMove);
            var value = Minimax(depth - 1, game, -10000, 10000, !isMaximisingPlayer);
            game.UndoMove(newGameMove);
            if(value >= bestMove)
            {
                bestMove = value;
                bestMoveFound = newGameMove;
            }
        }
        return bestMoveFound;
    }

    public int Minimax(int depth, Board game, int alpha, int beta, bool isMaximisingPlayer)
    {
        positionCount++;
        if (depth == 0)
        {
            return -EvaluateBoard(game);
        }

        Move[] newGameMoves = Shuffle<Move>(game.GetLegalMoves(false));


        if (isMaximisingPlayer)
        {
            int bestMove = -9999;
            foreach(var move in newGameMoves)
            {
                game.MakeMove(move);
                bestMove = Math.Max(bestMove, Minimax(depth - 1, game, alpha, beta, !isMaximisingPlayer));
                game.UndoMove(move);
                alpha = Math.Max(alpha, bestMove);
                if (beta <= alpha)
                {
                    return bestMove;
                }
            }
            return bestMove;
        }
        else
        {
            int bestMove = 9999;
            foreach(var move in newGameMoves)
            {
                game.MakeMove(move);
                bestMove = Math.Min(bestMove, Minimax(depth - 1, game, alpha, beta, !isMaximisingPlayer));
                game.UndoMove(move);
                beta = Math.Min(beta, bestMove);
                if (beta <= alpha)
                {
                    return bestMove;
                }
            }
            return bestMove;
        }
    }

    private int EvaluateBoard(Board board)
    {
        int totalEvaluation = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                totalEvaluation = totalEvaluation + GetPieceValue(board.GetPiece(new Square(i ,j)), i, j, board);
            }
        }
        return totalEvaluation;
    }

    private int GetPieceValue(Piece piece, int x, int y, Board board)
    {
        if (piece.PieceType == PieceType.None)
        {
            return 0;
        }
        int absoluteValue = GetAbsoluteValue(piece, piece.IsWhite, x, y, board);
        return piece.IsWhite ? absoluteValue : -absoluteValue;
    }

    private int GetAbsoluteValue(Piece piece, bool isWhite, int x, int y, Board board)
    {
        int value = 0;
        switch (piece.PieceType)
        {
            case PieceType.Pawn:
                value = 100 + (isWhite ? pawnEvalWhite[y][x] : pawnEvalBlack[y][x]);
                break;
            case PieceType.Rook:
                value = board.GetLegalMoves(false).Length < 15 ? 0 : 479 + (isWhite ? rookEvalWhite[y][x] : rookEvalBlack[y][x]);
                break;
            case PieceType.Knight:
                value = 280 + knightEval[y][x];
                break;
            case PieceType.Bishop:
                value = 320 + (isWhite ? bishopEvalWhite[y][x] : bishopEvalBlack[y][x]);
                break;
            case PieceType.Queen:
                value = 929 + evalQueen[y][x];
                break;
            case PieceType.King:
                value = 60000 + (isWhite ? kingEvalWhite[y][x] : kingEvalBlack[y][x]);
                break;
            default:
                throw new Exception("Unknown piece type: " + piece.PieceType);
        }
        return value;
    }
}