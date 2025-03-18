using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace Airlines
{
    public partial class bs
    {
        private DataTable table = new DataTable();
        private ICollectionView filteredPassengers;
        private string selectedPlaneCompany;

        public ICollectionView FilteredPassengers
        {
            get { return filteredPassengers; }
        }

        public bs()
        {
            InitializeComponent();
            CreateTable();
        }

        private void CreateTable()
        {
            table = ReadExcelFile(GVars.filePath);
            dtgPassengers.ItemsSource = table.DefaultView;
        }

        private static DataTable ReadExcelFile(string filePath)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Sheets[1];
            Range range = worksheet.UsedRange;

            DataTable dataTable = new DataTable();

            for (int col = 1; col <= range.Columns.Count; col++)
            {
                string columnName = (range.Cells[1, col] as Range).Value2?.ToString();
                if (string.IsNullOrEmpty(columnName))
                {
                    columnName = $"Column{col}";
                }

                DataColumn column = new DataColumn(columnName)
                {
                    DataType = typeof(string)
                };

                if (columnName == "arrivalDateTime" || columnName == "departureDateTime")
                    column.DataType = typeof(string);

                dataTable.Columns.Add(column);
            }

            int row = 2;
            while ((range.Cells[row, 1] as Range).Value2 != null)
            {
                DataRow dataRow = dataTable.NewRow();

                for (int col = 1; col <= range.Columns.Count; col++)
                {
                    var cellValue = (range.Cells[row, col] as Range).Value2;
                    string columnName = (range.Cells[1, col] as Range).Value2?.ToString();

                    if (columnName == "arrivalDateTime" || columnName == "departureDateTime")
                    {
                        if (cellValue != null)
                        {
                            if (cellValue is double oaDate)
                            {
                                DateTime date = DateTime.FromOADate(oaDate);
                                dataRow[col - 1] = date.ToString("dd/MM/yyyy HH:mm");
                            }
                            else if (cellValue is DateTime date)
                            {
                                dataRow[col - 1] = date.ToString("dd/MM/yyyy HH:mm");
                            }
                            else
                            {
                                dataRow[col - 1] = cellValue?.ToString();
                            }
                        }
                        else
                        {
                            dataRow[col - 1] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dataRow[col - 1] = cellValue?.ToString();
                    }
                }

                dataTable.Rows.Add(dataRow);
                row++;
            }

            workbook.Close(true);
            excelApp.Quit();

            return dataTable;
        }

        private void SaveChangesToExcel()
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(GVars.filePath);
            Worksheet worksheet = workbook.Sheets[1];
            Range range = worksheet.UsedRange;

            for (int row = 2; row <= table.Rows.Count + 1; row++)
            {
                DataRow dataRow = table.Rows[row - 2];
                for (int col = 1; col <= table.Columns.Count; col++)
                {
                    range.Cells[row, col].Value2 = dataRow[col - 1].ToString();
                }
            }

            workbook.Save();
            workbook.Close(true);
            excelApp.Quit();

            MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButton.OK);
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesToExcel();
        }

        private void FilterData()
        {
            string selectedPlaneCompany = cboPassengerSelection.SelectedValue?.ToString();
            string selectedStateFrom = cboCountry.SelectedValue?.ToString();
            string selectedStateIs = cboCountryIs.SelectedValue?.ToString();

            string filterExpression = "";

            if (int.TryParse(selectedPlaneCompany, out int planeCompany) && planeCompany != -1)
            {
                filterExpression += $"planeCompany = '{selectedPlaneCompany}'";
            }

            if (int.TryParse(selectedStateFrom, out int stateFrom) && stateFrom != -1)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND ";

                filterExpression += $"stateFrom = '{selectedStateFrom}'";
            }

            if (int.TryParse(selectedStateIs, out int stateIs) && stateIs != -1)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND ";

                filterExpression += $"stateTo = '{selectedStateIs}'";
            }

            if (!string.IsNullOrEmpty(filterExpression))
            {
                ((DataView)dtgPassengers.ItemsSource).RowFilter = filterExpression;
            }
            else
            {
                ((DataView)dtgPassengers.ItemsSource).RowFilter = string.Empty;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterData();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (DataRowView)dtgPassengers.SelectedItem;
            if (selectedItem != null)
            {
                int idToDelete = int.Parse(selectedItem.Row["Id"].ToString());
                DeleteUser(idToDelete);
            }
            else
            {
                MessageBox.Show("Please select a user from the list.");
            }
        }

        private void UpdateTable_Click(object sender, RoutedEventArgs e)
        {
            table = ReadExcelFile(GVars.filePath);
            dtgPassengers.ItemsSource = table.DefaultView;
            MessageBox.Show("Table updated successfully.", "Success", MessageBoxButton.OK);
        }

        private void DeleteUser(int userId)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(GVars.filePath);
            Worksheet worksheet = workbook.Sheets[1];
            Range range = worksheet.UsedRange;

            bool userFound = false;
            int row = 2;

            while ((range.Cells[row, 1] as Range).Value2 != null)
            {
                var cellValue = Convert.ToString((range.Cells[row, 1] as Range).Value2);
                if (int.TryParse(cellValue, out int currentUserId) && currentUserId == userId)
                {
                    worksheet.Rows[row].Delete();
                    userFound = true;
                    break;
                }
                row++;
            }

            if (userFound)
            {
                int currentRow = 2;
                while ((range.Cells[currentRow, 1] as Range).Value2 != null)
                {
                    worksheet.Cells[currentRow, 1].Value2 = currentRow - 1;
                    currentRow++;
                }

                workbook.Save();
                MessageBox.Show("User deleted and IDs reorganized successfully.", "Success", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("No users with the specified ID were found.", "User Not Found", MessageBoxButton.OK);
            }

            workbook.Close(true);
            excelApp.Quit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var newCompanyData = GVars.PlaneCompanyData;
            newCompanyData.Add(new KeyValuePair<int, string>(-1, "no selection"));
            cboPassengerSelection.ItemsSource = newCompanyData;

            var newCountryData = GVars.CountriesData;
            newCountryData.Add(new KeyValuePair<int, string>(-1, "no selection"));
            cboCountry.ItemsSource = newCountryData;

            cboCountryIs.ItemsSource = newCountryData; 
            cboCountryIs.SelectedValuePath = "Key";
            cboCountryIs.DisplayMemberPath = "Value";

            cbogrdplaneCompany.ItemsSource = GVars.PlaneCompanyData;
            cbogrdplaneCompany.SelectedValuePath = "Key";
            cbogrdplaneCompany.DisplayMemberPath = "Value";

            cbogrdFrom.ItemsSource = GVars.CountriesData;
            cbogrdFrom.SelectedValuePath = "Key";
            cbogrdFrom.DisplayMemberPath = "Value";

            cbogrdTo.ItemsSource = GVars.CountriesData;
            cbogrdTo.SelectedValuePath = "Key";
            cbogrdTo.DisplayMemberPath = "Value";

            CreateTable();
        }
    }
}
