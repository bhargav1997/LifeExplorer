using WagWander.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

namespace WagWander.Controllers
{
    public class UserMediaItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all UserMediaItems in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database.
        /// </returns>
        /// <example>
        /// GET: api/UserMediaItemdata/ListUserMediaItems
        /// </example>
        [HttpGet]
        [Route("api/UserMediaItemdata/ListUserMediaItems")]
        [ResponseType(typeof(UserMediaItemDto))]
        public IHttpActionResult ListUserMediaItems()
        {
            var userMediaItems = db.UserMediaItems.Include(u => u.User).Include(u => u.MediaItem).ToList();
            var userMediaItemDtos = userMediaItems.Select(ui => new UserMediaItemDto
            {
                UserMediaItemID = ui.UserMediaItemID,
                UserID = ui.UserID,
                MediaItemID = ui.MediaItemID,
                Title = ui.MediaItem.Title,
                Type = ui.MediaItem.Type,
                Rating = ui.Rating,
                Review = ui.Review,
                Status = ui.Status
            }).ToList();

            return Ok(userMediaItemDtos);
        }

        /// <summary>
        /// Returns all UserMediaItems in the system associated with a particular user.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database related to a particular user
        /// </returns>
        /// <param name="id">user Primary Key</param>
        /// <example>
        /// GET: api/UserMediaItemData/ListUserMediaItemsforuser/1
        /// </example>
        [HttpGet]
        [Route("api/UserMediaItemdata/ListUserMediaItemsForUser/{id}")]
        [ResponseType(typeof(IEnumerable<UserMediaItemDto>))]
        public IHttpActionResult ListUserMediaItemsForUser(int id)
        {
            
            var userMediaItems = db.UserMediaItems
                .Include(ui => ui.MediaItem)
                .Where(ui => ui.User.UserID == id)
                .Select(ui => new UserMediaItemDto
                {
                    UserMediaItemID = ui.UserMediaItemID,
                    UserID = ui.User.UserID,
                    UserName = ui.User.UserName,
                    MediaItemID = ui.MediaItemID,
                    Title = ui.MediaItem.Title,
                    Type = ui.MediaItem.Type,
                    Rating = ui.Rating,
                    Review = ui.Review,
                    Status = ui.Status
                }).ToList();

               Debug.WriteLine(userMediaItems);

            return Ok(userMediaItems);
        }

        /// <summary>
        /// Returns all UserMediaItems in the system associated with a particular user.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database related to a particular user
        /// </returns>
        /// <param name="id">user Primary Key</param>
        /// <example>
        /// GET: api/UserMediaItemData/FindUserMediaItem/1
        /// </example>
        [HttpGet]
        [Route("api/UserMediaItemdata/FindUserMediaItemForUser/{id}")]
        [ResponseType(typeof(UserMediaItem))]
        public IHttpActionResult FindUserMediaItemForUser(int id)
        {

            var userMediaItem = db.UserMediaItems
                .Include(ui => ui.MediaItem)
                .Include(ui => ui.User)
                .Where(ui => ui.UserMediaItemID == id)
                .Select(ui => new UserMediaItemDto
                {
                    UserMediaItemID = ui.UserMediaItemID,
                    UserID = ui.UserID,
                    MediaItemID = ui.MediaItemID,
                    Rating = ui.Rating,
                    Review = ui.Review,
                    Status = ui.Status,
                    UserName = ui.User.UserName
                });
            if (userMediaItem == null)
            {
                return NotFound();
            }

            return Ok(userMediaItem);
        }

