using TicTacToeApi.Models;

namespace TicTacToeApi.DTOs;

public class CreateGameResponse
{
    public string Code { get; set; } = string.Empty;
    public string Board { get; set; } = string.Empty;
    public string CurrentPlayer { get; set; } = string.Empty;
    public GameStatus Status { get; set; }
}