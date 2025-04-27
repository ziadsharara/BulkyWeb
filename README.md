# 🎨 ArtGallery API

A RESTful API for managing an online art gallery built with .NET.

## Features
- **Artwork Management**: Create, read, update, and delete artworks
- **Image Uploads**: Support for artwork images
- **Filtering & Sorting**: By category, price, and creation date
- **Like System**: Users can like/unlike artworks
- **Authentication**: JWT-based security

## Quick Start

```bash
# Clone repository
git clone https://github.com/yourusername/ArtGallery.git

# Navigate to project
cd ArtGallery

# Restore packages
dotnet restore

# Setup database
dotnet ef database update

# Run application
dotnet run
