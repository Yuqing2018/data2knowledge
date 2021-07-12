using MusicKG.WebApi.ClientWrapper.Extensions;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicKG.WebApi.ClientWrapper
{
    public class UserWrapper
    {
        public static async Task<UserLoginViewModel> LoginAsync(HttpClient httpClient, string url, string userName, string password)
        {
            var loginUrl = $"{url}/api/User/Login";

            var result = await httpClient.PostAsBodyAsync<UserLoginBindingModel, UserLoginViewModel>(loginUrl, new UserLoginBindingModel
            {
                Name = userName,
                Password = password
            });

            return result;
        }

        public static async Task LogoutAsync(HttpClient httpClient, string url, string token)
        {
            var logoutUrl = $"{url}/api/User/Logout";

            httpClient.AddToken(token);

            await httpClient.PostAsync(logoutUrl, null);
        }
    }
}
