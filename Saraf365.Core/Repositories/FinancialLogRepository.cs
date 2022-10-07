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
using Saraf365.Core.Models;

namespace Saraf365.Core.Repositories
{
    public class FinancialLogRepository : GenericRepository<FinancialLog>
    {
        public FinancialLogRepository() { }
        public FinancialLogRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<FinancialLog, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<FinancialLog, bool>>> predicates = new List<Expression<Func<FinancialLog, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xUserID":
                            Expression<Func<FinancialLog, bool>> temp = null;
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
                        case "xDescription":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xDescription == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xDescription != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xDescription.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xDescription == "" || t.xDescription == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAddBy":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xAddBy == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xAddBy != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xAddBy.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xAddBy == "" || t.xAddBy == null;
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

                        default:
                            break;
                    }
                }
            }
            Expression<Func<FinancialLog, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(FinancialLog), "FinancialLog");
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


        public IQueryable<FinancialLog> GetByUserID(long userID)
        {
            return (from ug in db.FinancialLog where ug.xUserID==userID orderby ug.xID descending select ug);
        }



        public long Log(long? userID, FinancialLogType type, string desc, string addBy, double amount)
        {
            FinancialLog instance = new FinancialLog();
            instance.xDate = DateTime.Now;
            instance.xDescription = desc;
            instance.xType = (byte)type;
            instance.xUserID = userID;
            instance.xAddBy = addBy;
            instance.xAmount = amount;
            

            Insert(instance);
            Save();
            return instance.xID;
        }

        public IQueryable<FinancialLog> GetByType(FinancialLogType type,long userID,int count=5)
        {
            return (from b in db.FinancialLog where b.xType == (byte)type && b.xUserID==userID orderby b.xID descending select b).Take(count);
        }

        


        public double GetShareHolderWithdrawalAmount(DateTime startDate, DateTime endDate)
        {
            if ((from c in db.FinancialLog where c.xDate >= startDate && c.xDate < endDate && (c.xType == (byte)FinancialLogType.ShareHolderWithdrawal) select c.xID).Count() == 0)
            {
                return 0;
            }
            return (from c in db.FinancialLog where c.xDate >= startDate && c.xDate < endDate && (c.xType == (byte)FinancialLogType.ShareHolderWithdrawal) select c.xAmount).Sum();
        }






      


        public double GetMiscCostAmount(DateTime startDate, DateTime endDate)
        {
            if ((from c in db.FinancialLog where c.xDate >= startDate && c.xDate < endDate && (c.xType == (byte)FinancialLogType.MiscCost) select c.xID).Count() == 0)
            {
                return 0;
            }
            return (from c in db.FinancialLog where c.xDate >= startDate && c.xDate < endDate && (c.xType == (byte)FinancialLogType.MiscCost) select c.xAmount).Sum();
        }


        public IQueryable<FinancialLog> GetRefferalShareRecords(long userID)
        {
            return (from f in db.FinancialLog where f.xType==(byte)FinancialLogType.RefferalShare && f.xUserID==userID select f);
        }

    }
}
