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
using Saraf365.Core.Enumerations;

namespace Saraf365.Core.Repositories
{
    public class SystemLogRepository : GenericRepository<SystemLog>
    {
        public SystemLogRepository() { }
        public SystemLogRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<SystemLog, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<SystemLog, bool>>> predicates = new List<Expression<Func<SystemLog, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xShortDetail":
                            Expression<Func<SystemLog, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xShortDetail == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xShortDetail != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xShortDetail.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xShortDetail == "" || t.xShortDetail == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xAdminEmail":
                            temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.Admin.xEmail == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.Admin.xEmail != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.Admin.xEmail.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.Admin.xEmail == "" || t.Admin.xEmail == null;
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

                        default:
                            break;
                    }
                }
            }
            Expression<Func<SystemLog, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(SystemLog), "SystemLog");
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



        public void Log(SystemLogType type,string shortDetail,string detail,long? adminID=null)
        {
            SystemLog instance = new SystemLog();
            instance.xDate = DateTime.Now;
            instance.xDetail = detail;
            instance.xShortDetail = shortDetail;
            instance.xType = (byte)type;

            if(adminID!=null)
            {
                instance.xAdminID = adminID;
            }

            Insert(instance);
            Save();
        }

        public void DeleteByDateAndType(DateTime date,SystemLogType type)
        {
            db.Database.ExecuteSqlCommand(string.Format("delete from SystemLog where xType={0} and xDate<='{1}'",(byte)type,date.ToString("yyyy-MM-dd")));
        }
    }
}
