using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using Haondt.Web.UI.Models;

namespace Odyssey.IntegrationTests.Services.Http
{
    public class HttpClientWrapper(HttpClient client)
    {
        public HttpRequestHeaders DefaultRequestHeaders => client.DefaultRequestHeaders;

        private static Dictionary<string, string> Formify(Object? obj)
        {
            if (obj == null)
                return [];
            var formified = new Dictionary<string, string>();
            foreach (var (k, v) in HxVals.FlattenObject(obj))
                formified[k] = $"{v}";

            return formified;
        }


        public async Task<HttpResponseMessageWrapper> PostAsFormDataAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value, CancellationToken cancellationToken = default) => await HttpResponseMessageWrapper.CreateAsync(await client.PostAsync(requestUri, new FormUrlEncodedContent(Formify(value)), cancellationToken));
        public async Task<HttpResponseMessageWrapper> PostAsFormDataAsync<TValue>(Uri? requestUri, TValue value, CancellationToken cancellationToken = default) => await HttpResponseMessageWrapper.CreateAsync(await client.PostAsync(requestUri, new FormUrlEncodedContent(Formify(value)), cancellationToken));
    }


    public class HttpResponseMessageWrapper
    {
        private readonly HttpResponseMessage _message;

        private HttpResponseMessageWrapper(HttpResponseMessage message)
        {
            _message = message;
            ContentString = "";
        }

        public static async Task<HttpResponseMessageWrapper> CreateAsync(HttpResponseMessage message)
        {
            return new HttpResponseMessageWrapper(message)
            {
                ContentString = await message.Content.ReadAsStringAsync()
            };
        }

        public void EnsureSuccessStatusCode()
        {
            if (_message.IsSuccessStatusCode)
                return;
            throw new HttpRequestException(
                $"Request failed with status code {_message.StatusCode}.\nResponse content:\n{ContentString}",
                null,
                _message.StatusCode);
        }

        public HttpStatusCode StatusCode => _message.StatusCode;
        public HttpContent Content => _message.Content;
        public string ContentString { get; private set; }
        public HttpResponseHeaders Headers => _message.Headers;
    }
}
