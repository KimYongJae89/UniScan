using DynMvp.Data;
using DynMvp.UI.Touch;
using Infragistics.Win.UltraWinTabControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base;
using UniEye.Base.UI;

namespace UniScanS.Common.UI
{
    public abstract class UiController
    {
        IUiControlPanel uiControlPanel;

        public void SetUiControlPanel(IUiControlPanel uiControlPanel)
        {
            this.uiControlPanel = uiControlPanel;
        }

        public virtual void UpdateTab(bool trained)
        {
            if (uiControlPanel == null)
                return;

            uiControlPanel.UpdateTab(trained);
        }
        
        protected void EnableTab(string key)
        {
            if (uiControlPanel == null)
                return;

            uiControlPanel.EnableTab(key);
        }

        protected void DisableTab(string key)
        {
            if (uiControlPanel == null)
                return;

            uiControlPanel.DisableTab(key);
        }

        public virtual void ChangeTab(string key)
        {
            if (uiControlPanel == null)
                return;

            if (uiControlPanel.GetCurrentTabKey() == key)
                return;

            uiControlPanel.ChangeTab(key);
        }

        public virtual void TabChanged(string key)
        {

        }
    }
}
