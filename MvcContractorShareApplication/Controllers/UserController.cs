using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContractorShareApplication.Models;
using System.Web.UI.HtmlControls;
using System.IO;
using MvcContractorShareApplication.Enumerations;

// User Controller
namespace MvcContractorShareApplication.Controllers
{
    public class UserController : Controller
    {
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();
        ContractorShareServiceReference.UserInfo UserInfo = new ContractorShareServiceReference.UserInfo();
        ContractorShareServiceReference.Rate RateInfo = new ContractorShareServiceReference.Rate();

        protected HtmlInputFile myFile;

        public ActionResult EditClientProfile(int id)
        {
            var myprofile = ContractorShareService.GetUserProfile(id.ToString());

            Client model = new Client();

            model.userId = id;
            model.Firstname = myprofile.Firstname;
            model.Surname = myprofile.Surname;
            model.Address = myprofile.Address;
            model.City = myprofile.City;
            model.Email = myprofile.Email;
            model.PostalCode = myprofile.PostalCode;
            model.Country = myprofile.Country;
            model.PhoneNumber = myprofile.PhoneNumber.ToString();
            model.MobileNumber = myprofile.MobileNumber.ToString();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientProfile(Client viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //File upload to be done
                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = System.IO.Path.GetFileName(upload.FileName);
                    var ContentType = upload.ContentType;
                }

                UserInfo.Email = viewModel.Email;
                UserInfo.Firstname = viewModel.Firstname;
                UserInfo.Surname = viewModel.Surname;
                UserInfo.PhoneNumber = Convert.ToInt64(viewModel.PhoneNumber);
                UserInfo.MobileNumber = Convert.ToInt64(viewModel.MobileNumber);
                UserInfo.Address = viewModel.Address;
                UserInfo.PostalCode = viewModel.PostalCode;
                UserInfo.City = viewModel.City;
                UserInfo.Country = viewModel.Country;
                UserInfo.Categories = new List<int>().ToArray();
                UserInfo.CompanyCoordX = 0;
                UserInfo.CompanyCoordY = 0;
                UserInfo.CompanyName = null;
                UserInfo.Description = null;
                UserInfo.PricePerHour = null;
                UserInfo.website = null;

                string result = ContractorShareService.EditUserProfile(viewModel.userId.ToString(), UserInfo);

                if (result == "OK")
                {
                    return RedirectToAction("Manage", "Account");
                }
                else
                {
                    ModelState.AddModelError("", result);
                    return View(viewModel);
                }
            }
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(viewModel);
        }

        public ActionResult ClientProfile(int id)
        {
            var myprofile = ContractorShareService.GetUserProfile(id.ToString());

            ViewClientModel model = new ViewClientModel();

            model.userId = (int)Session["userId"];
            model.Name = string.Concat(myprofile.Firstname, " ", myprofile.Surname);
            model.Address = myprofile.Address;
            model.City = string.Concat(myprofile.City, ", ", myprofile.Country);
            model.Email = myprofile.Email;
            model.PostalCode = myprofile.PostalCode;
            model.Country = myprofile.Country;
            model.PhoneNumber = myprofile.PhoneNumber.ToString();
            model.MobileNumber = myprofile.MobileNumber.ToString();

            return PartialView("ClientProfile", model);
        }

        public ActionResult MyContractorProfile()
        {
            int userid = Convert.ToInt32(Session["userId"]);

            return RedirectToAction("ContractorProfile", new
                    {
                        id = userid
                    });
        }

        public ActionResult ContractorProfile(int id, string activetab = "home")
        {
            var myprofile = ContractorShareService.GetUserProfile(id.ToString());

            var fromuser = Session["userId"].ToString();
            var userisfavourite = ContractorShareService.UserIsFavourite(fromuser, id.ToString());
            var userisblocked = ContractorShareService.UserIsBlocked(fromuser, id.ToString());

            ViewContractorModel model = new ViewContractorModel();

            model.userId = id;
            model.CompanyName = myprofile.CompanyName;
            model.Address = myprofile.Address;
            model.City = string.Concat(myprofile.City, ", ", myprofile.Country);
            model.Email = myprofile.Email;
            model.PostalCode = myprofile.PostalCode;
            model.Country = myprofile.Country;
            model.PhoneNumber = myprofile.PhoneNumber.ToString();
            model.MobileNumber = myprofile.MobileNumber.ToString();

            //list of categories
            model.Categories = new List<string>();

            foreach (var category in myprofile.Categories.ToList())
            {
                ServiceCategoryEnum categoryenum = (ServiceCategoryEnum)category;
                string categoryname = EnumHelper.GetDescription(categoryenum);
                model.Categories.Add(categoryname);
            }

            model.CompanyCoordX = myprofile.CompanyCoordX;
            model.CompanyCoordY = myprofile.CompanyCoordY;
            model.mapsource = string.Concat("http://maps.google.com/maps?z=12&t=m&q=loc:", model.CompanyCoordX, "+", model.CompanyCoordY, "&output=embed");
            model.Description = myprofile.Description;
            model.PricePerHour = myprofile.PricePerHour;
            model.website = myprofile.website;
            model.AverageRate = myprofile.AverageRate;
            model.NumOfRates = myprofile.NumOfRates;
            model.IsFavourite = userisfavourite;
            model.IsBlocked = userisblocked;

            model.Comments = GetUserComments(id);

            ViewBag.Active = activetab;

            return View(model);
        }

