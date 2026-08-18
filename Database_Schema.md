# Home Organizer Database Schema

This document describes the current backend database model for the Home Organizer API.

The current local database provider is SQLite. The same logical schema can later be moved to SQL Server or PostgreSQL.

## Current Relationship Model

```text
Home
  -> Room
      -> StorageUnit
          -> Item
```

Meaning:

- One home can contain many rooms.
- One room belongs to one home.
- One room can contain many storage units.
- One storage unit belongs to one room.
- One storage unit can contain many items.
- One item belongs to one storage unit.

The planned authenticated model will add users later:

```text
User
  -> Home
      -> Room
          -> StorageUnit
              -> Item
```

User ownership is not implemented yet.

## Tables

### Homes

Stores top-level homes created by the user.

| Column | Type | Required | Notes |
| --- | --- | --- | --- |
| Id | int | yes | Primary key |
| Name | string | yes | Home name |
| Description | string | yes | Short home description |

Entity navigation:

```csharp
public List<Room> Rooms { get; set; } = [];
```

### Rooms

Stores rectangular rooms inside a home.

| Column | Type | Required | Notes |
| --- | --- | --- | --- |
| Id | int | yes | Primary key |
| HomeId | int | yes | Foreign key to Homes.Id |
| Name | string | yes | Room name |
| PositionX | int | yes | X position on the home layout |
| PositionY | int | yes | Y position on the home layout |
| Width | int | yes | Room width; must be greater than zero |
| Height | int | yes | Room height; must be greater than zero |

Relationship:

```text
Rooms.HomeId -> Homes.Id
```

Entity navigation:

```csharp
public Home Home { get; set; } = null!;
public List<StorageUnit> StorageUnits { get; set; } = [];
```

### StorageUnits

Stores containers placed inside rooms, such as wardrobes, cabinets, shelves, boxes, and drawer units.

| Column | Type | Required | Notes |
| --- | --- | --- | --- |
| Id | int | yes | Primary key |
| RoomId | int | yes | Foreign key to Rooms.Id |
| Name | string | yes | Storage unit name |
| Type | StorageUnitType | yes | Stored as enum value |
| PositionX | int | yes | X position inside the room |
| PositionY | int | yes | Y position inside the room |
| Width | int | yes | Storage unit width; must be greater than zero |
| Height | int | yes | Storage unit height; must be greater than zero |

Relationship:

```text
StorageUnits.RoomId -> Rooms.Id
```

Entity navigation:

```csharp
public Room Room { get; set; } = null!;
public List<Item> Items { get; set; } = [];
```

### StorageUnitType

Allowed storage unit types:

```text
Wardrobe
Cabinet
DrawerUnit
Shelf
Box
Other
```

### Items

Stores user items inside storage units.

| Column | Type | Required | Notes |
| --- | --- | --- | --- |
| Id | int | yes | Primary key |
| StorageUnitId | int | yes | Foreign key to StorageUnits.Id |
| Name | string | yes | Item name |
| Description | string | yes | Item description |
| Quantity | int | yes | Must be greater than zero |

Relationship:

```text
Items.StorageUnitId -> StorageUnits.Id
```

Entity navigation:

```csharp
public StorageUnit StorageUnit { get; set; } = null!;
```

## Normalization Notes

The schema is normalized for the current project stage:

- Rooms do not duplicate home data; they store only `HomeId`.
- Storage units do not duplicate home or room data; they store only `RoomId`.
- Items do not duplicate home, room, or storage unit data; they store only `StorageUnitId`.

To find an item's full location, follow the relationship chain:

```text
Item.StorageUnitId
  -> StorageUnit.RoomId
      -> Room.HomeId
          -> Home.Id
```

Example:

```text
Passport
  -> Bedroom Wardrobe
      -> Bedroom
          -> My Home
```

## Current API Route Shape

Homes:

```text
GET    /homes
POST   /homes
GET    /homes/{id}
PUT    /homes/{id}
DELETE /homes/{id}
```

Rooms:

```text
GET    /homes/{homeId}/rooms
POST   /homes/{homeId}/rooms
GET    /homes/{homeId}/rooms/{roomId}
PUT    /homes/{homeId}/rooms/{roomId}
DELETE /homes/{homeId}/rooms/{roomId}
```

Storage units:

```text
GET    /homes/{homeId}/rooms/{roomId}/storage-units
POST   /homes/{homeId}/rooms/{roomId}/storage-units
GET    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}
PUT    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}
DELETE /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}
```

Items:

```text
GET    /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items
POST   /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items
GET    /items/{itemId}
PUT    /items/{itemId}
DELETE /items/{itemId}
```

## DTO Rule

Use separate DTOs for request and response shapes.

Create DTO:

```text
Data the frontend sends when creating a record.
Does not include Id.
Does not include parent route IDs.
```

Update DTO:

```text
Data the frontend sends when editing a record.
Does not include Id.
Usually does not include parent route IDs.
```

Response DTO:

```text
Data the API returns.
Includes Id.
Includes foreign key IDs when useful for the frontend.
```

Example:

```text
POST /homes/{homeId}/rooms/{roomId}/storage-units/{storageUnitId}/items
```

The route provides the location. The request body only describes the item:

```json
{
  "name": "Passport",
  "description": "Travel document",
  "quantity": 1
}
```

The response includes the generated ID and storage unit ID:

```json
{
  "id": 1,
  "storageUnitId": 4,
  "name": "Passport",
  "description": "Travel document",
  "quantity": 1
}
```

## Future Authentication Change

When authentication is added, `Home` should receive user ownership fields:

```text
Homes.UserId -> AspNetUsers.Id
```

Then every protected query must verify ownership through this chain:

```text
Item
  -> StorageUnit
      -> Room
          -> Home
              -> User
```

This prevents one user from accessing another user's data by changing an ID in the URL.
