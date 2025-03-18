using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Application = Microsoft.Office.Interop.Excel.Application;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace Airlines
{
    public static class GVars
    {
        public static string filePath = @"C:\Users\abel.alvarez\Documents\GitHub\Airlines\basededatos.xlsx";
        public static List<KeyValuePair<int, string>> PlaneCompanyData;
        public static List<KeyValuePair<int, string>> CountriesData;
    }

    public partial class MainWindow
    {
        System.Data.DataTable table = new System.Data.DataTable();
        private DataRow Information;

        public MainWindow()
        {
            InitializeComponent();
            CreateTable();
        }

        private void CreateTable()
        {
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Plane Company", typeof(string));
            table.Columns.Add("State From", typeof(int));
            table.Columns.Add("State To", typeof(int));
            table.Columns.Add("Arrival Date Time", typeof(DateTime));
            table.Columns.Add("Departure Date Time", typeof(DateTime));
            table.Columns.Add("Surname", typeof(string));
            table.Columns.Add("Contact Email", typeof(string));
            table.Columns.Add("Contact Telephone", typeof(string));
            table.Columns.Add("Business Class", typeof(bool));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompaniesFromExcel(GVars.filePath, PlaneCompanyComboBox);
            LoadCountriesFromExcel(GVars.filePath);

            StateFromComboBox.ItemsSource = GVars.CountriesData;
            StateFromComboBox.DisplayMemberPath = "Value";
            StateFromComboBox.SelectedValuePath = "Key";

            StateToComboBox.ItemsSource = GVars.CountriesData;
            StateToComboBox.DisplayMemberPath = "Value";
            StateToComboBox.SelectedValuePath = "Key";
        }

        private void LoadCompaniesFromExcel(string filePath, ComboBox comboBox)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Sheets[2];
            Range range = worksheet.UsedRange;

            GVars.PlaneCompanyData = new List<KeyValuePair<int, string>>();

            for (int row = 2; row <= range.Rows.Count; row++)
            {
                var idCell = range.Cells[row, 1] as Range;
                var nameCell = range.Cells[row, 2] as Range;

                if (idCell != null && nameCell != null)
                {
                    int id = Convert.ToInt32(idCell.Value2);
                    string name = nameCell.Value2?.ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        GVars.PlaneCompanyData.Add(new KeyValuePair<int, string>(id, name));
                    }
                }
            }

            workbook.Close(false);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);

            comboBox.ItemsSource = GVars.PlaneCompanyData;
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectedValuePath = "Key";
        }

        private void LoadCountriesFromExcel(string filePath)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Sheets[3];
            Range range = worksheet.UsedRange;

            GVars.CountriesData = new List<KeyValuePair<int, string>>();

            for (int row = 2; row <= range.Rows.Count; row++)
            {
                var idCell = range.Cells[row, 1] as Range;
                var nameCell = range.Cells[row, 2] as Range;

                if (idCell != null && nameCell != null)
                {
                    int id = Convert.ToInt32(idCell.Value2);
                    string country = nameCell.Value2?.ToString();
                    if (!string.IsNullOrEmpty(country))
                    {
                        GVars.CountriesData.Add(new KeyValuePair<int, string>(id, country));
                    }
                }
            }

            workbook.Close(false);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }

        private void Saveas_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                Application excelApp = new Application();
                Workbook workbook = excelApp.Workbooks.Open(GVars.filePath);
                Worksheet worksheet = workbook.Sheets[1];
                Range range = worksheet.UsedRange;

                int row = FindNextEmptyRow(worksheet);

                if (row != -1)
                {
                    InsertNewRow(worksheet, row);
                }

                workbook.Save();
                workbook.Close();
                excelApp.Quit();
                MessageBox.Show("Data saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFields()
        {
            if (PlaneCompanyComboBox.SelectedItem == null || string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Plane Company and Surname cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (StateFromComboBox.SelectedItem == null || StateToComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select valid states for 'State From' and 'State To'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidEmail(ContactEmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidPhoneNumber(ContactTelephoneTextBox.Text))
            {
                MessageBox.Show("Phone number must contain only numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidDateTime(ArrivalDatePicker.SelectedDate, ArrivalTimeTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Arrival Date and Time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidDateTime(DepartureDatePicker.SelectedDate, DepartureTimeTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Departure Date and Time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return !string.IsNullOrEmpty(phone) && phone.All(char.IsDigit);
        }

        private bool IsValidDateTime(DateTime? date, string timeText)
        {
            if (date == null || string.IsNullOrEmpty(timeText)) return false;

            DateTime dateTime;
            return DateTime.TryParseExact(timeText, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime);
        }

        private int FindNextEmptyRow(Worksheet worksheet)
        {
            Range range = worksheet.UsedRange;
            int row = 1;

            while (worksheet.Cells[row, 1].Value != null)
                row++;

            return row;
        }

        private void InsertNewRow(Worksheet worksheet, int row)
        {
            DateTime arrivalDate = ArrivalDatePicker.SelectedDate ?? DateTime.Now;
            DateTime departureDate = DepartureDatePicker.SelectedDate ?? DateTime.Now;

            string arrivalTimeText = ArrivalTimeTextBox.Text;
            string departureTimeText = DepartureTimeTextBox.Text;

            DateTime arrivalDateTime;
            if (DateTime.TryParseExact(arrivalTimeText, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime arrivalTime))
            {
                arrivalDateTime = new DateTime(arrivalDate.Year, arrivalDate.Month, arrivalDate.Day, arrivalTime.Hour, arrivalTime.Minute, 0);
            }
            else
            {
                arrivalDateTime = arrivalDate;
            }

            DateTime departureDateTime;
            if (DateTime.TryParseExact(departureTimeText, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime departureTime))
            {
                departureDateTime = new DateTime(departureDate.Year, departureDate.Month, departureDate.Day, departureTime.Hour, departureTime.Minute, 0);
            }
            else
            {
                departureDateTime = departureDate;
            }

            worksheet.Cells[row, 1].Value = GenerateNewId(worksheet, row - 1);
            worksheet.Cells[row, 2].Value = PlaneCompanyComboBox.SelectedValue.ToString();
            worksheet.Cells[row, 3].Value = StateFromComboBox.SelectedValue.ToString();
            worksheet.Cells[row, 4].Value = StateToComboBox.SelectedValue.ToString();
            worksheet.Cells[row, 5].Value = arrivalDateTime.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[row, 6].Value = departureDateTime.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[row, 7].Value = NameTextBox.Text;
            worksheet.Cells[row, 8].Value = ContactEmailTextBox.Text;
            worksheet.Cells[row, 9].Value = ContactTelephoneTextBox.Text;
            worksheet.Cells[row, 10].Value = BusinessClassCheckBox.IsChecked.HasValue ? BusinessClassCheckBox.IsChecked.Value.ToString() : "No";
        }

        private int GenerateNewId(Worksheet worksheet, int lastRow)
        {
            Range range = worksheet.UsedRange;
            int lastId = 0;
            int.TryParse(worksheet.Cells[lastRow, 1].Value?.ToString(), out lastId);

            return lastId + 1;
        }

        private int FindRowById(Worksheet worksheet, string id)
        {
            Range range = worksheet.UsedRange;
            int row = 1;
            while ((range.Cells[row, 1] as Range).Value2 != null)
            {
                if (worksheet.Cells[row, 1].Value?.ToString() == id)
                {
                    return row;
                }
                row++;
            }
            return -1;
        }

        private static System.Data.DataTable ReadExcelFile(string filePath, object userData)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Sheets[1];
            Range range = worksheet.UsedRange;

            System.Data.DataTable table = new System.Data.DataTable();

            for (int col = 1; col <= range.Columns.Count; col++)
            {
                string columnName = (range.Cells[1, col] as Range).Value2?.ToString();
                if (string.IsNullOrEmpty(columnName))
                {
                    columnName = $"Column{col}";
                }
                table.Columns.Add(columnName);
            }

            int row = 2;
            while ((range.Cells[row, 1] as Range).Value2 != null)
            {
                System.Data.DataRow dataRow = table.NewRow();
                for (int col = 1; col <= range.Columns.Count; col++)
                {
                    dataRow[col - 1] = (range.Cells[row, col] as Range).Value2;
                }
                table.Rows.Add(dataRow);
                row++;
            }

            workbook.Close(true);
            excelApp.Quit();

            return table;
        }

        private void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            var windowBs = new bs();
            windowBs.Show();
        }

        private void StateFromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
