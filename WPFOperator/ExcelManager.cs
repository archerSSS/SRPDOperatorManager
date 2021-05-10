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

        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ep = new ExcelPackage();
            currentRow = 1;
        }

        public void CreateXFile(string[] names, string[] types, string[] nums)
        {
            ep.Workbook.Worksheets.Add("Cards Handlers");
            var sheet = ep.Workbook.Worksheets[0];
            
            for (; currentRow < names.Length; currentRow++)
            {
                sheet.Cells["A" + currentRow].Value = names[currentRow - 1];
                sheet.Cells["B" + currentRow].Value = nums[currentRow - 1];
                sheet.Cells["C" + currentRow].Value = types[currentRow - 1];
            }
            currentRow = 1;

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
    }
}
