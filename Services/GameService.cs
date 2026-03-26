using Microsoft.EntityFrameworkCore;
using TicTacToeApi.Data;
using TicTacToeApi.DTOs;
using TicTacToeApi.Models;

namespace TicTacToeApi.Services;

public class GameService : IGameService
{
    private readonly GameDbContext _context;
    private readonly Random _random = new();

    public GameService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<CreateGameResponse> CreateGameAsync()
    {
        var code = await GenerateUniqueCodeAsync();

        var game = new Game
        {
            Code = code,
            Board = "---------",
            CurrentPlayer = "X",
            Status = GameStatus.InProgress,
            Winner = null,
            CreatedAt = DateTime.UtcNow
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return new CreateGameResponse
        {
            Code = game.Code,
            Board = game.Board,
            CurrentPlayer = game.CurrentPlayer,
            Status = game.Status
        };
    }

    public async Task<MoveResponse> MakeMoveAsync(string code, MoveRequest request)
    {
        var game = await _context.Games
            .Include(g => g.Moves)
            .FirstOrDefaultAsync(g => g.Code == code);

        if (game == null)
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Гру не знайдено."
            };
        }

        if (game.Status != GameStatus.InProgress)
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Гру вже завершено.",
                Code = game.Code,
                Board = game.Board,
                CurrentPlayer = game.CurrentPlayer,
                Status = game.Status,
                Winner = game.Winner
            };
        }

        if (request.Player != "X" && request.Player != "O")
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Гравець має бути X або O."
            };
        }

        if (request.Player != game.CurrentPlayer)
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Зараз не хід цього гравця.",
                Code = game.Code,
                Board = game.Board,
                CurrentPlayer = game.CurrentPlayer,
                Status = game.Status,
                Winner = game.Winner
            };
        }

        if (request.Cell < 0 || request.Cell > 8)
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Клітинка повинна бути в діапазоні від 0 до 8."
            };
        }

        var board = game.Board.ToCharArray();

        if (board[request.Cell] != '-')
        {
            return new MoveResponse
            {
                Success = false,
                Message = "Клітинка вже зайнята.",
                Code = game.Code,
                Board = game.Board,
                CurrentPlayer = game.CurrentPlayer,
                Status = game.Status,
                Winner = game.Winner
            };
        }

        board[request.Cell] = request.Player[0];
        game.Board = new string(board);

        game.Moves.Add(new Move
        {
            GameId = game.Id,
            Player = request.Player,
            Cell = request.Cell,
            CreatedAt = DateTime.UtcNow
        });

        if (CheckWin(game.Board, request.Player[0]))
        {
            game.Status = request.Player == "X" ? GameStatus.XWon : GameStatus.OWon;
            game.Winner = request.Player;
        }
        else if (!game.Board.Contains('-'))
        {
            game.Status = GameStatus.Draw;
            game.Winner = null;
        }
        else
        {
            game.CurrentPlayer = request.Player == "X" ? "O" : "X";
        }

        await _context.SaveChangesAsync();

        return new MoveResponse
        {
            Success = true,
            Message = "Хід виконано успішно.",
            Code = game.Code,
            Board = game.Board,
            CurrentPlayer = game.CurrentPlayer,
            Status = game.Status,
            Winner = game.Winner
        };
    }

    public async Task<GameStatusResponse?> GetGameStatusAsync(string code)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Code == code);

        if (game == null)
            return null;

        return new GameStatusResponse
        {
            Code = game.Code,
            Board = game.Board,
            CurrentPlayer = game.CurrentPlayer,
            Status = game.Status,
            Winner = game.Winner,
            CreatedAt = game.CreatedAt
        };
    }

    public async Task<List<GameStatusResponse>> GetHistoryAsync()
    {
        return await _context.Games
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => new GameStatusResponse
            {
                Code = g.Code,
                Board = g.Board,
                CurrentPlayer = g.CurrentPlayer,
                Status = g.Status,
                Winner = g.Winner,
                CreatedAt = g.CreatedAt
            })
            .ToListAsync();
    }

    private async Task<string> GenerateUniqueCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        while (true)
        {
            var code = new string(Enumerable.Range(0, 8)
                .Select(_ => chars[_random.Next(chars.Length)])
                .ToArray());

            var exists = await _context.Games.AnyAsync(g => g.Code == code);
            if (!exists)
                return code;
        }
    }

    private bool CheckWin(string board, char player)
    {
        int[][] wins =
        [
            [0, 1, 2],
            [3, 4, 5],
            [6, 7, 8],
            [0, 3, 6],
            [1, 4, 7],
            [2, 5, 8],
            [0, 4, 8],
            [2, 4, 6]
        ];

        return wins.Any(w =>
            board[w[0]] == player &&
            board[w[1]] == player &&
            board[w[2]] == player);
    }
}