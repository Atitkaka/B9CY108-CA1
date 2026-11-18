# B9CY108 Advanced Programming Techniques - CA_ONE

MSc Cyber Security, Dublin Business School.
Part I is in C# (Tasks 1 and 2) and Part II is in Python (Tasks 3 and 4).

## Folder structure

```
Task1_HospitalManagement/    C# menu-driven staff management app + MSTest unit tests
Task2_StudentRecords/        C# app storing 20 student records in a Dictionary
Task3_LibraryClientServer/   Python TCP client/server, members stored in MongoDB
Task4_WebScraping/           Python scraper: Wikipedia -> JSON -> CSV -> terminal
```

Each task folder has a `screenshots` folder with the results.

## Requirements

- .NET 8 SDK (or newer) for Tasks 1 and 2
- Python 3 for Tasks 3 and 4: `pip install -r requirements.txt`
- MongoDB running on `localhost:27017` for Task 3

## How to run

**Task 1**
```
cd Task1_HospitalManagement
dotnet run --project HospitalManagement
dotnet test
```

**Task 2**
```
cd Task2_StudentRecords
dotnet run --project StudentRecords
```

**Task 3** (start the server first, then the client in a second terminal)
```
cd Task3_LibraryClientServer
python Que3_server.py
python Que3_client.py
python Que3_server.py show      # lists the members saved in MongoDB
```

**Task 4**
```
cd Task4_WebScraping
python Que4.py
```
This creates `dublin_postal_districts.json` and `dublin_postal_districts.csv`.
