# Home Organizer — .NET Project Schema

## 1. Project Overview

**Home Organizer** is a web application that allows users to:

- Register and sign in.
- Create their own home.
- Draw rooms in a 2D bird's-eye view.
- Place predefined storage units such as wardrobes, cabinets, drawers, and shelves.
- Add items directly to specific storage units.
- Search for an item and see its exact location.
- Highlight the related room and storage unit on the 2D home layout.

Each home has a single 2D layout. The application does not model multiple levels.

The first version should use a simple **2D top-down approach**.  
A realistic 3D home designer is outside the initial project scope.

---

## 2. Recommended Technology Stack

### Backend

- **Language:** C#
- **Framework:** ASP.NET Core Web API
- **API style:** Controller-based REST API
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server (MSSQL)
- **Authentication:** ASP.NET Core Identity
- **API documentation/testing:** OpenAPI / Swagger

### Frontend

- **Language:** TypeScript
- **Framework:** React
- **Build tool:** Vite
- **2D canvas:** React Konva
- **Routing:** React Router
- **HTTP client:** Axios or the native Fetch API

### Development Tools

- Visual Studio Code
- .NET SDK
- Node.js
- Microsoft SQL Server or SQL Server Express
- SQL Server Management Studio or Azure Data Studio
- Git and GitHub
- Docker Compose — optional for the first version

---

## 3. Starting .NET Template

Create a controller-based ASP.NET Core Web API.

```bash
mkdir HomeOrganizer
cd HomeOrganizer

dotnet new sln -n HomeOrganizer
dotnet new webapi --use-controllers -n HomeOrganizer.Api
dotnet sln add HomeOrganizer.Api/HomeOrganizer.Api.csproj

code .
```

Run the application:

```bash
cd HomeOrganizer.Api
dotnet run
```

The backend should start as a single project.  
Do not begin with a complicated Clean Architecture solution containing many projects.

---

## 4. High-Level Architecture

```text
┌──────────────────────────────────┐
│ React + TypeScript Frontend      │
│                                  │
│ - Register / Login               │
│ - Home selection                 │
│ - 2D room editor                 │
│ - Furniture placement            │
│ - Item management                │
│ - Item search                    │
└────────────────┬─────────────────┘
                 │
                 │ HTTP requests and JSON
                 ▼
┌──────────────────────────────────┐
│ ASP.NET Core Web API             │
│                                  │
│ - Authentication                 │
│ - Home management                │
│ - Room management                │
│ - Storage-unit management        │
│ - Item management                │
│ - Search service                 │
└────────────────┬─────────────────┘
                 │
                 │ Entity Framework Core
                 ▼
┌──────────────────────────────────┐
│ Microsoft SQL Server Database    │
│                                  │
│ - Users                          │
│ - Homes                          │
│ - Rooms                          │
│ - StorageUnits                   │
│ - Items                          │
└──────────────────────────────────┘
```

---

## 5. Main User Flow

```text
Register or Login
        ↓
Create a Home
        ↓
Draw Rooms
        ↓
Place Cabinets or Wardrobes
        ↓
Add Items Directly to Storage Units
        ↓
Search for an Item
        ↓
Open the Correct Home
        ↓
Highlight the Room and Storage Unit
```

Example:

```text
User
└── My Home
    └── Bedroom
        └── Large Wardrobe
            └── Passport
```

---

## 6. Backend Request Cycle

The recommended backend flow is:

```text
Controller
    ↓
Service
    ↓
ApplicationDbContext
    ↓
Microsoft SQL Server
```

Example: the user places a wardrobe in a bedroom.

```text
1. The user drags a wardrobe onto the 2D canvas.

2. React reads its layout values:
   - Position X
   - Position Y
   - Width
   - Height
   - Rotation

3. React sends a POST request to the API.

4. StorageUnitsController receives the request.

5. StorageUnitService validates the operation.

6. Entity Framework Core creates the database record.

7. Microsoft SQL Server stores the wardrobe.

8. The API returns the created storage unit.

9. React updates the canvas using the returned data.
```

Example request:

