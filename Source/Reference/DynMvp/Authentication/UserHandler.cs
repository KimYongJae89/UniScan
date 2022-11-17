using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Authentication
{
    public delegate void UserChangedDelegate();
    public interface IUserHandlerListener
    {
        void UserChanged();
    }

    public class UserHandler
    {
        private UserSource userSource = null;
        public UserSource UserSource
        {
            get { return userSource; }
        }

        private UserList userList = new UserList();
        public UserList UserList
        {
            get { return userList; }
        }

        private User currentUser = new User();
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                    ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.User.Information, ErrorLevel.Info,
                        "User", "User Login: [{0}]", new string[] { currentUser.Id }, ""));
                    foreach (IUserHandlerListener listener in listenerList)
                        listener.UserChanged();
                }
            }
        }

        List<IUserHandlerListener> listenerList = new List<IUserHandlerListener>();

        static UserHandler instance = null;
        public static UserHandler Instance()
        {
            if (instance == null)
            {
                instance = new UserHandler();
            }

            return instance;
        }

        public void Initialize(string fileName)
        {
            userSource = new UserTextSource(fileName);
            userSource.LoadUserList(userList);
        }

        public void AddListener(IUserHandlerListener listener)
        {
            this.listenerList.Add(listener);
            listener.UserChanged();
        }

        public void RemoveListener(IUserHandlerListener listener)
        {
            this.listenerList.Remove(listener);
        }

        public User GetUser(string userId)
        {
            return userList.GetUser(userId);
        }

        public User GetUser(string userId, string password)
        {
            return userList.GetUser(userId, password);
        }

        public bool DoTask(string taskFullName, TaskDelegate job)
        {
            bool result = TaskAuthManager.Instance().DoTask(taskFullName, currentUser, false, job);
            if (result == false)
            {
                PopupForm form = new PopupForm();
                form.Show("", StringManager.GetString("Permission is denied"));
            }

            return result;
        }

        public bool IsGranted(string taskFullName)
        {
            return TaskAuthManager.Instance().IsGranted(taskFullName, currentUser);
        }

        public bool DoLicenseTask(string taskFullName, TaskDelegate taskDelegate)
        {
            bool result = TaskAuthManager.Instance().DoTask(taskFullName, currentUser, true, taskDelegate);
            if (result == false)
            {
                PopupForm form = new PopupForm();
                form.Show("", StringManager.GetString("Permission is denied or License is not available"));
            }

            return result;
        }

        public void SaveUserList()
        {
            userSource.SaveUserList(userList);
        }
    }
}
