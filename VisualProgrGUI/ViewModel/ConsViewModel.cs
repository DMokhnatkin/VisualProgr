using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrGUI.ViewModel
{
    public class ConsViewModel
    {
        public static ObservableCollection<string> Messages { get; private set; }

        /// <summary>
        /// Prints message
        /// </summary>
        /// <param name="message"></param>
        public static void Print(string message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Prints error message
        /// </summary>
        /// <param name="message"></param>
        public static void PrintError(string message)
        {
            Messages.Add("[Error] " + message);
        }

        static ConsViewModel()
        {
            Messages = new ObservableCollection<string>();
        }
    }
}
