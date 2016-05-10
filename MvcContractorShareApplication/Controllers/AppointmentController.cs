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

namespace MvcContractorShareApplication.Controllers
{
    public class AppointmentController : Controller
    {
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();

        public ActionResult MyActiveAppointments()
        {
            IEnumerable<MvcContractorShareApplication.Models.ViewAppointmentModel> appointments = new List<ViewAppointmentModel>();

            var appointmentsinfolist = ContractorShareService.GetActiveAppointments(Session["userId"].ToString());

            List<ViewAppointmentModel> appointmentslist = new List<ViewAppointmentModel>();

            foreach (var a in appointmentsinfolist)
            {
                ViewAppointmentModel appointment = new ViewAppointmentModel();

                var jobinfo = ContractorShareService.GetServiceRequest(a.JobId.ToString());

                var clientinfo = ContractorShareService.GetUserProfile(a.ClientId.ToString());

                var contractorinfo = ContractorShareService.GetUserProfile(a.ContractorId.ToString());

                appointment.AppointmentId = a.AppointmentId;
                appointment.ProposalId = a.ProposalId;
                appointment.JobId = a.JobId;
                appointment.JobName = jobinfo.Name;
                appointment.ClientId = a.ClientId;
                appointment.ClientName = string.Concat(clientinfo.Firstname, " ", clientinfo.Surname);
                appointment.ContractorId = a.ContractorId;
                appointment.ContractorName = contractorinfo.CompanyName;
                appointment.StatusId = a.StatusId;
                appointment.StatusName = ((AppointmentStatusEnum)a.StatusId).ToString();
                appointment.Address = jobinfo.Address;
                appointment.City = jobinfo.City;
                appointment.MeetingTime = a.MeetingTime;
                appointment.AproxDuration = string.Concat(a.AproxDuration.ToString(), " mins");

                appointmentslist.Add(appointment);
            }

            appointments = (IEnumerable<ViewAppointmentModel>)appointmentslist;
            return View(appointments);
        }

        public ActionResult MyClosedAppointments()
        {
            IEnumerable<MvcContractorShareApplication.Models.ViewAppointmentModel> appointments = new List<ViewAppointmentModel>();

            var appointmentsinfolist = ContractorShareService.GetClosedAppointments(Session["userId"].ToString());

            List<ViewAppointmentModel> appointmentslist = new List<ViewAppointmentModel>();

            foreach (var a in appointmentsinfolist)
            {
                ViewAppointmentModel appointment = new ViewAppointmentModel();

                var jobinfo = ContractorShareService.GetServiceRequest(a.JobId.ToString());

                var clientinfo = ContractorShareService.GetUserProfile(a.ClientId.ToString());

                var contractorinfo = ContractorShareService.GetUserProfile(a.ContractorId.ToString());

                appointment.AppointmentId = a.AppointmentId;
                appointment.ProposalId = a.ProposalId;
                appointment.JobId = a.JobId;
                appointment.JobName = jobinfo.Name;
                appointment.ClientId = a.ClientId;
                appointment.ClientName = string.Concat(clientinfo.Firstname, " ", clientinfo.Surname);
                appointment.ContractorId = a.ContractorId;
                appointment.ContractorName = contractorinfo.CompanyName;
                appointment.StatusId = a.StatusId;
                appointment.StatusName = ((AppointmentStatusEnum)a.StatusId).ToString();
                appointment.Address = jobinfo.Address;
                appointment.City = jobinfo.City;
                appointment.MeetingTime = a.MeetingTime;
                appointment.AproxDuration = string.Concat(a.AproxDuration.ToString(), " mins");

                appointmentslist.Add(appointment);
            }

            appointments = (IEnumerable<ViewAppointmentModel>)appointmentslist;
            return View(appointments);
        }


        public ActionResult ViewAppointment(int id)
        {
            var a = ContractorShareService.GetAppointment(id.ToString());

            ViewAppointmentModel appointment = new ViewAppointmentModel();

            var jobinfo = ContractorShareService.GetServiceRequest(a.JobId.ToString());

            var clientinfo = ContractorShareService.GetUserProfile(a.ClientId.ToString());

            var contractorinfo = ContractorShareService.GetUserProfile(a.ContractorId.ToString());

            appointment.AppointmentId = a.AppointmentId;
            appointment.ProposalId = a.ProposalId;
            appointment.JobId = a.JobId;
            appointment.JobName = jobinfo.Name;
            appointment.ClientId = a.ClientId;
            appointment.ClientName = string.Concat(clientinfo.Firstname, " ", clientinfo.Surname);
            appointment.ContractorId = a.ContractorId;
            appointment.ContractorName = contractorinfo.CompanyName;
            appointment.StatusId = a.StatusId;
            appointment.StatusName = ((AppointmentStatusEnum)a.StatusId).ToString();
            appointment.Address = jobinfo.Address;
            appointment.City = jobinfo.City;
            appointment.MeetingTime = a.MeetingTime;
            appointment.AproxDuration = string.Concat(a.AproxDuration.ToString(), " mins");

            return View(appointment);
        }

        public ActionResult CancelAppointment(int id)
        {
            CancelAppointmentModel appointment = new CancelAppointmentModel();

            var a = ContractorShareService.GetAppointment(id.ToString());

            var jobinfo = ContractorShareService.GetServiceRequest(a.JobId.ToString());

            var clientinfo = ContractorShareService.GetUserProfile(a.ClientId.ToString());

            var contractorinfo = ContractorShareService.GetUserProfile(a.ContractorId.ToString());

            appointment.AppointmentId = a.AppointmentId;
            appointment.ProposalId = a.ProposalId;
            appointment.JobId = a.JobId;
            appointment.JobName = jobinfo.Name;
            appointment.ClientId = a.ClientId;
            appointment.ClientName = string.Concat(clientinfo.Firstname, " ", clientinfo.Surname);
            appointment.ContractorId = a.ContractorId;
            appointment.ContractorName = contractorinfo.CompanyName;
            appointment.StatusId = a.StatusId;
            appointment.Address = jobinfo.Address;
            appointment.City = jobinfo.City;
            appointment.MeetingTime = a.MeetingTime;

            return View(appointment);
        }

        //CancelAppointment Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAppointment(CancelAppointmentModel model)
        {
            var result = ContractorShareService.CancelAppointment(model.AppointmentId.ToString());

            if (result.message == "OK")
            {
                return RedirectToAction("MyActiveAppointments", "Appointment");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }
        }
    }

}