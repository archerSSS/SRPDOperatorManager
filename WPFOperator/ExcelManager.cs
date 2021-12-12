using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFOperator
{
    class ExcelManager
    {
        private ExcelPackage ep;
        private int currentRow;
        private string firstColumn;
        private string secondColumn;
        private string thirdColumn;

        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            /*ep = new ExcelPackage();
            currentRow = 0;
            firstColumn = "";
            secondColumn = "";
            thirdColumn = "";*/
        }

        public void CreateXFile(string filename)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Excel Files");
            if (!dir.Exists)
            {
                dir.Create();
            }

            DeleteIfExist(dir + "/" + filename + ".xlsx");
            ep = new ExcelPackage(new FileInfo(dir + "/" + filename + ".xlsx"));
        }

        public void CreateXFile(string[] names, string[] types, string[] nums)
        {
            ep.Workbook.Worksheets.Add("Cards Handlers");
            var sheet = ep.Workbook.Worksheets[0];
            
            for (; currentRow < names.Length; currentRow++)
            {
                sheet.Cells["A" + (currentRow + 1)].Value = names[currentRow];
                sheet.Cells["B" + (currentRow + 1)].Value = nums[currentRow];
                sheet.Cells["C" + (currentRow + 1)].Value = types[currentRow];
            }
            currentRow = 0;

            DirectoryInfo dir = new DirectoryInfo("C:/OperatorManagerFiles");
            if (!dir.Exists)
            {
                dir.Create();
            }
            
            FileStream fs = new FileStream("C:/OperatorManagerFiles/OperatorCards.xlsx", FileMode.Create);
            byte[] bts = ep.GetAsByteArray();
            fs.Write(bts, 0, bts.Length);
            fs.Close();
        }

        public void CreateXFile()
        {
            var sheet = ep.Workbook.Worksheets[0];


            DirectoryInfo dir = new DirectoryInfo("C:/OperatorManagerFiles");
            if (!dir.Exists)
            {
                dir.Create();
            }

            FileStream fs = new FileStream("C:/OperatorManagerFiles/Cards and Employers.xlsx", FileMode.Create);
            byte[] bts = ep.GetAsByteArray();
            fs.Write(bts, 0, bts.Length);
            fs.Close();
        }


        public void CreateSheet(string name)
        {
            ep.Workbook.Worksheets.Add(name);
        }

        public void PutExcelData(string address, int value)
        {
            ep.Workbook.Worksheets[0].Cells[address].Value = value;
        }

        public void PutExcelData(string address, string value)
        {
            ep.Workbook.Worksheets[0].Cells[address].Value = value;
        }

        public void PutExcelData(string address, DateTime value)
        {

        }

        public void PutExcelData(string address, object value)
        {

        }

        private void DeleteIfExist(string dir)
        {
            if (IsFileExist(dir)) new FileInfo(dir).Delete();
        }

        private bool IsFileExist(string dir)
        {
            FileInfo fileinfo = new FileInfo(dir);
            return fileinfo.Exists;
        }

        public void SetFirstColumn(string t)
        {
            firstColumn = t;
        }

        public void SetSecondColumn(string t)
        {
            secondColumn = t;
        }

        public void SetThirdColumn(string t)
        {
            thirdColumn = t;
        }

        public void AddRowSecond(string t)
        {
            var sheet = ep.Workbook.Worksheets[0];
            if (firstColumn != "" && thirdColumn != "")
            {
                sheet.Cells["B" + currentRow].Value = t;
                currentRow++;
            }
        }

        public void SetCellValue(int list, string cell, string value)
        {
            var sheet = ep.Workbook.Worksheets[list];
            sheet.Cells[cell].Value = value;
        }

        public void Save()
        {
            ep.SaveAsync();
        }
    }
}