```http
POST /api/storage-units
Content-Type: application/json
```

```json
{
  "roomId": 15,
  "name": "Bedroom Wardrobe",
  "type": "Wardrobe",
  "positionX": 420,
  "positionY": 180,
  "width": 120,
  "height": 60,
  "rotation": 0
}
```

---

## 7. Domain Model

```text
ApplicationUser
    │
    └── Home
         │
         └── Room
              │
              └── StorageUnit
                   │
                   └── Item
```

### Relationship Summary

- One user can own multiple homes.
- One home can contain multiple rooms.
- One room can contain multiple storage units.
- One storage unit can contain multiple items.

---

## 8. Suggested Entities

### ApplicationUser

ASP.NET Core Identity manages the main user fields.

Additional user properties can be added later if necessary.

```csharp
public class ApplicationUser : IdentityUser
{
    public ICollection<Home> Homes { get; set; } = [];
}
```

### Home

```csharp
public class Home
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public ICollection<Room> Rooms { get; set; } = [];
}
```

### Room

For the first version, every room can be represented as a rectangle.

```csharp
public class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int HomeId { get; set; }

    public Home Home { get; set; } = null!;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public ICollection<StorageUnit> StorageUnits { get; set; } = [];
}
```

### StorageUnit

A storage unit represents a wardrobe, cabinet, shelf, drawer unit, box, or similar object.

```csharp
public class StorageUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public StorageUnitType Type { get; set; }

    public int RoomId { get; set; }

    public Room Room { get; set; } = null!;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Rotation { get; set; }

    public ICollection<Item> Items { get; set; } = [];
}
```

### StorageUnitType

```csharp
public enum StorageUnitType
{
    Wardrobe,
    Cabinet,
    DrawerUnit,
    Shelf,
    Box,
    Other
}
```

### Item

```csharp
public class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Quantity { get; set; } = 1;

    public int StorageUnitId { get; set; }

    public StorageUnit StorageUnit { get; set; } = null!;
}
```

---

## 9. Database Tables

The initial database should contain these tables:

```text
AspNetUsers
Homes
Rooms
StorageUnits
Items
```

### Important Foreign Keys

```text
Homes.UserId             → AspNetUsers.Id
Rooms.HomeId             → Homes.Id
StorageUnits.RoomId      → Rooms.Id
Items.StorageUnitId      → StorageUnits.Id
```

---

## 10. Suggested Project Folder Structure

```text
HomeOrganizer/
├── HomeOrganizer.sln
│
└── HomeOrganizer.Api/
    ├── Controllers/
    │   ├── AuthController.cs
    │   ├── HomesController.cs
    │   ├── RoomsController.cs
    │   ├── StorageUnitsController.cs
    │   ├── ItemsController.cs
    │   └── SearchController.cs
    │
    ├── Data/
    │   └── ApplicationDbContext.cs
    │
    ├── DTOs/
    │   ├── Auth/
    │   ├── Homes/
    │   ├── Rooms/
    │   ├── StorageUnits/
    │   └── Items/
    │
    ├── Entities/
    │   ├── ApplicationUser.cs
    │   ├── Home.cs
    │   ├── Room.cs
    │   ├── StorageUnit.cs
    │   └── Item.cs
    │
    ├── Enums/
    │   └── StorageUnitType.cs
    │
    ├── Services/
    │   ├── HomeService.cs
    │   ├── RoomService.cs
    │   ├── StorageUnitService.cs
    │   └── ItemSearchService.cs
    │
    ├── Program.cs
    ├── appsettings.json
    └── HomeOrganizer.Api.csproj
```

For a beginner project, repositories are optional.  
Entity Framework Core's `DbContext` already provides many repository-like operations.

---

## 11. Main API Endpoints

### Authentication

```text
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/logout
GET    /api/auth/me
```

### Homes

```text
GET    /api/homes
POST   /api/homes
GET    /api/homes/{homeId}
PUT    /api/homes/{homeId}
DELETE /api/homes/{homeId}
```

### Rooms

