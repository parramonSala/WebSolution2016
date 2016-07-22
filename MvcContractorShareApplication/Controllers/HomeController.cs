using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcContractorShareApplication.Filters;
using MvcContractorShareApplication.Models;
using MvcContractorShareApplication.Enumerations;
using System.Reflection;

namespace MvcContractorShareApplication.Controllers
{
    public class HomeController : Controller
    {
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();
        ContractorShareServiceReference.JobInfo serviceinfo = new ContractorShareServiceReference.JobInfo();
        ContractorShareServiceReference.Result result = new ContractorShareServiceReference.Result();
        ContractorShareServiceReference.CommentInfo commentinfo = new ContractorShareServiceReference.CommentInfo();
        ContractorShareServiceReference.TaskInfo taskinfo = new ContractorShareServiceReference.TaskInfo();

        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["usertype"]) == 1)
            {
                return RedirectToAction("ContractorIndex", "Home");
            }
            else if (Convert.ToInt32(Session["usertype"]) == 2)
            {
                return RedirectToAction("ClientIndex", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ClientIndex()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        public ActionResult ContractorIndex()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RegisterMain()
        {
            return View();
        }

        public ActionResult PostNewJob()
        {
            var model = new NewJobModel();
            return View(model);
        }

        //Post New Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PostNewJob(NewJobModel model)
        {
            if (ModelState.IsValid)
            {
                serviceinfo.Name = model.JobName;
                serviceinfo.Description = model.Description;
                serviceinfo.CategoryID = model.JobType;
                serviceinfo.Address = model.Address;
                serviceinfo.PostalCode = model.PostalCode;
                serviceinfo.City = model.City;
                serviceinfo.Country = model.Country;
                serviceinfo.ClientID = (int)Session["userId"];
                serviceinfo.StatusID = (int)ServiceStatusEnum.Open;
                serviceinfo.PostedDate = null;
                serviceinfo.Id = 0;

                //search coord where??
                serviceinfo.CoordX = -1;
                serviceinfo.CoordY = -1;

                string result = ContractorShareService.CreateServiceRequest(serviceinfo);

                if (result == "OK")
                {
                    return RedirectToAction("MyJobs", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(model);
        }

        public ActionResult MyJobs()
        {
            IEnumerable<MvcContractorShareApplication.Models.Job> jobs = new List<Job>();

            var serviceinfolist = ContractorShareService.GetMyCurrentServices(Session["userId"].ToString());

            List<Job> jobslist = new List<Job>();

            foreach (var s in serviceinfolist)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);
                job.Posted = s.PostedDate;

                jobslist.Add(job);
            }

            jobs = (IEnumerable<Job>)jobslist;
            return View(jobs);
        }

        public ActionResult MyClosedJobs()
        {
            IEnumerable<MvcContractorShareApplication.Models.Job> jobs = new List<Job>();

            var serviceinfolist = ContractorShareService.GetMyCompletedServices(Session["userId"].ToString());

            List<Job> jobslist = new List<Job>();

            foreach (var s in serviceinfolist)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);
                job.Posted = s.PostedDate;

                jobslist.Add(job);
            }

            jobs = (IEnumerable<Job>)jobslist;
            return View(jobs);
        }

        public ActionResult EditJob(int id)
        {
            var s = ContractorShareService.GetServiceRequest(id.ToString());

            EditJobModel editjob = new EditJobModel();
            editjob.JobId = s.Id;
            editjob.JobName = s.Name;
            editjob.Description = s.Description;
            editjob.StatusId = s.StatusID;
            editjob.Address = s.Address;
            editjob.PostalCode = s.PostalCode;
            editjob.City = s.City;
            editjob.Country = s.Country;
            editjob.JobType = s.CategoryID;
            editjob.ClientId = s.ClientID;
            editjob.CoordX = s.CoordX;
            editjob.CoordY = s.CoordY;

            return View(editjob);
        }

        //Edit Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditJob(EditJobModel model)
        {
            if (ModelState.IsValid)
            {
                serviceinfo.Name = model.JobName;
                serviceinfo.Description = model.Description;
                serviceinfo.CategoryID = model.JobType;
                serviceinfo.Address = model.Address;
                serviceinfo.PostalCode = model.PostalCode;
                serviceinfo.City = model.City;
                serviceinfo.Country = model.Country;

                //Hidden fields, not amended
                serviceinfo.Id = model.JobId;
                serviceinfo.StatusID = model.StatusId;
                serviceinfo.ClientID = model.ClientId;
                serviceinfo.CoordX = model.CoordX;
                serviceinfo.CoordY = model.CoordY;

                var result = ContractorShareService.EditJob(model.JobId.ToString(), serviceinfo);

                if (result.message == "OK")
                {
                    return RedirectToAction("DetailsJob", "Home", new { id = model.JobId });
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(model);
        }

        public ActionResult DetailsJob(int id, string activetab="home")
        {
            ViewJob detailsjob = new ViewJob();

            detailsjob = FillViewJobModel(id);

            ViewBag.Active = activetab;

            return View(detailsjob);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsJob(ViewJob model)
        {
            //Add Comment
            commentinfo.CreatedByUserId = (int)Session["userId"];
            commentinfo.Title = model.message;
            commentinfo.Message = model.message;
            commentinfo.JobId = model.JobId;
            commentinfo.Created = DateTime.Now;

            result = ContractorShareService.AddJobComment(model.JobId.ToString(), commentinfo);

            ViewJob job = FillViewJobModel(model.JobId);
            job.message = string.Empty;

            if (result.message == "OK")
            {
                ViewBag.Active = "messages";
                return View(job);
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(job);
            }

        }


        public ActionResult CancelJob(int id)
        {
            CancelJobModel canceljob = new CancelJobModel();

            var s = ContractorShareService.GetServiceRequest(id.ToString());

            canceljob.JobId = id;
            canceljob.JobName = s.Name;
            canceljob.Description = s.Description;

            ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
            canceljob.StatusName = servicestatusenum.ToString();

            canceljob.Address = s.Address;
            canceljob.PostalCode = s.PostalCode;
            canceljob.City = s.City;
            canceljob.Country = s.Country;
            canceljob.Posted = s.PostedDate;
            
            ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
            canceljob.CategoryName = EnumHelper.GetDescription(servicecategory);

            return View(canceljob);
        }

        //Cancel Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CancelJob(CancelJobModel model)
        {
                //Change Status of the Job to 4-Cancelled

                int statusid = (int)ServiceStatusEnum.Cancelled;

                result = ContractorShareService.ChangeServiceStatus(model.JobId.ToString(), statusid.ToString());
                    
                if (result.message == "OK")
                {
                    return RedirectToAction("MyJobs", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            
        }

        public ActionResult CompleteJob(int id)
        {
            CompleteJobModel completejob = new CompleteJobModel();

            var s = ContractorShareService.GetServiceRequest(id.ToString());

            completejob.JobId = id;
            completejob.JobName = s.Name;
            completejob.Description = s.Description;

            ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
            completejob.StatusName = servicestatusenum.ToString();

            completejob.Address = s.Address;
            completejob.PostalCode = s.PostalCode;
            completejob.City = s.City;
            completejob.Country = s.Country;
            completejob.Posted = s.PostedDate;

            ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
            completejob.CategoryName = EnumHelper.GetDescription(servicecategory);

            return View(completejob);
        }

        //Complete Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteJob(CompleteJobModel model)
        {
            //Change Status of the Job to Closed

            int statusid = (int)ServiceStatusEnum.Closed;

            result = ContractorShareService.ChangeServiceStatus(model.JobId.ToString(), statusid.ToString());

            if (result.message == "OK")
            {
                return RedirectToAction("ContractorMyJobs", "Home");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }

        }


        public ActionResult DeleteComment (int id, int jobid)
        {
            //Call DeleteComment function for Comment with Id = id

            result = ContractorShareService.DeleteJobComment(id.ToString(),jobid.ToString());

            if (result.message == "OK")
            {
                return RedirectToAction("DetailsJob", new { 
                    id = jobid,
                    activetab = "messages"
                });
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return RedirectToAction("DetailsJob", new
                {
                    id = jobid,
                    activetab = "messages"
                });
            }
        }

        public ActionResult ReOpenJob(int id)
        {
            CancelJobModel reopenjob = new CancelJobModel();

            var s = ContractorShareService.GetServiceRequest(id.ToString());

            reopenjob.JobId = id;
            reopenjob.JobName = s.Name;
            reopenjob.Description = s.Description;

            ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
            reopenjob.StatusName = servicestatusenum.ToString();

            reopenjob.Address = s.Address;
            reopenjob.PostalCode = s.PostalCode;
            reopenjob.City = s.City;
            reopenjob.Country = s.Country;
            reopenjob.Posted = s.PostedDate;

            ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
            reopenjob.CategoryName = EnumHelper.GetDescription(servicecategory);

            return View(reopenjob);
        }

        //Re-Open Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ReOpenJob(CancelJobModel model)
        {
            //Change Status of the Job to 1-Open

            int statusid = (int)ServiceStatusEnum.Open;

            result = ContractorShareService.ChangeServiceStatus(model.JobId.ToString(), statusid.ToString());

            if (result.message == "OK")
            {
                return RedirectToAction("MyJobs", "Home");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }
        }

        //Contractor Functionalities
        public ActionResult ContractorMyJobs()
        {
            IEnumerable<MvcContractorShareApplication.Models.Job> jobs = new List<Job>();

            var serviceinfolist = ContractorShareService.GetOpenServicesAssignedToMe((int)Session["userId"]);

            List<Job> jobslist = new List<Job>();

            foreach (var s in serviceinfolist)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);
                job.Posted = s.PostedDate;

                jobslist.Add(job);
            }

            jobs = (IEnumerable<Job>)jobslist;
            return View(jobs);
        }

        public ActionResult ContractorMyClosedJobs()
        {
            IEnumerable<MvcContractorShareApplication.Models.Job> jobs = new List<Job>();

            var serviceinfolist = ContractorShareService.GetClosedServicesAssignedToMe((int)Session["userId"]);

            List<Job> jobslist = new List<Job>();

            foreach (var s in serviceinfolist)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);
                job.Posted = s.PostedDate;

                jobslist.Add(job);
            }

            jobs = (IEnumerable<Job>)jobslist;
            return View(jobs);
        }

        public ActionResult FindJobs()
        {
            SearchJobModel model = new SearchJobModel();
            model.Jobs = new List<Job>();

            var contractorprofile = ContractorShareService.GetUserProfile(Session["userId"].ToString());

            model.CategoryId = contractorprofile.Categories.First();
            model.City = contractorprofile.City;
            model.PostalCode = contractorprofile.PostalCode.Substring(0, 2);

            var listofjobs = ContractorShareService.SearchServices(model.CategoryId, model.City, model.PostalCode);

            foreach (var s in listofjobs)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);

                model.Jobs.Add(job);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult FindJobs(SearchJobModel model)
        {
            var listofjobs = ContractorShareService.SearchServices(model.CategoryId, model.City, model.PostalCode);

            model.Jobs = new List<Job>();

            foreach (var s in listofjobs)
            {
                Job job = new Job();
                job.JobId = s.Id;
                job.JobName = s.Name;
                job.Description = s.Description;
                job.StatusId = s.StatusID;
                ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
                job.StatusName = servicestatusenum.ToString();
                job.Address = s.Address;
                job.PostalCode = s.PostalCode;
                job.City = s.City;
                job.Country = s.Country;
                job.ClientId = s.ClientID;
                job.CategoryId = s.CategoryID;
                ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
                job.CategoryName = EnumHelper.GetDescription(servicecategory);

                model.Jobs.Add(job);
            }

            return View(model);
        }

        public ActionResult ViewTask(int id, int jobid)
        {
            var taskdetails = ContractorShareService.GetTask(jobid.ToString(), id.ToString());

            var jobdetails = ContractorShareService.GetServiceRequest(jobid.ToString());

            Task task = new Task();
            task.TaskId = id;
            task.Name = taskdetails.Name;
            task.Description = taskdetails.Description;
            task.StatusId = taskdetails.StatusId;

            task.JobName = jobdetails.Name;

            TaskStatusEnum taskstatusenum = (TaskStatusEnum)taskdetails.StatusId;
            task.StatusName = taskstatusenum.ToString();

            task.JobId = taskdetails.ServiceId;
            task.CreatedOn = taskdetails.Created;

            return View(task);
        }

        public ActionResult CreateTask(int id)
        {
            Task task = new Task();
            task.JobId = id;

            var jobinfo = ContractorShareService.GetServiceRequest(id.ToString());
            task.JobName = jobinfo.Name;

            return View(task);
        }

        //Post Action for CreateTask
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(Task model)
        {
            if (ModelState.IsValid)
            {
                taskinfo.Name = model.Name;
                taskinfo.Description = model.Description;
                taskinfo.ServiceId = (int)model.JobId;

                taskinfo.StatusId = (int)TaskStatusEnum.Open;
                taskinfo.Created = DateTime.Now;

                result = ContractorShareService.CreateTask(model.JobId.ToString(), taskinfo );

                if (result.message == "OK")
                {
                    return RedirectToAction("DetailsJob", new
                    {
                        id = model.JobId,
                        activetab = "tasks"
                    });
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(model);
        }

        public ActionResult EditTask(int id, int jobid)
        {
            var t = ContractorShareService.GetTask(jobid.ToString(), id.ToString());

            EditTask edittask = new EditTask();
            edittask.Name = t.Name;
            edittask.Description = t.Description;
            edittask.StatusId = t.StatusId;
            edittask.JobId = jobid;
            edittask.TaskId = id;

            return View(edittask);
        }

        //Edit Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(EditTask model)
        {
            if (ModelState.IsValid)
            {
                taskinfo.Name = model.Name;
                taskinfo.Description = model.Description;
                taskinfo.StatusId = model.StatusId;

                //hidden fields, not amended
                taskinfo.TaskId = model.TaskId;
                taskinfo.ServiceId = (int)model.JobId;
               
                var result = ContractorShareService.EditTask(model.JobId.ToString(),
                                                             model.TaskId.ToString(),
                                                             taskinfo);
                    
                if (result.message == "OK")
                {
                    return RedirectToAction("DetailsJob", new
                    {
                        id = model.JobId,
                        activetab = "tasks"
                    });
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(model);
        }

        public ActionResult ChangeTaskStatus(int id, int jobid)
        {
            var t = ContractorShareService.GetTask(jobid.ToString(), id.ToString());

            ChangeTaskStatus edittask = new ChangeTaskStatus();
            edittask.StatusId = t.StatusId;
            edittask.Name = t.Name;
            edittask.JobId = jobid;
            edittask.TaskId = id;

            return View(edittask);
        }

        //Change Task Status- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTaskStatus(ChangeTaskStatus model)
        {
            if (ModelState.IsValid)
            {
                var result = ContractorShareService.ChangeTaskStatus(model.JobId.ToString(),
                                                                     model.TaskId.ToString(),
                                                                     model.StatusId.ToString());
                    
                   
                if (result.message == "OK")
                {
                    return RedirectToAction("DetailsJob", new
                    {
                        id = model.JobId,
                        activetab = "tasks"
                    });
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Some of the data entered is incorrect. Please try again");
            return View(model);
        }

        public ActionResult DeleteTask(int id, int jobid)
        {
            //Call DeleteTask function for task with Id = id

            result = ContractorShareService.DeleteTask(jobid.ToString(), id.ToString());
                
            if (result.message == "OK")
            {
                return RedirectToAction("DetailsJob", new
                {
                    id = jobid,
                    activetab = "tasks"
                });
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return RedirectToAction("DetailsJob", new
                {
                    id = jobid,
                    activetab = "tasks"
                });
            }
        }

        public ActionResult RateJobsList()
        {
            List<JobRate> jobratelist = new List<JobRate>();

            var jobstoberated = ContractorShareService.GetJobRateInfoList(Session["userId"].ToString());

            foreach (var j in jobstoberated)
            {
                JobRate jobrate = new JobRate();
                jobrate.JobId = j.JobId;
                jobrate.JobName = j.JobName;
                jobrate.JobCategoryId = j.JobCategoryId;
                jobrate.JobCategoryName = EnumHelper.GetDescription((ServiceCategoryEnum)j.JobCategoryId);
                jobrate.ContractorID = j.ContractorID;
                jobrate.ContractorName = j.ContractorName;
                jobrate.AverageRate = j.AverageRate;
                jobrate.AppointmentDate = j.AppointmentDate;
                jobrate.Price = "Total Price:" + j.Price.ToString();

                jobratelist.Add(jobrate);
            }

            return View(jobratelist);
        }


        //Private Functions

        private ViewJob FillViewJobModel(int id)
        {
            //GetJobInfo
            var s = ContractorShareService.GetServiceRequest(id.ToString());

            ViewJob detailsjob = new ViewJob();
            detailsjob.JobId = s.Id;
            detailsjob.JobName = s.Name;
            detailsjob.Description = s.Description;
            detailsjob.StatusId = s.StatusID;
            ServiceStatusEnum servicestatusenum = (ServiceStatusEnum)s.StatusID;
            detailsjob.StatusName = servicestatusenum.ToString();
            detailsjob.Address = s.Address;
            detailsjob.PostalCode = s.PostalCode;
            detailsjob.City = s.City;
            detailsjob.Country = s.Country;
            detailsjob.ClientId = s.ClientID;
            detailsjob.CategoryId = s.CategoryID;
            ServiceCategoryEnum servicecategory = (ServiceCategoryEnum)s.CategoryID;
            detailsjob.CategoryName = EnumHelper.GetDescription(servicecategory);
            detailsjob.Posted = s.PostedDate;
            detailsjob.ContractorId = s.ContractorID;

            //Get Comments
            List<Comment> jobcomments = GetJobComments(id);
            detailsjob.Comments = jobcomments;

            //Get Tasks
            List<Task> jobtasks = GetTasks(id);
            detailsjob.Tasks = jobtasks;

            return detailsjob;
        }

        private List<Comment> GetJobComments(int? id)
        {
            var comments = ContractorShareService.GetJobComments(id.ToString());

            List<Comment> jobcomments = new List<Comment>();

            foreach (var m in comments)
            {
                Comment jobcomment = new Comment();

                var user = ContractorShareService.GetUserProfile(m.CreatedByUserId.ToString());

                jobcomment.CommentId = m.CommentId;
                jobcomment.Title = m.Title;
                jobcomment.comment = m.Message;
                jobcomment.CreatedByUserId = m.CreatedByUserId;
                jobcomment.CreatedByUserName = string.IsNullOrEmpty(user.CompanyName) ? string.Concat(user.Firstname, " ", user.Surname) : user.CompanyName;
                jobcomment.Created = m.Created;
                jobcomment.JobId = m.JobId;

                jobcomments.Add(jobcomment);
            }

            return jobcomments;
        }

        private List<Task> GetTasks(int jobId)
        {
            var tasks = ContractorShareService.GetJobTasks(jobId.ToString());

            List<Task> jobtasks = new List<Task>();

            foreach (var t in tasks)
            {
                Task jobtask = new Task();

                jobtask.TaskId = t.TaskId;
                jobtask.Name = t.Name;
                jobtask.Description = t.Description;
                jobtask.StatusId = t.StatusId;

                TaskStatusEnum taskstatusenum = (TaskStatusEnum)t.StatusId;
                jobtask.StatusName = taskstatusenum.ToString();

                jobtask.JobId = t.ServiceId;
                jobtask.CreatedOn = t.Created;

                jobtasks.Add(jobtask);
            }

            return jobtasks;
        }
    }
}