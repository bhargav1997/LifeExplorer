using WagWander.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using System.Web;

namespace WagWander.Controllers {
    public class UserDataController : ApiController {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Users in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all users in the database.
        /// </returns>
        /// <example>
        /// GET: api/UserData/ListUsers
        /// </example>
        [HttpGet]
        [System.Web.Http.Route("api/UserData/ListUsers")]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult ListUsers() {

            List<User> Users = db.Users.ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(u => UserDtos.Add(new UserDto() {
                UserID = u.UserID,
                UserName = u.UserName,
                Bio = u.Bio,
                FavoriteGenre = u.FavoriteGenre,
                JoinDate = u.JoinDate,
                Location = u.Location,
            }));

            return Ok(UserDtos);
        }

        /// <summary>
        /// Returns a user by id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: the specified user details with thier list
        /// </returns>
        /// <example>
        /// GET: api/UserData/FindUser/2
        /// </example>
        [ResponseType(typeof(User))]
        [System.Web.Http.Route("api/UserData/FindUser/{id}")]
        [HttpGet]
        public IHttpActionResult FindUser(int id) {
            User User = db.Users.Find(id);
            UserDto UserDto = new UserDto() {
                UserID = User.UserID,
                UserName = User.UserName,
                Bio = User.Bio,
                FavoriteGenre = User.FavoriteGenre,
                JoinDate = User.JoinDate,
                Location = User.Location,

            };
            if (User == null) {
                return NotFound();
            }

            return Ok(UserDto);
        }

        /// <summary>
        /// Updates a particular user in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the user ID primary key</param>
        /// <param name="user">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [System.Web.Http.Route("api/UserData/UpdateUser/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, User user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != user.UserID) {

                return BadRequest();
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(user).Property(u => u.UserHasPic).IsModified = false;
            db.Entry(user).Property(u => u.PicExtension).IsModified = false;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Receives animal picture data, uploads it to the webserver and updates the animal's HasPic option
        /// </summary>
        /// <param name="id">the animal id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F animalpic=@file.jpg "https://localhost:xx/api/animaldata/uploadanimalpic/2"
        /// POST: api/animalData/UpdateanimalPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        [System.Web.Http.Route("api/UserData/UploadUserPic/{id}")]
        public IHttpActionResult UploadUserPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var userPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (userPic.ContentLength > 0)
                    {
                        //establish valid file types 
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(userPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Users/"), fn);

                                //save the file
                                userPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the animal haspic and picextension fields in the database
                                User SelectedUser = db.Users.Find(id);
                                SelectedUser.UserHasPic = haspic;
                                SelectedUser.PicExtension = extension;
                                db.Entry(SelectedUser).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("User's Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }

        /// <summary>
        /// Adds a user to the system
        /// </summary>
        /// <param name="user">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: User ID, User Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserData/AddUser
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(User))]
        [System.Web.Http.Route("api/UserData/AddUser")]
        [HttpPost]
        public IHttpActionResult AddUser(User user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            user.JoinDate = DateTime.Now;

            db.Users.Add(user);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes a user from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the user</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(User))]
        [System.Web.Http.Route("api/UserData/DeleteUser/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteUser(int id) {
            User user = db.Users.Find(id);
            if (user == null) {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all users in the system associated with a particular media item.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all users in the database related to a particular media item
        /// </returns>
        /// <param name="id">MediaItem Primary Key</param>
        /// <example>
        /// GET: api/UserData/ListUsersForMediaItem/1
        /// </example>
        [HttpGet]
        [System.Web.Http.Route("api/UserData/ListUsersForMediaItem/{id}")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult ListUsersForMediaItem(int id)
        {
            var users = db.UserMediaItems
                .Include(ui => ui.User)
                .Where(ui => ui.MediaItemID == id)
                .Select(ui => new UserDto
                {
                    UserID = ui.User.UserID,
                    UserName = ui.User.UserName,
                    Bio = ui.User.Bio,
                    FavoriteGenre = ui.User.FavoriteGenre,
                    JoinDate = ui.User.JoinDate,
                    Location = ui.User.Location
                }).ToList();

            return Ok(users);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id) {
            return db.Users.Count(u => u.UserID == id) > 0;
        }
    }
}