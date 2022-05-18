using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Markdig;
using Refit;
using System.Threading;
using System.Net.Http.Headers;
using System.Security.Claims;
using Fluxor;
using Frontend.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Frontend
{
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //Auth
            builder.Services.AddOidcAuthentication(opts =>{
                builder.Configuration.Bind("Auth0", opts.ProviderOptions);
                opts.UserOptions.RoleClaim = "permissions";
            }).AddAccountClaimsPrincipalFactory<ClaimsPrincipalFactory<RemoteUserAccount>>();

            builder.Services.AddScoped<AuthMessageHandler>();

            // API Clients
            builder.Services.AddRefitClient<IBlogData>()
                            .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://127.0.0.1:8787/api"); })
                            .AddHttpMessageHandler<AuthMessageHandler>();

            builder.Services.AddRefitClient<IStorageData>()
                            .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://127.0.0.1:8787/api"); })
                            .AddHttpMessageHandler<AuthMessageHandler>();

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://127.0.0.1:7878/api") });
            builder.Services.AddMudServices();

            builder.Services.AddSingleton(new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
            //builder.Services.AddSingleton<StateContainer>();
            builder.Services.AddFluxor(opts => {
                opts.ScanAssemblies(typeof(Program).Assembly)
                    .UseReduxDevTools()
                    .UseRouting();
            });

            builder.Services.AddScoped<StateFront>();

            await builder.Build().RunAsync();
        }
    }
    
    public class ClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount> where TAccount : RemoteUserAccount {
        
        public ClaimsPrincipalFactory(IAccessTokenProviderAccessor provider) : base(provider) { }
        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            var claimsIdentity = (ClaimsIdentity)user.Identity!;
            var tokenResult = await TokenProvider.RequestAccessToken();

            if (!tokenResult.TryGetToken(out AccessToken token) ||
                (new JwtSecurityTokenHandler().ReadToken(token.Value) is not JwtSecurityToken tokenS)) {
                return user;
            }
            claimsIdentity.AddClaims(tokenS.Claims.Where(c => c.Type == "permissions"));
            return user;
        }
    }
    
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler {
        public CustomAuthorizationMessageHandler( IAccessTokenProvider provider, NavigationManager navigationManager)
            : base(provider, navigationManager) {
                ConfigureHandler( 
                    authorizedUrls: new[] { "https://127.0.0.1:8787/api" }, scopes: new[] { "openid", "profile"}
                );
            }
    }

    public class AuthMessageHandler : DelegatingHandler{
        private readonly IAccessTokenProvider _provider;

        public AuthMessageHandler(IAccessTokenProvider provider) { 
            _provider = provider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken){
            var authHeader = request.Headers.Authorization;
            if (request.Headers.Authorization == null) return await base.SendAsync(request, cancellationToken);
            var tokenResult = await _provider.RequestAccessToken();
            if (authHeader is not null && tokenResult.TryGetToken(out AccessToken token)) {
                request.Headers.Authorization = new AuthenticationHeaderValue(authHeader.Scheme, token.Value);
            }
            return await base.SendAsync(request, cancellationToken);
        }
        
    }
    
    
    public class ApiHttpClient{
        public HttpClient httpClient;

        public ApiHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
