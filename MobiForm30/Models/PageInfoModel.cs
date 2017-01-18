using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiForm30.Models {
    public class PageInfoModel {

        public int Page { set; get; }
        public string SortCol { set; get; }
        public string SortType { set; get; }

        public PageInfoModel(int page, string sortCol, string sortType)
        {
            Page = page;
            SortCol = sortCol;
            SortType = sortType;
        }
    }
}