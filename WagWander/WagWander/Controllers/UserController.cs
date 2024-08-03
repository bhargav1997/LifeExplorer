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

namespace WagWander.Controllers {
    public class UserController : Controller {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();

        static UserController() {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

        // GET: User/List
        public ActionResult List() {
            //objective: communicate with our user data api to retrieve a list of users
            //curl https://localhost:44318/api/UserData/listusers

            string url = "userdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;


            IEnumerable<UserDto> users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;


            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id) {
            DetailsUser ViewModel = new DetailsUser();

            //objective: communicate with our user data api to retrieve one user
            //curl https://localhost:44318/api/userdata/finduser/{id}

            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            Debug.WriteLine("User received : ");
            Debug.WriteLine(SelectedUser.UserName);

            ViewModel.SelectedUser = SelectedUser;

            //show associated personal list with this user
            url = "usermediaitemdata/ListUserMediaItemsForUser/" + id;
            response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserMediaItemDto> UserPersonalList = response.Content.ReadAsAsync<IEnumerable<UserMediaItemDto>>().Result;
                ViewModel.UserPersonalList = UserPersonalList;
            }
            else
            {
                ViewModel.UserPersonalList = new List<UserMediaItemDto>();
            }

            return View(ViewModel);
        }


        public ActionResult Error() {

            return View();
        }

        // GET: User/New
        public ActionResult New() {
            
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user) {
            Debug.WriteLine("the json payload is :");
            //objective: add a new user into our system using the API
            //curl -H "Content-Type:application/json" -d @user.json https://localhost:44318/api/Userdata/adduser
            string url = "userdata/adduser/";


            string jsonpayload = jss.Serialize(user);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }


        }

        // GET: User/Edit/5
        public ActionResult Edit(int id) {

            //the existing user information
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;

            return View(SelectedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User user, HttpPostedFileBase UserPic) {

            string url = "userdata/updateuser/" + id;
            string jsonpayload = jss.Serialize(user);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if(response.IsSuccessStatusCode && UserPic != null)
            {
                //Updating the User Profile picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for user
                url = "UserData/UploadUserPic/" + id;
                Debug.WriteLine("Received Profile Picture "+UserPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(UserPic.InputStream);
                requestcontent.Add(imagecontent, "UserPic", UserPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id) {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id) {
            string url = "userdata/deleteuser/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }
        }  
    }
}