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
}