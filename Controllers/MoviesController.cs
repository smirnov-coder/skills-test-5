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

        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageSize, int page, [FromServices] IConfiguration configuration)
        {
            int 
                defaultPageSize = int.Parse(configuration["MoviesSettings:DefaultPageSize"]),
                maxPageSize = int.Parse(configuration["MoviesSettings:MaxPageSize"]);

            if (page < 1)
                return RedirectToAction("Index", new { page = 1, pageSize });

            if (pageSize < defaultPageSize || pageSize > maxPageSize)
                return RedirectToAction("Index", new { page, pageSize = defaultPageSize });

            int 
                totalCount = _movieRepository.TotalCount,
                maxPage = (int)Math.Ceiling(totalCount * 1d / pageSize);

            if (maxPage != 0 && page > maxPage)
                return RedirectToAction("Index", new { page = maxPage, pageSize });

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
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
                return NotFound();

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

            var entity = await _movieRepository.GetMovieAsync(model.Id);
            if (entity == null)
            {
                ModelState.AddModelError("", $"Фильм c ID={model.Id} не найден.");
                return View(model);
            }

            if (model.CreatedBy != User.Identity.Name)
                return Forbid();

            var movie = _mapper.Map<Movie>(model);
            if (model.Image != null)
            {
                string extension = Path.GetExtension(model.Image.FileName);
                movie.Poster = await _imageRepository.SaveImageAsync(model.Image.OpenReadStream(), extension);
            }
            else
            {
                movie.Poster = model.Poster;
            }

            string oldPoster = entity.Poster;
            _movieRepository.SaveMovie(movie);
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

            if (movie.CreatedBy != User.Identity.Name)
                return Forbid();

            string posterFilePath = movie.Poster;
            _movieRepository.DeleteMovie(movie);
            _imageRepository.DeleteImage(Path.GetFileName(posterFilePath));
            return RedirectToAction("Index", "Movies");
        }
    }
}
