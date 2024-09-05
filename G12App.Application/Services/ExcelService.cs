using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace G12App.Application.Services
{
    public class ExcelService : IExcelService
    {
        public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public ServiceResult<FileDto> CreateFile(FileSourceDto fileSource)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Relatórios");

            for (var i = 0; i < fileSource.Columns.Count; i++)
            {
                sheet.Cells[1, i + 1].Value = fileSource.Columns[i];
                sheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                sheet.Cells[1, i + 1].Style.Font.Bold = true;
                sheet.Cells[1, i + 1].Style.Font.SetFromFont("Calibri", 13, true);
                sheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.Black);
                sheet.Cells[1, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Rows[1].Height = 25;
            }

            for (var r = 0; r < fileSource.Data.Count; r++)
                for (var c = 0; c < fileSource.Data[r].Count; c++)
                {
                    sheet.Cells[r + 2, c + 1].Value = fileSource.Data[r][c];
                    sheet.Cells[r + 2, c + 1].Style.Font.SetFromFont("Calibri", 13, false);
                    sheet.Cells[r + 2, c + 1].Style.Font.Color.SetColor(Color.Black);
                    sheet.Cells[r + 2, c + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Rows[r + 2].Height = 25;
                }

            sheet.Cells.AutoFitColumns(120);

            var stream = new MemoryStream(package.GetAsByteArray());
            return new ServiceResult<FileDto>(new FileDto()
            {
                Stream = stream,
                ContentType = ContentType,
            });
        }
    }
}
