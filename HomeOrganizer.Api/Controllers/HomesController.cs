using HomeOrganizer.Api.Data;
using HomeOrganizer.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace HomeOrganizer.Api; 

[Route("homes")]
[ApiController]
public class HomesController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public HomesController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    //
    // GET: /homes
    //
    [HttpGet]
    public async Task<ActionResult<List<HomeDto>>> GetHomes()
    {
        var homes = await dbContext.Homes
            .Select(home => new HomeDto(home.Id, home.Name, home.Description))
            .ToListAsync();

        return Ok(homes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HomeDto>> GetHome(int id)
    {
        var home = await dbContext.Homes.FindAsync(id);

        if (home is null)
        {
            return NotFound();
        }

        return Ok(new HomeDto(home.Id, home.Name, home.Description));
    }

    //
    // POST: /homes
    //
    [HttpPost]
    public async Task<ActionResult<HomeDto>> CreateHome(CreateHomeDto home)
    {
        if (string.IsNullOrWhiteSpace(home.Name))
        {
            return BadRequest("Name is required.");
        }

        var newHome = new Home
        {
            Name = home.Name,
            Description = home.Description
        };

        dbContext.Homes.Add(newHome);
        await dbContext.SaveChangesAsync();

        var homeDto = new HomeDto(newHome.Id, newHome.Name, newHome.Description);

        return CreatedAtAction(nameof(GetHome), new { id = homeDto.Id }, homeDto);
    }

    //
    // PUT: /homes/{id}
    //
    [HttpPut("{id}")]
    public async Task<ActionResult<HomeDto>> UpdateHome(int id, UpdateHomeDto home)
    {
        if (string.IsNullOrWhiteSpace(home.Name))
        {
            return BadRequest("Name is required.");
        }

        var existingHome = await dbContext.Homes.FindAsync(id);

        if (existingHome is null)
        {
            return NotFound();
        }

        existingHome.Name = home.Name;
        existingHome.Description = home.Description;

        await dbContext.SaveChangesAsync();

        return Ok(new HomeDto(existingHome.Id, existingHome.Name, existingHome.Description));
    }

    //
    // DELETE: /homes/{id}
    //
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHome(int id)
    {
        var home = await dbContext.Homes.FindAsync(id);

        if (home is null)
        {
            return NotFound();
        }

        dbContext.Homes.Remove(home);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
