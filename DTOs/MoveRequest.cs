namespace TicTacToeApi.DTOs;

public class MoveRequest
{
    public string Player { get; set; } = string.Empty;
    public int Cell { get; set; }
}