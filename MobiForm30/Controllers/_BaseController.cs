using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobiForm30.Database;
using NLog;
using MobiForm30.Lib;

namespace MobiForm30.Controllers
{
    public class _BaseController : Controller
    {        
        public MobiFormDB MBFDB;
        public Logger LOG = LogManager.GetCurrentClassLogger();
        public ExceptionHandle ERRHD = new ExceptionHandle();
        public _BaseController()
        {
            MBFDB = new MobiFormDB();
        }
        ~_BaseController()
        {
        }
    }
}