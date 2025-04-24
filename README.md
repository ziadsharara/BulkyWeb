# ArtGallery API

## Overview
This is the backend API for an ArtGallery platform where users can manage artworks, bids, and users. It supports features like artwork creation, updating, deletion, image uploads, and secure user authentication via JWT tokens.

## Features

### **1. User Authentication**
- **JWT Authentication**: Secure access to the platform via JWT tokens.
- **Authorization**: Protected routes ensuring that only authenticated users can create, update, and delete artworks.

### **2. Artworks Management**
- **Create**: Users can create new artworks by submitting details like title, description, category, price, and an image.
- **Read**: Retrieve artwork details with various filters and sorts.
- **Update**: Update artwork information, including title, description, price, etc.
- **Delete**: Delete artworks from the platform.

### **3. Image Upload**
- Users can upload images associated with artworks.
- Image files are stored on the server and can be accessed via a URL.

### **4. API Endpoints**
- **GET /api/artworks**: Fetch all artworks.
- **GET /api/artworks/{id}**: Get artwork by ID.
- **POST /api/artworks**: Create a new artwork.
- **PUT /api/artworks/{id}**: Update artwork details.
- **DELETE /api/artworks/{id}**: Delete an artwork.
- **POST /api/artworks/upload-image**: Upload an image for artwork.

## Setup

### **1. Prerequisites**
- .NET 6.0 SDK
- SQL Server
- Postman or any API testing tool

### **2. Environment Variables**
Ensure to configure the following in the `appsettings.json` or in your environment variables:
- `DefaultConnection`: Connection string to your SQL database.
- `Jwt:Issuer`: Issuer of the JWT token.
- `Jwt:Audience`: Audience of the JWT token.
- `Jwt:Key`: Secret key used for signing the JWT token.

## Future Enhancements
Real-time features using sockets (e.g., notifications, live bids).
Integration with payment gateways for handling transactions.

