using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    public class ImageRepository : IImageRepository
    {
        private IWebHostEnvironment _environment;
        private readonly string _directoryName = "user-images";

        public string ImagesDirectory { get; }

        public ImageRepository(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ImagesDirectory = Path.Combine(_environment.WebRootPath, _directoryName);
            if (!Directory.Exists(ImagesDirectory))
                Directory.CreateDirectory(ImagesDirectory);
        }

        public async Task<string> SaveImageAsync(Stream imageStream, string extension)
        {
            string fileName = GenerateUniqueFileName(extension);
            string filePath = Path.Combine(ImagesDirectory, fileName);
            imageStream.Position = 0;
            using (var file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await imageStream.CopyToAsync(file);
            }
            return $"~/{_directoryName}/{fileName}";
        }

        private string GenerateUniqueFileName(string extension)
        {
            return string.Format("{0}{1}", Guid.NewGuid(), extension);
        }

        public void DeleteImage(string fileName)
        {
            string filePath = Path.Combine(ImagesDirectory, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
