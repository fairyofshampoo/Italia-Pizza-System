using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Signatures.Validation.V1.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for BalanceLogView.xaml
    /// </summary>
    public partial class BalanceLogView : Page
    {
        CashierLog CashierLogData { get; set; }
        public BalanceLogView(CashierLog cashierLog)
        {
            CashierLogData = cashierLog;
            InitializeComponent();
            SetDataInPage();
        }

        private void SetDataInPage()
        {
            DateTime reportTime = CashierLogData.openingDate ?? DateTime.Now;
            txtCashierName.Text = "Corte efectuado por: " + UserSingleton.Instance.Name;
            txtDate.Text = "Fecha de inicio de corte: " + reportTime.ToShortDateString();
            txtOpeningBalance.Text = CashierLogData.initialBalance.ToString();
            decimal totalOrders = CashierLogData.ordersCashin;
            txtOrders.Text = FormatCurrency(totalOrders);
            decimal cashin = CashierLogData.miscellaneousCashin ?? 0;
            txtCashin.Text = FormatCurrency(cashin);
            decimal totalCashin = totalOrders + cashin;
            txtTotalCashin.Text = FormatCurrency(totalCashin);

            decimal totalSupplyOrders = CashierLogData.supplierOrderCashout;
            txtSupplierOrder.Text = FormatCurrency(totalSupplyOrders);

            decimal cashout = CashierLogData.miscellaneousCashout;
            txtCashout.Text = FormatCurrency(cashout);
            decimal totalCashout = totalSupplyOrders + cashout;
            txtTotalCashout.Text = FormatCurrency(totalCashout);
            decimal totalCash = CashierLogData.initialBalance + totalCashin - (totalCashout);
            txtFinalBalance.Text = FormatCurrency(totalCash);
            decimal realCash = CashierLogData.finalBalance;
            txtRealFinalBalance.Text = FormatCurrency(realCash);
        }

        private void BtnDownloadReport_Click(object sender, RoutedEventArgs e)
        {
            CreateReport();
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
        private void CreateReport()
        {
            DateTime reportTime = CashierLogData.openingDate ?? DateTime.Now;
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
                DateTime reportTime = CashierLogData.openingDate ?? DateTime.Now;

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

                decimal finalBalanceReal = CashierLogData.finalBalance;
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

        private string FormatCurrency(decimal amount)
        {
            return "$" + amount.ToString("0.00");
        }
    }
}
