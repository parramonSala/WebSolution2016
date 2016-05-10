using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcContractorShareApplication.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int? ProposalId { get; set; }
        public int JobId { get; set; }
        public int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int StatusId { get; set; }
        public DateTime? MeetingTime { get; set; }
        public bool Active { get; set; }
        public decimal? AproxDuration { get; set; }
        public decimal? LocationCoordX { get; set; }
        public decimal? LocationCoordY { get; set; }
    }

    public class ViewAppointmentModel
    {
        public int AppointmentId { get; set; }
        public int? ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public int StatusId { get; set; }        
        public string StatusName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string AproxDuration { get; set; }
    }

    public class CancelAppointmentModel
    {
        public int AppointmentId { get; set; }
        public int? ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public int StatusId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? MeetingTime { get; set; }
    }


}
