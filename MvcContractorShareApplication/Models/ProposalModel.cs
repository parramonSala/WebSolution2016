using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcContractorShareApplication.Models
{
    public class Proposal
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int StatusId { get; set; }

        public bool Active { get; set; }
        public string Message { get; set; }
        [DataType(DataType.Date)]
        public DateTime ProposedTime { get; set; }
        public decimal AproxDuration { get; set; }
        public decimal ProposedPrice { get; set; }
        public DateTime? Created { get; set; }
    }

    public class Message
    {
        public int messageId { get; set; }
        public int ProposalId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string message { get; set; }
        public DateTime? Sent { get; set; }
    }

    public class CreateProposalModel
    {
        public int JobId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string JobName { get; set; }

        [Required]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProposedTime { get; set; }
        public decimal AproxDuration { get; set; }
        public decimal ProposedPrice { get; set; }
    }

    public class ViewProposalModel
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public int StatusId { get; set; }        
        public string StatusName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public bool Active { get; set; }
        public DateTime? ProposedTime { get; set; }
        public string AproxDuration { get; set; }
        public string ProposedPrice { get; set; }
        public DateTime? Created { get; set; }
        public int? UpdatedByUserId { get; set; }
        public string ScreenMessage { get; set; }

        //Messages

        //for the new message
        public string Message { get; set; }
        public int MessageFromUserId { get; set; }
        public int MessageToUserId { get; set; }

        //old messages
        public List<Message> Messages { get; set; }

        public Message OriginalMessage { get; set; }

    }

    public class EditProposalModel
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string StatusName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string Message { get; set; }
        public DateTime? ProposedTime { get; set; }
        public decimal? AproxDuration { get; set; }
        public decimal? ProposedPrice { get; set; }
    }

    public class CancelProposalModel
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime? Created { get; set; }
    }

    public class AcceptProposalModel
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string Message { get; set; }
        public DateTime? ProposedTime { get; set; }
        public string AproxDuration { get; set; }
        public string ProposedPrice { get; set; }
        public DateTime? Created { get; set; }
        public int? UpdatedByUserId { get; set; }
    }

    public class ReplyProposalModel
    {
        public int ProposalId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string OrginialMessage { get; set; }

        //for the new message
        public string Message { get; set; }
        public int MessageFromUserId { get; set; }
        public int MessageToUserId { get; set; }
        public string MessageToUserName { get; set; }

        //old messages
        public List<Message> Messages { get; set; }
    }

}
