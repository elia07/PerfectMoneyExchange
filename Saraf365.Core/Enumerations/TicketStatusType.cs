using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum TicketStatusType
    {
        [Display(Name = "Txt_WaitingForResponse")]
        WaitingForResponse,
        [Display(Name = "Txt_Responded")]
        Responded

    }
}
