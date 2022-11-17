﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using UniScanS.Data;
using UniScanS.Data.Model;
using System.Reflection;
using DynMvp.Authentication;

namespace UniScanS.UI.Report
{
    public partial class ReportPage : UserControl, IMainTabPage, IMultiLanguageSupport
    {
        bool onUpdateData = false;
        IReportPanel reportPanel;

        public ReportPage()
        {
            onUpdateData = true;

            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            labelStart.Text = StringManager.GetString(this.GetType().FullName, labelStart.Text);
            labelEnd.Text = StringManager.GetString(this.GetType().FullName, labelEnd.Text);

            reportPanel = SystemManager.Instance().UiChanger.CreateReportPanel();
            reportContainer.Controls.Add((Control)reportPanel);
            
            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";

            onUpdateData = false;
        }
        
        public void RefreshList()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            modelList.Rows.Clear();
            lotList.Rows.Clear();

            List<ModelDescriptionS> modelDescriptionList = new List<ModelDescriptionS>();
            List<ProductionS> productionList = new List<ProductionS>();

            foreach (ProductionS production in SystemManager.Instance().ProductionManager.List)
            {
                if (production.Total == 0)
                    continue;

                ProductionS productionG = (ProductionS)production;

                if (productionG.StartTime < startDate.Value.Date || productionG.StartTime > endDate.Value.Date.AddDays(1))
                    continue;

                if (productionG.Name.Contains(findModelName.Text) == false)
                    continue;

                ModelDescriptionS modelDescription = new ModelDescriptionS();
                modelDescription.Name = productionG.Name;
                modelDescription.Paste = productionG.Paste;
                modelDescription.Thickness = Convert.ToSingle(productionG.Thickness);

                ModelDescriptionS md = (ModelDescriptionS)SystemManager.Instance().ModelManager.GetModelDescription(modelDescription);
                if (md != null)
                {
                    if (modelDescriptionList.Contains(md) == false)
                        modelDescriptionList.Add(md);
                }

                if (productionG.LotNo.Contains(findLotName.Text) == false)
                    continue;

                productionList.Add(productionG);
            }

            int index = 1;
            foreach (ModelDescriptionS md in modelDescriptionList)
            {
                int rowIndex = modelList.Rows.Add(index, md.Name, md.Thickness, md.Paste.ToString());
                modelList.Rows[rowIndex].Tag = md;
                index++;
            }

            foreach (ProductionS p in productionList)
            {
                int rowIndex = lotList.Rows.Add(p.StartTime, p.Name, p.LotNo);
                lotList.Rows[rowIndex].Tag = p;
            }

            modelList.Sort(modelList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            lotList.Sort(lotList.Columns[0], System.ComponentModel.ListSortDirection.Descending);

            totalModel.Text = modelDescriptionList.Count.ToString();
            totalLot.Text = productionList.Count.ToString();

            onUpdateData = false;
        }
        
        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (lotList.SelectedRows.Count < 1)
                return;

            ProductionS production = (ProductionS)lotList.SelectedRows[0].Tag;
            reportPanel.Search(production);
        }

        private void findModelName_TextChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void findLotName_TextChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }
        public void EnableControls() { }

        public void UpdateLanguage()
        {
            onUpdateData = true;

            StringManager.UpdateString(this);

            onUpdateData= false;
        }

        private void SelectModelList()
        {
            if (onUpdateData == true)
                return;

            if (modelList.SelectedRows.Count == 0)
                return;
            
            ModelDescriptionS md = (ModelDescriptionS)modelList.SelectedRows[0].Tag;
            findModelName.Text = md.Name;
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            SelectModelList();
        }

        private void modelList_Click(object sender, EventArgs e)
        {
            SelectModelList();
        }

        private void ReportPage_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
                RefreshList();
        }

        public void EnableControls(UserType userType)
        {
            throw new NotImplementedException();
        }
    }
}
