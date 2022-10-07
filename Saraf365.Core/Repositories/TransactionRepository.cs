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
using Saraf365.Core;

namespace Saraf365.Core.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>
    {
        public TransactionRepository() { }
        public TransactionRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<Transaction, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<Transaction, bool>>> predicates = new List<Expression<Func<Transaction, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xEmail":
                            Expression<Func<Transaction, bool>> temp = null;
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
                        case "xGatewayName":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.Gateway.xName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.Gateway.xName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.Gateway.xName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.Gateway.xName == "" || t.Gateway.xName == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xPaymentInfo":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xPaymentInfo == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xPaymentInfo != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xPaymentInfo.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xPaymentInfo == "" || t.xPaymentInfo == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xBankRef":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xBankRef == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xBankRef != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xBankRef.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xBankRef == "" || t.xBankRef == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xInvoice_key":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xInvoice_key == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xInvoice_key != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xInvoice_key.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xInvoice_key == "" || t.xBankRef == null;
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
                        case "xVerified":
                            temp = null;
                            bool xVerifiedValue = Convert.ToBoolean(item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xVerified == xVerifiedValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xVerified != xVerifiedValue;
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

                        case "xCommisionPercent":
                            temp = null;
                            double xCommisionPercentValue = Convert.ToDouble(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xComissionPercent == xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xComissionPercent >= xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xComissionPercent > xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xComissionPercent <= xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xComissionPercent < xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xComissionPercent != xCommisionPercentValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xComissionAmount":
                            temp = null;
                            long xComissionAmountValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xComissionAmount == xComissionAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xComissionAmount >= xComissionAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xComissionAmount > xComissionAmountValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xComissionAmount <= xComissionAmountValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xComissionAmount < xComissionAmountValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xComissionAmount != xComissionAmountValue;
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
            Expression<Func<Transaction, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(Transaction), "Transaction");
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


        public IQueryable<Transaction> GetUserTransactions(long userID)
        {
            return (from t in db.Transaction where t.xUserID == userID orderby t.xDate descending select t);
        }

        public bool isBankRefExist(string bankRef)
        {
            return (from t in db.Transaction where t.xBankRef==bankRef select t).Any();
        }

        public Transaction getByInvoiceKey(string invoice_Key)
        {
            return (from t in db.Transaction where t.xInvoice_key==invoice_Key select t).SingleOrDefault();
        }

        public Transaction GetByBankRef(string bankRef)
        {
            return (from t in db.Transaction where t.xBankRef == bankRef select t).SingleOrDefault();
        }

        public long GetToalBuyInForUser(long userID)
        {
            if((from t in db.Transaction where t.xUserID == userID && t.xVerified select t.xAmount).Count()==0)
            {
                return 0;
            }
            return (from t in db.Transaction where t.xUserID==userID && t.xVerified select t.xAmount).Sum();
        }


        public int GetVeirfiedCountByDate(DateTime startDate, DateTime endDate)
        {
            return (from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified select c.xID).Count();
        }

        public long GetVeirfiedAmountByDate(DateTime startDate, DateTime endDate)
        {
            if((from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified select c.xAmount).Count()==0)
            {
                return 0;
            }
            return (from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified select c.xAmount).Sum();
        }

        public long GetVeirfiedAmountByDateAndGatewayID(DateTime startDate, DateTime endDate,long gatewayID)
        {
            if ((from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified && c.xGatewayID== gatewayID select c.xAmount).Count() == 0)
            {
                return 0;
            }
            return (from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified && c.xGatewayID == gatewayID select c.xAmount).Sum();
        }

        public IQueryable<Transaction> GetByDate(DateTime startDate, DateTime endDate)
        {
            return (from c in db.Transaction where c.xDate >= startDate && c.xDate < endDate && c.xVerified select c);
        }


        public long GetAllVerifiedAmount()
        {
            if((from t in db.Transaction where t.xVerified select t.xAmount).Count()==0)
            {
                return 0;
            }
            return (from t in db.Transaction where t.xVerified select t.xAmount).Sum();
        }
        public double GetAllVerifiedCommisionAmount()
        {
            if ((from t in db.Transaction where t.xVerified select t.xComissionAmount).Count() == 0)
            {
                return 0;
            }
            return (from t in db.Transaction where t.xVerified select t.xComissionAmount).Sum();
        }
    }
}
