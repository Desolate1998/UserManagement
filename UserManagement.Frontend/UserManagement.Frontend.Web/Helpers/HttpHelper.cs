using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserManagement.Frontend.Web.Models.Helper;

namespace UserManagement.Frontend.Web.Helpers
{
    public class HttpHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        private readonly string _url = configuration.GetValue<string>("BackendHost") ?? throw new NullReferenceException("BackendHost cannot be null");

        /// <summary>
        /// Makes a POST request to the backend
        /// </summary>
        /// <typeparam name="T">The response expected</typeparam>
        /// <param name="subRoute">The sub route on the backend api</param>
        /// <param name="postData">The post data</param>
        public async Task<ErrorOr<T>> SendPostRequestAsync<T>(string subRoute, string postData) 
        {
            try
            {
                using var httpClient = new HttpClient();
                string? token = httpContextAccessor?.HttpContext?.Session.GetString("JwtToken");
                if (token is not null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var content = new StringContent(postData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(_url + subRoute, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();
                string responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ErrorOr<T>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (data?.Errors?[0].Code == "Unauthorized") throw new UnauthorizedAccessException();
                return data;
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
        public async Task<ErrorOr<T>> SendGetRequestAsync<T>(string subRoute)
        {
            try
            {
                using var httpClient = new HttpClient();
                string? token = httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
                if (token is not null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                HttpResponseMessage response = await httpClient.GetAsync(_url + subRoute);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();
                
                string responseContent = await response.Content.ReadAsStringAsync();
                var data=  JsonSerializer.Deserialize<ErrorOr<T>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (data?.Errors?[0].Code == "Unauthorized") throw new UnauthorizedAccessException();
                return data;
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
    }
}
