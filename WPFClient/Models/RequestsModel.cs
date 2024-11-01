using System.Net.Http;

namespace WPFClient.Models
{
    /// <summary>
    /// Model class for Requests Page
    /// </summary>
    public class RequestsModel : ModelBase
    {

        public event EventHandler<string>? RequestCompleted;

        public RequestsModel() : base() { }

        /// <summary>
        /// Method to call API Get endpoint for fixed requests
        /// </summary>
        /// <param name="numOfRequests"></param>
        public void MakeRequests(int numOfRequests)
        {
            HttpResponseMessage? response = null;

            for (int i = 0; i < numOfRequests; i++)
            {
                try
                {
                    response = _client.GetAsync($"/api/Users/1").GetAwaiter().GetResult();

                    RequestCompleted?.Invoke(this, $"[info][{Environment.CurrentManagedThreadId}] request completed with status: {(int)response.StatusCode}");
                }
                catch (Exception)
                {
                    var code = response == null ? 000 : (int)response.StatusCode;
                    RequestCompleted?.Invoke(this, $"[info] request completed with status: {code}");
                } 
            }
        }
    }
}
