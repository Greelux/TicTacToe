namespace TicTacToeApi.Models;

public class Move
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game? Game { get; set; }

    public string Player { get; set; } = string.Empty;
    public int Cell { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}