namespace AuthArmor.Sdk.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel;

    public static class ConfigureService
    {
        public static IServiceCollection AddAuthArmorSdkServices(this IServiceCollection sCollection)
        {
            sCollection.AddSingleton<Auth.AuthService>();
            sCollection.AddSingleton<User.UserService>();
            return sCollection;
        }
    }
}
