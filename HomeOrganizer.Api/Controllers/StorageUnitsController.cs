using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using HomeOrganizer.Api.Data;
using HomeOrganizer.Api.Entities;

namespace HomeOrganizer.Api; 


[Route("homes/{homeId}/rooms/{roomId}/storage-units")]
[ApiController]
public class StorageUnitsController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public StorageUnitsController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /*

    GET    /homes/{homeId}/rooms/{roomId}/storage-units
    POST   /homes/{homeId}/rooms/{roomId}/storage-units
    GET    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}
    PUT    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}

    DELETE /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId} 
    */

    //GET    /homes/{homeId}/rooms/{roomId}/storage-units

    [HttpGet]
    public async Task<ActionResult<List<StorageUnitDto>>> GetStorageUnits(int homeId, int roomId)
    {
        var roomExists = await dbContext.Rooms.AnyAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (!roomExists)
        {
            return NotFound("Room not found.");
        }

        var storageUnits = await dbContext.StorageUnits
                .Where(storageUnit => storageUnit.RoomId == roomId)
                .Select(storageUnit => new StorageUnitDto(
                    storageUnit.Id,
                    storageUnit.RoomId,
                    storageUnit.Name,
                    storageUnit.Type,
                    storageUnit.PositionX,
                    storageUnit.PositionY,
                    storageUnit.Width,
                    storageUnit.Height))
                .ToListAsync();

            return Ok(storageUnits);
    }

    // GET    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}
    [HttpGet("{storageUnitId}")]
    public async Task<ActionResult<StorageUnitDto>> GetStorageUnit(int homeId, int roomId, int storageUnitId)
    {
        var roomExists = await dbContext.Rooms.AnyAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (!roomExists)
        {
            return NotFound("Room not found.");
        }

        var storageUnit = await dbContext.StorageUnits
            .Where(su => su.Id == storageUnitId && su.RoomId == roomId)
            .Select(su => new StorageUnitDto(
                su.Id,
                su.RoomId,
                su.Name,
                su.Type,
                su.PositionX,
                su.PositionY,
                su.Width,
                su.Height))
            .FirstOrDefaultAsync();

        if (storageUnit == null)
        {
            return NotFound("Storage unit not found.");
        }

        return Ok(storageUnit);
    }

    // POST   /homes/{homeId}/rooms/{roomId}/storage-units
    [HttpPost]
    public async Task<ActionResult<StorageUnitDto>> CreateStorageUnit(int homeId, int roomId, CreateStorageUnitDto createStorageUnitDto)
    {
        if (string.IsNullOrWhiteSpace(createStorageUnitDto.Name))
        {
            return BadRequest("Name is required.");
        }
        if (createStorageUnitDto.Width <= 0 || createStorageUnitDto.Height <= 0)
        {
            return BadRequest("Width and height must be greater than zero.");
        }

        var roomExists = await dbContext.Rooms.AnyAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (!roomExists)
        {
            return NotFound("Room not found.");
        }

        var newStorageUnit = new StorageUnit
        {
            RoomId = roomId,
            Name = createStorageUnitDto.Name,
            Type = createStorageUnitDto.Type,
            PositionX = createStorageUnitDto.PositionX,
            PositionY = createStorageUnitDto.PositionY,
            Width = createStorageUnitDto.Width,
            Height = createStorageUnitDto.Height
        };

        dbContext.StorageUnits.Add(newStorageUnit);
        await dbContext.SaveChangesAsync();

        var storageUnitDto = new StorageUnitDto(
            newStorageUnit.Id,
            newStorageUnit.RoomId,
            newStorageUnit.Name,
            newStorageUnit.Type,
            newStorageUnit.PositionX,
            newStorageUnit.PositionY,
            newStorageUnit.Width,
            newStorageUnit.Height);

        return CreatedAtAction(nameof(GetStorageUnit), new { homeId, roomId, storageUnitId = storageUnitDto.Id }, storageUnitDto);
    }

    //PUT    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}

    [HttpPut("{storageUnitId}")]
    public async Task<ActionResult<StorageUnitDto>> UpdateStorageUnit(int homeId, int roomId, int storageUnitId, UpdateStorageUnitDto updateStorageUnitDto)
    {
        if (string.IsNullOrWhiteSpace(updateStorageUnitDto.Name))
        {
            return BadRequest("Name is required.");
        }
        if (updateStorageUnitDto.Width <= 0 || updateStorageUnitDto.Height <= 0)
        {
            return BadRequest("Width and height must be greater than zero.");
        }

        var roomExists = await dbContext.Rooms.AnyAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (!roomExists)
        {
            return NotFound("Room not found.");
        }

        var existingStorageUnit = await dbContext.StorageUnits
            .FirstOrDefaultAsync(su => su.Id == storageUnitId && su.RoomId == roomId);

        if (existingStorageUnit is null)
        {
            return NotFound("Storage unit not found.");
        }

        existingStorageUnit.Name = updateStorageUnitDto.Name;
        existingStorageUnit.Type = updateStorageUnitDto.Type;
        existingStorageUnit.PositionX = updateStorageUnitDto.PositionX;
        existingStorageUnit.PositionY = updateStorageUnitDto.PositionY;
        existingStorageUnit.Width = updateStorageUnitDto.Width;
        existingStorageUnit.Height = updateStorageUnitDto.Height;

        await dbContext.SaveChangesAsync();

        var storageUnitDto = new StorageUnitDto(
            existingStorageUnit.Id,
            existingStorageUnit.RoomId,
            existingStorageUnit.Name,
            existingStorageUnit.Type,
            existingStorageUnit.PositionX,
            existingStorageUnit.PositionY,
            existingStorageUnit.Width,
            existingStorageUnit.Height);

        return Ok(storageUnitDto);
    }

    //DELETE /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId} 
    [HttpDelete("{storageUnitId}")]
    public async Task<IActionResult> DeleteStorageUnit(int homeId, int roomId, int storageUnitId)
    {
        var roomExists = await dbContext.Rooms.AnyAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (!roomExists)
        {
            return NotFound("Room not found.");
        }

        var existingStorageUnit = await dbContext.StorageUnits
            .FirstOrDefaultAsync(su => su.Id == storageUnitId && su.RoomId == roomId);

        if (existingStorageUnit is null)
        {
            return NotFound("Storage unit not found.");
        }

        dbContext.StorageUnits.Remove(existingStorageUnit);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

}
