using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Models.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DataContext _context;

        public FlightsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
        //GET: api/flights/Oslo/London
        [HttpGet("{fromlocation}/{tolocation}")]
        public async Task<IActionResult> GetFlightsFromTo([FromRoute] string fromLocation, [FromRoute] string toLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Skulle have været .ToListAsync() bag på linje 59 med et await keyword foran _context
            var flight =
                 _context.Flights.Where(o => o.FromLocation == fromLocation && o.ToLocation == toLocation);

            if (flight == null)
            {
                return BadRequest("The location could not be found");
            }
            else
            {
                return Ok(flight);
            }
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight([FromRoute] int id, [FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flight.Id)
            {
                return BadRequest();
            }

            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Flights
        [HttpPost]
        public async Task<IActionResult> PostFlight([FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.Id }, flight);
        }

        // DELETE: api/Flights/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteFlight([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var flight = await _context.Flights.FindAsync(id);
        //    if (flight == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Flights.Remove(flight);
        //    await _context.SaveChangesAsync();

        //    return Ok(flight);
        //}

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}