using RyazanSpace.Core.Exceptions;

namespace RyazanSpace.Services.ServerCloud.API.Services
{
    public class CloudService
    {
        private readonly string _basePath = "D:\\Desktop\\4 KURS\\Diplom\\Cloud";


        /// <summary>
        /// Позволяет загрузить ресурс
        /// </summary>
        /// <param name="file">Ресурс</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<string> Upload(byte[] file, CancellationToken cancel = default)
        {
            string filename = Guid.NewGuid().ToString();
            string pathToFile = Path.Combine(_basePath, filename);

            await File.WriteAllBytesAsync(pathToFile, file, cancel).ConfigureAwait(false);
            return filename;
        }

        /// <summary>
        /// Позволяет скачать ресурс
        /// </summary>
        /// <param name="filename">Имя ресурса</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<byte[]> Download(string filename, CancellationToken cancel = default)
        {
            string pathToFile = Path.Combine(_basePath, filename);
            if (!File.Exists(pathToFile)) throw new NotFoundException("Ресурс не найден!");
            var bytes = await File.ReadAllBytesAsync(pathToFile, cancel).ConfigureAwait(false);
            return bytes;
        }
    }
}
