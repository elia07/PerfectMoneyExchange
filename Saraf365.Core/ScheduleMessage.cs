//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Saraf365.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScheduleMessage
    {
        public long xID { get; set; }
        public Nullable<long> xSystemFileID { get; set; }
        public string xTitle { get; set; }
        public string xBody { get; set; }
        public string xExpression { get; set; }
        public bool xIsActive { get; set; }
    
        public virtual SystemFile SystemFile { get; set; }
    }
}
