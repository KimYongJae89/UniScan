using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace AutoParamTuner.Base
{
    internal class Command : ICommand
    {
        private readonly Action<Control, object> Action;
        private readonly Func<bool> CanExcute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<Control, object> action)
        {
            this.Action = action;
        }

        public Command(Action<Control, object> action, Func<bool> canExcute)
        {
            this.Action = action;
            this.CanExcute = canExcute;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExcute == null)
                return true;

            return this.CanExcute();
        }

        public void Execute(Control parent, object parameter)
        {
            this.Action(parent, parameter);
        }

        public void Execute(object parameter)
        {
            this.Action(null, parameter);
        }

        public static void ExcuteCommand(Control parent, Command command, object parameter = null)
        {
            try
            {
                command.Execute(parent, parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(parent, ex.Message, ex.GetType().Name);
            }
        }

    }
}
