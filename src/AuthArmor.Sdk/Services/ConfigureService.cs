namespace AuthArmor.Sdk.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel;

    public static class ConfigureService
    {
        public static IServiceCollection AddAuthArmorSdkServices(this IServiceCollection sCollection)
        {
            sCollection.AddTransient<Auth.AuthService>();
            sCollection.AddTransient<User.UserService>();
            return sCollection;
        }
    }
}
