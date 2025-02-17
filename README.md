# 🎬 Movie World Project
This is a full-stack web application that provides movie data fetched from two different providers, displaying details based on the lowest price. The frontend is built with **React**, and the backend is developed using **.NET 8 (C#)**. The application allows users to retrieve movie details, such as title, price, and rating, by interacting with the backend API. The backend communicates with external movie providers, implements necessary retries due to flaky APIs, and the frontend gracefully handles scenarios like missing data or API failures.

Unit tests are written to ensure the backend logic functions as expected, ensuring a reliable and efficient movie data retrieval process.

## 📌 Table of Contents
1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Setup and Installation](#setup-and-installation)
   - [Frontend Setup](#frontend-setup)
   - [Backend Setup](#backend-setup)
   - [Running Unit Tests](#running-unit-tests)
4. [Configuration](#configuration)
5. [Folder Structure](#folder-structure)

## 🚀 Features
✅ **Movie Data Fetching** – Fetches and displays movie details (title, price, rating, poster).  
✅ **Authentication** – Uses `x-access-token` to authenticate provider endpoint.  
✅ **Caching** – Reduces repeated interactions with provider endpoints.  
✅ **Interactive UI** – Built with React for a dynamic and user-friendly experience.  
✅ **Backend API** – Developed in .NET 8 (C#) to handle movie data retrieval.  
✅ **Unit Testing** – Written in XUnit to test backend services, ensuring reliable performance.  

## 🛠 Technologies Used
- **Frontend**: React, Fetch API  
- **Backend**: .NET 8 (C#)  
- **Testing**: XUnit, Moq  

## 🔧 Setup and Installation
### **Prerequisites**
Ensure you have the following installed:
- **Node.js** for the React frontend  
- **.NET SDK** for the backend API  
- **Visual Studio 2022** or another preferred IDE  

### **Frontend Setup**
1. Open the `movieworld.ui` directory in your preferred IDE (e.g., Visual Studio Code).  
2. Install dependencies:
   npm install 
3. Start the development server:
   npm run dev
4. The frontend will be available at:
   - `http://localhost:3000`  
   - `http://localhost:3001` (alternative port)  

### **Backend Setup**
1. Open the `MovieWorld.Api` solution in your IDE (e.g., Visual Studio 2022).  
2. Run the backend application.  
3. Update the backend URL in the frontend project by modifying `.env.local` (Line 1).  

### ✅ Running Unit Tests
1. Open the `MovieWorld.Api` solution in Visual Studio 2022.  
2. Run the unit tests.  
3. Ensure all tests pass successfully.  

## ⚙️ Configuration
- **Authentication** – Uses `x-access-token` for API request validation.  
- **Movie Provider URLs** – Configured for `Cinemaworld` and `Filmworld`.  
- **Caching & Retry Mechanism** – Optimized to reduce API calls and handle failures.  
- **Rate Limiting** – Implemented to manage request traffic efficiently.  
- Most configurations are set in `appsettings.json`.  

## 📂 Folder Structure
```
movie-world/
│
├── **movieworld.ui/**                 # React frontend application│                        
│
└── **MovieWorld.Api/**                # .NET Core backend application
    ├── **MovieWorld.Api/**            # Controllers and API routing
        └── **appsettings.json**           # Configuration file (API keys, URLs, rate limits)
    ├── **MovieWorld.Infra/**          # External Movie Providers API integration
    ├── **MovieWorld.Service/**        # Business logic for handling movie data
    ├── **MovieWorld.Api.sln**         # .NET solution file
    ├── **MovieWorld.Tests/**          # Unit tests for backend functionality
    