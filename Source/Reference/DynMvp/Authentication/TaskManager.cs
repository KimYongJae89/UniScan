using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using DynMvp.Base;

namespace DynMvp.Authentication
{
    public class TaskItem
    {
        int code;
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public TaskItem(int code, string name)
        {
            this.code = code;
            this.name = name;
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", code.ToString(), name);
        }
    }

    public class TaskItemTable
    {
        List<TaskItem> taskItemList = new List<TaskItem>();

        public IEnumerator<TaskItem> GetEnumerator()
        {
            return taskItemList.GetEnumerator();
        }

        public void Reset()
        {
            taskItemList.Clear();
        }

        public TaskItem AddTaskItem(int code, string name)
        {
            TaskItem task = GetTaskItem(code);
            if (task != null)
                return null;

            task = new TaskItem(code, name);
            taskItemList.Add(task);

            return task;
        }

        public TaskItem GetTaskItem(string name)
        {
            foreach (TaskItem taskItem in taskItemList)
            {
                if (taskItem.Name == name)
                    return taskItem;
            }

            return null;
        }

        public TaskItem GetTaskItem(int code)
        {
            foreach (TaskItem taskItem in taskItemList)
            {
                if (taskItem.Code == code)
                    return taskItem;
            }

            return null;
        }

        public int GetTaskItemCode(string name)
        {
            TaskItem taskItem = GetTaskItem(name);
            if (taskItem != null)
                return taskItem.Code;
            
            return GetNextTaskItemCode();
        }

        public int GetNextTaskItemCode()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (GetTaskItem(i) == null)
                    return i;
            }

            throw new TooManyItemsException();
        }

        public void Save(string filePath)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement taskItemListElement = xmlDocument.CreateElement("", "TaskItemList", "");
            xmlDocument.AppendChild(taskItemListElement);

            foreach (TaskItem taskItem in taskItemList)
            {
                XmlElement taskItemElement = xmlDocument.CreateElement("", "TaskItem", "");
                taskItemListElement.AppendChild(taskItemElement);

                XmlHelper.SetValue(taskItemElement, "Code", taskItem.Code.ToString());
                XmlHelper.SetValue(taskItemElement, "Name", taskItem.Name);
            }

            xmlDocument.Save(filePath);
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath) == false)
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);

            XmlElement taskItemListElement = xmlDocument.DocumentElement;
            foreach (XmlElement taskItemElement in taskItemListElement)
            {
                if (taskItemElement.Name == "TaskItem")
                {
                    int code = Convert.ToInt32(XmlHelper.GetValue(taskItemElement, "Code", "0"));
                    string name = XmlHelper.GetValue(taskItemElement, "Name", "");

                    taskItemList.Add(new TaskItem(code, name));
                }
            }
        }
    }

    public delegate void TaskDelegate();

    public class TaskAuthManager
    {
        const int UnknownTaskItemCode = -1;

        static TaskAuthManager instance = null;
        public static TaskAuthManager Instance()
        {
            if (instance == null)
            {
                instance = new TaskAuthManager();
            }

            return instance;
        }

        TaskItemTable taskItemTable = new TaskItemTable();
        public TaskItemTable TaskItemTable
        {
            get { return taskItemTable; }
        }

        public bool DoTask(string taskName, User user, bool checkLicense, TaskDelegate taskDelegate)
        {
            if (user.IsSuperAccount == true)
            {
                taskDelegate();
                return true;
            }

            TaskItem taskItem = taskItemTable.GetTaskItem(taskName);
            if (taskItem == null)
                return false;

            if (checkLicense)
            {
                if (LicenseManager.IsAvailable(taskItem.Name) == false)
                    return false;
            }

            if (user.permissionControl.IsGranted(taskItem.Code))
            {
                taskDelegate();
                return true;
            }

            return false;
        }

        public bool IsGranted(string taskItemName, User user)
        {
            if (user.IsSuperAccount == true)
                return true;

            TaskItem taskItem;
            taskItem = taskItemTable.GetTaskItem(taskItemName);

            if (taskItem == null)
                return false;

            return user.permissionControl.IsGranted(taskItem.Code);
        }

        public TaskItem AddTaskItem(string taskName)
        {
            TaskItem taskItem = taskItemTable.GetTaskItem(taskName);
            if (taskItem == null)
            {
                int taskItemCode = taskItemTable.GetNextTaskItemCode();
                return taskItemTable.AddTaskItem(taskItemCode, taskName);
            }
            else
                return null;
        }

        public void SaveTaskAuthTable(string filePath)
        {
            taskItemTable.Save(filePath);
        }

        public void LoadTaskAuthTable(string filePath)
        {
            taskItemTable.Load(filePath);
        }
    }
}
