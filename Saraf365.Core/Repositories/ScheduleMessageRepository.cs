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
    public class ScheduleMessageRepository : GenericRepository<ScheduleMessage>
    {
        public ScheduleMessageRepository() { }
        public ScheduleMessageRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<ScheduleMessage, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<ScheduleMessage, bool>>> predicates = new List<Expression<Func<ScheduleMessage, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xTitle":
                            Expression<Func<ScheduleMessage, bool>> temp = null;
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
            Expression<Func<ScheduleMessage, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(ScheduleMessage), "ScheduleMessage");
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

    }
}
