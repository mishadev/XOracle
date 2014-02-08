using System;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Azure.Web.Front
{
    public class BetBindingModel
    {
        [Required]
        [Display(Name = "Event Id")]
        public Guid EventId { get; set; }

        [Required]
        [Display(Name = "Outcomes Type")]
        public string OutcomesType { get; set; }

        [Required]
        [Display(Name = "Bet Amount")]
        public decimal BetAmount { get; set; }
    }
}