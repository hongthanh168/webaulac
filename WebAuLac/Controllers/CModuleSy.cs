using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using WebAuLac.Models;
namespace WebAuLac.Controllers
{
    public class CModuleSy
    {
        private AuLacEntities dc = new AuLacEntities();
        public void BaoCaoKeHoachDieuDong(String server, int planID)
        {

            //string fileNameTemplate = server + "\\Mau_KHDD.xltx";
            //object xlApp;
            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlSheet;
            //object misValue = System.Reflection.Missing.Value;


            //Type t = Type.GetTypeFromProgID("Excel.Application");
            //xlApp = System.Activator.CreateInstance(t);

            //((Microsoft.Office.Interop.Excel.Application)xlApp).Visible = true;

            //xlWorkBook = ((Microsoft.Office.Interop.Excel.Application)xlApp).Workbooks.Open(fileNameTemplate);
            //xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            object xlApp;
            Workbook xlBook;
            Worksheet xlSheet;
            Type t = Type.GetTypeFromProgID("Excel.Application");
            xlApp = System.Activator.CreateInstance(t);
     
            String link = server + "//Mau_KHDD.xltx";
            xlBook = ((Microsoft.Office.Interop.Excel.Application)xlApp).Workbooks.Open(link);
            xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            ((Microsoft.Office.Interop.Excel.Application)xlApp).Visible = true;

            //TIEUDE
            HRM_PLAN plan = (from p in dc.HRM_PLAN
                             where p.PlanID == planID
                             select p).SingleOrDefault();

            xlSheet.Cells[8, 1] = plan.PlanName;


            Microsoft.Office.Interop.Excel.Range mrange;
            int row0 = 14;
            int i = 14;
            int icu = 14;
            var detailPlan = from p in dc.HRM_DETAILPLAN
                             where p.PlanID == planID
                             orderby p.CrewOffDepartmentID
                             select p;
            String tenTau = "";

            foreach (HRM_DETAILPLAN dp in detailPlan)
            {
                HRM_EMPLOYEE thuyenvien = (from p in dc.HRM_EMPLOYEE
                                           where p.EmployeeID == dp.CrewOffID
                                           select p).SingleOrDefault();
                //1. Tên tàu                

                if (tenTau != dp.DIC_DEPARTMENT.Description || i == 14)
                {
                    tenTau = dp.DIC_DEPARTMENT.Description;
                    xlSheet.Cells[i, 1] = dp.DIC_DEPARTMENT.Description;

                    if (i != 14)
                    {
                        Microsoft.Office.Interop.Excel.Range mRange2;
                        mRange2 = xlSheet.Range[xlSheet.Cells[icu, 1], xlSheet.Cells[i - 1, 1]];
                        mRange2.Merge();
                        mRange2.Font.Bold = true;
                        mRange2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        mRange2.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        icu = i;
                    }
                }

                if (thuyenvien != null)
                {
                    //2. Họ và tên thuyền viên
                    xlSheet.Cells[i, 2] = thuyenvien.FirstName + " " + thuyenvien.LastName;
                    //3. Chức danh trên tàu
                    xlSheet.Cells[i, 3] = dp.CrewOffPosition;
                    //4. Năm sinh
                    if (thuyenvien.BirthDay != null)
                    {
                        xlSheet.Cells[i, 4] = thuyenvien.BirthDay.Value.Year.ToString();
                    }
                    //5. Ngày xuống tàu
                    xlSheet.Cells[i, 5] = Convert.ToDateTime(dp.DateOff).ToString("dd/MM/yyyy");
                    //6. Thời gian đi tàu
                    xlSheet.Cells[i, 6] = dp.TimeOff;
                }

                HRM_EMPLOYEE dutru = (from p in dc.HRM_EMPLOYEE
                                      where p.EmployeeID == dp.CrewOnID
                                      select p).SingleOrDefault();

                if (dutru != null)
                {
                    //7. Họ tên dự trữ
                    xlSheet.Cells[i, 7] = dutru.FirstName + " " + dutru.LastName;
                    //8. Năm sinh
                    if (dutru.BirthDay != null)
                    {
                        xlSheet.Cells[i, 8] = dutru.BirthDay.Value.Year.ToString();
                    }
                    //9. Trình độ
                    if(dp.DIC_EDUCATION != null)
                        xlSheet.Cells[i, 9] = dp.DIC_EDUCATION.Description;
                    //10.QT Đi tàu
                    xlSheet.Cells[i, 10] = dp.CrewOnHistory;
                    //11. Thời gian ở dự trữ
                    xlSheet.Cells[i, 11] = dp.TimeOn;
                    //12. Note
                    if (dp.Note != null)
                        xlSheet.Cells[i, 12] = dp.Note;
                }

                Microsoft.Office.Interop.Excel.Range mRange1;
                mRange1 = xlSheet.Range[xlSheet.Cells[i, 1], xlSheet.Cells[i - 1, 12]];
                var border_duoi = mRange1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom];
                border_duoi.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border_duoi.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                i++;

            }

