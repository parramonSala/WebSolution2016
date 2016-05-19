using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcContractorShareApplication.Models
{
    public class JobRate
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int JobCategoryId { get; set; }
        public string JobCategoryName { get; set; }
        public int? ContractorID { get; set; }
        public string ContractorName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Price { get; set; }
        public double? AverageRate { get; set; }
    }

    public class Rate
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int JobId { get; set; }
        public string ToUserName { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
    }

}
