# Guide: Opsætning af Lokal Database

Dette er en guide til, hvordan I får Floozys Hotel databasen til at køre lokalt på jeres egen maskine.

## Forudsætninger
*   I har **SQL Server Management Studio (SSMS)** installeret.
*   I har en lokal SQL Server instans kørende (f.eks. `LocalDB` eller `SQLEXPRESS`).

---

## Trin 1: Opret Database og Tabeller

1.  Åbn **SQL Server Management Studio**.
2.  Log ind på jeres lokale server (ofte `(localdb)\MSSQLLocalDB` eller `.\SQLEXPRESS`).
3.  Klik på knappen **"New Query"** (eller tryk `Ctrl` + `N`).
4.  Find filen `SQL/01_CreateSchema.sql` i projektmappen.
5.  Kopier **hele indholdet** af filen og sæt det ind i dit query-vindue i SSMS.
6.  Tryk på **"Execute"** (eller tast `F5`).
    *   *Resultat:* I bunden skal der stå "Database schema created successfully!".

## Trin 2: Indsæt Testdata

1.  Slet alt tekst i det nuværende query-vindue (eller åbn et nyt).
2.  Find filen `SQL/02_InsertTestData.sql` i projektmappen.
3.  Kopier **hele indholdet** af filen og sæt det ind i SSMS.
4.  Tryk på **"Execute"** (eller tast `F5`).
    *   *Resultat:* Scriptet kører og indsætter værelser, gæster og bookinger. Der vil stå "Generated bookings for the next 5 years." til sidst.

## Trin 3: Opdater Connection String i Projektet

Nu hvor databasen kører, skal C# applikationen vide, at den skal forbinde til din lokale database i stedet for Azure.

1.  Åbn projektet i Visual Studio.
2.  Find filen `Floozys Hotel/App.config`.
3.  Find linjen med `<add name="HotelBooking" ... />`.
4.  Udskift `connectionString` værdien, så den peger på din lokale server.

**Hvis du bruger LocalDB (Standard i VS):**
```xml
<add name="HotelBooking" 
     connectionString="Server=(localdb)\MSSQLLocalDB;Database=HotelBooking;Trusted_Connection=True;MultipleActiveResultSets=true" 
     providerName="Microsoft.Data.SqlClient" />
```

**Hvis du bruger SQL Express:**
```xml
<add name="HotelBooking" 
     connectionString="Server=.\SQLEXPRESS;Database=HotelBooking;Trusted_Connection=True;TrustServerCertificate=True;" 
     providerName="Microsoft.Data.SqlClient" />
```

## Trin 4: Kør Programmet

1.  Gem `App.config`.
2.  Start applikationen (`F5`).
3.  I skal nu kunne se data (bookinger, gæster, værelser) der hentes fra jeres egen lokale database!
