using HomeOrganizer.Api.Data;
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
        public ActionResult<IEnumerable<ItemDto>> GetItems()
        {
            return Ok(dbContext.Items);
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(int id)
        {
            var item = dbContext.Items.FirstOrDefault(i => i.Id == id);

            return item is not null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto newItem)
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
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetItem), new { id = newItemInstance.Id }, newItemInstance);
        }
    }
}
