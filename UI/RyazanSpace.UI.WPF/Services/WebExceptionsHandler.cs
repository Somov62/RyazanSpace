using RyazanSpace.UI.WPF.Services.Locator;
using System;
using System.Net.Http;

namespace RyazanSpace.UI.WPF.Services
{
    public class WebExceptionsHandler
    {
        public void Handle(Exception ex) 
        {
            string message = ex.Message;
            if (ex is HttpRequestException)
            {
                message = "Связь с сервером не установлена. Проверьте ваше подключение к интернету.";
            }
            ServiceLocator.Instanse.Mbox.ShowInfo(message);
        }

    }
}
