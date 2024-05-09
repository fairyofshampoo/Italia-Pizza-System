using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for InventoryReport.xaml
    /// </summary>
    public partial class InventoryReport : Page
    {
        bool isInventoryEmpty;
        Dictionary<string, ReportUC> reportDictionary = new Dictionary<string, ReportUC>();
        public InventoryReport()
        {
            InitializeComponent();
            GetSupplies();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "Al regresar a la pantalla anterior perderá sus cambios, ¿desea continuar?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                NavigationService.GoBack();
            }
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InventoryReport.pdf");

            if (isInventoryEmpty)
            {
                GenerateEmptyInventoryReport(filePath);
            }
            else
            {
                GeneratePDF(filePath);
            }

            MessageBox.Show("El informe se ha generado correctamente y se ha guardado en tu escritorio.", "Informe generado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateEmptyInventoryReport(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document doc = new Document(pdf);

                doc.Add(new iText.Layout.Element.Paragraph($"Fecha de creación: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"));
                doc.Add(new iText.Layout.Element.Paragraph("No hay insumos registrados"));

                doc.Close();
            }
        }

        private void GeneratePDF(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document doc = new Document(pdf);

                doc.Add(new iText.Layout.Element.Paragraph($"Fecha de creación: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"));

                iText.Layout.Element.Table table = CreateDataTable();
                doc.Add(table);

                doc.Close();
            }
        }

        private iText.Layout.Element.Table CreateDataTable()
        {
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(5);

            table.AddCell("Nombre");
            table.AddCell("Cantidad");
            table.AddCell("Área de Suministro");
            table.AddCell("Unidad de Medida");
            table.AddCell("Existencia actual");

            if (suppliesListView.Items.Count == 0)
            {
                Cell cell = new Cell().Add(new iText.Layout.Element.Paragraph("No hay insumos registrados"));
                cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table.AddCell(cell);
            }
            else
            {
                foreach (var item in suppliesListView.Items)
                {
                    if (item is ReportUC reportUC)
                    {
                        table.AddCell(reportUC.txtName.Text);
                        table.AddCell(reportUC.txtAmount.Text);
                        table.AddCell(reportUC.txtSupplyArea.Text);
                        table.AddCell(reportUC.txtUnit.Text);
                        table.AddCell(reportUC.txtChangeCurrentAmount.Text);
                    }
                }
            }

            return table;
        }

        private void ShowSupplies(List<Supply> suppliesList)
        {
            suppliesListView.Items.Clear();
            reportDictionary.Clear();
            ReportUC reportCard = new ReportUC();
            reportCard.SetTitleData();
            suppliesListView.Items.Add(reportCard);
            foreach (Supply supply in suppliesList)
            {
                AddSupplyToList(supply);
            }
        }

        private void AddSupplyToList(Supply supply)
        {
            ReportUC reportCard = new ReportUC();
            reportDictionary.Add(supply.name, reportCard);
            reportCard.SetSupplyData(supply);
            suppliesListView.Items.Add(reportCard);
        }

        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetSuppliesByStatus(true);

            if (availableSupplies.Count > 0)
            {
                ShowSupplies(availableSupplies);
            }
            else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void ShowNoSuppliesMessage()
        {
            isInventoryEmpty = true;
            suppliesListView.Items.Clear();
            Label lblNoSupplies = new Label();
            lblNoSupplies.Style = (System.Windows.Style)FindResource("NoSuppliesLabelStyle");
            lblNoSupplies.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoSupplies.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoSupplies);
        }
    }
}
