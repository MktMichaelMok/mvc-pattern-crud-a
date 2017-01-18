using Dapper;
using MobiForm30.Models;
using MvcPaging;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using MobiForm30.Lib;
using MobiForm30.Controllers;


namespace MobiForm30.Database {
    public class MobiFormDB {
        public string dbConnStr = WebConfigurationManager.AppSettings["DBConnStr"].ToString();
        // log and exception handling
        private Logger LOG = LogManager.GetCurrentClassLogger();
        private ExceptionHandle ERRHD = new ExceptionHandle();

        // for test ony
        public int PageSize = 10;

        public Int64 CompanySeq = 1;
        public Int64 UserSeq = 1;
        public string UserID = "Admin";
        public string UserName = "Admin";
        //
        public MobiFormDB()
        {
            // a constructor
        }

        #region === Role ===

        public IPagedList<T> GetRolePageList<T>(out string err, int page = 1, int pageSize = 10, string sortCol = "RoleId", string sortOrder = "Asc")
        {
            err = "";
            List<T> result = new List<T>();
            int count = default(int);

            int s = (page - 1) * pageSize + 1;
            int e = page * pageSize + 1;

            #region sql statment
            string sqlCount = string.Format(@"select count(RoleSeq) from tblRole where CompanySeq = {0};", CompanySeq);
            string sql = string.Format(@" SELECT Row_Number() OVER(ORDER BY $sortCol$ $sortOrder$) AS ROWNum, x.* 
                                    FROM
                                    (
                                    SELECT r.*, 
                                        (select u.UserName+ ' ,' from tblUser u, tblUserRole ur 
                                        where ur.RoleSeq = r.RoleSeq and u.UserSeq = ur.UserSeq FOR XML PATH('')) as UserNameList  
                                    FROM tblRole r
                                    WHERE r.CompanySeq = {0}
                                    ) x", CompanySeq);
            sql = string.Format("select * from ({0}) as t where ROWNum between @s and @e ",
                        sql.Replace("$sortCol$", sortCol).Replace("$sortOrder$", sortOrder));
            #endregion sql statement

            #region dynamic params
            var p = new DynamicParameters();
            p.Add("@s", s);
            p.Add("@e", e);
            #endregion params
            try {
                using (SqlConnection conn = new SqlConnection(dbConnStr)) {
                    using (var m = conn.QueryMultiple(sqlCount + sql, p)) {
                        count = m.Read<int>().Single();
                        result = m.Read<T>().ToList();
                    }
                }
                //
                foreach (var a in (result as List<RoleViewModel>)) {
                    if (!string.IsNullOrEmpty(a.UserNameList)) {
                        a.UserNameList = a.UserNameList.Substring(0, a.UserNameList.Length - 2); // remove last "space + ,"
                    }
                }
            }
            catch (Exception ex) {
                err = ex.Message;
                // write error log
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));                 
                return null;
            }
            return result.ToPagedList(page - 1, pageSize, count);
        }

        public int InsertRole(out string err, RoleViewModel model)
        {
            int ret = 0;
            err = "";

            string s = "insert into tblRole(RoleID, RoleName, CompanySeq) Values(@id, @name, @compSeq)";
            try {
                using (SqlConnection conn = new SqlConnection(dbConnStr)) {
                    ret = conn.Execute(s, new { @id = model.RoleID, @name = model.RoleName, @compSeq = CompanySeq });
                }
            }
            catch (Exception ex) {
                ret = -1;
                err = ex.Message;
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
            }
            return ret;
        }

        public int DeleteRole(out string err, long roleSeq)
        {
            int ret = 0;
            err = "";

            string s = "delete from tblRole where RoleSeq=@roleSeq and CompanySeq=@compSeq";
            try {
                using (SqlConnection conn = new SqlConnection(dbConnStr)) {
                    ret = conn.Execute(s, new { @roleSeq = roleSeq, @compSeq = CompanySeq });
                }
            }
            catch (Exception ex) {
                ret = -1;
                err = ex.Message;
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
            }
            return ret;
        }

        public int UpdateRole(out string err, RoleViewModel model)
        {
            int ret = 0;
            err = "";

            string s = "update tblRole set RoleID=@id, RoleName=@name where RoleSeq=@seq";
            try {
                using (SqlConnection conn = new SqlConnection(dbConnStr)) {
                    ret = conn.Execute(s, new { @id = model.RoleID, @name = model.RoleName, @seq = model.RoleSeq });
                }
            }
            catch (Exception ex) {
                ret = -1;
                err = ex.Message;
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
            }
            return ret;
        }


        public RoleViewModel GetRole(out string err, long roleSeq)
        {
            RoleViewModel result = new RoleViewModel();
            err = "";
            try {
                using (SqlConnection conn = new SqlConnection(dbConnStr)) {
                    result = conn.Query<RoleViewModel>(@"select * from tblRole where RoleSeq=@roleSeq and CompanySeq=@compSeq",
                                new { @roleSeq = roleSeq, @compSeq = CompanySeq }).FirstOrDefault();
                }
            }
            catch (Exception ex) {
                result = null;
                err = ex.Message;
                LOG.Fatal(ERRHD.BuildExceptionMessage(ex));
                // write error log
            }
            return result;
        }

        #endregion



    }
}