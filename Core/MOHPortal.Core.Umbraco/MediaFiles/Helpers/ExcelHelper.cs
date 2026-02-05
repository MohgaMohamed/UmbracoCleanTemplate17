using ClosedXML.Excel;

namespace MOHPortal.Core.Umbraco.MediaFiles.Helpers
{
    public class ExcelHelper : IExcelHelper
    { 
        public MemoryStream Export<T>(List<T> dataList, string sheetName)
        {
            if (dataList?.Count <= default(int))
                return null;

            using IXLWorkbook workbook = new XLWorkbook();
            IXLWorksheet addWorksheet = workbook.AddWorksheet(sheetName);
            addWorksheet.Range(1, 1, 1, 200).Style.Font.SetBold(true);

            addWorksheet.FirstCell().InsertTable(dataList, false);
            addWorksheet.Columns().AdjustToContents();
            MemoryStream stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream;
        }
    }
}
