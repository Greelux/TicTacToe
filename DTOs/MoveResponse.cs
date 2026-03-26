using TicTacToeApi.Models;

namespace TicTacToeApi.DTOs;

public class MoveResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Board { get; set; } = string.Empty;
    public string CurrentPlayer { get; set; } = string.Empty;
    public GameStatus Status { get; set; }
    public string? Winner { get; set; }
}