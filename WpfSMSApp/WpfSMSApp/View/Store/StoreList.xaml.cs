using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WpfSMSApp.Helper;
using System.Windows.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace WpfSMSApp.View.Store

{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StoreList : Page
    {
        public StoreList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Store 테이블 데이터 읽어와야함
                List<Model.Store> stores = new List<Model.Store>();
                List<Model.StockStore> stockStores = new List<Model.StockStore>();
                stores = Logic.DataAccess.GetStores();

                // stores 데이터를 stockStores로 옮김
                foreach (Model.Store item in stores)
                {
                    var store = new Model.StockStore()
                    {
                        StoreID = item.StoreID,
                        StoreName = item.StoreName,
                        StoreLocation = item.StoreLocation,
                        ItemStatus = item.ItemStatus,
                        TagID = item.TagID,
                        BarcodeID = item.BarcodeID,
                        StockQuantity = 0
                    };
                    
                    store.StockQuantity = Logic.DataAccess.GetStocks().Where(t => t.StoreID.Equals(store.StoreID)).Count();
                    stockStores.Add(store);
                }

                this.DataContext = stockStores;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 StoreList Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //NavigationService.Navigate(new EditUser());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnEditUser : {ex}");
                throw ex;
            }
        }

        private void BtnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //NavigationService.Navigate(new DeactiveUser());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnEditUser : {ex}");
                throw ex;
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File (*.pdf)|*.pdf";
            saveDialog.FileName = "";
            if (saveDialog.ShowDialog() == true)
            {
                // PDF 변환
                try
                {
                    // 0.PDF 사용 폰트 설정
                    string nanumPath = Path.Combine(Environment.CurrentDirectory, @"NanumGothic.ttf");
                    BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumTitle = new iTextSharp.text.Font(nanumBase, 20f); // 20 타이틀용 나눔폰트;
                    var nanumContent = new iTextSharp.text.Font(nanumBase, 12f); // 12 내용 나눔폰트;

                    //iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER);
                    string pdfFilePath = saveDialog.FileName;

                    // 1.PDF 객체 생성
                    iTextSharp.text.Document pdfDoc = new Document(PageSize.A4);
                    
                    // 2.PDF 내용 만들기
                    Paragraph title = new Paragraph("부경대 재고관리시스템(SMS)\n", nanumTitle);
                    Paragraph subTitle = new Paragraph($"사용자리스트 exported : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n\n", nanumContent);

                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count);
                    pdfTable.WidthPercentage = 100; // 전체 사이즈 다 쓰는거

                    // 그리드 헤더 작업
                    foreach (DataGridColumn column in GrdData.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(), nanumContent));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cell);
                    }

                    // 각 셀 사이즈 조절
                    float[] columnsWidth = new float[] { 7f, 15f, 10f, 15f, 28f, 12f, 10f };
                    pdfTable.SetWidths(columnsWidth);

                    // 그리드 Row 작업
                    foreach (var item in GrdData.Items)
                    {
                        if (item is Model.User)
                        {
                            var temp = item as Model.User;
                            // UserID
                            PdfPCell cell = new PdfPCell(new Phrase(temp.UserID.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);
                            // UserIdentityNumber
                            cell = new PdfPCell(new Phrase(temp.UserIdentityNumber.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                            // UserSurname
                            cell = new PdfPCell(new Phrase(temp.UserSurname.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                            // UserName
                            cell = new PdfPCell(new Phrase(temp.UserName.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                            // UserEmail
                            cell = new PdfPCell(new Phrase(temp.UserEmail.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                            // UserAdmin
                            cell = new PdfPCell(new Phrase(temp.UserAdmin.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                            // UserActivated
                            cell = new PdfPCell(new Phrase(temp.UserActivated.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                        }
                    }

                    // 3.PDF 파일 생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        // 2번에서 만들 내용 추가
                        pdfDoc.Add(title);
                        pdfDoc.Add(subTitle);
                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close(); // 옵션
                    }

                    Commons.ShowMessageAsync("PDF변환", "PDF 익스포트 성공");
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 BtnExportPdf_Click : {ex}");
                }
            }
        }

        private void BtnAddStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddStore());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnAddStore_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditStore_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem == null)
            {
                Commons.ShowMessageAsync("창고수정", "수정할 창고를 선택하세요");
                return;
            }

            try
            {
                var storeId = (GrdData.SelectedItem as Model.Store).StoreID;
                NavigationService.Navigate(new EditStore(storeId));
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnEditStore_Click : {ex}");
                throw ex;
            }
        }

        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            dialog.FileName = "";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    IWorkbook workbook = new XSSFWorkbook(); // xlsx용 // new HSSFWorkbook(); xls용 (이전 버전용)
                    ISheet sheet = workbook.CreateSheet("Sheet1"); // 이름변경 가능
                                                                   // 헤더 row
                    IRow rowHeader = sheet.CreateRow(0);
                    ICell cell = rowHeader.CreateCell(0);
                    cell.SetCellValue("순번");
                    cell = rowHeader.CreateCell(1);
                    cell.SetCellValue("창고명");
                    cell = rowHeader.CreateCell(2);
                    cell.SetCellValue("창고위치");
                    cell = rowHeader.CreateCell(3);
                    cell.SetCellValue("재고수");

                    for (int i = 0; i < GrdData.Items.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1);
                        if (GrdData.Items[i] is Model.StockStore)
                        {
                            var stockStore = GrdData.Items[i] as Model.StockStore;
                            ICell dataCell = row.CreateCell(0);
                            dataCell.SetCellValue(stockStore.StoreID);
                            dataCell = row.CreateCell(1);
                            dataCell.SetCellValue(stockStore.StoreName);
                            dataCell = row.CreateCell(2);
                            dataCell.SetCellValue(stockStore.StoreLocation);
                            dataCell = row.CreateCell(3);
                            dataCell.SetCellValue(stockStore.StockQuantity);
                        }
                    }

                    // 파일저장
                    using (var fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        workbook.Write(fs);
                    }

                    Commons.ShowMessageAsync("엑셀저장", "엑셀 export 성공!");
                }
                catch (Exception ex)
                {
                    Commons.ShowMessageAsync("예외", $"예외 발생 : {ex}");
                }
            }
        }
    }
}
