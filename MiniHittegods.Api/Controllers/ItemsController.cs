using Microsoft.AspNetCore.Mvc;
using MiniHittegods.Application;
using MiniHittegods.Api.Models;
using MiniHittegods.Domain;

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

    [HttpPost]
    public async Task<IActionResult> CreateItem(
    [FromBody] CreateItemRequest request)
    {
        try
        {
            var item = await _foundItemService.CreateItemAsync(
                request.Title,
                request.FoundLocation,
                request.Description,
                request.Category);

            return Created($"/api/items/{item.Id}", item);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems(
        [FromQuery] FoundItemStatus? status = null,
        [FromQuery] string? category = null,
        [FromQuery] string? q = null)
    
    {
        var items = await _foundItemService.GetAllItemsAsync(
            status,
            category,
            q
        );
        return Ok(items);
    }
}