            Microsoft.Office.Interop.Excel.Range mRange3;
            mRange3 = xlSheet.Range[xlSheet.Cells[icu, 1], xlSheet.Cells[i - 1, 1]];
            mRange3.Merge();
            mRange3.Font.Bold = true;
            mRange3.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            mRange3.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //Border
            //Border các cột
            Microsoft.Office.Interop.Excel.Range mRange;

            for (int col = 1; col <= 12; col++)
            {
                mRange = xlSheet.Range[xlSheet.Cells[14, col], xlSheet.Cells[i - 1, col]];
                var border_ngang = mRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight];
                border_ngang.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border_ngang.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            }






        }
        public void QuyetDinhDieuDong(String server, int ID_QTCT)
        {


            HRM_EMPLOYMENTHISTORY emhis = (from p in dc.HRM_EMPLOYMENTHISTORY
                                           where p.EmploymentHistoryID == ID_QTCT
                                           select p).SingleOrDefault();

            viewHRM_EMPLOYMENTHISTORY vemhis = (from p in dc.viewHRM_EMPLOYMENTHISTORY
                                                where p.EmploymentHistoryID == ID_QTCT
                                                select p).SingleOrDefault();






            Boolean is_on = false;

            HRM_DETAILPLAN dtplan = (from p in dc.HRM_DETAILPLAN
                                     where p.CrewOffHistoryID == emhis.EmploymentHistoryID
                                     select p).SingleOrDefault();
            if (dtplan == null)
            {
                dtplan = (from p in dc.HRM_DETAILPLAN
                          where p.CrewOnHistoryID == emhis.EmploymentHistoryID
                          select p).SingleOrDefault();

                is_on = true;
            }

            String donvi, chucvu, tentau, nguoinhanbangiao;
            tentau = dtplan.DIC_DEPARTMENT.DepartmentName;
            if (is_on)
            {
                chucvu = dtplan.CrewOnPosition;
                donvi = "BỘ PHẬN DỰ TRỮ THUYỀN VIÊN";
                nguoinhanbangiao = "";
            }
            else
            {
                chucvu = dtplan.CrewOffPosition;
                donvi = dtplan.DIC_DEPARTMENT.DepartmentName;
                nguoinhanbangiao = (from p in dc.HRM_EMPLOYEE
                                    where p.EmployeeID == dtplan.CrewOnID
                                    select p.FirstName + " " + p.LastName).Single();
            }

            Object oMissing = System.Reflection.Missing.Value;
            Object oTemplatePath;
            if (dtplan.CrewOffID == null || dtplan.CrewOnID == null || is_on)
                oTemplatePath = server + "\\Mau_QDDD_CaNhan.dotx";
            else
                oTemplatePath = server + "\\Mau_QDDD.dotx";

            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document wordDoc = new Document();

            wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);

            foreach (Field myMergeField in wordDoc.Fields)
            {


                Microsoft.Office.Interop.Word.Range rngFieldCode = myMergeField.Code;
                String fieldText = rngFieldCode.Text;

                // ONLY GETTING THE MAILMERGE FIELDS

                if (fieldText.StartsWith(" MERGEFIELD"))
                {
                    // THE TEXT COMES IN THE FORMAT OF
                    // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
                    // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

                    Int32 endMerge = fieldText.IndexOf("\\");
                    Int32 fieldNameLength = fieldText.Length - endMerge;
                    String fieldName = fieldText.Substring(11, endMerge - 11);

                    // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE

                    fieldName = fieldName.Trim();
                    // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//

                    // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE

                    //Các trường cần thay thế
                    //1. Name
                    if (fieldName == "Name")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(emhis.HRM_EMPLOYEE.FirstName.ToUpper() + " " + emhis.HRM_EMPLOYEE.LastName.ToUpper());
                    }

                    //2. NamSinh
                    if (fieldName == "NamSinh")
                    {
                        myMergeField.Select();
                        if (emhis.HRM_EMPLOYEE.BirthDay != null)
                            wordApp.Selection.TypeText(emhis.HRM_EMPLOYEE.BirthDay.Value.Year.ToString());
                    }

                    //3. DonVi
                    if (fieldName == "DonVi")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(donvi.ToUpper());
                    }
                    //4. ChucVu
                    if (fieldName == "ChucVu")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(chucvu.ToUpper());
                    }
                    //5. DonViMoi
                    if (fieldName == "DonViMoi")
                    {
                        myMergeField.Select();
                        if (vemhis.DepartmentID == 18 || vemhis.DepartmentID == 19 || vemhis.DepartmentID == 20 || vemhis.DepartmentID == 21)
                            wordApp.Selection.TypeText("BỘ PHẬN DỰ TRỮ THUYỀN VIÊN");
                        else
                            wordApp.Selection.TypeText(vemhis.DepartmentName.ToUpper());

                    }
                    //6. ChucVuMoi
                    if (fieldName == "ChucVuMoi")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(vemhis.ChucVu.ToUpper());
                    }
                    //7. NgayQD
                    if (fieldName == "NgayQD")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(vemhis.DecisionDate.Value.ToString("dd/MM/yyyy"));
                    }
                    //8. NguoiNhanBanGiao
                    if (fieldName == "NguoiNhanBanGiao")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(nguoinhanbangiao);
                    }
                    //9. TenTau
                    if (fieldName == "TenTau")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(tentau);
                    }
                    //10. NoiDungQuyetDinh
                    if (fieldName == "NoiDungQuyetDinh")
                    {
                        if (vemhis.ContentDecision != null)
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(vemhis.ContentDecision.ToUpper());
                        }
                    }
                    //11. SoQD
                    if (fieldName == "SoQD")
                    {
                        if (vemhis.ContentDecision != null)
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(vemhis.DecisionNo.ToString().ToUpper());
                        }
                    }
                    //11. Ngay
                    if (fieldName == "Ngay")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(vemhis.DecisionDate.Value.Day.ToString());
                    }
                    //11. Thang
                    if (fieldName == "Thang")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(vemhis.DecisionDate.Value.Month.ToString());
                    }
                    //11. Nam
                    if (fieldName == "Nam")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(vemhis.DecisionDate.Value.Year.ToString());
                    }
                }
            }
            wordApp.Visible = true;
            //wordDoc.SaveAs("myfile.doc");
            //wordApp.Documents.Open("myFile.doc");
            //wordApp.Application.Quit();
        }
        public void DanhSachDoiBangCap(String server, string[] listEmployeeID)
        {
           


            object xlApp;
            Workbook xlBook;
            Worksheet xlSheet;
            Type t = Type.GetTypeFromProgID("Excel.Application");
            xlApp = System.Activator.CreateInstance(t);

            String link = server + "//Mau_DSBC.xltx";
            xlBook = ((Microsoft.Office.Interop.Excel.Application)xlApp).Workbooks.Open(link);
            xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            ((Microsoft.Office.Interop.Excel.Application)xlApp).Visible = true;

          


            //Microsoft.Office.Interop.Excel.Range mrange;
            //int row0 = 14;
            int i = 14;
            //int icu = 14;
            //Duyệt qua danh sách để in ra
            int stt = 1;
            foreach (string item in listEmployeeID)
            {
                char[] delimiterChars = { '|' };
                string[] list = item.Split(delimiterChars);
                int employeeID = Convert.ToInt32(list[0].Trim());
                int degreeID = Convert.ToInt32(list[1].Trim());
                HRM_EMPLOYEE_DEGREE emdre = (from a in dc.HRM_EMPLOYEE_DEGREE
                                             where a.EmployeeID ==employeeID && a.DegreeID == degreeID
                                              select a).First();
                
 
                HRM_EMPLOYEE thuyenvien = (from p in dc.HRM_EMPLOYEE
                                           where p.EmployeeID == emdre.EmployeeID
                                           select p).SingleOrDefault();

                DIC_DEGREE dre = (from a in dc.DIC_DEGREE
                                  where a.DegreeID == emdre.DegreeID
                                  select a).First();
                if (thuyenvien != null)
                {
                    //1. Số thứ tự
                    xlSheet.Cells[i, 1] = stt++;
                    //2. Họ và tên thuyền viên
                    xlSheet.Cells[i, 2] = thuyenvien.FirstName + " " + thuyenvien.LastName;
                    //3. Bằng cấp
                    xlSheet.Cells[i, 3] = dre.DegreeName;
                    //4. Ngày cấp
                   
                        xlSheet.Cells[i, 4] = Convert.ToDateTime(emdre.DegreeDate).ToString("dd/MM/yyyy");

                    //5. Ngày hết hạn
                    xlSheet.Cells[i, 5] = Convert.ToDateTime(emdre.ExpirationDate).ToString("dd/MM/yyyy");
                     
                }

                
                //Microsoft.Office.Interop.Excel.Range mRange1;
                //mRange1 = xlSheet.Range[xlSheet.Cells[i, 1], xlSheet.Cells[i - 1, 6]];
                //var border_duoi = mRange1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom];
                //border_duoi.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                //border_duoi.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                i++;

            }

            //Microsoft.Office.Interop.Excel.Range mrange;
            //int row0 = 14;
             i = 14;
            //int icu = 14;
            //Duyệt qua danh sách để in ra
             stt = 1;
            foreach (string item in listEmployeeID)
            {



                Microsoft.Office.Interop.Excel.Range mRange1;
                mRange1 = xlSheet.Range[xlSheet.Cells[i, 1], xlSheet.Cells[i - 1, 6]];
                var border_duoi = mRange1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom];
                border_duoi.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border_duoi.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                i++;

            }

            //Microsoft.Office.Interop.Excel.Range mRange3;
            //mRange3 = xlSheet.Range[xlSheet.Cells[icu, 1], xlSheet.Cells[i - 1, 1]];
            //mRange3.Merge();
            //mRange3.Font.Bold = true;
            //mRange3.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //mRange3.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //Border
            //Border các cột
            try
            {
                Microsoft.Office.Interop.Excel.Range mRange;

                for (int col = 1; col <= 6; col++)
                {
                    mRange = xlSheet.Range[xlSheet.Cells[14, col], xlSheet.Cells[i - 1, col]];
                    var border_ngang = mRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight];
                    border_ngang.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    border_ngang.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                }
            }
            catch (Exception)
            {

                
            }
         


        }
    }
}