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

using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;

namespace MvcContractorShareApplication.Controllers
{
    public class CalendarController : Controller
    {
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();
        ContractorShareServiceReference.EventInfo eventinfo = new ContractorShareServiceReference.EventInfo();
        static int userId;

        public ActionResult Calendar(int id)
        {
            //Being initialized in that way, scheduler will use CalendarController.Data as a the datasource and CalendarController.Save to process changes
            var scheduler = new DHXScheduler(this);

            /*
             * It's possible to use different actions of the current controller
             *      var scheduler = new DHXScheduler(this);     
             *      scheduler.DataAction = "ActionName1";
             *      scheduler.SaveAction = "ActionName2";
             * 
             * Or to specify full paths
             *      var scheduler = new DHXScheduler();
             *      scheduler.DataAction = Url.Action("Data", "Calendar");
             *      scheduler.SaveAction = Url.Action("Save", "Calendar");
             */

            /*
             * The default codebase folder is ~/Scripts/dhtmlxScheduler. It can be overriden:
             *      scheduler.Codebase = Url.Content("~/customCodebaseFolder");
             */

            userId = id;

            scheduler.InitialDate = DateTime.Now;
            scheduler.Config.first_hour = 8;
            scheduler.Config.last_hour = 19;

            if (userId != (int)Session["userId"])
            {
                scheduler.Config.isReadonly = true;
            }



            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            List<CalendarEvent> calendarevents = new List<CalendarEvent>();

            var eventsinfolist = ContractorShareService.GetUserEvents(userId.ToString());

            foreach (var e in eventsinfolist)
            {
                CalendarEvent calendarevent = new CalendarEvent();

                calendarevent.id = e.EventId;
                calendarevent.text = e.Name;
                calendarevent.start_date = e.Start_Date;
                calendarevent.end_date = e.End_Date;

                calendarevents.Add(calendarevent);
            }

            var data = new SchedulerAjaxData(calendarevents);

            return (ContentResult)data;

                    /*new List<CalendarEvent>{ 
                        new CalendarEvent{
                            id = 1, 
                            text = "Sample Event", 
                            start_date = new DateTime(2016, 07, 03, 6, 00, 00), 
                            end_date = new DateTime(2016, 07, 03, 8, 00, 00)
                        },
                        new CalendarEvent{
                            id = 2, 
                            text = "New Event", 
                            start_date = new DateTime(2016, 07, 05, 9, 00, 00), 
                            end_date = new DateTime(2016, 07, 05, 12, 00, 00)
                        },
                        new CalendarEvent{
                            id = 3, 
                            text = "Multiday Event", 
                            start_date = new DateTime(2016, 07, 03, 10, 00, 00), 
                            end_date = new DateTime(2016, 07, 10, 12, 00, 00)
                        }
                    }*/
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = (CalendarEvent)DHXEventsHelper.Bind(typeof(CalendarEvent), actionValues);

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        //add event
                        eventinfo.UserId = (int)Session["userId"];
                        eventinfo.Name = changedEvent.text;
                        eventinfo.Start_Date = changedEvent.start_date;
                        eventinfo.End_Date = changedEvent.end_date;

                        var result = ContractorShareService.CreateEvent(Session["userId"].ToString(), eventinfo);
                        break;
                        
                        //do insert
                        //action.TargetId = changedEvent.id;//assign postoperational id
                    case DataActionTypes.Delete:
                        //delete event
                        result = ContractorShareService.DeleteEvent(Session["userId"].ToString(), changedEvent.id.ToString());

                        break;
                    default:// "update"                          
                        //edit event
                        eventinfo.UserId = (int)Session["userId"];
                        eventinfo.Name = changedEvent.text;
                        eventinfo.Start_Date = changedEvent.start_date;
                        eventinfo.End_Date = changedEvent.end_date;
                        eventinfo.EventId = changedEvent.id;

                        result = ContractorShareService.EditEvent(Session["userId"].ToString(), changedEvent.id.ToString(), eventinfo);

                        break;
                }
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }


    }
}