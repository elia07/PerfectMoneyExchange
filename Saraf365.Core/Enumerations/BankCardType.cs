using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum BankCardType
    {
        [Display(Name = "مخزن")]
        Repository,
        [Display(Name = "ورودی")]
        Input,
        [Display(Name = "خروجی")]
        Output
    }
}
