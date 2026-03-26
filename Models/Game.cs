namespace TicTacToeApi.Models;

public class Game
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Board { get; set; } = "---------";
    public string CurrentPlayer { get; set; } = "X";
    public GameStatus Status { get; set; } = GameStatus.InProgress;
    public string? Winner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Move> Moves { get; set; } = new();
}