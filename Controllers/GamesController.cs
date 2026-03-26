using Microsoft.AspNetCore.Mvc;
using TicTacToeApi.DTOs;
using TicTacToeApi.Services;

namespace TicTacToeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateGameResponse>> CreateGame()
    {
        var result = await _gameService.CreateGameAsync();
        return Ok(result);
    }

    [HttpPost("{code}/move")]
    public async Task<ActionResult<MoveResponse>> MakeMove(string code, [FromBody] MoveRequest request)
    {
        var result = await _gameService.MakeMoveAsync(code, request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<GameStatusResponse>> GetStatus(string code)
    {
        var result = await _gameService.GetGameStatusAsync(code);

        if (result == null)
            return NotFound(new { message = "Гру не знайдено." });

        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<GameStatusResponse>>> GetHistory()
    {
        var result = await _gameService.GetHistoryAsync();
        return Ok(result);
    }
}