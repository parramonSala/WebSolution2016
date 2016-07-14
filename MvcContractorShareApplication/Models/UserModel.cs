using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcContractorShareApplication.Models
{
    public class Client
    {
        public int userId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
    }

    public class UserComment
    {
        public int? CommentId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public string FromUserName { get; set; }
        public string comment { get; set; }
        public double Rating { get; set; }
        public DateTime? Created { get; set; }
    }

    public class Contractor
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

        public int userId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public decimal CompanyCoordX { get; set; }
        public decimal CompanyCoordY { get; set; }
        public string website { get; set; }
        public string Description { get; set; }
        public double? PricePerHour { get; set; }
        public int[] selectedCategories { get; set; }
        public List<string> CategoriesList { get; set; }

        public double? AverageRate { get; set; }
    }

    public class ViewClientModel
    {
        public int userId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
    }

    public class ViewContractorModel
    {
        public string PageTitle { get; set; }

        public int userId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public decimal CompanyCoordX { get; set; }
        public decimal CompanyCoordY { get; set; }
        public string mapsource { get; set; }
        public string website { get; set; }
        public string Description { get; set; }
        public List<string> Categories { get; set; }
        public double? PricePerHour { get; set; }
        public double? AverageRate { get; set; }
        public int? NumOfRates { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsBlocked { get; set; }

        public List<UserComment> Comments { get; set; }
    }

    public class SearchContractorModel
    {
        public class Category
        {
            public int CategoryId { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<Category> Categories =
            new List<Category>
            {
                new Category {CategoryId = 0, Value = " All "},
                new Category {CategoryId = 1, Value = " Architectural Services "},
                new Category {CategoryId = 2, Value = " Bathroom Fitting "},
                new Category {CategoryId = 3, Value = " Bricklaying "},
                new Category {CategoryId = 4, Value = " Carpet Fitting "}
            };

        public int CategoryId { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public double PricePerHour { get; set; }

        public List<Contractor> Contractors { get; set; }

    }

    public class DenounceUserModel
    {
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string Comment { get; set; }
        
    }

}
