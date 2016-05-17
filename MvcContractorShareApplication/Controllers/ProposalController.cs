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
    public class ProposalController : Controller
    {
        ContractorShareServiceReference.ContractorShareClient ContractorShareService = new ContractorShareServiceReference.ContractorShareClient();
        ContractorShareServiceReference.ProposalInfo proposalinfo = new ContractorShareServiceReference.ProposalInfo();
        ContractorShareServiceReference.MessageInfo messageinfo = new ContractorShareServiceReference.MessageInfo();
        ContractorShareServiceReference.UpdateProposalStatusInfo proposalstatusinfo = new ContractorShareServiceReference.UpdateProposalStatusInfo();
        
        public ActionResult CreateNewProposal(int id)
        {
            var model = new CreateProposalModel();
            
            var jobinfo = ContractorShareService.GetServiceRequest(id.ToString());

            model.JobId = id;
            model.JobName = jobinfo.Name;
            model.ToUserId = jobinfo.ClientID;
            model.ProposedTime = DateTime.Now;
            
            return View(model);
        }

        //Post New Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProposal(CreateProposalModel model)
        {
            if (ModelState.IsValid)
            {
                proposalinfo.Active = true;
                proposalinfo.JobId = model.JobId;
                proposalinfo.ToUserId = model.ToUserId;
                proposalinfo.FromUserId = (int)Session["userId"];
                proposalinfo.StatusId = (int)(int)ProposalStatusEnum.Open;
                proposalinfo.Message = model.Message;
                proposalinfo.ProposedPrice = model.ProposedPrice;
                proposalinfo.ProposedTime = (DateTime)model.ProposedTime;
                proposalinfo.AproxDuration = model.AproxDuration;

                var result = ContractorShareService.CreateProposal(proposalinfo);

                if (result.message == "OK")
                {
                    return RedirectToAction("MyProposals", "Proposal");
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


        public ActionResult MyProposals()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult LoadMyActiveProposals()
        {
            IEnumerable<MvcContractorShareApplication.Models.ViewProposalModel> proposals = new List<ViewProposalModel>();

            var proposalsinfolist = ContractorShareService.GetActiveProposals(Session["userId"].ToString(),true);

            List<ViewProposalModel> proposalslist = new List<ViewProposalModel>();

            foreach (var p in proposalsinfolist)
            {
                ViewProposalModel proposal = new ViewProposalModel();

                var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

                var fromuserinfo = ContractorShareService.GetUserProfile(p.FromUserId.ToString());

                var touserinfo = ContractorShareService.GetUserProfile(p.ToUserId.ToString());

                proposal.FromUserId = p.FromUserId;
                if (string.IsNullOrEmpty(fromuserinfo.CompanyName))
                {
                    proposal.FromUserName = string.Concat(fromuserinfo.Firstname, " ", fromuserinfo.Surname);
                }
                else
                {
                    proposal.FromUserName = fromuserinfo.CompanyName;
                }

                proposal.ToUserId = p.ToUserId;
                if (string.IsNullOrEmpty(touserinfo.CompanyName))
                {
                    proposal.ToUserName = string.Concat(touserinfo.Firstname, " ", touserinfo.Surname);
                }
                else
                {
                    proposal.ToUserName = touserinfo.CompanyName;
                }
               

                proposal.ProposalId = p.ProposalId;
                proposal.JobId = p.JobId;
                proposal.JobName = jobinfo.Name;
                proposal.Address = jobinfo.Address;
                proposal.City = jobinfo.City;

                proposal.StatusId = p.StatusId;
                ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
                proposal.StatusName = proposalstatusenum.ToString();

                proposal.Active = p.Active;
                proposal.Message = p.Message;
                proposal.ProposedTime = p.ProposedTime;
                proposal.ProposedPrice = string.Concat("£ ", p.ProposedPrice.ToString());
                proposal.AproxDuration = string.Concat(p.AproxDuration.ToString(), " hours");
                proposal.UpdatedByUserId = p.UpdatedByUserId;
                if (proposal.UpdatedByUserId != (int)Session["userId"])
                {
                    proposal.ScreenMessage = "Updated";
                }

                proposalslist.Add(proposal);
            }

            proposals = (IEnumerable<ViewProposalModel>)proposalslist;
            return PartialView("MyActiveProposals", proposals);
        }

        public ActionResult LoadMyClosedProposals()
        {
            IEnumerable<MvcContractorShareApplication.Models.ViewProposalModel> proposals = new List<ViewProposalModel>();

            var proposalsinfolist = ContractorShareService.GetMyClosedProposals(Session["userId"].ToString());

            List<ViewProposalModel> proposalslist = new List<ViewProposalModel>();

            foreach (var p in proposalsinfolist)
            {
                ViewProposalModel proposal = new ViewProposalModel();

                var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

                var fromuserinfo = ContractorShareService.GetUserProfile(p.FromUserId.ToString());

                var touserinfo = ContractorShareService.GetUserProfile(p.ToUserId.ToString());

                proposal.FromUserId = p.FromUserId;
                if (string.IsNullOrEmpty(fromuserinfo.CompanyName))
                {
                    proposal.FromUserName = string.Concat(fromuserinfo.Firstname, " ", fromuserinfo.Surname);
                }
                else
                {
                    proposal.FromUserName = fromuserinfo.CompanyName;
                }

                proposal.ToUserId = p.ToUserId;
                if (string.IsNullOrEmpty(touserinfo.CompanyName))
                {
                    proposal.ToUserName = string.Concat(touserinfo.Firstname, " ", touserinfo.Surname);
                }
                else
                {
                    proposal.ToUserName = touserinfo.CompanyName;
                }

                proposal.ProposalId = p.ProposalId;
                proposal.JobId = p.JobId;
                proposal.JobName = jobinfo.Name;
                proposal.Address = jobinfo.Address;
                proposal.City = jobinfo.City;

                proposal.StatusId = p.StatusId;
                ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
                proposal.StatusName = proposalstatusenum.ToString();

                proposal.Active = p.Active;
                proposal.Message = p.Message;
                proposal.ProposedTime = p.ProposedTime;
                proposal.ProposedPrice = string.Concat("£ ", p.ProposedPrice.ToString());
                proposal.AproxDuration = string.Concat(p.AproxDuration.ToString(), " hours");
                proposal.Created = p.Created;

                proposalslist.Add(proposal);
            }

            proposals = (IEnumerable<ViewProposalModel>)proposalslist;
            return PartialView("MyClosedProposals", proposals);
        }

        public ActionResult ViewProposal(int id, string activetab = "home")
        {
            ViewProposalModel proposal = new ViewProposalModel();
            proposal = FillViewProposalModel(id);

            ViewBag.Active = activetab;
            return View(proposal);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ViewProposal(ViewProposalModel model)
        {
            //Send Message
            messageinfo.FromUserId = (int)Session["userId"];
            messageinfo.ToUserId = model.MessageToUserId;
            messageinfo.Message = model.Message;
            messageinfo.ProposalId = model.ProposalId;
            messageinfo.Created = DateTime.Now;

            var result = ContractorShareService.SendProposalMessage(model.ProposalId.ToString(), messageinfo);

            ViewProposalModel proposal = FillViewProposalModel(model.ProposalId);

            if (result.message == "OK")
            {
                ViewBag.Active = "messages";
                return View(proposal);
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }

        }

        public ActionResult ReplyProposal(int id)
        {
            ViewProposalModel proposal = new ViewProposalModel();
            proposal = FillViewProposalModel(id);
            ViewBag.Active = "messages";
            return View("ViewProposal", proposal);
        }

        public ActionResult EditProposal(int id)
        {
            var model = new EditProposalModel();

            var p = ContractorShareService.GetProposal(id.ToString());
            
            var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());
            
            model.ProposalId = id;
            model.JobId = p.JobId;
            model.JobName = jobinfo.Name;
            model.AproxDuration = p.AproxDuration;
            model.ProposedPrice = p.ProposedPrice;
            model.ProposedTime = p.ProposedTime;
            
            return View(model);
        }

        //Edit Proposal- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditProposal(EditProposalModel model)
        {
            if (ModelState.IsValid)
            {
                var p = ContractorShareService.GetProposal(model.ProposalId.ToString());
                
                //details obtained from the view
                proposalinfo.ProposalId = model.ProposalId;
                proposalinfo.ProposedPrice = Convert.ToInt32(model.ProposedPrice);
                proposalinfo.ProposedTime = (DateTime)model.ProposedTime;
                proposalinfo.AproxDuration = Convert.ToInt32(model.AproxDuration);

                //details from the proposal
                proposalinfo.Active = p.Active;
                proposalinfo.JobId = p.JobId;
                proposalinfo.ToUserId = p.ToUserId;
                proposalinfo.FromUserId = p.FromUserId;
                proposalinfo.StatusId = p.StatusId;
                proposalinfo.Message = p.Message;
               
                var result = ContractorShareService.EditProposal(model.ProposalId.ToString(),proposalinfo);

                if (result.message == "OK")
                {
                    return RedirectToAction("ViewProposal", "Proposal", new { id = model.ProposalId });
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

        public ActionResult CancelProposal(int id)
        {
            CancelProposalModel cancelproposal = new CancelProposalModel();

            var p = ContractorShareService.GetProposal(id.ToString());

            var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

            cancelproposal.ProposalId = p.ProposalId;
            cancelproposal.JobId = p.JobId;
            cancelproposal.JobName = jobinfo.Name;
            cancelproposal.Address = jobinfo.Address;
            cancelproposal.City = jobinfo.City;

            cancelproposal.StatusId = p.StatusId;
            ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
            cancelproposal.StatusName = proposalstatusenum.ToString();

            cancelproposal.Created = p.Created;

            return View(cancelproposal);
        }

        //Cancel Job- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CancelProposal(CancelProposalModel model)
        {
            //Change Status of the Proposal to 4-Cancelled

            int statusid = (int)ProposalStatusEnum.Cancelled;

            proposalstatusinfo = new ContractorShareServiceReference.UpdateProposalStatusInfo();
            proposalstatusinfo.StatusId = statusid;
            proposalstatusinfo.UpdatedByUserId = (int)Session["userId"];

            var result = ContractorShareService.UpdateProposalStatus(model.ProposalId.ToString(),proposalstatusinfo);

            if (result.message == "OK")
            {
                return RedirectToAction("MyProposals", "Proposal");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }

        }

        public ActionResult RejectProposal(int id)
        {
            CancelProposalModel rejectproposal = new CancelProposalModel();

            var p = ContractorShareService.GetProposal(id.ToString());

            var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

            rejectproposal.ProposalId = p.ProposalId;
            rejectproposal.JobId = p.JobId;
            rejectproposal.JobName = jobinfo.Name;
            rejectproposal.Address = jobinfo.Address;
            rejectproposal.City = jobinfo.City;

            rejectproposal.StatusId = p.StatusId;
            ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
            rejectproposal.StatusName = proposalstatusenum.ToString();

            rejectproposal.Created = p.Created;

            return View(rejectproposal);
        }

        //Reject Proposal- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RejectProposal(CancelProposalModel model)
        {
            //Change Status of the Proposal to Rejected

            int statusid = (int)ProposalStatusEnum.Rejected;

            proposalstatusinfo = new ContractorShareServiceReference.UpdateProposalStatusInfo();
            proposalstatusinfo.StatusId = statusid;
            proposalstatusinfo.UpdatedByUserId = (int)Session["userId"];

            var result = ContractorShareService.UpdateProposalStatus(model.ProposalId.ToString(), proposalstatusinfo);

            if (result.message == "OK")
            {
                return RedirectToAction("MyProposals", "Proposal");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }

        }

        public ActionResult AcceptProposal(int id)
        {
            var p = ContractorShareService.GetProposal(id.ToString());

            var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

            var fromuserinfo = ContractorShareService.GetUserProfile(p.FromUserId.ToString());

            var touserinfo = ContractorShareService.GetUserProfile(p.ToUserId.ToString());

            AcceptProposalModel proposal = new AcceptProposalModel();
            proposal.ProposalId = p.ProposalId;
            proposal.JobId = p.JobId;
            proposal.JobName = jobinfo.Name;
            proposal.Address = jobinfo.Address;
            proposal.City = jobinfo.City;

            proposal.StatusId = p.StatusId;
            ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
            proposal.StatusName = proposalstatusenum.ToString();

            proposal.FromUserId = p.FromUserId;
            if (string.IsNullOrEmpty(fromuserinfo.CompanyName))
            {
                proposal.FromUserName = string.Concat(fromuserinfo.Firstname, " ", fromuserinfo.Surname);
            }
            else
            {
                proposal.FromUserName = fromuserinfo.CompanyName;
            }

            proposal.ToUserId = p.ToUserId;
            if (string.IsNullOrEmpty(touserinfo.CompanyName))
            {
                proposal.ToUserName = string.Concat(touserinfo.Firstname, " ", touserinfo.Surname);
            }
            else
            {
                proposal.ToUserName = touserinfo.CompanyName;
            }
            proposal.ProposedTime = p.ProposedTime;
            proposal.ProposedPrice = string.Concat("£ ", p.ProposedPrice.ToString());
            proposal.AproxDuration = string.Concat(p.AproxDuration.ToString(), " hours");
            proposal.Created = p.Created;

            return View(proposal);
        }

        //Accept Proposal- Post Action
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptProposal(AcceptProposalModel model)
        {
            //Change status to Accepted
            int statusid = (int)ProposalStatusEnum.Accepted;

            proposalstatusinfo = new ContractorShareServiceReference.UpdateProposalStatusInfo();
            proposalstatusinfo.StatusId = statusid;
            proposalstatusinfo.UpdatedByUserId = (int)Session["userId"];

            var result = ContractorShareService.UpdateProposalStatus(model.ProposalId.ToString(), proposalstatusinfo);

            if (result.message == "OK")
            {
                return RedirectToAction("MyProposals", "Proposal");
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }
            
        }

        public ActionResult DeleteMessage(int id, int proposalid)
        {
            //Call DeleteMessage function for Message with Id = id

            var result = ContractorShareService.DeleteMessage(proposalid.ToString(), id.ToString());
                
            if (result.message == "OK")
            {
                return RedirectToAction("ViewProposal", new
                {
                    id = proposalid,
                    activetab = "messages"
                });
            }
            else
            {
                ModelState.AddModelError("", result.message);
                return RedirectToAction("ViewProposal", new
                {
                    id = proposalid,
                    activetab = "messages"
                });
            }
        }

        //Private functions
        private ViewProposalModel FillViewProposalModel(int id)
        {
            var p = ContractorShareService.GetProposal(id.ToString());

            var jobinfo = ContractorShareService.GetServiceRequest(p.JobId.ToString());

            var fromuserinfo = ContractorShareService.GetUserProfile(p.FromUserId.ToString());

            var touserinfo = ContractorShareService.GetUserProfile(p.ToUserId.ToString());

            ViewProposalModel proposal = new ViewProposalModel();
            proposal.ProposalId = p.ProposalId;
            proposal.JobId = p.JobId;
            proposal.JobName = jobinfo.Name;
            proposal.Address = jobinfo.Address;
            proposal.City = jobinfo.City;

            proposal.StatusId = p.StatusId;
            ProposalStatusEnum proposalstatusenum = (ProposalStatusEnum)p.StatusId;
            proposal.StatusName = proposalstatusenum.ToString();

            proposal.FromUserId = p.FromUserId;
            if (string.IsNullOrEmpty(fromuserinfo.CompanyName))
            {
                proposal.FromUserName = string.Concat(fromuserinfo.Firstname, " ", fromuserinfo.Surname);
            }
            else
            {
                proposal.FromUserName = fromuserinfo.CompanyName;
            }

            proposal.ToUserId = p.ToUserId;
            if (string.IsNullOrEmpty(touserinfo.CompanyName))
            {
                proposal.ToUserName = string.Concat(touserinfo.Firstname, " ", touserinfo.Surname);
            }
            else
            {
                proposal.ToUserName = touserinfo.CompanyName;
            }
            proposal.Active = p.Active;
            proposal.ProposedTime = p.ProposedTime;
            proposal.ProposedPrice = string.Concat("£ ", p.ProposedPrice.ToString());
            proposal.AproxDuration = string.Concat(p.AproxDuration.ToString(), " hours");
            proposal.Created = p.Created;

            Message original_message = new Message();
            original_message.ProposalId = p.ProposalId;
            original_message.message = p.Message;
            original_message.FromUserId = proposal.FromUserId;
            original_message.FromUserName = proposal.FromUserName;
            original_message.ToUserId = proposal.ToUserId;
            original_message.ToUserName = proposal.ToUserName;
            original_message.Sent = proposal.Created;
            proposal.OriginalMessage = original_message;

            List<Message> proposalmessages = GetProposalMessages(id.ToString());
            proposal.Messages = proposalmessages;

            //New Message
            if (p.FromUserId == (int)Session["userId"])
            {
                proposal.MessageToUserId = p.ToUserId;
            }
            else
            {
                proposal.MessageToUserId = p.FromUserId;
            }
            proposal.Message = "Write your message here...";

            return proposal;
        }

        private List<Message> GetProposalMessages(string id)
        {
            var messages = ContractorShareService.GetProposalMessages(id.ToString());

            List<Message> proposalmessages = new List<Message>();

            foreach (var m in messages)
            {
                Message proposalmessage = new Message();

                var fromuserinfo = ContractorShareService.GetUserProfile(m.FromUserId.ToString());
                var touserinfo = ContractorShareService.GetUserProfile(m.ToUserId.ToString());

                proposalmessage.messageId = m.MessageId;
                proposalmessage.ProposalId = m.ProposalId;
                proposalmessage.FromUserId = m.FromUserId;
                proposalmessage.FromUserName = string.IsNullOrEmpty(fromuserinfo.CompanyName) ? string.Concat(fromuserinfo.Firstname, " ", fromuserinfo.Surname) : fromuserinfo.CompanyName;

                proposalmessage.ToUserId = m.ToUserId;
                proposalmessage.ToUserName = string.IsNullOrEmpty(touserinfo.CompanyName) ? string.Concat(touserinfo.Firstname, " ", touserinfo.Surname) : touserinfo.CompanyName;

                proposalmessage.message = m.Message;
                proposalmessage.Sent = m.Created;

                proposalmessages.Add(proposalmessage);
            }

            return proposalmessages;
        }



    }
}