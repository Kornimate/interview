using System.Collections.ObjectModel;
using System.Diagnostics;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// ViewModel for Requests Pages
    /// </summary>
    public class RequestsVM
    {
        protected readonly RequestsModel _model;
        public ObservableCollection<string> Logs { get; set; }

        public RequestsVM()
        {
            Logs = new ObservableCollection<string>();

            _model = new RequestsModel();
            _model.RequestCompleted += RequestCompleted;
        }

        /// <summary>
        /// Method to add request data to UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void RequestCompleted(object? sender, string message)
        {
            Logs.Add(message);
        }

        /// <summary>
        /// Method to make requests to API and bennchmark the results to print on UI
        /// </summary>
        /// <param name="numOfRequests"></param>
        public virtual void StartRequests(int numOfRequests)
        {
            Stopwatch sw = Stopwatch.StartNew();

            _model.MakeRequests(numOfRequests);

            sw.Stop();

            Logs.Add($"[info] requests: {numOfRequests}");
            Logs.Add($"[info] elapsed time: {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}
