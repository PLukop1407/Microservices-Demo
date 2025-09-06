# Microservices Architecture Demo

A simple project demonstrating a **FastAPI-based back-end** and a **C# front-end** that communicate over HTTP.

This project was created as part of my university coursework, where I chose to implement a **microservices architecture** using **Python (FastAPI)** and **C#**.

The recommended approach was using Java, but I opted for a full-stack approach with a Python back-end and a C# front-end. The C# front-end was a bold, and frankly, bad choice, but I was pressed for time and turned to what I knew. Better alternatives include React, Vue, etc., and I will possibly create a new front-end in the future.

---
## Features
- Basic **CRUD operations** for managing **items** and **stock** in a synthetic, non-persistent database.
- **HTTP communication** between the Python back-end (FastAPI) and the C# WPF front-end using **Netwonsoft.Json** for JSON serialization/deserialization.
- A **C# WPF front-end** that interacts with the **FastAPI back-end** over HTTP, displaying various CRUD operations: item property changes, stock reorders, report generation.

---
## Technologies used
- **FastAPI** (Python) - Back-end framework
- **Uvicorn** (for serving FastAPI)
- **Pydantic** (Data validation)
- **C# WPF** (Windows Presentation Foundation) - Front-end
- **Newtonsoft.Json** - For JSON serialization/deserialization in the C# WPF front-end

---

## Future Developments
- Switching to SQLite for database persistence
- C# WPF front-end swapped out for a more appropriate framework (React, Vue, etc.)
- Front-end authentication for users

---
## Areas for improvement
- C# as a choice for a front-end was a last minute decision due to time constraints. Its HTTP communication capabilities aren't the best, and a more suitable approach would've been to use something like **React**. I plan on revisiting this.
- Synthetic database provides no persistence between sessions - this was a decision based purely on the time constraint I was facing, and I'd like to update this to an SQLite database to provide editable, persistent data.
- Use of docker containers to **truly** separate microservices would've been fantastic. Right now, the project as it is, works on a demonstrative level, but I'd love to come back and implement the use of docker containers to allow each microservice to run independently. Would be great for isolation, scalability, and even just deployment.
- As mentioned before, use of proper authorisation would've been nice to see as well. Even basic OAuth or JWT-based authentication would've contributed a lot to this project by providing secure user sessions. Future plans could involve implementing some kind of user authentication via OAuth2 or JWT tokens.
---

## Setup & Installation

### 1. Clone repo
### 2. Back-end setup:
- Navigate to ``Backend/`` directory
```bash
cd Backend
```
- Create and activate python venv
```bash
python -m venv venv

.\venv\Scripts\activate
```
- Install dependencies
```bash
pip install -r requirements.txt
```
- Run the FastAPI back-end
```bash
uvicorn app.main:app --reload
```
### 3. Front-end Setup (C# WPF Client):
- Open the C# WPF project located in ``Frontend/DE-store.sln``.
- Ensure Newtonsoft.Json is present with NuGet
- Build and run WPF project in Visual Studio. The front-end will communicate with the FastAPI back-end to fetch and display data. Will show an error if back-end is not running.

---
### License

This project is for portfolio and demonstration purposes only. Feel free to use it for learning or experimentation. No formal license is attached, but feel free to reach out if you've any questions.