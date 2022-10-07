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
    public class CartTransferHistoryRepository : GenericRepository<CartTransferHistory>
    {
        public CartTransferHistoryRepository() { }
        public CartTransferHistoryRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<CartTransferHistory, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<CartTransferHistory, bool>>> predicates = new List<Expression<Func<CartTransferHistory, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xCodePeigiri":
                            Expression<Func<CartTransferHistory, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCodePeigiri == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCodePeigiri != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xCodePeigiri.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xCodePeigiri == "" || t.xCodePeigiri == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCodeErja":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCodeErja == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCodeErja != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xCodeErja.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xCodeErja == "" || t.xCodeErja == null;
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
                                    temp = t => t.Transaction.User.xEmail == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.Transaction.User.xEmail != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.Transaction.User.xEmail.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.Transaction.User.xEmail == "" || t.Transaction.User.xEmail == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xDocumentNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xDocumentNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xDocumentNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xDocumentNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xDocumentNumber == "" || t.xDocumentNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCardNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCardNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCardNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xCardNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xCardNumber == "" || t.xCardNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xBankCardNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.BankCard.xCardNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.BankCard.xCardNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.BankCard.xCardNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.BankCard.xCardNumber == "" || t.BankCard.xCardNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xBankAccountNumber":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.BankCard.xAccountNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.BankCard.xAccountNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.BankCard.xAccountNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.BankCard.xAccountNumber == "" || t.BankCard.xAccountNumber == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xTransactionID":
                            temp = null;
                            Int64 xTransactionIDValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xTransactionID == xTransactionIDValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xTransactionID >= xTransactionIDValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xTransactionID > xTransactionIDValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xTransactionID <= xTransactionIDValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xTransactionID < xTransactionIDValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xTransactionID != xTransactionIDValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAmountIn":
                            temp = null;
                            Int64 xAmountValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xAmountIn == xAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xAmountIn >= xAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xAmountIn > xAmountValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xAmountIn <= xAmountValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xAmountIn < xAmountValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xAmountIn != xAmountValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAmountOut":
                            temp = null;
                            Int64 xAmountOutValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xAmountOut == xAmountOutValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xAmountOut >= xAmountOutValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xAmountOut > xAmountOutValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xAmountOut <= xAmountOutValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xAmountOut < xAmountOutValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xAmountOut != xAmountOutValue;
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
                        case "xDocumentDate":
                            temp = null;
                            DateTime xDocumentDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xDocumentDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xDocumentDate >= xDocumentDateValue && t.xDocumentDate <= xDocumentDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xDocumentDate >= xDocumentDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xDocumentDate > xDocumentDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xDocumentDate <= xDocumentDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xDocumentDate < xDocumentDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xDocumentDate < xDocumentDateValue || t.xDocumentDate > xDocumentDateValueRange;
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
            Expression<Func<CartTransferHistory, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(CartTransferHistory), "CartTransferHistory");
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

        public CartTransferHistory GetByCodePeigiri(string codePeigiri,long amount,DateTime date)
        {
            DateTime fromDate = date.AddDays(-1);
            DateTime toDate = date;
            return (from c in db.CartTransferHistory where c.xCodePeigiri == codePeigiri && c.xAmountIn== amount && c.xDate>= fromDate && c.xDate<=toDate orderby c.xDate descending select c).FirstOrDefault();
        }

        public CartTransferHistory GetByCodeErja(string codeErja,long amount, DateTime date)
        {
            DateTime fromDate = date.AddDays(-1);
            DateTime toDate = date;
            return (from c in db.CartTransferHistory where c.xCodeErja == codeErja && c.xAmountIn == amount && c.xDate >= fromDate && c.xDate <= toDate orderby c.xDate descending select c).FirstOrDefault();
        }

        public int GetUnTransactionedItemsCount()
        {
            return (from c in db.CartTransferHistory where c.xTransactionID==null && c.xAmountIn!=0 && c.xAmountOut==0 select c).Count();
        }


        public DateTime GetLastFetchDateByBankCardID(long bankCardID)
        {
            return (from c in db.CartTransferHistory where c.xBankCardID==bankCardID orderby c.xDate descending select c.xDocumentDate).FirstOrDefault();
        }

        public bool IsDocumentNumberExist(string documentID)
        {
            return (from c in db.CartTransferHistory where c.xDocumentNumber==documentID select c).Any();
        }
        public IQueryable<CartTransferHistory> GetUnProccessedByCardNumber(string cardNumber)
        {
            return (from c in db.CartTransferHistory.Include("BankCard") where c.xCardNumber==cardNumber && c.xTransactionID == null select c);
        }
        public CartTransferHistory GetUnProccessedByCardNumberAndAmount(string cardNumber,long amount)
        {
            return (from c in db.CartTransferHistory.Include("BankCard") where c.xCardNumber==cardNumber && c.xAmountIn == amount && c.xTransactionID == null  select c).FirstOrDefault();
        }
        public IQueryable<CartTransferHistory> GetNoneTransactedByCardNumber(string cardNumber)
        {
            return (from c in db.CartTransferHistory where c.xTransactionID == null && c.xCardNumber == cardNumber select c);
                 
        }
    }
}
