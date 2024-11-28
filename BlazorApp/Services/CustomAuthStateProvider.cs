using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;
using BlazorApp.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService localStorage;

        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

            var accessToken = localStorage.GetItem<string>("accessToken");
            if (accessToken != null ) 
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
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
                var response = await httpClient.PostAsJsonAsync("login", new { email, password });
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    var accessToken = jsonResponse["accessToken"].ToString();
                    var refreshToken = jsonResponse["refreshToken"].ToString();

                    localStorage.SetItem("accessToken", accessToken);
                    localStorage.SetItem("refreshToken", refreshToken);

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

        public async Task Logout()
        {
            localStorage.RemoveItem("accessToken");
            localStorage.RemoveItem("refreshToken");
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<FormResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("register", registerDto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await LoginAsync(registerDto.Email, registerDto.Password);
                    return loginResponse;
                }
                
                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(strResponse);
                var jsonResponse = JsonNode.Parse(strResponse);
                var errorsObject = jsonResponse!["errors"]!.AsObject();
                var errorsList = new List<string>();
                foreach (var error in errorsObject)
                {
                    errorsList.Add(error.Value![0]!.ToString());
                }

                var formResult = new FormResult 
                { 
                    Succeeded = false, 
                    Error = errorsList.ToArray() 
                };

                return formResult;
            }
            catch (Exception ex)
            {
                return new FormResult { Succeeded = false, Error = ["Connection Error"] };
            }
        }
    }

    public class FormResult
    {
        public bool Succeeded { get; set; }
        public string[] Error { get; set; } = [];
    }
}