```text
GET    /api/homes/{homeId}/rooms
POST   /api/homes/{homeId}/rooms
GET    /api/rooms/{roomId}
PUT    /api/rooms/{roomId}
DELETE /api/rooms/{roomId}
```

### Storage Units

```text
GET    /api/rooms/{roomId}/storage-units
POST   /api/rooms/{roomId}/storage-units
GET    /api/storage-units/{storageUnitId}
PUT    /api/storage-units/{storageUnitId}
DELETE /api/storage-units/{storageUnitId}
```

### Items

```text
GET    /api/storage-units/{storageUnitId}/items
POST   /api/storage-units/{storageUnitId}/items
GET    /api/items/{itemId}
PUT    /api/items/{itemId}
DELETE /api/items/{itemId}
```

### Search

```text
GET /api/items/search?query=passport
```

---

## 12. Search Cycle

```text
User types "passport"
        ↓
React sends:
GET /api/items/search?query=passport
        ↓
ASP.NET Core verifies the authenticated user
        ↓
ItemSearchService searches only the user's homes
        ↓
Entity Framework Core joins:
Items
StorageUnits
Rooms
Homes
        ↓
The API returns the item's complete location
        ↓
React opens the correct home
        ↓
React highlights the room
        ↓
React highlights the storage unit
```

Example API response:

```json
{
  "itemId": 81,
  "itemName": "Passport",
  "quantity": 1,
  "homeId": 1,
  "homeName": "My Home",
  "roomId": 15,
  "roomName": "Bedroom",
  "storageUnitId": 23,
  "storageUnitName": "Large Wardrobe",
  "positionX": 420,
  "positionY": 180,
  "width": 120,
  "height": 60,
  "rotation": 0
}
```

---

## 13. 2D Frontend Model

The first editor should use simple shapes.

### Room

Represent a room as a rectangle containing:

```text
id
name
positionX
positionY
width
height
```

### Storage Unit

Represent a storage unit as a draggable rectangle or icon containing:

```text
id
name
type
roomId
positionX
positionY
width
height
rotation
```

### Canvas Operations

The frontend should support:

- Add room
- Select room
- Move room
- Resize room
- Delete room
- Add storage unit
- Move storage unit
- Resize storage unit
- Rotate storage unit
- Select storage unit
- Open storage-unit contents
- Highlight a room
- Highlight a storage unit

---

## 14. Ownership and Security Rules

Every database operation must verify that the requested resource belongs to the current user.

Example ownership chain:

```text
Item
→ StorageUnit
→ Room
→ Home
→ User
```

A user must never be able to access another user's home by changing an ID in the URL.

Examples:

```text
GET /api/homes/1
DELETE /api/items/45
PUT /api/rooms/12
```

Before completing an operation, the backend must confirm that the related `Home.UserId` matches the authenticated user's ID.

---

## 15. DTO Strategy

Do not return Entity Framework entities directly from controllers.

Use request and response DTOs.

### CreateRoomRequest

```csharp
public class CreateRoomRequest
{
    public string Name { get; set; } = string.Empty;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }
}
```

### RoomResponse

```csharp
public class RoomResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }
}
```

---

## 16. Validation Examples

The backend should validate:

- Home name cannot be empty.
- Room width and height must be greater than zero.
- Storage-unit width and height must be greater than zero.
- Item name cannot be empty.
- Item quantity must be greater than zero.
- The selected room must belong to the current user.
- The selected storage unit must belong to the current user.
- A storage unit should be placed inside its assigned room.
- Search text should be trimmed before querying.

Example:

```csharp
if (request.Width <= 0 || request.Height <= 0)
{
    return BadRequest("Width and height must be greater than zero.");
}
```

---

## 17. Development Phases

### Phase 1 — Project Setup

- Install the .NET SDK.
- Create the solution and Web API.
- Run the generated project.
- Understand `Program.cs`.
- Understand controllers and routing.
- Enable OpenAPI or Swagger.

### Phase 2 — Microsoft SQL Server and Entity Framework Core

