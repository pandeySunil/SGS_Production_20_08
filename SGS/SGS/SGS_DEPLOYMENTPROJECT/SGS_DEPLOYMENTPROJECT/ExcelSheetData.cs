using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS_DEPLOYMENTPROJECT
{
  
    public class ExcelSheetData
{
    public List<Rows> rw = new List<Rows>();
       
        public List<Rows> GetRows(Microsoft.Office.Interop.Excel.Range xlRange) {
            int i = 2;
            Console.WriteLine("Excell Sheet Data Loading Started");
            while (xlRange!=null&& xlRange.Rows!=null&& i <= xlRange.Rows.Count) {
                Console.WriteLine("Loading row ---  "+i);
                rw.Add(new Rows() { SwCode = (xlRange.Cells[i, 4].Value2.ToString().Trim('/')),
                    TryLed = xlRange.Cells[i, 2].Value2.ToString().Trim('/'),
                    CavityLed = xlRange.Cells[i, 3].Value2.ToString().Trim('/'),
                    DesMessage = xlRange.Cells[i, 5].Value2.ToString().Trim('/'),
                    ConnectorNo = xlRange.Cells[i, 6].Value2.ToString().Trim('/'),
                    ImageId = xlRange.Cells[i, 7].Value2.ToString().Trim('/'),
                    ImageFile = xlRange.Cells[i, 8].Value2.ToString().Trim('/')
                });
                i++;
            }
            return rw;
        }
    }

  public   class Rows {
        public string SwCode { get; set; }
        public string TryLed { get; set; }
        public string CavityLed { get; set; }
        public string ImageId { get; set; }
        public string DesMessage { get; set; }
        public string ConnectorNo { get; set; }
        public string ImageFile { get; set; }


    }
}
