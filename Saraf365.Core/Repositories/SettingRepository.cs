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
    public class SettingRepository : GenericRepository<Setting>
    {
        public SettingRepository() { }
        public SettingRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<Setting, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<Setting, bool>>> predicates = new List<Expression<Func<Setting, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xKey":
                            Expression<Func<Setting, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xKey == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xKey != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xKey.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xKey == "" || t.xKey == null;
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
            Expression<Func<Setting, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(Setting), "Setting");
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

        public string GetByKey(string key)
        {
            var value= (from s in db.Setting where s.xKey == key select s.xValue).SingleOrDefault();
            if (value != null)
            {
                return value;
            }
            return "";
        }

        public Setting GetBy(string key)
        {
            return (from s in db.Setting where s.xKey == key select s).SingleOrDefault();
        }

        public void UpdateTopHandJackPotValue(double value)
        {
            /*var instance = GetBy("TopHandJackpotValue");
            long newValue = Convert.ToInt64(instance.xValue);
            newValue += Convert.ToInt64(value);
            instance.xValue = newValue.ToString();
            Update(instance);
            Save();*/
        }


        public void UpdateBadBeatJackPotValue(double value)
        {
            /*var instance = GetBy("BadBeatJackpotValue");
            long newValue = Convert.ToInt64(instance.xValue);
            newValue += Convert.ToInt64(value);
            instance.xValue = newValue.ToString();
            Update(instance);
            Save();*/
        }
    }
}
