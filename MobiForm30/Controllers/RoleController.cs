using MobiForm30.Lib;
using MobiForm30.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;


/// <summary>
/// Template: CRUD-A:
/// Author: Michael Mok
/// Date: 2017-01-13
/// Version: v1.0.0
/// </summary>
/// TODO: 1.DataAnnotations still does not work.
/// TODO: 2.Jquery UI Dialog image missed.
/// TODO: 3. Add NLog module
/// TODO: 4. Add Confirm Dialog by JQuery UI
namespace MobiForm30.Controllers
{
    public class RoleController : _BaseController
    {
        // GET: Role
        public ActionResult Index()
        {
            return View("List");
        }

        // GET: Role/Details/5
        public ActionResult List()
        {           
            return View();
        }

        public ActionResult ListPartial(int page = 1, string sortCol = "RoleID", string sortType = "asc")
        {
            string err;

            try {
                var result = MBFDB.GetRolePageList<RoleViewModel>(out err, page, MBFDB.PageSize, sortCol, sortType);
                ViewBag.Page = page;
                ViewBag.SortCol = sortCol;
                ViewBag.SortType = sortType;
                //
                Session["PageInfo"] = new PageInfoModel(page, sortCol, sortType);
                if (string.IsNullOrEmpty(err)) {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return PartialView("ListPartial", result);
                }
                else {
                    LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DB_ACCESS, "MBFDB.GetRolePageList"));
                    return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DB_ACCESS));
                }
            }
            catch (Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");
            }            
        }

        public ActionResult Create()
        {
            try {
                var model = new RoleViewModel();
                return PartialView(model);
            }
            catch (Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");
            }            
        }       

        [HttpPost]        
        public ActionResult Create(string roleID, string roleName)
        {
            int ret = 0;
            string err = "";            

            if (!ModelState.IsValid)
                return RedirectToAction("Create");            

            if (string.IsNullOrEmpty(roleID) || string.IsNullOrEmpty(roleName)) {
                LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DATA_EMPTY));
                return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DATA_EMPTY));
            }

            try {
                var model = new RoleViewModel();
                model.RoleID = roleID;
                model.RoleName = roleName;
                ret = MBFDB.InsertRole(out err, model);
                if (ret > 0) {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var pageInfo = (PageInfoModel)Session["PageInfo"];
                    return RedirectToAction("ListPartial", new { page = pageInfo.Page, SortCol = pageInfo.SortCol, SortType = pageInfo.SortType });
                }
                else {
                    LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DB_INSERT, "MBFDB.InsertRole"));
                    return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DB_INSERT));
                }
            }
            catch (Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");
            }            
        }

        public ActionResult Delete(int id)
        {
            string err;
            try {                
                int ret = MBFDB.DeleteRole(out err, id);
                if (ret > 0) {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var pageInfo = (PageInfoModel)Session["PageInfo"];
                    return RedirectToAction("ListPartial", new { page = pageInfo.Page, SortCol = pageInfo.SortCol, SortType = pageInfo.SortType });
                }
                else {
                    LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DB_DELETE, "MBFDB.DeleteRole"));
                    return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DB_DELETE));
                }
            }
            catch (Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");
            }
        }


        // GET: Role/Edit/5
        public ActionResult Edit(long id)
        {
            string err;            

            try {
                var model = MBFDB.GetRole(out err, id);
                if (string.IsNullOrEmpty(err)) {
                    return PartialView(model);
                }
                else {
                    LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DB_ACCESS, "MBFDB.GetRole"));
                    return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DB_ACCESS));
                }
            }
            catch (Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");
            }

        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(long roleSeq, string roleID, string roleName)
        {
            string err;

            if (!ModelState.IsValid)
                return RedirectToAction("Edit");

            if (string.IsNullOrEmpty(roleID) || string.IsNullOrEmpty(roleName)) {
                LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DATA_EMPTY));
                return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DATA_EMPTY));
            }

            try {                
                var model = new RoleViewModel();
                model.RoleSeq = roleSeq;
                model.RoleID = roleID;
                model.RoleName = roleName;
                int ret = MBFDB.UpdateRole(out err, model);
                if (ret > 0) {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var pageInfo = (PageInfoModel)Session["PageInfo"];
                    return RedirectToAction("ListPartial", new { page = pageInfo.Page, SortCol = pageInfo.SortCol, SortType = pageInfo.SortType });
                }
                else {
                    LOG.Trace(ERRHD.BuildTraceMessage(ExceptionHandle.MBF_ERROR.ERR_DB_UPDATE, "MBFDB.UpdateRole"));                    
                    return new HttpStatusCodeResult(500, ERRHD.BuildUserMsg(ExceptionHandle.MBF_ERROR.ERR_DB_UPDATE)); // Unauthorized
                }
            }
            catch(Exception ex) {
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                return new HttpStatusCodeResult(500, "exception error.");                
            }            
        }


    }
}
