namespace RyazanSpace.Interfaces.Cloud
{
    public interface ICloud
    {
        /// <summary>
        /// Метод для загрузки файлов в облачное хранилище
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Ссылка на скачивание файла</returns>
        Task<string> Upload(byte[] file, CancellationToken cancel);
    }
}
