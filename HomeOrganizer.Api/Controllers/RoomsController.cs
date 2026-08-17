using HomeOrganizer.Api.Data;
using HomeOrganizer.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace HomeOrganizer.Api; 

[Route("homes/{homeId}/rooms")]
[ApiController]

public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public RoomsController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    //
    // GET: api/homes/{homeId}/rooms
    //
    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetRooms(int homeId)
    {
        var rooms = await dbContext.Rooms
            .Where(room => room.HomeId == homeId)
            .Select(room => new RoomDto(room.Id, room.HomeId, room.Name, room.PositionX, room.PositionY, room.Width, room.Height))
            .ToListAsync();

        return Ok(rooms);
    }

    //
    // POST: api/homes/{homeId}/rooms
    //
    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(int homeId, CreateRoomDto room)
    {
        if (string.IsNullOrWhiteSpace(room.Name))
        {
            return BadRequest("Name is required.");
        }
        if (room.Width <= 0 || room.Height <= 0)
        {
            return BadRequest("Width and height must be greater than zero.");
        }

        var newRoom = new Room
        {
            HomeId = homeId,
            Name = room.Name,
            PositionX = room.PositionX,
            PositionY = room.PositionY,
            Width = room.Width,
            Height = room.Height
        };

        dbContext.Rooms.Add(newRoom);
        await dbContext.SaveChangesAsync();

        var createdRoomDto = new RoomDto(newRoom.Id, newRoom.HomeId, newRoom.Name, newRoom.PositionX, newRoom.PositionY, newRoom.Width, newRoom.Height);

        return CreatedAtAction(nameof(GetRooms), new { homeId = homeId }, createdRoomDto);
    }

    //
    // PUT: api/homes/{homeId}/rooms/{roomId}
    //
    [HttpPut("{roomId}")]
    public async Task<ActionResult<RoomDto>> UpdateRoom(int homeId, int roomId, UpdateRoomDto room)
    {
        if (string.IsNullOrWhiteSpace(room.Name))
        {
            return BadRequest("Name is required.");
        }
        if (room.Width <= 0 || room.Height <= 0)
        {
            return BadRequest("Width and height must be greater than zero.");
        }

        var existingRoom = await dbContext.Rooms
            .FirstOrDefaultAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (existingRoom is null)
        {
            return NotFound();
        }

        existingRoom.Name = room.Name;
        existingRoom.PositionX = room.PositionX;
        existingRoom.PositionY = room.PositionY;
        existingRoom.Width = room.Width;
        existingRoom.Height = room.Height;

        await dbContext.SaveChangesAsync();

        return Ok(new RoomDto(existingRoom.Id, existingRoom.HomeId, existingRoom.Name, existingRoom.PositionX, existingRoom.PositionY, existingRoom.Width, existingRoom.Height));
    }

    //
    // DELETE: api/homes/{homeId}/rooms/{roomId}
    //
    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoom(int homeId, int roomId)
    {
        var room = await dbContext.Rooms
            .FirstOrDefaultAsync(room => room.Id == roomId && room.HomeId == homeId);

        if (room is null)
        {
            return NotFound();
        }

        dbContext.Rooms.Remove(room);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
