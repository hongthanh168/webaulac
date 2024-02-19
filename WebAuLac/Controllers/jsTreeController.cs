using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public class jsTreeController : Controller
    {
        private AuLacEntities db = new AuLacEntities();
        // GET: jsTree
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetTreeData()
        {
            
                List<JsTreeModel> nodes = new List<JsTreeModel>();
                //add những node có parentID null
                List<DIC_DEPARTMENT> dvs = db.DIC_DEPARTMENT.Where(x => x.ParentID == null).OrderBy(x=> x.DepartmentID).ToList();
                foreach (DIC_DEPARTMENT dv in dvs)
                {
                    JsTreeModel node = new JsTreeModel();
                    node.id = dv.DepartmentID;
                    node.text = dv.DepartmentName;
                    SetChildren(node);
                    nodes.Add(node);
                }
                //AlreadyPopulated = true;
                return Json(nodes);
            
        }
        public void SetChildren(JsTreeModel node)
        {
            List<DIC_DEPARTMENT> dvs = db.DIC_DEPARTMENT.Where(x => x.ParentID == node.id).OrderBy(x => x.DepartmentID).ToList();
            foreach (DIC_DEPARTMENT dv in dvs)
            {
                JsTreeModel childnode = new JsTreeModel();
                childnode.id = dv.DepartmentID;
                childnode.text = dv.DepartmentName;
                SetChildren(childnode);
                node.children.Add(childnode);
            }
        }
        [HttpPost]
        public ActionResult DoJsTreeOperation(JsTreeOperationData data)
        {
            DIC_DEPARTMENT dv = new DIC_DEPARTMENT();
            int id = 0;
            switch (data.Operation)
            {
                case JsTreeOperation.CopyNode:
                case JsTreeOperation.CreateNode:
                    //todo: save data
                    dv = new DIC_DEPARTMENT();
                    dv.ParentID = int.Parse(data.ParentId);
                    dv.DepartmentName = data.Text;
                    dv.IsLast = true;
                    db.DIC_DEPARTMENT.Add(dv);
                    db.SaveChanges();
                    return Json(new { id = dv.DepartmentID }, JsonRequestBehavior.AllowGet);

                case JsTreeOperation.DeleteNode:
                    //todo: save data
                    id = int.Parse(data.Id);
                    dv = db.DIC_DEPARTMENT.Find(id);
                    db.DIC_DEPARTMENT.Remove(dv);
                    db.SaveChanges();
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);

                case JsTreeOperation.MoveNode:
                    //todo: save data
                    id = int.Parse(data.Id);
                    dv = db.DIC_DEPARTMENT.Find(id);
                    dv.ParentID = int.Parse(data.ParentId);
                    db.Entry(dv).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);                   

                case JsTreeOperation.RenameNode:
                    //kiểm tra có tên nào trùng không
                    if(db.DIC_DEPARTMENT.Any(x => x.DepartmentName == data.Text))
                    {
                        return Json(new { KetQua = false, ThongBao = "Bị trùng tên đơn vị" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //todo: save data
                        id = int.Parse(data.Id);
                        dv = db.DIC_DEPARTMENT.Find(id);
                        dv.DepartmentName = data.Text;
                        db.Entry(dv).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { KetQua = true, ThongBao = "Đã lưu vào cơ sở dữ liệu" }, JsonRequestBehavior.AllowGet);
                    }
                    

                default:
                    throw new InvalidOperationException(string.Format("{0} is not supported.", data.Operation));
            }
        }

    }
}