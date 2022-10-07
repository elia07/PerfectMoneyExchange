using Saraf365.Core.Enumerations;
using Saraf365.Core.RepositoryDefinition;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.HelperClasses;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() { }
        public UserRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<User, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<User, bool>>> predicates = new List<Expression<Func<User, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xEmail":
                            Expression<Func<User, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xEmail == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xEmail != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xEmail.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xEmail == "" || t.xEmail == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xNationalIDNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xNationalIDNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xNationalIDNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xNationalIDNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xNationalIDNumber == "" || t.xNationalIDNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xFullName":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xFullName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xFullName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xFullName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xFullName == "" || t.xFullName == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        
                        case "xCellphone":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCellphone == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCellphone != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xCellphone.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xCellphone == "" || t.xCellphone == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xHasNotVerifiedBankAccount":
                            temp = null;
                            bool xHasNotVerifiedBankAccountValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.UserBankAccount.Where(x=>x.xIsVerified).Count() == t.UserBankAccount.Count();
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.UserBankAccount.Where(x => x.xIsVerified).Count() != t.UserBankAccount.Count();
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.UserBankAccount.Count() != 0;
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.UserBankAccount.Count() == 0;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xIsActive":
                            temp = null;
                            bool xIsDisableValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsActive == xIsDisableValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsActive != xIsDisableValue;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xIsEmailValidated":
                            temp = null;
                            bool xIsEmailValidatedValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsEmailValidated == xIsEmailValidatedValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsEmailValidated != xIsEmailValidatedValue;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xIsNationalIDValidated":
                            temp = null;
                            bool xIsNationalIDValidatedValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsNationalIDValidated == xIsNationalIDValidatedValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsNationalIDValidated != xIsNationalIDValidatedValue;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;


                        case "xIsVip":
                            temp = null;
                            bool xIsVipValue = Convert.ToBoolean(item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsVip == xIsVipValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsVip != xIsVipValue;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCellphoneActivated":
                            temp = null;
                            bool xCellphoneActivatedValue = Convert.ToBoolean(item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCellphoneActivated == xCellphoneActivatedValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCellphoneActivated != xCellphoneActivatedValue;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xLevel":
                            temp = null;
                            Int64 xLevelValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xLevel == xLevelValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xLevel >= xLevelValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xLevel > xLevelValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xLevel <= xLevelValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xLevel < xLevelValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xLevel != xLevelValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;



                       
                        case "xIPGRestriction":
                            temp = null;
                            DateTime xIPGRestrictionValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xIPGRestrictionValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xIPGRestriction >= xIPGRestrictionValue && t.xIPGRestriction <= xIPGRestrictionValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xIPGRestriction >= xIPGRestrictionValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xIPGRestriction > xIPGRestrictionValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xIPGRestriction <= xIPGRestrictionValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xIPGRestriction < xIPGRestrictionValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xIPGRestriction < xIPGRestrictionValue || t.xIPGRestriction > xIPGRestrictionValueRange;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        
                        case "xSignupDate":
                            temp = null;
                            DateTime xSignupDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xSignupDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xSignupDate >= xSignupDateValue && t.xSignupDate <= xSignupDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xSignupDate >= xSignupDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xSignupDate > xSignupDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xSignupDate <= xSignupDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xSignupDate < xSignupDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xSignupDate < xSignupDateValue || t.xSignupDate > xSignupDateValueRange;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xLastSigninDate":
                            temp = null;
                            DateTime xLastSigninDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xLastSigninDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xLastSigninDate >= xLastSigninDateValue && t.xSignupDate <= xLastSigninDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xLastSigninDate >= xLastSigninDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xLastSigninDate > xLastSigninDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xLastSigninDate <= xLastSigninDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xLastSigninDate < xLastSigninDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xLastSigninDate < xLastSigninDateValue || t.xLastSigninDate > xLastSigninDateValueRange;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        default:
                            break;
                    }
                }
            }
            Expression<Func<User, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(User), "User");
            foreach (var item in predicates)
            {
                if (res == null)
                {
                    res = item;
                }
                else
                {

                    res = res.And(item);

                }

            }

            return res;
            //http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
        }

        public User Authentication(User instance)
        {
            return (from u in db.User where u.xEmail.ToLower() == instance.xEmail.ToLower() && u.xPassword == instance.xPassword select u).SingleOrDefault();
        }
        public User Authentication(string xEmail, string xPassword)
        {
            return (from u in db.User where u.xEmail.ToLower() == xEmail.ToLower() && u.xPassword == xPassword select u).SingleOrDefault();
        }


        public User GetbyEmail(string email)
        {
            return (from u in db.User where u.xEmail==email select u).SingleOrDefault();
        }

        public User GetbyUsername(string username)
        {
            return (from u in db.User where u.xEmail == username select u).SingleOrDefault();
        }


        public User GetbyCellphone(string cellphone)
        {
            return (from u in db.User where u.xCellphone == cellphone select u).SingleOrDefault();
        }





      

       
        public int InActiveUserCount()
        {
            return (from u in db.User where u.xIsActive==false select u).Count();
        }


        public int GetSignUpCountByDate(DateTime startDate, DateTime endDate)
        {
            return (from c in db.User where c.xSignupDate >= startDate && c.xSignupDate < endDate select c.xID).Count();
        }

        public int GetUserCount(bool isBot=false)
        {
            return (from u in db.User select u.xID).Count();
        }



        public IQueryable<User> GetBySigninDate(DateTime date)
        {
            return (from u in db.User where u.xLastSigninDate>=date select u);
        }

        public User GetByIDAndLoadNationaIDCard(long userID)
        {
            return (from u in db.User.Include("SystemFile") where u.xID==userID select u).SingleOrDefault();
        }
        public User GetByID(long userID)
        {
            return (from u in db.User where u.xID == userID select u).SingleOrDefault();
        }
        public User GetByIDAndLoadWithdrawal(long userID)
        {
            return (from u in db.User.Include("Withdrawal") where u.xID == userID select u).SingleOrDefault();
        }
        public User GetByIDAndLoadWithdrawalAndBankAccountAndSystemFile(long userID)
        {
            return (from u in db.User.Include("Withdrawal").Include("UserBankAccount").Include("SystemFile") where u.xID == userID select u).SingleOrDefault();
        }
        public User GetByIDAndLoadUserBankAccount(long userID)
        {
            return (from u in db.User.Include("UserBankAccount") where u.xID == userID select u).SingleOrDefault();
        }

        public User GetbyInviteCode(long inviteCode)
        {
            return (from u in db.User where u.xInviteID==inviteCode select u).FirstOrDefault();
        }

        public IQueryable<User> GetAgentUSers(long agentID)
        {
            return (from u in db.User where u.xAgentID==agentID select u);

        }
    }
}
