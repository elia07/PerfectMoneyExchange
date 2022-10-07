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
    public class TelegramSupportRepository : GenericRepository<TelegramSupport>
    {
        public TelegramSupportRepository() { }
        public TelegramSupportRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<TelegramSupport, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<TelegramSupport, bool>>> predicates = new List<Expression<Func<TelegramSupport, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xMessage":
                            Expression<Func<TelegramSupport, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xMessage == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xMessage != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xMessage.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xMessage == "" || t.xMessage == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xUsername":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xUsername == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xUsername != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xUsername.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xUsername == "" || t.xUsername == null;
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

                        case "xResponseDate":
                            temp = null;
                            DateTime xResponseDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xResponseDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xResponseDate >= xResponseDateValue && t.xResponseDate <= xResponseDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xResponseDate >= xResponseDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xResponseDate > xResponseDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xResponseDate <= xResponseDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xResponseDate < xResponseDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xResponseDate < xResponseDateValue || t.xResponseDate > xResponseDateValueRange;
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
            Expression<Func<TelegramSupport, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(TelegramSupport), "TelegramSupport");
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

        public IQueryable<TelegramSupport> GetLastMessages(long chatID,int count)
        {
            return (from t in db.TelegramSupport where t.xChatID==chatID orderby t.xID descending select t).Skip(1).Take(count);
        }

        public int GetOpenTicketsCount()
        {
            return (from t in db.TelegramSupport where t.xResponseAdmin==null select t).Count();
        }

        public IQueryable<TelegramSupport> GetAllBeforeDate(DateTime date)
        {
            return (from t in db.TelegramSupport where t.xDate<=date select t);
        }
    }
}
