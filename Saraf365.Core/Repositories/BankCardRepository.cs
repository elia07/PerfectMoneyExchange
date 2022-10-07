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
    public class BankCardRepository : GenericRepository<BankCard>
    {
        public BankCardRepository() { }
        public BankCardRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<BankCard, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<BankCard, bool>>> predicates = new List<Expression<Func<BankCard, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xBankName":
                            Expression<Func<BankCard, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xBankName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xBankName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xBankName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xBankName == "" || t.xBankName == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCardHolderName":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xCardHolderName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xCardHolderName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xCardHolderName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xCardHolderName == "" || t.xCardHolderName == null;
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
                                    temp = t => t.xAccountNumber == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xAccountNumber != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xAccountNumber.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xAccountNumber == "" || t.xAccountNumber == null;
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
                        case "xType":
                            temp = null;
                            byte value = Convert.ToByte((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xType == value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xType != value;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xMinToTransfer":
                            temp = null;
                            Int64 xMinToTransferValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xMinToTransfer == xMinToTransferValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xMinToTransfer >= xMinToTransferValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xMinToTransfer > xMinToTransferValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xMinToTransfer <= xMinToTransferValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xMinToTransfer < xMinToTransferValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xMinToTransfer != xMinToTransferValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xBalance":
                            temp = null;
                            Int64 xBalanceValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xBalance == xBalanceValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xBalance >= xBalanceValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xBalance > xBalanceValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xBalance <= xBalanceValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xBalance < xBalanceValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xBalance != xBalanceValue;
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
                        default:
                            break;
                    }
                }
            }
            Expression<Func<BankCard, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(BankCard), "BankCard");
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

        public IQueryable<BankCard> GetInputCards()
        {
            return (from b in db.BankCard where b.xIsActive && b.xType==(byte)BankCardType.Input select b);
        }

        public BankCard GetAppropriteRepoCard()
        {
            return (from b in db.BankCard where b.xIsActive && b.xType==(byte)BankCardType.Repository orderby b.xLastUsageDate descending select b).FirstOrDefault();
        }


        public IQueryable<BankCard> GetNoneInputCards()
        {
            return (from b in db.BankCard where b.xType==(byte)BankCardType.Output || b.xType == (byte)BankCardType.Repository select b);
        }

        public IQueryable<BankCard> GetOutputCards()
        {
            return (from b in db.BankCard where b.xType==(byte)BankCardType.Output && b.xIsActive select b);
        }

        public BankCard GetAppropriteForCheckHolderName()
        {
            return (from b in db.BankCard where b.xIsActive && b.xType==(byte)BankCardType.Output select b).FirstOrDefault();
        }


        
    }
}
