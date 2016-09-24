using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualProgrGUI.ViewModel.Commands
{
    public static class Base
    {
        /// <summary>
        /// Command when port was checked
        /// </summary>
        public static RoutedCommand CheckPort { get; private set; }

        static Base()
        {
            CheckPort = new RoutedCommand("CheckPort", typeof(Base));
        }
    }
}
