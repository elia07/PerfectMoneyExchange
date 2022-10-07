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
    public class TicketRepository : GenericRepository<Ticket>
    {
        public TicketRepository() { }
        public TicketRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<Ticket, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<Ticket, bool>>> predicates = new List<Expression<Func<Ticket, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)

                {
                    switch (item.MentionField.Name)
                    {
                        case "xUserID":
                            Expression<Func<Ticket, bool>> temp = null;
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
                        case "xTitle":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xTitle == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xTitle != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xTitle.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xTitle == "" || t.xTitle == null;
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
            Expression<Func<Ticket, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(Ticket), "Ticket");
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



        public IQueryable<Ticket> GetUserTickets(long userID)
        {
            return (from t in db.Ticket where t.xUserID==userID orderby t.xID descending select t);
        }

        public int GetOpenTicketsCount()
        {
            return (from t in db.Ticket where t.xStatus==(byte)TicketStatusType.WaitingForResponse select t).Count();
        }

    }
}
