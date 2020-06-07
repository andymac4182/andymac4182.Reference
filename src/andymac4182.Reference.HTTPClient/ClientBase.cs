namespace andymac4182.Reference.HTTPClient
{
    internal abstract class ClientBase
    {
        public Func<Task<string>>? RetrieveAuthorizationToken { get; set; }

        // Called by implementing swagger client classes
        protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();

            if (RetrieveAuthorizationToken != null)
            {
                var token = await RetrieveAuthorizationToken();
                msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return msg;
        }

    }
}