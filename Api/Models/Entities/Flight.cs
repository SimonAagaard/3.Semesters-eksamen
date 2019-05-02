using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.Entities
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Flytype")]
        
        //For validation kunne man f.eks. have tilføjet nedenstående 2 linjer
        //[Required]
        //[MaxLength (200)]
        public string AircraftType { get; set; }
        [DisplayName("Lokation")]
        public string FromLocation { get; set; }
        [DisplayName("Destination")]
        public string ToLocation { get; set; }
        [DisplayName("Afgangstid")]
        public DateTime DepartureTime { get; set; }
        [DisplayName("Ankomsttid")]
        public DateTime ArrivalTime { get; set; }

        //For en optimistic lock skulle nedenstående 2 linjer have været implementeret
        //[Timestamp]
        // public byte[] RowVersion { get; set; }

        //public Flight(string aircraftType, string fromLocation, string toLocation, DateTime departureTime, DateTime arrivalTime)
        //{
        //    AircraftType = aircraftType;
        //    FromLocation = fromLocation;
        //    ToLocation = toLocation;
        //    DepartureTime = departureTime;
        //    ArrivalTime = arrivalTime;
        //}
    }
}
