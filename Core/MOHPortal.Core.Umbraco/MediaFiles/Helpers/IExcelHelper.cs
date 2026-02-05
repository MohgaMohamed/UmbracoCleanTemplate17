namespace MOHPortal.Core.Umbraco.MediaFiles.Helpers
{
    public interface IExcelHelper
    {
        MemoryStream Export<T>(List<T> dataList, string sheetName);
    }
}
