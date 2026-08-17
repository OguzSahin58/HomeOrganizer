using HomeOrganizer.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using HomeOrganizer.Api.Entities;

namespace HomeOrganizer.Api
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        
        private readonly ApplicationDbContext dbContext;

        public ItemsController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> GetItems()
        {
            var items = await dbContext.Items
                .Select(item => new ItemDto(item.Id, item.Name, item.Description, item.LastModifiedDate))
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await dbContext.Items.FirstOrDefaultAsync(i => i.Id == id);

            return item is not null
                ? Ok(new ItemDto(item.Id, item.Name, item.Description, item.LastModifiedDate))
                : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto newItem)
        {
            if (string.IsNullOrWhiteSpace(newItem.Name))
            {
                return BadRequest("Name is required.");
            }
            if (string.IsNullOrWhiteSpace(newItem.Description))
            {
                return BadRequest("Description is required.");
            }
            if (newItem.LastModifiedDate > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("LastModifiedDate cannot be in the future.");
            }
            var newItemInstance = new Item
            {
                Name = newItem.Name,
                Description = newItem.Description,
                LastModifiedDate = newItem.LastModifiedDate
            };

            dbContext.Add(newItemInstance);
            await dbContext.SaveChangesAsync();

            var itemDto = new ItemDto(newItemInstance.Id, newItemInstance.Name, newItemInstance.Description, newItemInstance.LastModifiedDate);

            return CreatedAtAction(nameof(GetItem), new { id = itemDto.Id }, itemDto);
        }
    }
}
