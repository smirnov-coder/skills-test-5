using System.IO;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    public interface IImageRepository
    {
        string ImagesDirectory { get; }

        Task<string> SaveImageAsync(Stream imageStream, string extension);

        void DeleteImage(string fileName);
    }
}