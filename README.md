# Dokumentacja Projektu MVCMovieBase
# Opis Projektu
MVCMovieBase to projekt oparty na architekturze MVC (Model-View-Controller) w środowisku .NET. 
Celem projektu jest zarządzanie kolekcją recenzji filmowych. Aplikacja umożliwia dodawanie, edytowanie, usuwanie i przeglądanie recenzji filmów.

# Technologie
.NET Core MVC
C#
Entity Framework Core
HTML
CSS
JavaScript

# Struktura Projektu
Controllers/ - Kontrolery obsługujące żądania HTTP
Models/ - Modele reprezentujące dane aplikacji
Views/ - Widoki renderujące interfejs użytkownika
wwwroot/ - Zasoby statyczne (CSS, JavaScript)
appsettings.json - Konfiguracja aplikacji
Startup.cs - Konfiguracja i konfiguracja usług aplikacji

# Model Danych
Aplikacja wykorzystuje model danych do przechowywania recenzji filmowych. Przykładowy model MovieReview może wyglądać tak:

csharp
Copy code
public class MovieReview
{
public int Id { get; set; }
public string Title { get; set; }
public string ReviewText { get; set; }
public int Rating { get; set; }
}
# Kontrolery
HomeController - Obsługuje żądania związane z główną stroną i przeglądaniem recenzji.
ReviewsController - Zarządza operacjami CRUD (Create, Read, Update, Delete) dla recenzji.

# Widoki
Index.cshtml - Strona główna wyświetlająca listę recenzji.
Create.cshtml - Strona do dodawania nowej recenzji.
Edit.cshtml - Strona do edytowania istniejącej recenzji.

# Konfiguracja
Konfiguracja aplikacji, takie jak połączenie do bazy danych, znajduje się w pliku appsettings.json.
Połączenie do bazy danych jest skonfigurowane w pliku Startup.cs.

# Baza Danych
Aplikacja korzysta z Entity Framework Core do komunikacji z bazą danych.
Model danych jest mapowany na tabelę w bazie danych.

# Uruchamianie Projektu
Skonfiguruj połączenie do bazy danych w pliku appsettings.json.

# Uruchom migracje, aby utworzyć bazę danych:

bash
Copy code
dotnet ef migrations add InitialCreate
dotnet ef database update
Uruchom projekt:

bash
Copy code
dotnet run
Aplikacja będzie dostępna pod adresem http://localhost:5000 (lub https://localhost:5001).

# Autor [Paulina D., Jola N.]
