using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Infrastructure.Security.FacebookApiResponses;

namespace Infrastructure.Security
{
    public class FacebookAccessor : IFacebookAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<FacebookAppSettings> _config;

        public FacebookAccessor(IOptions<FacebookAppSettings> config)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _config = config;
        }

        public async Task<FacebookUserInfo> FacebookLogin(string accessToken)
        {
            var appAccessTokenResponse = await _httpClient.GetStringAsync($"/oauth/access_token?client_id={_config.Value.AppId}&client_secret={_config.Value.AppSecret}&grant_type=client_credentials");

            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

            var userAccessTokenValidationResponse = await _httpClient.GetStringAsync($"debug_token?input_token={accessToken}&access_token={appAccessToken.AccessToken}");

            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return null;
            }

            var userInfoResponse = await _httpClient.GetStringAsync($"me?fields=id,email,first_name,picture&access_token={accessToken}");

            var userInfo = JsonConvert.DeserializeObject<FacebookUserInfo>(userInfoResponse);

            return userInfo;
        }
    }
}