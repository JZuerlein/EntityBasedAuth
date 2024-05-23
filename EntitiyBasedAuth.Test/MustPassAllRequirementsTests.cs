using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiyBasedAuth.Test
{
    public class MustPassAllRequirements : IDisposable
    {
        private readonly IAuthorizationService _authorizationService;

        public MustPassAllRequirements()
        {
            _authorizationService = BuildAuthorizationService(services =>
            {
                services.AddAuthorizationCore(options =>
                {
                    var reqs = new List<IAuthorizationRequirement>();
                    reqs.Add(new RolesAuthorizationRequirement(new string[] { "HumanResources" }));
                    reqs.Add(new RolesAuthorizationRequirement(new string[] { "IT" }));

                    var newPolicy = new AuthorizationPolicy(reqs, new string[] { });

                    options.AddPolicy("PassBoth", newPolicy);
                });
            });
        }

        private IAuthorizationService BuildAuthorizationService(Action<IServiceCollection> setupServices = null)
        {
            var services = new ServiceCollection();
            services.AddAuthorizationCore();
            services.AddLogging();
            services.AddOptions();
            setupServices?.Invoke(services);
            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }

        public void Dispose()
        {
        }
    }

}
