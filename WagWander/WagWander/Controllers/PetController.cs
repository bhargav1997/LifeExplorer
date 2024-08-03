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
    public class PetController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PetController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/petdata/");
        }

        // GET: Pet/List
        public ActionResult List()
        {
            // Objective: communicate with our pet data api to retrieve a list of pets
            string url = "listpets";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PetDto> pets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;

            return View(pets);
        }

        // GET: Pet/Details/5
        public ActionResult Details(int id)
        {
            // Communicate with our pet data api to retrieve one pet
            string url = "findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;

            // Now fetch inquiries for this pet
            url = "ListInquiriesForPet/" + id;
            HttpResponseMessage inquiryResponse = client.GetAsync(url).Result;

            IEnumerable<InquiryDto> inquiries = inquiryResponse.Content.ReadAsAsync<IEnumerable<InquiryDto>>().Result;

            ViewBag.Inquiries = inquiries;

            return View(selectedPet);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Pet/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Pet pet)
        {
            Debug.WriteLine("The JSON payload is:");
            string url = "addpet";

            string jsonPayload = jss.Serialize(pet);
            Debug.WriteLine(jsonPayload);

            HttpContent content = new StringContent(jsonPayload);
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

        // GET: Pet/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            // Grab the pet information
            string url = "findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;

            return View(selectedPet);
        }

        // POST: Pet/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Pet pet)
        {
            try
            {
                Debug.WriteLine("The new pet info is:");
                Debug.WriteLine(pet.Name);
                Debug.WriteLine(pet.Species);
                Debug.WriteLine(pet.Breed);
                Debug.WriteLine(pet.Age);

                // Serialize into JSON
                // Send the request to the API
                string url = "updatepet/" + id;

                string jsonPayload = jss.Serialize(pet);
                Debug.WriteLine(jsonPayload);

                HttpContent content = new StringContent(jsonPayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Pet/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;

            return View(selectedPet);
        }

        // POST: Pet/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "deletepet/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }

        public ActionResult Volunteer()
        {
            return View();
        }
        public ActionResult AdoptionProcess()
        {
            return View();
        }

        public ActionResult SuccessStories()
        {
            return View();
        }

    }
}
