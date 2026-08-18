using HomeOrganizer.Api.Data;
using HomeOrganizer.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeOrganizer.Api;

[Route("homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public ItemsController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET: /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items
    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetItems(int homeId, int roomId, int storageUnitId)
    {
        var storageUnitExists = await StorageUnitExists(homeId, roomId, storageUnitId);

        if (!storageUnitExists)
        {
            return NotFound("Storage unit not found.");
        }

        var items = await dbContext.Items
            .Where(item => item.StorageUnitId == storageUnitId)
            .Select(item => new ItemDto(item.Id, item.StorageUnitId, item.Name, item.Description, item.Quantity))
            .ToListAsync();

        return Ok(items);
    }

    // POST: /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(int homeId, int roomId, int storageUnitId, CreateItemDto newItem)
    {
        if (string.IsNullOrWhiteSpace(newItem.Name))
        {
            return BadRequest("Name is required.");
        }
        if (newItem.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        var storageUnitExists = await StorageUnitExists(homeId, roomId, storageUnitId);

        if (!storageUnitExists)
        {
            return NotFound("Storage unit not found.");
        }

        var item = new Item
        {
            StorageUnitId = storageUnitId,
            Name = newItem.Name,
            Description = newItem.Description,
            Quantity = newItem.Quantity
        };

        dbContext.Items.Add(item);
        await dbContext.SaveChangesAsync();

        var itemDto = new ItemDto(item.Id, item.StorageUnitId, item.Name, item.Description, item.Quantity);

        return CreatedAtAction(nameof(GetItem), new { itemId = itemDto.Id }, itemDto);
    }

    // GET: /items/{itemId}
    [HttpGet("/items/{itemId}")]
    public async Task<ActionResult<ItemDto>> GetItem(int itemId)
    {
        var item = await dbContext.Items
            .Where(item => item.Id == itemId)
            .Select(item => new ItemDto(item.Id, item.StorageUnitId, item.Name, item.Description, item.Quantity))
            .FirstOrDefaultAsync();

        return item is not null ? Ok(item) : NotFound();
    }

    // PUT: /items/{itemId}
    [HttpPut("/items/{itemId}")]
    public async Task<ActionResult<ItemDto>> UpdateItem(int itemId, UpdateItemDto updatedItem)
    {
        if (string.IsNullOrWhiteSpace(updatedItem.Name))
        {
            return BadRequest("Name is required.");
        }
        if (updatedItem.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        var item = await dbContext.Items.FindAsync(itemId);

        if (item is null)
        {
            return NotFound();
        }

        item.Name = updatedItem.Name;
        item.Description = updatedItem.Description;
        item.Quantity = updatedItem.Quantity;

        await dbContext.SaveChangesAsync();

        return Ok(new ItemDto(item.Id, item.StorageUnitId, item.Name, item.Description, item.Quantity));
    }

    // DELETE: /items/{itemId}
    [HttpDelete("/items/{itemId}")]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        var item = await dbContext.Items.FindAsync(itemId);

        if (item is null)
        {
            return NotFound();
        }

        dbContext.Items.Remove(item);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> StorageUnitExists(int homeId, int roomId, int storageUnitId)
    {
        return await dbContext.StorageUnits
            .AnyAsync(storageUnit =>
                storageUnit.Id == storageUnitId &&
                storageUnit.RoomId == roomId &&
                storageUnit.Room.HomeId == homeId);
    }
}
