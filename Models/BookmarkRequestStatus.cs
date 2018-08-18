using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Isracard.Models
{
    public class BookmarkRequestStatus
    {
        public BookmarkRequestStatus()
        {
            this.Status = true;
            this.StatusType = BookmarkStatusType.OK;
            this.Content = new Dictionary<string, object>();
        }
        public string Text { get; set; }
        public bool Status { get; set; }
        public BookmarkStatusType StatusType { get; set; }
        public Dictionary<string , object> Content { get; set; }

        public enum BookmarkStatusType
        {
            OK,
            Empty,
            AlreadyAdded,
            Exception,
            RouteException
        }

        public string toJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}