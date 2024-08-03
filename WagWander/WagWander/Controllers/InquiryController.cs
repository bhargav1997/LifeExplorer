using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Script.Serialization;
using WagWander.Models;

namespace WagWander.Controllers
{
    public class InquiryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static InquiryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/inquirydata/");
        }

        // GET: Inquiry/ListInquiries
        public ActionResult ListInquiries()
        {
            string url = "listinquiries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<InquiryDto> inquiries = response.Content.ReadAsAsync<IEnumerable<InquiryDto>>().Result;

            return View(inquiries);
        }

        // GET: Inquiry/Details/5
        public ActionResult DetailsInquiry(int InquiryId)
        {
            string url = "findinquiry/" + InquiryId;
            HttpResponseMessage response = client.GetAsync(url).Result;

            InquiryDto selectedInquiry = response.Content.ReadAsAsync<InquiryDto>().Result;

            return View(selectedInquiry);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Inquiry/NewInquiry
        public ActionResult NewInquiry(int PetId)
        {
            ViewBag.PetId = PetId;
            return View();
        }

        // POST: Inquiry/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(InquiryDto inquiry)
        {
            string url = "addinquiry";
            string jsonpayload = jss.Serialize(inquiry);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListInquiries");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Inquiry/Edit/5
        public ActionResult EditInquiry(int id)
        {
            string url = "findinquiry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            InquiryDto selectedInquiry = response.Content.ReadAsAsync<InquiryDto>().Result;

            return View(selectedInquiry);
        }

        // POST: Inquiry/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Inquiry inquiry)
        {
            string url = "updateinquiry/" + id;
            string jsonpayload = jss.Serialize(inquiry);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Inquiry/Delete/5
        [Authorize]
        public ActionResult DeleteInquiry(int InquiryId)
        {
            string url = "findinquiry/" + InquiryId;
            HttpResponseMessage response = client.GetAsync(url).Result;

            InquiryDto selectedInquiry = response.Content.ReadAsAsync<InquiryDto>().Result;

            return View(selectedInquiry);
        }

        // POST: Inquiry/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "deleteinquiry/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListInquiries");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
