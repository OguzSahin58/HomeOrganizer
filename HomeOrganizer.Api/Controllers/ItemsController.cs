using Microsoft.AspNetCore.Mvc;

namespace HomeOrganizer.Api
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> Items = new()
        {
            new ItemDto(1, "Bavul", "Item container", DateOnly.FromDateTime(DateTime.Now)),
            new ItemDto(2, "Canta", "Item for carrying belongings", DateOnly.FromDateTime(DateTime.Now)),
            new ItemDto(3, "Ayakkabi", "Footwear", DateOnly.FromDateTime(DateTime.Now))
        };

        [HttpGet]
        public ActionResult<IEnumerable<ItemDto>> GetItems()
        {
            return Ok(Items);
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(int id)
        {
            var item = Items.Find(i => i.Id == id);

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
            ItemDto item = new(
                Items.Count + 1,
                newItem.Name,
                newItem.Description,
                newItem.LastModifiedDate
            );

            Items.Add(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }
    }
}
