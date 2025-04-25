ArtGallery API Documentation
Overview
REST API for managing an online art gallery built with .NET 6/7 and Entity Framework Core.

🔑 Core Features
Image uploads with artwork creation

Advanced filtering/sorting (category, tags, price, date)

JWT Authentication

Like/Unlike system

Paginated responses

📋 Endpoint Reference
Artworks
Endpoint	Method	Auth	Description
/api/artworks	GET	❌	Get paginated artworks (filter/sort supported)
/api/artworks/{id}	GET	❌	Get single artwork
/api/artworks	POST	✅	Create new artwork
/api/artworks/{id}	PUT	✅	Update artwork
/api/artworks/{id}	DELETE	✅	Delete artwork
/api/artworks/upload-image	POST	✅	Upload image file
/api/artworks/{id}/like	POST	✅	Like artwork
/api/artworks/{id}/unlike	POST	✅	Remove like
🔍 Sample Requests
1. Get Artworks (Filtered & Sorted)
http
GET /api/artworks?category=Portrait&sortBy=price&sortDirection=desc&pageSize=5
Response:

json
{
  "pageNumber": 1,
  "totalCount": 23,
  "items": [
    {
      "id": 42,
      "title": "Mona Lisa",
      "price": 9999,
      "imageUrl": "/uploads/mona.jpg",
      "likes": 1056
    }
  ]
}
2. Upload Image
http
POST /api/artworks/upload-image
Content-Type: multipart/form-data

[image file]
Response:

json
{
  "imageUrl": "/uploads/abc123.jpg"
}
⚙️ Setup
Clone & Install

bash
git clone https://github.com/yourrepo/ArtGallery
dotnet restore
Database

bash
dotnet ef database update
Run

bash
dotnet run
🛠️ Tech Stack
Backend: .NET 6/7

Database: SQL Server (EF Core)

Auth: JWT Bearer Tokens

Docs: Swagger UI

📌 Database Schema
Diagram
Code













🚧 Future Roadmap
User rating system

Artwork commenting

Advanced search filters

Admin moderation panel

💡 Tip: Use Authorization: Bearer [token] header for protected endpoints.

This version:
✅ Clean, scannable layout
✅ Tables for endpoints
✅ Minimal emoji use
✅ Code snippets where needed
✅ Mermaid diagram for DB schema
✅ Direct to-the-point structure
