using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualProgrGUI.ViewModel.Commands;

namespace VisualProgrGUI.ViewModel
{
    public class AuthorizationWindowViewModel : DependencyObject
    {
        public SimpleCommand AuthorizateCommand { get; private set; }

        public SimpleCommand CreateUserCommand { get; private set; }

        private UdpClient _server;
        private IPEndPoint _sender;

        #region Dependency property
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(AuthorizationWindowViewModel), new PropertyMetadata(""));

        #endregion

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public AuthorizationWindowViewModel()
        {
            AuthorizateCommand = new SimpleCommand(AuthorizateExcecuted);
            CreateUserCommand = new SimpleCommand(CreateUserExcecuted);

            _server = new UdpClient("127.0.0.1", 9050);
            _sender = new IPEndPoint(IPAddress.Any, 0);
        }

        /// <summary>
        /// Authorizate
        /// </summary>
        void AuthorizateExcecuted(object parameter)
        {
            Tuple<string, string, Window> par = parameter as Tuple<string, string, Window>;

            byte[] data = new byte[1024];
            string stringData;

            string passHash = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(par.Item2)));

            data = Encoding.UTF8.GetBytes("authorizate+" + par.Item1 + "+" + passHash);
            _server.Send(data, data.Length);

            // Get answer from server
            data = _server.Receive(ref _sender);

            stringData = Encoding.UTF8.GetString(data, 0, data.Length);

            if (stringData == "success")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                (parameter as Tuple<string, string, Window>).Item3.Close();
            }
            else
            {
                Message = "Invalid user name or password";
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        void CreateUserExcecuted(object parameter)
        {
            Tuple<string, string> par = parameter as Tuple<string, string>;

            byte[] data = new byte[1024];
            string stringData;

            string passHash = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(par.Item2)));

            data = Encoding.UTF8.GetBytes("create+" + par.Item1 + "+" + passHash);
            _server.Send(data, data.Length);

            // Get answer from server
            data = _server.Receive(ref _sender);

            stringData = Encoding.UTF8.GetString(data, 0, data.Length);

            if (stringData == "success")
            {
                Message = "User created";
            }
            else
            {
                Message = "User exists";
            }
        }
    }
}
