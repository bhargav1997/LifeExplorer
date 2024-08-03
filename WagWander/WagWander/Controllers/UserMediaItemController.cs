using WagWander.Models;
using WagWander.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WagWander.Controllers
{
    public class UserMediaItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();
        static UserMediaItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

        // GET: UserMediaItem/List
        public ActionResult List()
        {
            //objective: communicate with our UserMediaItem data api to retrieve a list of UserMediaItems
            //curl https://localhost:44387/api/UserMediaItemdata/listUserMediaItems

            string url = "UserMediaItemdata/ListUserMediaItems";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserMediaItemDto> UserMediaItems = response.Content.ReadAsAsync<IEnumerable<UserMediaItemDto>>().Result;


            return View(UserMediaItems);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: UserMediaItem/New
        [Authorize]
        public ActionResult New(int id)
        {
            var userMediaItem = new UserMediaItem
            {
                UserID = id
            };

            ViewBag.MediaItems = db.MediaItems.ToList();
            return View(userMediaItem);
        }

        // POST: UserMediaItem/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(UserMediaItem UserMediaItem)
        {
            Debug.WriteLine("the json payload is :");

            //objective: add a new UserMediaItem into our system using the API
            //curl -H "Content-Type:application/json" -d @UserMediaItem.json https://localhost:44387/api/UserMediaItemdata/addUserMediaItem
            string url = "UserMediaItemdata/addUserMediaItem";


            string jsonpayload = jss.Serialize(UserMediaItem);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "User", new {id = UserMediaItem.UserID});
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: UserMediaItem/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "UserMediaItemData/FindUserMediaItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            UserMediaItemDto selectedListItem = response.Content.ReadAsAsync<UserMediaItemDto>().Result;

            ViewBag.MediaItems = db.MediaItems.ToList();
            return View(selectedListItem);
        }

        // POST: UserMediaItem/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, UserMediaItem UserMediaItem)
        {

            string url = "UserMediaItemdata/updateUserMediaItem/" + id;
            string jsonpayload = jss.Serialize(UserMediaItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "User", new { id = UserMediaItem.UserID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: UserMediaItem/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our animal data api to retrieve one user
            //curl https://localhost:44318/api/usermediaitemdata/FindUserMediaItem/{id}

            string url = "UserMediaItemData/FindUserMediaItem/" + id;
            Debug.WriteLine(url);
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.Write(response.StatusCode);
            UserMediaItemDto UserPersonalMediaItem = response.Content.ReadAsAsync<UserMediaItemDto>().Result;
            
            
            return View(UserPersonalMediaItem);
        }


        // GET: UserMediaItem/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "usermediaItemdata/FindUserMediaItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            UserMediaItemDto selectedUserMediaItem = response.Content.ReadAsAsync<UserMediaItemDto>().Result;
            return View(selectedUserMediaItem);
        }

        // POST: UserMediaItem/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "UserMediaItemdata/deleteUserMediaItem/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list", "MediaItem");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}