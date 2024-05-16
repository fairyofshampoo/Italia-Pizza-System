﻿using ItaliaPizza.DataLayer.DAO;
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
using ItaliaPizza.ApplicationLayer;
using System.IO.Packaging;
using System.Windows.Media;
using iText.Kernel.Colors;
using iText.Kernel.Geom;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for InventoryReport.xaml
    /// </summary>
    public partial class InventoryReport : Page
    {
        bool isInventoryEmpty;
        public Dictionary<Supply, ReportUC> suppliesDictionary = new Dictionary<Supply, ReportUC>();
        public Dictionary<Product, ReportUC> productsDictionary = new Dictionary<Product, ReportUC>();
        public InventoryReport()
        {
            InitializeComponent();
            GetSupplies();
            CreateReport();
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
            if (ValidateNewAmounts() && ValidateNotes())
            {
                UpdateAmounts();
                CreateReport();
                NavigationService.GoBack();

                DialogManager.ShowSuccessMessageBox("Se han actualizado los valores en el inventario");
            } else
            {
                DialogManager.ShowWarningMessageBox("Corrige las cantidades en rojo a números válidos y no dejes campos vacíos (máx. 6 caracteres)");
            }
        }

        private bool ValidateNotes()
        {
            bool isValid = true;
            foreach (ReportUC report in suppliesDictionary.Values)
            {
                if (report.isDifferent && string.IsNullOrEmpty(report.txtNote.Text))
                {
                    isValid = false;
                    break;
                }
            }

            foreach (ReportUC reportUC in productsDictionary.Values)
            {
                if (reportUC.isDifferent && string.IsNullOrEmpty(reportUC.txtNote.Text))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        private void UpdateAmounts()
        {
            foreach (var kvp in suppliesDictionary)
            {
                ReportUC report = kvp.Value;
                if (report.isDifferent)
                {
                    if (decimal.TryParse(report.txtChangeCurrentAmount.Text, out decimal newAmount))
                    {
                        SupplyDAO supplyDAO = new SupplyDAO();
                        supplyDAO.ModifySupplyAmount(kvp.Key.name, newAmount);
                    }
                }
            }


            foreach (var keyValuePair in productsDictionary)
            {
                ReportUC reportUC = keyValuePair.Value;
                if (reportUC.isDifferent)
                {
                    ProductDAO productDAO = new ProductDAO();
                    productDAO.UpdateProductAmount(keyValuePair.Key.productCode, Convert.ToInt32(reportUC.txtChangeCurrentAmount.Text));
                }
            }
        }

        private bool ValidateNewAmounts()
        {
            bool isValid = true;
            foreach (ReportUC report in suppliesDictionary.Values )
            {
                if (!report.isValid)
                {
                    isValid = false;
                    break;
                }
            }

            foreach(ReportUC reportUC in productsDictionary.Values )
            {
                if (!reportUC.isValid)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        private void CreateReport()
        {
            string fileName = "ReporteInventario-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".pdf";
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            if (isInventoryEmpty)
            {
                GenerateEmptyInventoryReport(filePath);
            }
            else
            {
                GeneratePDF(filePath);
            }

            DialogManager.ShowSuccessMessageBox("El reporte se ha guardado en tu escritorio.");
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
                PageSize pageSize = PageSize.LETTER;
                Document doc = new Document(pdf);

                doc.Add(new iText.Layout.Element.Paragraph($"Fecha de creación: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"));
                doc.Add(new iText.Layout.Element.Paragraph($"Creado por: {UserSingleton.Instance.Name}"));

                iText.Layout.Element.Table table = CreateDataTable();
                doc.Add(table);

                doc.Close();
            }
        }

        private iText.Layout.Element.Table CreateDataTable()
        {
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(6);
            table.SetMaxWidth(600);

            int count = 0;
            foreach (var item in suppliesListView.Items)
            {
                if (item is ReportUC reportUC)
                {
                    System.Windows.Media.Color backgroundColor = ((SolidColorBrush)reportUC.Background).Color;

                    table.AddCell(CreateCellWithBackground(reportUC.txtName.Text, backgroundColor));
                    table.AddCell(CreateCellWithBackground(reportUC.txtAmount.Text, backgroundColor));
                    table.AddCell(CreateCellWithBackground(reportUC.txtSupplyArea.Text, backgroundColor));
                    table.AddCell(CreateCellWithBackground(reportUC.txtUnit.Text, backgroundColor));

                    if (count == 0)
                    {
                        table.AddCell(CreateCellWithBackground(reportUC.txtCurrentAmount.Text, backgroundColor));
                        table.AddCell(CreateCellWithBackground("Notas", backgroundColor));
                    }
                    else
                    {
                        table.AddCell(CreateCellWithBackground(reportUC.txtChangeCurrentAmount.Text, backgroundColor));
                        table.AddCell(CreateCellWithBackground(reportUC.txtNote.Text, backgroundColor));
                    }

                    count++;
                }
            }

            return table;
        }
        private Cell CreateCellWithBackground(string text, System.Windows.Media.Color backgroundColor)
        {
            Cell cell = new Cell();
            cell.Add(new Paragraph(text));
            DeviceRgb deviceRgb = new DeviceRgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);

            cell.SetBackgroundColor(deviceRgb);
            return cell;
        }


        private void ShowInventory(List<Supply> suppliesAndProducts)
        {
            suppliesListView.Items.Clear();
            ReportUC reportCard = new ReportUC();
            reportCard.SetTitleData();
            suppliesListView.Items.Add(reportCard);

            foreach (Supply item in suppliesAndProducts)
            {
                AddItemToList(item);
            }
        }

        private void AddItemToList(object item)
        {
            ReportUC reportCard = new ReportUC();
            reportCard.InventoryReport = this;
            reportCard.SetObjectData(item);
            suppliesListView.Items.Add(reportCard);
        }

        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableItems = supplyDAO.GetSuppliesByStatus(true);

            if (availableItems.Count > 0)
            {
                ShowInventory(availableItems);
            }
            else
            {
                ShowNoItemsMessage();
            }
        }

        private void ShowNoItemsMessage()
        {
            isInventoryEmpty = true;
            suppliesListView.Items.Clear();
            Label lblNoSupplies = new Label();
            lblNoSupplies.Style = (System.Windows.Style)FindResource("NoItemsLabelStyle");
            lblNoSupplies.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoSupplies.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoSupplies);
        }
    }
}
