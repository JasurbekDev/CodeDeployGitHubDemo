using CatalogUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CatalogUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            //Hosted web API REST Service base url 
            string Baseurl = "https://localhost:44372/";
            List<Product> ProdInfo = new List<Product>();
            using (var client = new HttpClient())
            {
                //Passing service base url 
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format 
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                 HttpResponseMessage Res = await client.GetAsync("api/Product");
                //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                     ProdInfo = JsonConvert.DeserializeObject<List<Product>>(PrResponse);
                }
                //returning the Product list to view 
                return View(ProdInfo);
            }
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            string Baseurl = "http://localhost:51842/";
            Product student = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);
                //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                     student = JsonConvert.DeserializeObject<Product>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(student);
        }
        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product prod)
        {
            try
            {
                // TODO: Add update logic here
                string Baseurl = "http://localhost:51842/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);
                    Product student = null;
                    //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var PrResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Product list
 student = JsonConvert.DeserializeObject<Product>(PrResponse);
                    }
                    prod.ProductCategory = student.ProductCategory;
                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<Product>("api/Product/" + prod.Id,
                    prod);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            //return View(student);
 return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        }

    }