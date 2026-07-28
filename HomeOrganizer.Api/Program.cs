using HomeOrganizer.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

List<ItemDto> items = new List<ItemDto>
{
    new ItemDto(1, "Bavul", "Item container", DateOnly.FromDateTime(DateTime.Now)),
    new ItemDto(2, "Canta", "Item for carrying belongings", DateOnly.FromDateTime(DateTime.Now)),
    new ItemDto(3, "Ayakkabi", "Footwear", DateOnly.FromDateTime(DateTime.Now))
};


// GET /items
app.MapGet("/items", () => items);


// get /items/{id}
app.MapGet("/items/{id}", (int id) =>
{
    var item = items.Find(i => i.Id == id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});


// POST /items
app.MapPost("/items", (CreateItemDto newItem) =>
{
    ItemDto item = new(
        items.Count + 1,
        newItem.Name,
        newItem.Description,
        newItem.LastModifiedDate
    );
    items.Add(item);
    return Results.Created($"/items/{item.Id}", item);
});

app.Run();