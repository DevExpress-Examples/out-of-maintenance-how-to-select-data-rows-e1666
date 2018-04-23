using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Grid;
using DXGrid_RowSelection.nwindDataSetTableAdapters;

namespace DXGrid_RowSelection {
    public partial class Window1 : Window {
        private List<bool> unboundDataSource = new List<bool>();
        public Window1() {
            InitializeComponent();
            nwindDataSet.ProductsDataTable gridData = new ProductsTableAdapter().GetData();
            FillUnboundDataSource(gridData.Rows.Count);
            grid.DataSource = gridData;
        }

        private void FillUnboundDataSource(int capacity) {
            for (int i = 0; i < capacity; i++)
                unboundDataSource.Add(false);
        }

        private void grid_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e) {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                e.TotalValue = 0;
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                if ((bool)e.GetValue("Selected"))
                    e.TotalValue = Convert.ToDecimal(e.TotalValue) + 
                        Convert.ToDecimal(e.GetValue("UnitPrice"));
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e) {
            if (e.Column.FieldName == "Selected") {
                if (e.IsGetData)
                    e.Value = unboundDataSource[e.ListSourceRowIndex];
                if (e.IsSetData)
                    unboundDataSource[e.ListSourceRowIndex] = (bool)e.Value;
            }
        }

        private void TableView_MouseDoubleClick(object sender, 
                System.Windows.Input.MouseButtonEventArgs e) {
            int rowHandle = grid.View.GetRowHandleByMouseEventArgs(e);
            if (grid.IsValidRowHandle(rowHandle)) {
                grid.SetCellValue(rowHandle, "Selected", 
                        !(bool)grid.GetCellValue(rowHandle, "Selected"));
                grid.RefreshData();
            }
        }
    }
}
