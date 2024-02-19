using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuLac.Models
{
    public class tableDieuDongBacThang
    {
        public string Loai { get; set; }
        public int DepartmentID { get; set; }
        public string Description { get; set; }
        public string cot1 { get; set; }
        public string cot2 { get; set; }
        public string cot3 { get; set; }
        public string cot4 { get; set; }
        public string cot5 { get; set; }
        public string cot6 { get; set; }
        public string cot7 { get; set; }
        public string cot8 { get; set; }
        public string cot9 { get; set; }
        public string cot10 { get; set; }
        public string cot11 { get; set; }
        public string cot12 { get; set; }
        public string cot13 { get; set; }
        public string cot14 { get; set; }
        public string cot15 { get; set; }
        public string cot16 { get; set; }
        public string cot17 { get; set; }
        public string cot18 { get; set; }
        public string cot19 { get; set; }
        public string cot20 { get; set; }
        public string cot21 { get; set; }
        public tableDieuDongBacThang()
        {
            cot1 = "";
            cot10 = "";
            cot11 = "";
            cot12 = "";
            cot13 = "";
            cot14 = "";
            cot15 = "";
            cot16 = "";
            cot17 = "";
            cot18 = "";
            cot19 = "";
            cot2 = "";
            cot20 = "";
            cot21 = "";
            cot3 = "";
            cot4 = "";
            cot5 = "";
            cot6 = "";
            cot7 = "";
            cot8 = "";
            cot9 = "";
            DepartmentID = 0;
            Description = "";
            Loai = "DDBACTHANG";
        }
        public string LayGiaTriCot(int i)
        {
            string result = "";
            switch (i)
            {
                case 1:
                    result = cot1;
                    break;
                case 10:
                    result = cot10;
                    break;
                case 11:
                    result = cot11;
                    break;
                case 12:
                    result = cot12;
                    break;
                case 13:
                    result = cot13;
                    break;
                case 14:
                    result = cot14;
                    break;
                case 15:
                    result = cot15;
                    break;
                case 16:
                    result = cot16;
                    break;
                case 17:
                    result = cot17;
                    break;
                case 18:
                    result = cot18;
                    break;
                case 19:
                    result = cot19;
                    break;
                case 20:
                    result = cot20;
                    break;
                case 21:
                    result = cot21;
                    break;
                case 2:
                    result = cot2;
                    break;
                case 3:
                    result = cot3;
                    break;
                case 4:
                    result = cot4;
                    break;
                case 5:
                    result = cot5;
                    break;
                case 6:
                    result = cot6;
                    break;
                case 7:
                    result = cot7;
                    break;
                case 8:
                    result = cot8;
                    break;
                case 9:
                    result = cot9;
                    break;
            }
            return result;
        }
    }    
}