using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for CashierReportView.xaml
    /// </summary>
    public partial class CashierReportView : Page
    {
        private OrderDAO orderDAO = new OrderDAO();
        private CashoutDAO cashoutDAO = new CashoutDAO();
        private SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
        private DateTime reportTime;
        public CashierReportView()
        {
            InitializeComponent();
            CheckLogExistences();
        }

        private void CheckLogExistences()
        {
            CashierLogDAO cashierLogDAO = new CashierLogDAO();
            CashierLog activeCashierLog = cashierLogDAO.GetActiveCashierLog();

            if (activeCashierLog != null)
            {
                reportTime = activeCashierLog.openingDate ?? DateTime.Now;
                SetReportDataInPage(activeCashierLog.initialBalance);
            }
            else
            {
                CreateNewCashierLog();
            }
        }

        private void CreateNewCashierLog()
        {
            CashierLog newCashierLog = new CashierLog
            {
                openingDate = DateTime.Now.AddYears(-1),
                initialBalance = 0,
                status = 1
            };

            CashierLogDAO cashierLogDAO = new CashierLogDAO();
            cashierLogDAO.AddCashierLog(newCashierLog);
            reportTime = newCashierLog.openingDate ?? DateTime.Now;
            SetFirstReportDataInPage();
        }

        private void CreateReport()
        {
            string fileName = "Corte de caja-" + reportTime.ToString("yyyy-MM-dd-HH-mm-ss") + ".pdf";
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            GeneratePDF(filePath);

            DialogManager.ShowSuccessMessageBox("El reporte se ha guardado en tu escritorio.");
        }

        private void GeneratePDF(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                PageSize pageSize = PageSize.LETTER;
                Document doc = new Document(pdf);

                doc.Add(new iText.Layout.Element.Paragraph($"Fecha de creación: {reportTime.ToString("yyyy-MM-dd HH:mm:ss")}"));
                doc.Add(new iText.Layout.Element.Paragraph($"Corte efectuado por: {UserSingleton.Instance.Name}"));

                doc.Add(CreateColoredParagraph($"Saldo inicial: {txtOpeningBalance.Text}", Colors.Orange));

                doc.Add(new iText.Layout.Element.Paragraph("ENTRADAS"));
                iText.Layout.Element.Table cashinTable = CreateCashinTable();
                doc.Add(cashinTable);
                doc.Add(new iText.Layout.Element.Paragraph("SALIDAS"));

                iText.Layout.Element.Table cashoutTable = CreateCashoutTable();
                doc.Add(cashoutTable);

                string finalBalanceCalculated = txtFinalBalance.Text;
                doc.Add(CreateColoredParagraph($"Saldo final calculado: {finalBalanceCalculated}", GetOrangeColor()));

                decimal finalBalanceReal = CalculateFinalBalance();
                doc.Add(CreateColoredParagraph($"Saldo final real: {FormatCurrency(finalBalanceReal)}", Colors.LightGray));

                doc.Close();
            }
        }

        private iText.Layout.Element.Paragraph CreateColoredParagraph(string text, System.Windows.Media.Color color)
        {
            DeviceRgb deviceColor = new DeviceRgb(color.R, color.G, color.B);
            iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph(text);
            paragraph.SetFontColor(deviceColor);
            return paragraph;
        }

        private iText.Layout.Element.Table CreateCashoutTable()
        {
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(2);
            table.SetMaxWidth(600);

            table.AddCell(CreateCellWithBackground("Concepto", Colors.LightGray));
            table.AddCell(CreateCellWithBackground("Subtotal", Colors.LightGray));
            table.AddCell(CreateCellWithBackground("Pedidos a proveedores", Colors.White));
            table.AddCell(CreateCellWithBackground(txtSupplierOrder.Text, Colors.White));
            table.AddCell(CreateCellWithBackground("Gastos varios", Colors.White));
            table.AddCell(CreateCellWithBackground(txtCashout.Text, Colors.White));
            table.AddCell(CreateCellWithBackground("Total salidas", Colors.LightGreen));
            table.AddCell(CreateCellWithBackground(txtTotalCashout.Text, Colors.LightGreen));

            return table;
        }
        private iText.Layout.Element.Table CreateCashinTable()
        {
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(2);
            table.SetMaxWidth(600);

            table.AddCell(CreateCellWithBackground("Concepto", Colors.LightGray));
            table.AddCell(CreateCellWithBackground("Subtotal", Colors.LightGray));
            table.AddCell(CreateCellWithBackground("Pedidos", Colors.White));
            table.AddCell(CreateCellWithBackground(txtOrders.Text, Colors.White));
            table.AddCell(CreateCellWithBackground("Ingresos varios", Colors.White));
            table.AddCell(CreateCellWithBackground(txtCashin.Text, Colors.White));
            table.AddCell(CreateCellWithBackground("Total entradas", Colors.LightGreen));
            table.AddCell(CreateCellWithBackground(txtTotalCashin.Text, Colors.LightGreen));

            return table;
        }
        public static System.Windows.Media.Color GetOrangeColor()
        {
            byte redValue = 0xFF;
            byte greenValue = 0x7B;
            byte blueValue = 0x00;

            return System.Windows.Media.Color.FromRgb(redValue, greenValue, blueValue);
        }

        private Cell CreateCellWithBackground(string text, System.Windows.Media.Color backgroundColor)
        {
            Cell cell = new Cell();
            cell.Add(new iText.Layout.Element.Paragraph(text));
            DeviceRgb deviceRgb = new DeviceRgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);

            cell.SetBackgroundColor(deviceRgb);
            return cell;
        }

        private void SetReportDataInPage(decimal initialBalance)
        {
            txtCashierName.Text = "Corte efectuado por: " + UserSingleton.Instance.Name;
            txtDate.Text = "Fecha de inicio de corte: " + reportTime.ToShortDateString();
            txtOpeningBalance.Text = initialBalance.ToString();
            decimal totalOrders = orderDAO.GetSumOfTotalOrdersByDate(reportTime);
            txtOrders.Text = FormatCurrency(totalOrders);
            decimal cashin = GetCashinTotalByDate();
            txtCashin.Text = FormatCurrency(cashin);
            decimal totalCashin = totalOrders + cashin;
            txtTotalCashin.Text = FormatCurrency(totalCashin);

            decimal totalSupplyOrders = supplyOrderDAO.GetSumOfTotalSupplierOrdersByDate(reportTime);
            txtSupplierOrder.Text = FormatCurrency(totalSupplyOrders);

            decimal cashout = GetCashoutTotalByDate();
            txtCashout.Text = FormatCurrency(cashout);
            decimal totalCashout = totalSupplyOrders + cashout;
            txtTotalCashout.Text = FormatCurrency(totalCashout);
            decimal totalCash = initialBalance + totalCashin - (totalCashout);
            txtFinalBalance.Text = FormatCurrency(totalCash);
        }

        private void SetFirstReportDataInPage()
        {
            txtCashierName.Text = "Corte efectuado por: " + UserSingleton.Instance.Name;
            txtDate.Text = "Fecha de inicio de corte: " + reportTime.ToShortDateString();
            decimal firstOpeningBalance = 0;
            txtOpeningBalance.Text = "$" + firstOpeningBalance;

            decimal totalOrders = orderDAO.GetSumOfTotalOrders();
            txtOrders.Text = FormatCurrency(totalOrders);
            decimal cashin = cashoutDAO.GetSumOfCashin();
            txtCashin.Text = FormatCurrency(cashin);
            decimal totalCashin = totalOrders + cashin;
            txtTotalCashin.Text = FormatCurrency(totalCashin);

            decimal totalSupplierOrders = supplyOrderDAO.GetSumOfTotalSupplierOrders();
            txtSupplierOrder.Text = FormatCurrency(totalSupplierOrders);

            decimal cashout = cashoutDAO.GetSumOfCashout();
            txtCashout.Text = FormatCurrency(cashout);
            decimal totalCashout = totalSupplierOrders + cashout;
            txtTotalCashout.Text = FormatCurrency(totalCashout);
            decimal totalCash = firstOpeningBalance + totalCashin - totalCashout;
            txtFinalBalance.Text = FormatCurrency(totalCash);
        }

        private decimal GetCashinTotalByDate()
        {
            byte cashinType = 1;
            decimal totalCashin = cashoutDAO.GetTotalCashoutsByDateAndType(reportTime, cashinType);
            return totalCashin;
        }

        private decimal GetCashoutTotalByDate()
        {
            byte cashoutType = 0;
            decimal totalCashout = cashoutDAO.GetTotalCashoutsByDateAndType(reportTime, cashoutType);
            return totalCashout;
        }

        private string FormatCurrency(decimal amount)
        {
            return "$" + amount.ToString("0.00");
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            CashReconciliationHistory reconciliationHistory = new CashReconciliationHistory();
            NavigationService.Navigate(reconciliationHistory);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> moneyTextBoxes = new List<TextBox>
            {
                txt1000Bill, txt500Bill, txt200Bill, txt100Bill, txt50Bill, txt20Bill,
                txt20Coin, txt10Coin, tx5Coin, txt2Coin, txt1Coin, txt50Cent
            };

            foreach (TextBox textBox in moneyTextBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "0";
                }
            }
            decimal finalBalanceReal = CalculateFinalBalance();
            decimal finalBalanceCalculated = decimal.Parse(txtFinalBalance.Text.Replace("$", ""));

            if (finalBalanceReal != finalBalanceCalculated)
            {
                DialogManager.ShowWarningMessageBox("El saldo final calculado no coincide con el saldo final real.");
            }

            CreateReport();
            CloseCashierLog(finalBalanceReal);
            GoBack();
        }

        private decimal CalculateFinalBalance()
        {
            Dictionary<TextBox, decimal> moneyValues = new Dictionary<TextBox, decimal>
            { 
                { txt1000Bill, 1000 }, { txt500Bill, 500 }, { txt200Bill, 200 },
                { txt100Bill, 100 }, { txt50Bill, 50 }, { txt20Bill, 20 },
                { txt20Coin, 20 }, { txt10Coin, 10 }, { tx5Coin, 5 },
                { txt2Coin, 2 }, { txt1Coin, 1 }, { txt50Cent, 0.5m }
            };

            decimal finalBalanceReal = 0;

            foreach (var pair in moneyValues)
            {
                TextBox textBox = pair.Key;
                decimal value = pair.Value;
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    if (decimal.TryParse(textBox.Text, out decimal amount))
                    {
                        finalBalanceReal += amount * value;
                    }
                }
            }

            return finalBalanceReal;
        }


        private void CloseCashierLog(decimal finalBalanceReal)
        {
            CashierLogDAO cashierLogDAO = new CashierLogDAO();
            cashierLogDAO.CloseActiveCashierLog(finalBalanceReal, DateTime.Now, UserSingleton.Instance.Email);
        }

        private void Money_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Validations.IsNumber(e.Text))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 8)
            {
                e.Handled = true;
            }
        }
    }
}
