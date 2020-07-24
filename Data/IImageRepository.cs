using System.IO;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    /// <summary>
    /// Реопзиторий для хранения файлов изображений постеров фильмов.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Относительный путь к папке хранения файлов изображений.
        /// </summary>
        string ImagesDirectory { get; }

        /// <summary>
        /// Асинхронно сохраняет изображение в репозитории.
        /// </summary>
        /// <param name="imageStream">Объект потока изображения. Поток должен быть открыт.</param>
        /// <param name="extension">Расширение файла изображения, включая символ точки.</param>
        /// <returns>Относительный путь к файлу изображения, с префиксом '~'.</returns>
        Task<string> SaveImageAsync(Stream imageStream, string extension);

        /// <summary>
        /// Удаляет файл изображения из репозитория.
        /// </summary>
        /// <param name="fileName">Полное имя файла изображения, включая расширение.</param>
        void DeleteImage(string fileName);
    }
}
