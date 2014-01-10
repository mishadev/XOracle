using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XOracle.Web.Front.Models
{
    public class EventBindingModels
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string ImageUri { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Event Relation Type")]
        public string EventRelationType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Participants Accounts")]
        public string ParticipantsAccounts { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Judging Accounts")]
        public string JudgingAccounts { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Expected Event Condition")]
        public string ExpectedEventCondition { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Close Date")]
        public DateTime CloseDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Currency Type")]
        public string CurrencyType { get; set; }

        [Required]
        [Display(Name = "Start Rate")]
        public double StartRate { get; set; }

        [Required]
        [Display(Name = "End Rate")]
        public double EndRate { get; set; }

        [Required]
        [Display(Name = "Locus Rage")]
        public double LocusRage { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Algorithm Type")]
        public string AlgorithmType { get; set; }
    }
}