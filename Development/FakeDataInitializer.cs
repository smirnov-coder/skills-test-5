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
                _db.Movies.Add(new Movie
                {
                    Title = faker.Lorem.Sentence(faker.Random.Number(1, 10)),
                    Description = faker.Lorem.Paragraphs(faker.Random.Number(5, 15)),
                    Director = faker.Person.FullName,
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
