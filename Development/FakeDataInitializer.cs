using Bogus;
using Microsoft.AspNetCore.Identity;
using SkillsTest.Data;
using SkillsTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Development
{
    /// <summary>
    /// Вспомогательный класс для генерации фейковых данных.
    /// </summary>
    public class FakeDataInitializer
    {
        private MoviesDbContext _db;
        private UserManager<IdentityUser> _userManager;

        public FakeDataInitializer(MoviesDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public void Initialize()
        {
            CreateUsers();
            CreateMovies();
        }

        private void CreateMovies()
        {
            if (_db.Movies.Any())
                return;

            var faker = new Faker("en");
            for (int i = 1; i < 100; i++)
            {
                string
                    title = faker.Lorem.Sentence(faker.Random.Number(1, 10)),
                    description = faker.Lorem.Paragraphs(faker.Random.Number(5, 15)),
                    director = faker.Person.FullName;
                _db.Movies.Add(new Movie
                {
                    Title = title.Substring(0, title.Length > 100 ? 100 : title.Length),
                    Description = description.Substring(0, description.Length > 1000 ? 1000 : description.Length),
                    Director = director.Substring(0, director.Length > 50 ? 50 : director.Length),
                    Year = faker.Random.Number(1930, DateTime.Now.Year),
                    Poster = faker.Image.PicsumUrl(faker.Random.Number(500, 1200), faker.Random.Number(500, 1200)),
                    CreatedBy = faker.PickRandom(GetFakeUserCollection().Select(user => user.Email))
                });
                _db.SaveChanges();
            }
        }

        private void CreateUsers()
        {
            if (_userManager.Users.Any())
                return;

            GetFakeUserCollection().ForEach(user =>
            {
                _userManager.CreateAsync(user, "User_123").Wait();
            });
        }

        private List<IdentityUser> GetFakeUserCollection()
        {
            return new List<IdentityUser>
            {
                new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com", EmailConfirmed = true },
                new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com", EmailConfirmed = true }
            };
        }
    }
}
