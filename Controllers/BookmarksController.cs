using Isracard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Isracard.Controllers
{

    //! 
    public class BookmarksController : Controller
    {
        // GET: Bookmarks

            public ActionResult Index()
        {
            return File(Server.MapPath("Views/Bookmarks/") + "index.html", "text/html");
        }

        [System.Web.Mvc.HttpPost]
        public ContentResult AddToBookmarks(Bookmark bookmark)
        {
            BookmarkRequestStatus response = new BookmarkRequestStatus();
            try
            {
                if (Session["Bookmarks"] == null)
                {
                    Session["Bookmarks"] = new List<Bookmark>();
                }
                if (!findBookmark(bookmark.Id)) { 
                    ((List<Bookmark>)Session["Bookmarks"]).Add(bookmark);
                }
                else {
                    response.Status = false;
                    response.StatusType = BookmarkRequestStatus.BookmarkStatusType.AlreadyAdded;
                    response.Text = "Bookmark is already added";
                }
            }
            catch (Exception ex) {
                response.Status = false;
                response.StatusType = BookmarkRequestStatus.BookmarkStatusType.Exception;
                response.Text = ex.Message;
            }
            response.Content.Add("RepId", bookmark);
            return new ContentResult()
            {
                ContentType = "application/json",
                Content = response.toJson()
            };
        }

        [System.Web.Mvc.HttpGet]
        public ContentResult GetAllBookmarks()
        {
            BookmarkRequestStatus response = new BookmarkRequestStatus();
            try
            {
                if (Session["Bookmarks"] == null)
                {
                    response.Status = false;
                    response.StatusType = BookmarkRequestStatus.BookmarkStatusType.Empty;
                    response.Text = "Your list of bookmarks is empty";
                }else
                {
                    response.Content.Add("Bookmarks", (List<Bookmark>)Session["Bookmarks"]);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusType = BookmarkRequestStatus.BookmarkStatusType.Exception;
                response.Text = ex.Message;
            }
            return new ContentResult
            {
                ContentType = "application/json",
                Content = response.toJson()
            };
        }

        private bool findBookmark(int id)
        {
            foreach(Bookmark bookmark in (List<Bookmark>)Session["Bookmarks"])
            {
                if(bookmark.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}