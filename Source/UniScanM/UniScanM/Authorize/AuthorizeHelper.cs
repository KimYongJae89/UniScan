using DynMvp.Authentication;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanM.Authorize
{
    static public class AuthorizeHelper
    {
        public static User Authorize()
        {
            LogInForm form = new LogInForm();
            if (form.ShowDialog() == DialogResult.OK)
                return form.LogInUser;
            //return (((int)form.LogInUser.UserType & (int)userType) > 0);

            return null;
        }

        public static bool Authorize(UserType userType)
        {
            LogInForm form = new LogInForm();
            if (form.ShowDialog() == DialogResult.OK)
                return (((int)form.LogInUser.UserType & (int)userType) > 0);

            return false;
        }

    }
}