        /// <summary>
        /// Returns all UserMediaItems in the system associated with a particular user.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all UserMediaItems in the database related to a particular user
        /// </returns>
        /// <param name="id">user Primary Key</param>
        /// <example>
        /// GET: api/UserMediaItemData/FindUserMediaItem/1
        /// </example>
        [HttpGet]
        [Route("api/UserMediaItemdata/FindUserMediaItemForMediaItem/{id}")]
        [ResponseType(typeof(UserMediaItem))]
        public IHttpActionResult FindUserMediaItemForMediaItem(int id)
        {

            var userMediaItem = db.UserMediaItems
                .Include(ui => ui.MediaItem)
                .Include(ui => ui.User)
                .Where(ui => ui.MediaItemID == id)
                .Select(ui => new UserMediaItemDto
                {
                    UserMediaItemID = ui.UserMediaItemID,
                    UserID = ui.UserID,
                    MediaItemID = ui.MediaItemID,
                    Rating = ui.Rating,
                    Review = ui.Review,
                    Status = ui.Status,
                    UserName = ui.User.UserName
                });
            if (userMediaItem == null)
            {
                return NotFound();
            }

            return Ok(userMediaItem);
        }


        /// <summary>
        /// Returns usermediaitem in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An usermediaitem in the system matching up to the userMediaItem ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the userMediaItem</param>
        /// <example>
        /// GET: api/MediaItemData/FinduserMediaItem/5
        /// </example>
        [ResponseType(typeof(MediaItemDto))]
        [Route("api/UserMediaItemdata/FindUserMediaItem/{id}")]
        [HttpGet]
        public IHttpActionResult FindUserMediaItem(int id)
        {
            UserMediaItem UserMediaItem = db.UserMediaItems.Find(id);
            UserMediaItemDto UserMediaItemDto = new UserMediaItemDto()
            {
                UserMediaItemID = UserMediaItem.UserMediaItemID,
                UserID = UserMediaItem.UserID,
                MediaItemID = UserMediaItem.MediaItemID,
                Rating = UserMediaItem.Rating,
                Review = UserMediaItem.Review,
                Status = UserMediaItem.Status,
                Title = UserMediaItem.MediaItem.Title,
                Type = UserMediaItem.MediaItem.Type,
                UserName = UserMediaItem.User.UserName
            };
            if (UserMediaItem == null)
            {
                return NotFound();
            }

            return Ok(UserMediaItemDto);
        }


        /// <summary>
        /// Updates a particular UserMediaItem in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the UserMediaItem ID primary key</param>
        /// <param name="UserMediaItem">JSON FORM DATA of an UserMediaItem</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemData/UpdateUserMediaItem/5
        /// FORM DATA: UserMediaItem JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [Route("api/UserMediaItemdata/UpdateUserMediaItem/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateUserMediaItem(int id, UserMediaItem UserMediaItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != UserMediaItem.UserMediaItemID)
            {

                return BadRequest();
            }

            db.Entry(UserMediaItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMediaItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an UserMediaItem to the system
        /// </summary>
        /// <param name="UserMediaItem">JSON FORM DATA of an UserMediaItem</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: UserMediaItem ID, UserMediaItem Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemData/AddUserMediaItem
        /// FORM DATA: UserMediaItem JSON Object
        /// </example>
        [ResponseType(typeof(UserMediaItem))]
        [Route("api/UserMediaItemdata/AddUserMediaItem")]
        [HttpPost]
        public IHttpActionResult AddUserMediaItem(UserMediaItem UserMediaItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            db.UserMediaItems.Add(UserMediaItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = UserMediaItem.UserMediaItemID }, UserMediaItem);
        }

        /// <summary>
        /// Deletes a UserMediaItem from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the UserMediaItem</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserMediaItemData/DeleteUserMediaItem/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(UserMediaItem))]
        [Route("api/UserMediaItemdata/DeleteUserMediaItem/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteUserMediaItem(int id)
        {
            UserMediaItem UserMediaItem = db.UserMediaItems.Find(id);
            if (UserMediaItem == null)
            {
                return NotFound();
            }

            db.UserMediaItems.Remove(UserMediaItem);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserMediaItemExists(int id)
        {
            return db.UserMediaItems.Count(e => e.UserMediaItemID == id) > 0;
        }
    }
}