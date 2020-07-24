using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SkillsTest.Data;
using SkillsTest.Domain.Entities;
using SkillsTest.Models;

namespace SkillsTest.Controllers
{
    /// <summary>
    /// Контроллер для работы с каталогом фильмов.
    /// </summary>
    [Authorize]
    public class MoviesController : Controller
    {
        private IMovieRepository _movieRepository;
        private IImageRepository _imageRepository;
        private IMapper _mapper;

        public MoviesController(IMovieRepository movieRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Главная страница. Отображает каталог фильмов.
        /// </summary>
        /// <param name="pageSize">Количество фильмов на странице.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="configuration">
        /// Объект для доступа к конфигурации приложения в файле appSettings.json. Количество фильмов на странице
        /// по умолчанию и максимальное хранятся в файле концигурации в секции MoviesSettings.
        /// </param>
        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageSize, int page, [FromServices] IConfiguration configuration)
        {
            int 
                defaultPageSize = int.Parse(configuration["MoviesSettings:DefaultPageSize"]),
                maxPageSize = int.Parse(configuration["MoviesSettings:MaxPageSize"]);

            // Если номер страницы 0 или отрицательное число, то редиректим на первую страницу.
            if (page < 1)
                return RedirectToAction("Index", new { page = 1, pageSize });

            // Если количество фильмов на странице меньше значения по умолчанию или больше максимального, то редиректим
            // на указанную страницу с размером страницы по умолчанию.
            if (pageSize < defaultPageSize || pageSize > maxPageSize)
                return RedirectToAction("Index", new { page, pageSize = defaultPageSize });

            int
                // Общее количество фильмов в каталоге.
                totalCount = _movieRepository.TotalCount,
                // Сколько всего страниц при заданном количестве фильмов на страницу.
                maxPage = (int)Math.Ceiling(totalCount * 1d / pageSize);

            // Если номер текущей страницы больше, чем всего страниц, то редиректим на последнюю страницу.
            if (maxPage != 0 && page > maxPage)
                return RedirectToAction("Index", new { page = maxPage, pageSize });

            // Определим смещение - сколько фильмов пропустить от начала.
            int offset = page * pageSize - pageSize;
            var movies = await _movieRepository.GetMoviesAsync(pageSize, offset);
            var model = new PaginatedMoviesViewModel
            {
                Pagination = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    ItemsTotalCount = totalCount
                },
                Movies = movies
            };
            return View(model);
        }

        [Route("movie/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }

        [HttpGet]
        [Route("movie/{id:int}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            // Если фильма с таким ID не существует, то редиректим на страницу 404.
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
                return NotFound();

            // Если редактирование фильма запрашивает не его создатель, то редиректим на страницу 403.
            if (movie.CreatedBy != User.Identity.Name)
                return Forbid();

            var model = _mapper.Map<MovieBindingModel>(movie);
            return View(model);
        }

        [HttpPost]
        [Route("movie/{id:int}/edit")]
        public async Task<IActionResult> Edit(MovieBindingModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Небольшой гвард, вдруг в запросе сохранение редактирования придёт неправильный ID.
            var entity = await _movieRepository.GetMovieAsync(model.Id);
            if (entity == null)
            {
                ModelState.AddModelError("", $"Фильм c ID={model.Id} не найден.");
                return View(model);
            }

            // Если редактирование фильма запрашивает не его создатель, то редиректим на страницу 403.
            if (model.CreatedBy != User.Identity.Name)
                return Forbid();

            var movie = _mapper.Map<Movie>(model);
            if (model.Image != null)
            {
                // Если пользователь хочет сменить постер фильма, то сохраняем новое изображение в папке 'user-images'.
                string extension = Path.GetExtension(model.Image.FileName);
                movie.Poster = await _imageRepository.SaveImageAsync(model.Image.OpenReadStream(), extension);
            }
            else
            {
                // Иначе оставляем прежний постер.
                movie.Poster = model.Poster;
            }

            // Сохранить ссылку на прежний постер фильма.
            string oldPoster = entity.Poster;

            // Обновить данные фильма в репозитории.
            _movieRepository.SaveMovie(movie);

            // Только после того, как данные фильма были успешно обновлены и если постер был обновлён, то удалить
            // старый постер.
            if (model.Image != null)
                _imageRepository.DeleteImage(Path.GetFileName(oldPoster));
            
            TempData.Add("Message", "Данные фильма успешно обновлены.");
            var updatedModel = _mapper.Map<MovieBindingModel>(movie);
            return View(updatedModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(EmptyModel());
        }

        private MovieBindingModel EmptyModel()
        {
            return new MovieBindingModel
            {
                CreatedBy = User.Identity.Name
            };
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieBindingModel model)
        {
            // Необходимых встроенных атрибутов валидации 'IFormFile' нет, по-хорошему надо бы написать кастомный,
            // но ограничимся ручной проверкой.
            if (model.Image == null)
                ModelState.AddModelError("Image", "Необходимо добавить постер фильма.");

            if (!ModelState.IsValid)
                return View(model);

            var movie = _mapper.Map<Movie>(model);
            string extension = Path.GetExtension(model.Image.FileName);
            movie.Poster = await _imageRepository.SaveImageAsync(model.Image.OpenReadStream(), extension);

            _movieRepository.SaveMovie(movie);
            TempData.Add("Message", "Фильм успешно добавлен в каталог.");
            return View(EmptyModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
                return NotFound();

            // Не позволим пользователю, не являющемуся создателем фильма, удалить его.
            if (movie.CreatedBy != User.Identity.Name)
                return Forbid();

            // Сохранить ссылку на файл изображения постера фильма.
            string posterFilePath = movie.Poster;

            // Удалить фильм из репозитория.
            _movieRepository.DeleteMovie(movie);

            // Если фильм удалён успешно, то удалить и постер.
            _imageRepository.DeleteImage(Path.GetFileName(posterFilePath));

            return RedirectToAction("Index", "Movies");
        }
    }
}
