# APBD-CW7-S27747 Mini Helpdesk

Prosta aplikacja ASP.NET Core MVC do obsługi zgłoszeń helpdesk.

## Technologie

- .NET 9
- ASP.NET Core MVC
- Razor Views
- Entity Framework Core
- SQLite
- xUnit

## Jak uruchomić aplikację

Z katalogu głównego repozytorium:

```bash
dotnet run --project MiniHelpdesk.Web

Po uruchomieniu aplikacja jest dostępna pod adresem pokazanym w konsoli.

Uruchomienie testów z katalogu głównego repozytorium:
dotnet test APBD-CW7-S27747.sln

1. Dlaczego kolejność middleware w Program.cs ma znaczenie?

Kolejność middleware ma znaczenie, ponieważ żądanie HTTP przechodzi przez middleware w takiej kolejności, w jakiej zostały dodane w Program.cs. Jeśli middleware obsługujący wyjątki będzie dodany za późno, może nie przechwycić błędów z wcześniejszych elementów pipeline.

2. Czym różni się app.Use od app.Run?

app.Use dodaje middleware, który może wykonać swoją logikę i przekazać żądanie dalej. app.Run kończy pipeline i nie przekazuje żądania do kolejnego middleware.

3. Dlaczego kontroler nie powinien zawierać całej logiki aplikacji?

Kontroler powinien obsługiwać żądanie i zwracać odpowiedź, a logika biznesowa powinna być w warstwie Service. Dzięki temu kod jest czytelniejszy, łatwiejszy do testowania i łatwiejszy do rozbudowy.

4. Co daje test jednostkowy warstwy Service?

Test jednostkowy warstwy Service pozwala sprawdzić logikę biznesową bez uruchamiania całej aplikacji i bez prawdziwej bazy danych. Dzięki temu można szybko sprawdzić walidację, zmianę statusu i działanie transakcji.

5. Co powinno się stać, jeśli zapis zgłoszenia się uda, ale zapis komentarza zakończy się błędem?

Nie powinno zostać zapisane ani zgłoszenie, ani komentarz. Obie operacje są wykonywane w jednej transakcji, więc w przypadku błędu transakcja nie zostaje zatwierdzona.