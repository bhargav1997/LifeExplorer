using WagWander.Models;
using WagWander.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WagWander.Controllers
{
    public class MediaItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();

        static MediaItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

    // GET: MediaItem/List
    public ActionResult List()
        {
            //objective: communicate with our mediaitem data api to retrieve a list of mediaitem
            //curl https://localhost:44341/api/MediaItemdata/listMediaItems
            string url = "MediaItemData/ListMediaItems/";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("==>" + response);

            IEnumerable<MediaItemDto> MediaItem = response.Content.ReadAsAsync<IEnumerable<MediaItemDto>>().Result;

            return View(MediaItem);
        }

        // GET: MediaItem/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our mediaitem data api to retrieve one mediaitem
            //curl https://localhost:44341/api/mediaitemdata/findmediaitem/{id}
            DetailsMediaItem ViewModel = new DetailsMediaItem();

            string url = "mediaitemdata/findmediaitem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MediaItemDto SelectedMediaItem = response.Content.ReadAsAsync<MediaItemDto>().Result;

            ViewModel.SelectedMediaItem = SelectedMediaItem;

            url = "UserData/ListUsersForMediaItem/" + id;

            response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserDto> RelatedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                Debug.WriteLine(RelatedUsers);
                ViewModel.RelatedUsers = RelatedUsers;
            }
            else
            {
                ViewModel.RelatedUsers = new List<UserDto>();
            }

            url = "UserMediaItemData/FindUserMediaItemForMediaItem/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<UserMediaItemDto> RelatedUserLists = response.Content.ReadAsAsync<IEnumerable<UserMediaItemDto>>().Result;
            ViewModel.RelatedUserLists = RelatedUserLists;
            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: MediaItem/New
        [Authorize]
        public ActionResult New()
        {
            var locations = db.Locations.Select(l => new {
                LocationId = l.LocationId,
                LocationName = l.LocationName
            }).ToList();

            ViewBag.Locations = new SelectList(locations, "LocationId", "LocationName");

            return View();
        }

        // POST: MediaItem/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(MediaItem MediaItem)
        {
            Debug.WriteLine("the json payload is :");

            //objective: add a new mediaitem into our system using the API
            //curl -H "Content-Type:application/json" -d @mediaitem.json https://localhost:44341/api/mediaitemdata/addmediaitem 
            string url = "MediaItemdata/addMediaItem";

            string jsonpayload = jss.Serialize(MediaItem);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: MediaItem/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "mediaitemdata/findmediaitem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MediaItemDto selectedMediaItem = response.Content.ReadAsAsync<MediaItemDto>().Result;
            return View(selectedMediaItem);
        }

        // POST: MediaItem/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, MediaItem MediaItem)
        {

            string url = "mediaitemdata/updatemediaitem/" + id;
            string jsonpayload = jss.Serialize(MediaItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: MediaItem/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "mediaitemdata/findmediaitem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MediaItemDto selectedMediaItem = response.Content.ReadAsAsync<MediaItemDto>().Result;
            return View(selectedMediaItem);
        }

        // POST: MediaItem/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "Mediaitemdata/deletemediaitem/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}