- Install Microsoft SQL Server.
- Add the Microsoft SQL Server connection string.
- Install Entity Framework Core packages.
- Create `ApplicationDbContext`.
- Create the first entities.
- Create and apply the first migration.

Example commands:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Phase 3 — Basic Home CRUD

Implement:

- Create home
- List the current user's homes
- Get one home
- Update home
- Delete home

Initially, authentication may be temporarily replaced with a test user ID while learning CRUD.  
Remove that temporary approach before completing the application.

### Phase 4 — Rooms

Implement:

- Create rectangular rooms directly inside a home.
- Store room coordinates and dimensions.
- Update room layout.
- Delete rooms.

### Phase 5 — Basic React Frontend

- Create the React application.
- Connect React to the API.
- Display homes.
- Load room layout data.
- Render rooms as rectangles.

### Phase 6 — Storage Units

- Create predefined storage-unit types.
- Drag storage units onto rooms.
- Save positions to the backend.
- Update positions after dragging.
- Delete storage units.

### Phase 7 — Items

- Add items directly to storage units.
- Edit item details.
- Delete items.
- Display storage-unit contents in a panel or modal.

### Phase 8 — Search

- Search items by name.
- Return complete location information.
- Open the correct home.
- Highlight the correct room.
- Highlight the correct storage unit.

### Phase 9 — Authentication

- Add ASP.NET Core Identity.
- Register users.
- Sign users in.
- Protect API endpoints.
- Associate homes with authenticated users.
- Add ownership checks.

### Phase 10 — Testing and Deployment

- Add service-level unit tests.
- Add API integration tests.
- Test ownership rules.
- Test search results.
- Create production configuration.
- Deploy the backend, frontend, and database.

---

## 18. Minimum Viable Product

The first complete version should support only:

- User registration
- User login
- Create one or more homes
- Add rectangular rooms directly to a home
- Move and resize rooms
- Place predefined cabinets and wardrobes
- Add items directly to storage units
- Search items
- Highlight the correct room and storage unit

---

## 19. Features to Postpone

Do not include these features in the first version:

- 3D rendering
- Real-world architectural measurements
- Curved or irregular walls
- Automatic room collision detection
- Multiple users sharing one home
- Real-time collaborative editing
- Mobile application
- QR-code labels
- Barcode scanning
- Image recognition
- AI-based item categorisation
- Advanced full-text search
- Furniture marketplace
- Complex Clean Architecture

These can be added after the MVP works correctly.

---

## 20. Recommended First Milestone

The first milestone should prove that the full request cycle works.

### Goal

Create a home through the API and store it in Microsoft SQL Server.

### Tasks

1. Create the ASP.NET Core Web API.
2. Install Entity Framework Core.
3. Connect Microsoft SQL Server.
4. Create the `Home` entity.
5. Add `ApplicationDbContext`.
6. Create a migration.
7. Create `HomesController`.
8. Implement `POST /api/homes`.
9. Implement `GET /api/homes`.
10. Test both endpoints.

### Expected Result

```text
Client
  ↓
HomesController
  ↓
ApplicationDbContext
  ↓
Microsoft SQL Server
  ↓
JSON response
```

Do not start the 2D canvas until this basic backend cycle works.

---

## 21. Example Learning Order

A beginner-friendly learning order is:

```text
C# classes and objects
        ↓
ASP.NET Core project structure
        ↓
Controllers and routing
        ↓
HTTP methods and status codes
        ↓
Dependency injection
        ↓
Entity Framework Core
        ↓
Microsoft SQL Server relationships
        ↓
DTOs and validation
        ↓
Authentication and authorization
        ↓
React fundamentals
        ↓
React Konva and canvas interaction
```

---

## 22. Final Project Direction

Use:

```text
ASP.NET Core Web API
+ Entity Framework Core
+ Microsoft SQL Server
+ ASP.NET Core Identity
+ React
+ TypeScript
+ React Konva
```

Begin with a single controller-based API project and a simple rectangular 2D home-layout model.

The most important first objective is not visual quality. It is proving that this complete flow works:

```text
Create data
→ Save data
→ Load data
→ Edit data
→ Search data
→ Display its location
```

