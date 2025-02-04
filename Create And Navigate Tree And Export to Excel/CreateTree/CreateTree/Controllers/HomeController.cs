using CreateTree.Classes;
using CreateTree.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CreateTree.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private int lastRow = 0;
        private int lastColumn = 0;
        private List<Color> colors;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            colors = new List<Color>
            {
                Color.FromArgb(255, 242, 204),
                Color.FromArgb(217, 255, 242),
                Color.FromArgb(252, 228, 214),
                Color.FromArgb(237, 237, 237),
                Color.FromArgb(221, 235, 247),
                Color.FromArgb(255, 242, 204),
            };
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateExcel()
        {
            var rawList = GenerateData.Generate();
            TreeModel tree = new TreeModel() { Id = 0, Name = "Category" };
            GenerateTree(tree, rawList);
            int row = 1;
            string json = JsonConvert.SerializeObject(tree);

            await using var stream = new MemoryStream();
            using (var excel = new ExcelPackage(stream))
            {
                var worksheet = excel.Workbook.Worksheets.Add("Categories");
                NavigateTree(tree, ref row, 1, worksheet);
                worksheet.DeleteColumn(1);
                Formatting(worksheet);

                excel.SaveAs(stream);
            }
            stream.Position = 0;
            return File(stream.ToArray(), MimeTypes.TextXlsx, "Categories.xlsx");
        }

        private void Formatting(ExcelWorksheet worksheet)
        {
            worksheet.View.RightToLeft = true;
            for (int i = 1; i <= lastColumn; i++)
            {
                worksheet.Cells[1, i].Value = $"Level {i}";

                worksheet.SelectedRange[1, i, lastRow, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.SelectedRange[1, i, lastRow, i].Style.Fill.BackgroundColor.SetColor(colors[i]);
            }

            worksheet.SelectedRange[1, 1, lastRow, lastColumn].Style.Border.BorderAround(ExcelBorderStyle.Thick);
            worksheet.SelectedRange[1, 1, lastRow, lastColumn].AutoFitColumns();

            worksheet.SelectedRange[1, 1, 1, lastColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.SelectedRange[1, 1, 1, lastColumn].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));
            // worksheet.SelectedRange[1, lastColumn].Style.Border.BorderAround(ExcelBorderStyle.Thick);
            /*var a = worksheet.SelectedRange[1, 1, lastRow, lastColumn];
            foreach(var cell in a)
            {
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thick);
            }*/
        }

        public void GenerateTree(TreeModel tree, List<BaseModel> rawTreeInfo)
        {
            tree.Child.AddRange(rawTreeInfo.Where(x => x.ParentCategoryId == tree.Id).Select(x => new TreeModel { Name = x.Name, Id = x.Id }).ToList());
            foreach (TreeModel treeModel in tree.Child)
            {
                if (rawTreeInfo.Any(x => x.ParentCategoryId == treeModel.Id))
                {
                    GenerateTree(treeModel, rawTreeInfo);
                }
            }
        }

        public void NavigateTree(TreeModel tree, ref int row, int level, ExcelWorksheet worksheet)
        {
            if (lastRow < row)
            {
                lastRow = row;
            }
            if (lastColumn < level)
            {
                lastColumn = level - 1;
            }
            worksheet.Cells[row, level].Value = tree.Name;
            worksheet.Cells[row, level].Style.Border.BorderAround(ExcelBorderStyle.Hair);
            foreach (TreeModel node in tree.Child)
            {
                row++;
                NavigateTree(node, ref row, level + 1, worksheet);
            }
        }
    }
}