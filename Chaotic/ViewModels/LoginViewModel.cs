using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace Chaotic.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IConfiguration _config;
        public LoginViewModel(IConfiguration config)
        {
            _config = config;
            var t = config["Secrets:AniListApiSecret"];
            Console.WriteLine(t);
        }
    }
}
