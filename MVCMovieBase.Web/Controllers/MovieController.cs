using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCMovieBase.Common;
using MVCMovieBase.Services.Interfaces;
using MVCMovieBase.Services.Services;
using MVCMovieBase.Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCMovieBase.Web.Controllers
{
    // Wszelkie akcje podejmowane przez użytkownika są przechwytywane do kontrolera,
    // który je odczytuje, następuje np. zmiana stanu (update, delete), read nie zmienia stanu. 
   //Natępnie aktualizowany bądź nie jest model.Po aktualizacji modelu zaktualizowany jest widok,
   //który widzi użytkownik.

    //crud - wzorzec 

    public class MovieController : Controller

        //dependency injection - Program.cs [24-25]
        // wstrzykiwanie zależności do klasy lub obiektu zamiast tworzenia ich bezpośrednio 
        // wewnątrz tych elementów 
    {
        private readonly IMovieService _movieService;
        private readonly ISession session;

        public MovieController(IMovieService movieService, IHttpContextAccessor httpContextAccessor)

            //IHttpContextAccessor - pozwala na dostęp do kontekstu żądania HTTP 
            //IMovieService - to interfejs zapewniające dostęp do serwisu 
        {
            _movieService = movieService;
            session = httpContextAccessor.HttpContext.Session;  
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()

        //IActionResult – to jest taki interfejs do zwracania odpowiedzi webowych 

        // W tej warstwie oddelegowuje do serwisu pobranie wszystkich filmów 
        // Zmienna var allDbMovies - _movieService.GetAll()
        // Moim ViewModelem jest lista. Nstępnie lista filmów przekazywana jest do widoku 

        {
            var allDbMovies = _movieService.GetAll();

            var viewModel = new List<MovieViewModel>();
            foreach (var dbMovie in allDbMovies)
            {
                viewModel.Add(new MovieViewModel
                {
                    Id = dbMovie.Id,
                    Title = dbMovie.Title,
                    Country = dbMovie.Country,
                    Director = dbMovie.Director,
                    Genre = dbMovie.Genre,
                    Year = dbMovie.Year,
                    ScoreAverage = dbMovie.ScoreAverage,
                    ScoreVotesCount = dbMovie.ScoreVotesCount
                });
            }

            return View(viewModel);
        }

        //Autoryzacja wykorzystujemy using Microsoft.AspNetCore.Authorization;
        // Program.cs [42] odpalenie Admin Usera 

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        //IActionResult – to jest taki interfejs do zwracania odpowiedzi webowych 

        // dwołujemy się do warstwy serwisowej przez _movieService 
        // MovieService to warstwa pośrednia, ponieważ nie chcemy bezpośrednio wchodzić do bazy 
        {
            _movieService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // nameof jest używana do generowania adresu URL dla akcji Index, która pozwala 
        // na wyświetlenie listy wszystkich filmów 

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new MovieViewModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(MovieViewModel newMovie)
        {
            if (!ModelState.IsValid)
            {
                return View(newMovie);
            }
            // jeśli dane formularza są ważne, akcja tworzy nowy obiekt MovieDTO z
            // właściwości obiektu new Movie 
            var movieDto = new MovieDTO()
            {
                Year = newMovie.Year,
                Country = newMovie.Country,
                Director = newMovie.Director,
                Genre = newMovie.Genre,
                Title = newMovie.Title,
                ScoreAverage = newMovie.ScoreAverage,
                ScoreVotesCount = newMovie.ScoreVotesCount
            };

            // Żeby stworzyć nowy film odwołujemy się do warstwy serwisowej przez _movieService 
            // MovieService to warstwa pośrednia, ponieważ nie chcemy bezpośrednio
            // korzystać z obiektów w bazie danych Movie.cs, więc korzystamy z MovieDTO(data transfer object)

            // Czyli na koniec nowy obiekt Movie DTO jest przekierowywany do metody
            // _movieService.Create, która dodaje do do bazy 

            var createdSuccessfully = _movieService.Create(movieDto);
            if (!createdSuccessfully)
            {
                return View(newMovie);
            }

            // sesja dodana jest do Porgram.cs [23] 
            // włączanie sesji [28] 

            session.SetString("RecentlyAdded", newMovie.Title);

            return RedirectToAction(nameof(Index));

            // index 
            // Jeśli ta zmienna display nie jest null, to wyświetli ten kod w html 
            // Zmienna display w Index
// Jest HttpContextAccessor[22], z niego próbujemy wziąć TryGetValue o kluczu „RecentlyAdded’’.
// Taki sam klucz jest zdefiniowany w Controlerze w modelu Create. 
// I jeśli wyciąganie display się powiedzie, to chcemy wyświetlić string Default.GetString

        }

        [Authorize]
        [HttpGet]
        public IActionResult Update(int id)
        {
            // również odwołujemy się do warstwy serwisowej; poieramy id i jeśli dana pozycja istnieje
            //jesteśmy przekirowywani do ViewModel formularza i możemy wprowadzić zmiany 

            var dbMovie = _movieService.GetMovieToUpdate(id);

            if(dbMovie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new MovieViewModel
            {
                Id = dbMovie.Id,
                Title = dbMovie.Title,
                Country = dbMovie.Country,
                Director = dbMovie.Director,
                Genre = dbMovie.Genre,
                Year = dbMovie.Year,
                ScoreAverage = dbMovie.ScoreAverage,
                ScoreVotesCount = dbMovie.ScoreVotesCount
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(MovieViewModel updatedMovieViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedMovieViewModel);
            }

            var movieDto = new MovieDTO
            {
                Id = updatedMovieViewModel.Id,
                Title = updatedMovieViewModel.Title,
                Country = updatedMovieViewModel.Country,
                Director = updatedMovieViewModel.Director,
                Genre = updatedMovieViewModel.Genre,
                Year = updatedMovieViewModel.Year,
                ScoreAverage = updatedMovieViewModel.ScoreAverage,
                ScoreVotesCount = updatedMovieViewModel.ScoreVotesCount
            };

            var updateSuccessful = _movieService.Update(movieDto);

            if (!updateSuccessful)
            {
                return View(updatedMovieViewModel);
            }

            return RedirectToAction(nameof(Index));

            // nameof jest używana do generowania adresu URL dla akcji Index, która pozwala 
            // na wyświetlenie listy wszystkich filmów 
        }
    }
}
