using Saraf365.Core.Enumerations;
using Saraf365.Core.RepositoryDefinition;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.HelperClasses;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Repositories
{
    public class WithdrawalRepository : GenericRepository<Withdrawal>
    {
        public WithdrawalRepository() { }
        public WithdrawalRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<Withdrawal, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<Withdrawal, bool>>> predicates = new List<Expression<Func<Withdrawal, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xUserID":
                            Expression<Func<Withdrawal, bool>> temp = null;
                            Int64 xUserIDValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xUserID == xUserIDValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xUserID >= xUserIDValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xUserID > xUserIDValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xUserID <= xUserIDValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xUserID < xUserIDValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xUserID != xUserIDValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xEmail":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.User.xEmail == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.User.xEmail != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.User.xEmail.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.User.xEmail == "" || t.User.xEmail == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCartNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.UserBankAccount.xCartNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.UserBankAccount.xCartNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.UserBankAccount.xCartNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.UserBankAccount.xCartNumber == "" || t.UserBankAccount.xCartNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAccountNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.UserBankAccount.xAccountNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.UserBankAccount.xAccountNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.UserBankAccount.xAccountNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.UserBankAccount.xAccountNumber == "" || t.UserBankAccount.xAccountNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xShebaNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.UserBankAccount.xShebaNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.UserBankAccount.xShebaNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.UserBankAccount.xShebaNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.UserBankAccount.xShebaNumber == "" || t.UserBankAccount.xShebaNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xDate":
                            temp = null;
                            DateTime xDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xDate >= xDateValue && t.xDate <= xDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xDate >= xDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xDate > xDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xDate <= xDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xDate < xDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xDate < xDateValue || t.xDate > xDateValueRange;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xSettlementDate":
                            temp = null;
                            DateTime xSettlementDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xSettlementDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xSettlementDate >= xSettlementDateValue && t.xSettlementDate <= xSettlementDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xSettlementDate >= xSettlementDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xSettlementDate > xSettlementDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xSettlementDate <= xSettlementDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xSettlementDate < xSettlementDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xSettlementDate < xSettlementDateValue || t.xSettlementDate > xSettlementDateValueRange;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAmount":
                            temp = null;
                            Int64 xAmountValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xAmount == xAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xAmount >= xAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xAmount > xAmountValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xAmount <= xAmountValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xAmount < xAmountValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xAmount != xAmountValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xStatus":
                            temp = null;
                            byte xStatusValue = Convert.ToByte((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xStatus == xStatusValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xStatus != xStatusValue;
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
            Expression<Func<Withdrawal, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(Withdrawal), "Withdrawal");
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

        public IQueryable<Withdrawal> GetUserWithdrawals(long userID)
        {
            return (from w in db. Withdrawal where w.xUserID==userID orderby w.xID descending select w);
        }

        public void InsertWithdrawalWithBankType(long userID,long userBankAccountID,int amount)
        {
            Withdrawal instance = new Withdrawal();
            instance.xAmount = amount;
            instance.xDate = DateTime.Now;
            instance.xStatus = (byte)WithdrawalStatusType.WaitingForAccept;
            instance.xUserBankAccountID = userBankAccountID;
            instance.xUserID = userID;
            instance.xWithdrawalType = (byte)WithdrawalType.Cash;

            Insert(instance);
            Save();
        }

        public void InsertPerfectWithdrawal(long userID, int amount,double currenyAmount)
        {
            Withdrawal instance = new Withdrawal();
            instance.xAmount = amount;
            instance.xSecondCurrencyAmmount = currenyAmount;
            instance.xDate = DateTime.Now;
            instance.xStatus = (byte)WithdrawalStatusType.WaitingForAccept;
            instance.xUserID = userID;
            instance.xWithdrawalType = (byte)WithdrawalType.PerfectMoney;

            Insert(instance);
            Save();
        }

        public long GetToalCashoutForUser(long userID)
        {
            if((from t in db.Withdrawal where t.xUserID == userID && t.xStatus == (byte)WithdrawalStatusType.Paied select t.xAmount).Count()==0)
            {
                return 0;
            }
            return (from t in db.Withdrawal where t.xUserID == userID && t.xStatus==(byte)WithdrawalStatusType.Paied select t.xAmount).Sum();
        }

        public int GetTotalWaitings()
        {
            return (from w in db.Withdrawal where w.xStatus==(byte)WithdrawalStatusType.WaitingForAccept select w).Count();
        }

        public int GetCountByDate(DateTime startDate, DateTime endDate)
        {
            return (from c in db.Withdrawal where c.xDate >= startDate && c.xDate < endDate select c.xID).Count();
        }

        public long GetAllNotRejectedValue()
        {
            if((from w in db.Withdrawal where w.xStatus != (byte)WithdrawalStatusType.Rejected select w.xAmount).Count()==0)
            {
                return 0;
            }
            return (from w in db.Withdrawal where w.xStatus!=(byte)WithdrawalStatusType.Rejected select w.xAmount).Sum();
        }

        public Withdrawal GetByIDAndLoadUserAndBankAccount(long xID)
        {
            return (from w in db.Withdrawal.Include("User").Include("UserBankAccount") where w.xID==xID select w).SingleOrDefault();
        }
    }
}
