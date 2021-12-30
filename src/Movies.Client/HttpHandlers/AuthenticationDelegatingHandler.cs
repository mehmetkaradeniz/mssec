using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClientCredentialsTokenRequest _clientCredentialsTokenRequest;

        public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _clientCredentialsTokenRequest = clientCredentialsTokenRequest ?? throw new ArgumentNullException(nameof(clientCredentialsTokenRequest));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("IDPClient");
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_clientCredentialsTokenRequest);
            if (tokenResponse.IsError)
                throw new HttpRequestException("Something went wrong while requesting the access token.");

            request.SetBearerToken(tokenResponse.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
