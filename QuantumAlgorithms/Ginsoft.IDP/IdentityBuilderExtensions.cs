using Ginsoft.IDP.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ginsoft.IDP
{
    public static class IdentityBuilderExtensions
    {
        public static IIdentityServerBuilder AddGinsoftUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IGinsoftUserRepository, GinsoftUserRepository>();
            builder.AddProfileService<GinsoftUserProfileService>();
            return builder;
        }
    }
}
