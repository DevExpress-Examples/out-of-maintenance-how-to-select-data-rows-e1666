Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports DevExpress.Wpf.Grid
Imports DXGrid_RowSelection.nwindDataSetTableAdapters

Namespace DXGrid_RowSelection
	Partial Public Class Window1
		Inherits Window
		Private unboundDataSource As New List(Of Boolean)()
		Public Sub New()
			InitializeComponent()
			Dim gridData As nwindDataSet.ProductsDataTable = New ProductsTableAdapter().GetData()
			FillUnboundDataSource(gridData.Rows.Count)
			grid.DataSource = gridData
		End Sub

		Private Sub FillUnboundDataSource(ByVal capacity As Integer)
			For i As Integer = 0 To capacity - 1
				unboundDataSource.Add(False)
			Next i
		End Sub

		Private Sub grid_CustomSummary(ByVal sender As Object, _
				ByVal e As DevExpress.Data.CustomSummaryEventArgs)
			If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Start Then
				e.TotalValue = 0
			End If
			If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Calculate Then
				If CBool(e.GetValue("Selected")) Then
					e.TotalValue = Convert.ToDecimal(e.TotalValue) + _
						Convert.ToDecimal(e.GetValue("UnitPrice"))
				End If
			End If
		End Sub

		Private Sub grid_CustomUnboundColumnData(ByVal sender As Object, _
				ByVal e As GridColumnDataEventArgs)
			If e.Column.FieldName = "Selected" Then
				If e.IsGetData Then
					e.Value = unboundDataSource(e.ListSourceRowIndex)
				End If
				If e.IsSetData Then
					unboundDataSource(e.ListSourceRowIndex) = CBool(e.Value)
				End If
			End If
		End Sub

		Private Sub TableView_MouseDoubleClick(ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs)
			Dim rowHandle As Integer = grid.View.GetRowHandleByMouseEventArgs(e)
			If grid.IsValidRowHandle(rowHandle) Then
				grid.SetCellValue(rowHandle, "Selected", _
					(Not CBool(grid.GetCellValue(rowHandle, "Selected"))))
				grid.RefreshData()
			End If
		End Sub
	End Class
End Namespace