        public ActionResult EditContractorProfile(int id)
        {
            var myprofile = ContractorShareService.GetUserProfile(id.ToString());

            Contractor model = new Contractor();

            model.userId = id;
            model.CompanyName = myprofile.CompanyName;
            model.Email = myprofile.Email;
            model.Address = myprofile.Address;
            model.City = myprofile.City;
            model.PostalCode = myprofile.PostalCode;
            model.Country = myprofile.Country;
            model.PhoneNumber = myprofile.PhoneNumber.ToString();
            model.MobileNumber = myprofile.MobileNumber.ToString();
            model.website = myprofile.website;
            model.PricePerHour = myprofile.PricePerHour;
            model.Description = myprofile.Description;

            //selected categories
            model.selectedCategories = myprofile.Categories;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditContractorProfile(Contractor EditContractorModel)
        {
            if (ModelState.IsValid)
            {

                UserInfo.Email = EditContractorModel.Email;
                UserInfo.CompanyName = EditContractorModel.CompanyName;
                UserInfo.PhoneNumber = Convert.ToInt64(EditContractorModel.PhoneNumber);
                UserInfo.MobileNumber = Convert.ToInt64(EditContractorModel.MobileNumber);
                UserInfo.Address = EditContractorModel.Address;
                UserInfo.PostalCode = EditContractorModel.PostalCode;
                UserInfo.City = EditContractorModel.City;
                UserInfo.Country = EditContractorModel.Country;
                UserInfo.website = EditContractorModel.website;
                UserInfo.PricePerHour = EditContractorModel.PricePerHour;
                UserInfo.Description = EditContractorModel.Description;

                UserInfo.Categories = EditContractorModel.selectedCategories;

                UserInfo.CompanyCoordX = 0;
                UserInfo.CompanyCoordY = 0;

                UserInfo.Firstname = null;
                UserInfo.Surname = null;

                string result = ContractorShareService.EditUserProfile(EditContractorModel.userId.ToString(), UserInfo);

                if (result == "OK")
                {
                    return RedirectToAction("MyContractorProfile", "User");
                }
                else
                {
                    ModelState.AddModelError("", result);
                    return View(EditContractorModel);
                }
            }
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(EditContractorModel);
        }

        public ActionResult ContractorSearch()
        {
            SearchContractorModel model = new SearchContractorModel();
            model.Contractors = new List<Contractor>();

            var userprofile = ContractorShareService.GetUserProfile(Session["userId"].ToString());

            model.City = userprofile.City;
            model.PostalCode = userprofile.PostalCode.Substring(0, 2);
            model.PricePerHour = 0;
            model.CategoryId = 0;

            var contractorlist = ContractorShareService.SearchContractors(model.CategoryId, model.City, model.PostalCode, model.PricePerHour);

            foreach (var s in contractorlist)
            {
                Contractor contractor = new Contractor();
                contractor.CompanyName = s.CompanyName;
                contractor.Address = s.Address;
                contractor.PostalCode = s.PostalCode;
                contractor.City = s.City;

                //list of categories
                contractor.CategoriesList = new List<string>();

                foreach (var category in s.Categories.ToList())
                {
                    ServiceCategoryEnum categoryenum = (ServiceCategoryEnum)category;
                    string categoryname = EnumHelper.GetDescription(categoryenum);
                    contractor.CategoriesList.Add(categoryname);
                }

                contractor.PricePerHour = s.PricePerHour;
                contractor.AverageRate = s.AverageRate;

                model.Contractors.Add(contractor);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ContractorSearch(SearchContractorModel model)
        {
            var contractorlist = ContractorShareService.SearchContractors(model.CategoryId, model.City, model.PostalCode, model.PricePerHour);

            model.Contractors = new List<Contractor>();

            foreach (var s in contractorlist)
            {
                Contractor contractor = new Contractor();
                contractor.userId = s.ID;
                contractor.CompanyName = s.CompanyName;
                contractor.Address = s.Address;
                contractor.PostalCode = s.PostalCode;
                contractor.City = s.City;

                //list of categories
                contractor.CategoriesList = new List<string>();

                foreach (var category in s.Categories.ToList())
                {
                    ServiceCategoryEnum categoryenum = (ServiceCategoryEnum)category;
                    string categoryname = EnumHelper.GetDescription(categoryenum);
                    contractor.CategoriesList.Add(categoryname);
                }

                contractor.PricePerHour = s.PricePerHour;
                contractor.AverageRate = s.AverageRate;

                model.Contractors.Add(contractor);
            }

            return View(model);
        }

        public ActionResult AddFavourite(int id)
        {
            string fromuser = Session["userId"].ToString();

            var result = ContractorShareService.AddFavourite(fromuser, id);
            
            return RedirectToAction("ContractorProfile", new
            {
                id = id
            });   
        }

        public ActionResult RemoveFavourite(int id)
        {
            string fromuser = Session["userId"].ToString();

            var result = ContractorShareService.RemoveFavourite(fromuser, id.ToString());

            return RedirectToAction("ContractorProfile", new
            {
                id = id
            });
        }

        public ActionResult FavouritesList()
        {
            IEnumerable<MvcContractorShareApplication.Models.Contractor> contractors = new List<Contractor>();

            var contractorlist = ContractorShareService.GetUserFavourites(Session["userId"].ToString());

            List<Contractor> contractorslist = new List<Contractor>();

            if (contractorlist != null)
            {
                foreach (var s in contractorlist)
                {
                    Contractor contractor = new Contractor();
                    contractor.CompanyName = s.CompanyName;
                    contractor.Address = s.Address;
                    contractor.PostalCode = s.PostalCode;
                    contractor.City = s.City;

                    //list of categories
                    contractor.CategoriesList = new List<string>();

                    foreach (var category in s.Categories.ToList())
                    {
                        ServiceCategoryEnum categoryenum = (ServiceCategoryEnum)category;
                        string categoryname = EnumHelper.GetDescription(categoryenum);
                        contractor.CategoriesList.Add(categoryname);
                    }

                    contractor.PricePerHour = s.PricePerHour;
                    contractor.AverageRate = s.AverageRate;

                    contractorslist.Add(contractor);
                }

                contractors = (IEnumerable<Contractor>)contractorslist;
            }

            return View(contractors);
        }

        public ActionResult BlockUser(int id)
        {
            string fromuser = Session["userId"].ToString();

            var result = ContractorShareService.BlockUser(fromuser, id);

            return RedirectToAction("ContractorProfile", new
            {
                id = id
            });
        }

        public ActionResult DenounceUser(int id)
        {
            DenounceUserModel model = new DenounceUserModel();

            var userprofile = ContractorShareService.GetUserProfile(id.ToString());

            model.ToUserId = id;
            model.ToUserName = string.IsNullOrEmpty(userprofile.CompanyName) ? string.Concat(userprofile.Firstname, " ", userprofile.Surname) : userprofile.CompanyName;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DenounceUser(DenounceUserModel model)
        {
            string fromuser = Session["userId"].ToString();
            var result = ContractorShareService.AddDenunce(fromuser, model.ToUserId, model.Comment, false);

            if (result=="OK")
            {
                return RedirectToAction("ContractorProfile", new
                {
                    id = model.ToUserId
                });
            }

            return View(model);
        }

        //Rating
        public ActionResult AddRate(int id)
        {
            var jobdetails = ContractorShareService.GetServiceRequest(id.ToString());
            var contractordetails = ContractorShareService.GetUserProfile(jobdetails.ContractorID.ToString());

            Rate model = new Rate();
            model.JobId = id;
            model.FromUserId = jobdetails.ClientID;
            model.ToUserId = (int)jobdetails.ContractorID;
            model.ToUserName= contractordetails.CompanyName;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddRate(Rate rate)
        {
            RateInfo.FromUserId = rate.FromUserId;
            RateInfo.ToUserId = rate.ToUserId;
            RateInfo.ServiceId = rate.JobId;
            if (rate.Rating == 0) RateInfo.Rating = 1;
            else RateInfo.Rating = rate.Rating;
            RateInfo.Comment = rate.Comment;

            var result = ContractorShareService.AddRating(rate.ToUserId.ToString(), RateInfo);

            if (result.message == "OK")
            {
                return RedirectToAction("RateJobsList","Home");
            }

            return View(rate);

        }

        public ActionResult DeleteRating(int id, int ToUserId)
        {
            //Call DeleteRating function for Rating with Id = id

            var result = ContractorShareService.DeleteRating(ToUserId.ToString(),id.ToString());

            if (result.message == "OK")
            {
                return RedirectToAction("ContractorProfile", new
                {
                    id = ToUserId,
                    activetab = "messages"
                });
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return RedirectToAction("ContractorProfile", new
                {
                    id = ToUserId,
                    activetab = "messages"
                });
            }
        }

        private List<UserComment> GetUserComments(int ToUserId)
        {
            var comments = ContractorShareService.GetUserRates(ToUserId.ToString());

            List<UserComment> usercomments = new List<UserComment>();

            foreach (var comment in comments)
            {
                UserComment usercomment = new UserComment();
                usercomment.CommentId = comment.RateId;
                usercomment.FromUserId = comment.FromUserId;
                usercomment.ToUserId = comment.ToUserId;
                usercomment.comment = comment.Comment;
                usercomment.Created = comment.Created;
                usercomment.Rating = comment.Rating;

                var fromuserprofile = ContractorShareService.GetUserProfile(comment.FromUserId.ToString());
                usercomment.FromUserName = String.Concat(fromuserprofile.Firstname, " ", fromuserprofile.Surname);

                usercomments.Add(usercomment);
            }

            return usercomments;
        }
    }
}
