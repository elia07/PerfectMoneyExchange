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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.FinancialLog = new HashSet<FinancialLog>();
            this.Ticket = new HashSet<Ticket>();
            this.Transaction = new HashSet<Transaction>();
            this.UserBankAccount = new HashSet<UserBankAccount>();
            this.Users = new HashSet<User>();
            this.Withdrawal = new HashSet<Withdrawal>();
        }
    
        public long xID { get; set; }
        public Nullable<long> xNationalIDImage { get; set; }
        public string xNationalIDNumber { get; set; }
        public string xFullName { get; set; }
        public string xEmail { get; set; }
        public string xPassword { get; set; }
        public bool xIsActive { get; set; }
        public string xDescription { get; set; }
        public bool xIsEmailValidated { get; set; }
        public bool xIsNationalIDValidated { get; set; }
        public double xLevel { get; set; }
        public System.DateTime xSignupDate { get; set; }
        public Nullable<System.DateTime> xLastSigninDate { get; set; }
        public bool xIsVip { get; set; }
        public bool xCellphoneActivated { get; set; }
        public string xActivationCode { get; set; }
        public string xCellphone { get; set; }
        public Nullable<System.DateTime> xIPGRestriction { get; set; }
        public string xSalt { get; set; }
        public Nullable<System.DateTime> xActivationCodeExpireAt { get; set; }
        public string xCellphoneActivationCode { get; set; }
        public Nullable<System.DateTime> xLastSendEmailDate { get; set; }
        public Nullable<System.DateTime> xLastSendSmsDate { get; set; }
        public long xWallet { get; set; }
        public Nullable<long> xInviteID { get; set; }
        public Nullable<long> xAgentID { get; set; }
        public long xRefferalSharePercent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FinancialLog> FinancialLog { get; set; }
        public virtual SystemFile SystemFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Ticket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transaction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBankAccount> UserBankAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        public virtual User Agent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Withdrawal> Withdrawal { get; set; }
    }
}