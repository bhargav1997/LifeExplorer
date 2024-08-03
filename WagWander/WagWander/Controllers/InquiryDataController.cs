using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WagWander.Models;

namespace WagWander.Controllers
{
    public class InquiryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all inquiries in the database.
        /// </summary>
        /// <returns>
        /// CONTENT: A list of inquiries with their details.
        /// </returns>
        /// <example>
        /// GET: api/InquiryData/ListInquiries
        /// </example>

        [HttpGet]
        [Route("api/InquiryData/ListInquiries")]
        public IEnumerable<InquiryDto> ListInquiries()
        {
            List<Inquiry> Inquiries = db.Inquiries.ToList();
            List<InquiryDto> InquiryDtos = new List<InquiryDto>();

            Inquiries.ForEach(i => InquiryDtos.Add(new InquiryDto()
            {
                InquiryId = i.InquiryId,
                PetName = i.PetName,
                PetId = i.PetId,
                Username = i.Username,
                InquiryText = i.InquiryText
            }));

            return InquiryDtos;
        }

        /// <summary>
        /// Retrieves details of a specific inquiry based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the inquiry to retrieve.</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// GET: api/InquiryData/FindInquiry/5
        /// </example>

        [ResponseType(typeof(InquiryDto))]
        [HttpGet]
        [Route("api/InquiryData/FindInquiry/{id}")]
        public IHttpActionResult FindInquiry(int id)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            InquiryDto InquiryDto = new InquiryDto()
            {
                InquiryId = inquiry.InquiryId,
                PetName = inquiry.PetName,
                PetId = inquiry.PetId,
                Username = inquiry.Username,
                InquiryText = inquiry.InquiryText
            };

            return Ok(InquiryDto);
        }

        /// <summary>
        /// Updates the details of an existing inquiry based on the provided ID and inquiry data.
        /// </summary>
        /// <param name="id">The ID of the inquiry to update.</param>
        /// <param name="inquiry">The updated inquiry data.</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// POST: api/InquiryData/UpdateInquiry/5
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/InquiryData/UpdateInquiry/{id}")]
        public IHttpActionResult UpdateInquiry(int id, Inquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inquiry.InquiryId)
            {
                return BadRequest();
            }

            db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquiryExists(id))
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
        /// Adds a new inquiry to the database.
        /// </summary>
        /// <param name="inquiry">The inquiry data to add.</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// POST: api/InquiryData/AddInquiry
        /// </example>


        [HttpPost]
        [Route("api/inquirydata/addinquiry")]
        [ResponseType(typeof(Inquiry))]
        public IHttpActionResult AddInquiry(Inquiry inquiry)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inquiries.Add(inquiry);
            db.SaveChanges();

            return Ok(inquiry);
        }

        /// <summary>
        /// Deletes an inquiry from the database based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the inquiry to delete.</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// POST: api/InquiryData/DeleteInquiry/5
        /// </example>

        [ResponseType(typeof(Inquiry))]
        [HttpPost]
        [Route("api/InquiryData/DeleteInquiry/{id}")]
        public IHttpActionResult DeleteInquiry(int id)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return NotFound();
            }

            db.Inquiries.Remove(inquiry);
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

        private bool InquiryExists(int id)
        {
            return db.Inquiries.Count(e => e.InquiryId == id) > 0;
        }
    }
}
