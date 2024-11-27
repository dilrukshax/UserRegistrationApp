using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;

        public CustomAuthStateProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            /*
            //var user = new ClaimsPrincipal(new ClaimsIdentity());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "mrfibuli"),
            }, "test auth type"));

            return Task.FromResult(new AuthenticationState(user));
            */

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            try
            {
                var response = await httpClient.GetAsync("manage/info");
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    var email = jsonResponse["email"].ToString();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Email, email),
                    };
                    var identity = new ClaimsIdentity(claims, "Token");
                    user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new AuthenticationState(user);
        }


        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    var accessToken = jsonResponse["accessToken"].ToString();
                    var refreshToken = jsonResponse["refreshToken"].ToString();

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    return new FormResult { Succeeded = true };
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<string[]>();
                    return new FormResult { Succeeded = false, Error = ["Bad Email or Password"] };
                }
            }
            catch (Exception ex)
            {
                return new FormResult { Succeeded = false, Error = ["Connection Error"] };
            }
        }

        public class FormResult
        {
            public bool Succeeded { get; set; }
            public string[] Error { get; set; } = [];
        }
    }
}
