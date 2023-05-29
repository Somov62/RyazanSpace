using RyazanSpace.Interfaces.Cloud;
using System.Net.Http.Json;

namespace RyazanSpace.Services.YandexCloud
{
    public class YandexCloudProvider : ICloud
    {
        private readonly HttpClient _client;
        private const string _authHeaderContent = " OAuth y0_AgAAAAArtK1HAAnywQAAAADjzuvf8qwzhzM1T_CT9O4ml6e0kxiqrRM";

        public YandexCloudProvider(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://cloud-api.yandex.net/v1/disk/");
        }

        public async Task<string> Upload(byte[] file, CancellationToken cancel)
        {
            //Генерируем имя файла на диске
            string pathToFile = Uri.EscapeDataString(Guid.NewGuid().ToString());

            //Обращаемся к диску, чтобы получить ссылку на загрузку
            HttpRequestMessage getUploadLinkRequestMsg = new();
            getUploadLinkRequestMsg.Method = HttpMethod.Get;
            getUploadLinkRequestMsg.Headers.Add("Authorization", _authHeaderContent);
            getUploadLinkRequestMsg.RequestUri = new Uri("resources/upload?path=" + pathToFile, UriKind.Relative);

            var getUploadLinkResponse = await _client.SendAsync(getUploadLinkRequestMsg, cancel).ConfigureAwait(false);
            var uploadLink = await getUploadLinkResponse
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<LinkResponse>(cancellationToken : cancel)
                .ConfigureAwait(false);

            //Загружаем файл на диск. В качестве адреса указываем полученную с сервера ссылку
            HttpRequestMessage uploadFileRequestMessage = new()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(uploadLink.Href),
                Content = new ByteArrayContent(file)
            };
            (await _client
                .SendAsync(uploadFileRequestMessage, cancel)
                .ConfigureAwait(false))
                .EnsureSuccessStatusCode();

            //Получаем ссылку на скачивание файла
            HttpRequestMessage getDownloadLinkRequestMsg = new();
            getDownloadLinkRequestMsg.Method = HttpMethod.Get;
            getDownloadLinkRequestMsg.Headers.Add("Authorization", _authHeaderContent);
            getDownloadLinkRequestMsg.RequestUri = new Uri("resources/download?path=" + pathToFile, UriKind.Relative);

            var downloadLinkResponse = await _client.SendAsync(getDownloadLinkRequestMsg, cancel).ConfigureAwait(false);
            var downloadLink = await downloadLinkResponse
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<LinkResponse>(cancellationToken: cancel)
               .ConfigureAwait(false);
            return downloadLink.Href;
        }
    }

    public class LinkResponse
    {
        public string Href { get; set; }
        public string Method { get; set; }
        public bool Templated { get; set; }
    }
}