using TicTacToeApi.DTOs;

namespace TicTacToeApi.Services;

public interface IGameService
{
    Task<CreateGameResponse> CreateGameAsync();
    Task<MoveResponse> MakeMoveAsync(string code, MoveRequest request);
    Task<GameStatusResponse?> GetGameStatusAsync(string code);
    Task<List<GameStatusResponse>> GetHistoryAsync();
}