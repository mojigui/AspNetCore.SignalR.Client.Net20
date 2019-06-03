using AspNetCore.SignalR.Client.Net20.Connection;

namespace AspNetCore.SignalR.Client.Net20.Client
{
    /// <summary>
    /// A factory for creating <see cref="HttpConnection"/> instances.
    /// </summary>
    public class HttpConnectionFactory
    {
        private readonly HttpConnectionOptions _httpConnectionOptions;

        public HttpConnectionFactory(HttpConnectionOptions httpConnectionOptions)
        {
            _httpConnectionOptions = httpConnectionOptions;
        }

        public HttpConnection Connect(TransferFormat transferFormat)
        {
            var connection = new HttpConnection(_httpConnectionOptions);
            try
            {
                connection.Start(transferFormat);
                return connection;
            }
            catch
            {
                // Make sure the connection is disposed, in case it allocated any resources before failing.
                connection.Dispose();
                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose(HttpConnection connection)
        {
            connection.Dispose();
        }
    }
}
