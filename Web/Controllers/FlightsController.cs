using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Models.Entities;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        private Uri BaseEndPoint { get; set; }


        public FlightsController(DataContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            BaseEndPoint = new Uri("https://localhost:44341/api/flights");
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return View(JsonConvert.DeserializeObject<List<Flight>>(data));
        }

        // GET: Flights/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var flight = await _context.Flights
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (flight == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(flight);
        //}

        // GET: Flights/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] Flight flight)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(flight);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(flight);
        //}

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var response = await _httpClient.GetAsync(BaseEndPoint.ToString() + $"/{id}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<Flight>(data);

            if (flight == null)
            {
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //For at consume API'et skulle nedenstående 2 linjer, have været implementeret
                    //var response = await _httpClient.PutAsJsonAsync<Flight>(BaseEndPoint + $"/{id}", flight);
                    //response.EnsureSuccessStatusCode();
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [FromForm] Flight editedFlight)
        //{
        //    var response = await _httpClient.GetAsync(BaseEndPoint.ToString() + $"/{id}", HttpCompletionOption.ResponseHeadersRead);
        //    response.EnsureSuccessStatusCode();
        //    var data = await response.Content.ReadAsStringAsync();
        //    var flight = JsonConvert.DeserializeObject<Flight>(data);

        //    if (flight is null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Flight toBeUpdatedFlight = new Flight(editedFlight.AircraftType, editedFlight.FromLocation, editedFlight.ToLocation, editedFlight.DepartureTime, editedFlight.ArrivalTime);
        //            var postResponse = _httpClient.PostAsJsonAsync<Flight>(BaseEndPoint, toBeUpdatedFlight);
        //            postResponse.Result.EnsureSuccessStatusCode();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FlightExists(flight.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(flight);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Flight flight)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:44341/api/flights");
        //        //post
        //        var putTask = client.PutAsJsonAsync<Flight>("flights", flight);
        //        putTask.Wait();

        //        var result = putTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return View(flight);
        //}


        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
