using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcContractorShareApplication.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Address  { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int ClientId { get; set; }
        public int? ContractorId { get; set; }
        public string Posted { get; set; }
    }

    public class Comment
    {
        public int? CommentId { get; set; }
        public int? JobId { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string Title { get; set; }
        public string comment { get; set; }
        public DateTime? Created { get; set; }
    }

    public class Task
    {
        public int TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int? JobId { get; set; }
        public string JobName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class EditTask
    {
        public class Status
        {
            public int StatusId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Status> StatusList =
           new List<Status>
            {
                new Status {StatusId = 1, Value = " Open "},
                new Status {StatusId = 2, Value = " In Progress "},
                new Status {StatusId = 3, Value = " Closed "},
                new Status {StatusId = 4, Value = " Cancelled "},
          };

        public int TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ChangeTaskStatus
    {
        public class Status
        {
            public int StatusId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Status> StatusList =
           new List<Status>
            {
                new Status {StatusId = 1, Value = " Open "},
                new Status {StatusId = 2, Value = " In Progress "},
                new Status {StatusId = 3, Value = " Closed "},
                new Status {StatusId = 4, Value = " Cancelled "},
          };

        public int TaskId { get; set; }
        public string Name { get; set; }
        public int JobId { get; set; }
        public int StatusId { get; set; }
    }


    public class NewJobModel
    {
        public class Category
        {
            public int CategoryId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Category> Categories =
            new List<Category>
            {
                new Category {CategoryId = 1, Value = " Architectural Services "},
                new Category {CategoryId = 2, Value = " Bathroom Fitting "},
                new Category {CategoryId = 3, Value = " Bricklaying "},
                new Category {CategoryId = 4, Value = " Carpet Fitting "}
            };

        [Required]
        [Display(Name = "Title")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int JobType { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
    }

    public class ViewJob
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int ClientId { get; set; }
        public int? ContractorId { get; set; }
        public string Posted { get; set; }

        //current comments
        public string message { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Task> Tasks { get; set; }
    }


    public class EditJobModel
    {
        public class Category
        {
            public int CategoryId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Category> Categories =
            new List<Category>
            {
                new Category {CategoryId = 1, Value = " Architectural Services "},
                new Category {CategoryId = 2, Value = " Bathroom Fitting "},
                new Category {CategoryId = 3, Value = " Bricklaying "},
                new Category {CategoryId = 4, Value = " Carpet Fitting "}
            };

        [Required]
        [Display(Name = "Title")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int JobType { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        //Hidden fields
        public int JobId { get; set; }
        public int StatusId { get; set; }
        public int ClientId { get; set; }
        public decimal CoordX { get; set; }
        public decimal CoordY { get; set; }

    }

    public class CancelJobModel
    {
        public string JobName { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string StatusName { get; set; }

        //Hidden fields
        public int JobId { get; set; }
        public string Posted { get; set; }

    }

    public class CompleteJobModel
    {
        public string JobName { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string StatusName { get; set; }

        //Hidden fields
        public int JobId { get; set; }
        public string Posted { get; set; }

    }

    public class SearchJobModel
    {
        public class Category
        {
            public int CategoryId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Category> Categories =
            new List<Category>
            {
                new Category {CategoryId = 1, Value = " Architectural Services "},
                new Category {CategoryId = 2, Value = " Bathroom Fitting "},
                new Category {CategoryId = 3, Value = " Bricklaying "},
                new Category {CategoryId = 4, Value = " Carpet Fitting "}
            };

        public int CategoryId { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
