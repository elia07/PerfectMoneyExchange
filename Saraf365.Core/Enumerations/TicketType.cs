using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum TicketType
    {
        [Display(Name = "Txt_Financial")]
        Financial,
        [Display(Name = "Txt_Technical")]
        Technical


    }
}
