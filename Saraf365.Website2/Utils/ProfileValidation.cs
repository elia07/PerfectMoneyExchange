using Saraf365.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saraf365.Website2.Utils
{
    public static class ProfileValidation
    {
        public static int Validate(User instance)
        {
            int MyProfileNotifs = 0;
            if ((SectionInfo.Setting.IDCartVerification && instance.xNationalIDImage == null)) {
                if(!instance.xIsNationalIDValidated)
                {
                    MyProfileNotifs++;
                }
                
            }
            if (SectionInfo.Setting.EmailActivation && instance.xIsEmailValidated == false) { MyProfileNotifs++; }
            if (SectionInfo.Setting.CellphoneActivation && instance.xCellphoneActivated == false) { MyProfileNotifs++; }
            if (instance.UserBankAccount.Where(x=>x.xIsVerified).Count() < SectionInfo.Setting.UserBankAccountCountForVerification) { MyProfileNotifs++; }

            return MyProfileNotifs;
        }
    }
}