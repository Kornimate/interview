using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPFClient.Views;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// Main window instance class
    /// </summary>
    class MainWindowVM : ObservableObject
    {

        private readonly static int REQUESTS_NUM = GetRequestsNumber();

        private CancellationTokenSource? cts;

        #region Commands
        public ICommand OpenCreateUserPageCommand { get; private set; }
        public ICommand OpenListUsersPageCommand { get; private set; }
        public ICommand OpenSynchronizedRequestsPageCommand { get; private set; }
        public ICommand OpenParallelRequestsPageCommand { get; private set; }

        #endregion

        public MainWindowVM()
        {
            OpenCreateUserPageCommand = new RelayCommand(OpenCreateUserPage);
            OpenListUsersPageCommand = new RelayCommand(OpenListUsersPage);
            OpenSynchronizedRequestsPageCommand = new RelayCommand(OpenSynchronizedRequestsPage);
            OpenParallelRequestsPageCommand = new RelayCommand(OpenParallelRequestsPage);
        }

        #region Private Instance Methods

        /// <summary>
        /// Parallelly start Requests pages to reach parallel execution
        /// </summary>
        private void OpenParallelRequestsPage()
        {
            var paramData = ComputeRequestsNumberForParallelExecution(REQUESTS_NUM);

            cts = new CancellationTokenSource(); //not to run too long
            cts.CancelAfter(TimeSpan.FromSeconds(10));

            List<Thread> threads = [];

            try
            {
                Task.Run(() =>
                {
                    CreateThreads(ref threads, paramData);

                    JoinThreads(ref threads);

                }, cts.Token).Wait();
            }
            catch (Exception)
            {
                foreach (Thread t in threads)
                {
                    if (t != null && t.IsAlive)
                        t.Interrupt();
                }
            }
            finally
            {
                cts.Dispose();
            }
        }

        /// <summary>
        /// Method to create threads for parallel execution
        /// </summary>
        /// <param name="threads"></param>
        /// <param name="paramData"></param>
        private void CreateThreads(ref List<Thread> threads, (int, int) paramData)
        {
            for (int i = 0; i < paramData.Item1; i++)
            {
                Thread t = new(new ThreadStart(() => OpenRequestsPage(paramData.Item2)));
                t.SetApartmentState(ApartmentState.STA);

                if (t.IsAlive)
                    threads.Add(t);

                t.Start();
            }
        }

        /// <summary>
        /// Method to join threads
        /// </summary>
        /// <param name="threads"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void JoinThreads(ref List<Thread> threads)
        {
            if (cts == null)
                throw new InvalidOperationException("cancellation token must be set");

            foreach (Thread t in threads)
            {
                if (cts!.Token.IsCancellationRequested)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }

                t.Join(TimeSpan.FromSeconds(5));
            }
        }

        /// <summary>
        /// Open Requests page synchronouosly
        /// </summary>
        private void OpenSynchronizedRequestsPage()
        {
            OpenRequestsPage(REQUESTS_NUM);
        }

        /// <summary>
        /// Open list users page
        /// </summary>
        private void OpenListUsersPage()
        {
            var listUsersPage = new ListUsers();
            listUsersPage.ShowDialog();
        }

        /// <summary>
        /// Open create user page
        /// </summary>
        private void OpenCreateUserPage()
        {
            var createUserPage = new CreateUser();
            createUserPage.ShowDialog();
        }

        /// <summary>
        /// Method to open 1 requests page
        /// </summary>
        /// <param name="numOfRequests"></param>
        /// <param name="isParallel"></param>
        private void OpenRequestsPage(int numOfRequests, bool isParallel = false)
        {
            Window requests = null!;
            try
            {
                requests = new Requests(numOfRequests, isParallel);
                requests.Show();
                Dispatcher.Run();
            }
            catch (Exception)
            {
                requests?.Close();
                requests?.Dispatcher.InvokeShutdown();
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfRequests"></param>
        /// <returns>Tuple indicating the number of threads and the number of API calls per thread</returns>
        private static (int, int) ComputeRequestsNumberForParallelExecution(int numOfRequests)
        {
            int maxThreads = Math.Min((int)Math.Floor((double)(Environment.ProcessorCount / 2)), 6); //get the number of processors half or 6 for upper bound

            var oneThread = (int)(numOfRequests / maxThreads); //calculate number of requests per thread

            return (maxThreads, oneThread);
        }

        /// <summary>
        /// Method to get number of requests from config file
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int GetRequestsNumber()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["Requests"]?.ToString() ?? "1000");
            }
            catch
            {
                return 1000;
            }
        }
    }
}
