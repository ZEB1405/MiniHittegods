using Microsoft.AspNetCore.Mvc;
using MiniHittegods.Application;

namespace MiniHittegods.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly FoundItemService _foundItemService;

    public ItemsController(FoundItemService foundItemService)
    {
        _foundItemService = foundItemService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetItemByIdAsync(Guid id)
    {
        try
        {
            var item = await _foundItemService.GetItemByIdAsync(id);
            return Ok(item);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}