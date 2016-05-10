using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;


namespace MvcContractorShareApplication.Enumerations
{
    public enum ModelEnum
    {
        Client = 1,
        Contractor = 2
    }

    public enum TaskStatusEnum
    {
        Open = 1,
        InProgress = 2,
        Closed = 3,
        Cancelled = 4
    }

    public enum ServiceStatusEnum
    {
        Open = 1,
        InProgress = 2,
        Closed = 3,
        Cancelled = 4
    }

    public enum ServiceCategoryEnum
    {
        [Description("Architectural Services")]
        Architectural_Services = 1,
        
        [Description("Bathroom Fitting")]        
        Bathroom_Fitting = 2,

        [Description("Bricklaying")]        
        Bricklaying = 3,
        
        [Description("Carpet Fitting")]        
        Carpet_Fitting = 4
    }

    public enum ProposalStatusEnum
    {
        Open = 1,
        Pending = 2,
        Cancelled = 4,
        Accepted = 5,
        Rejected = 6
    }

    public enum AppointmentStatusEnum
    {
        Open = 1,
        Cancelled = 4,
        Closed = 7
    }
}