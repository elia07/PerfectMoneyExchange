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
using Saraf365.Core.Utils;

namespace Saraf365.Core.Repositories
{
    public class AdminNotificationRepository : GenericRepository<AdminNotification>
    {
        public AdminNotificationRepository() { }
        public AdminNotificationRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<AdminNotification, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<AdminNotification, bool>>> predicates = new List<Expression<Func<AdminNotification, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xResponsibleAdmin":
                            Expression<Func<AdminNotification, bool>> temp = null;
                            Int64 xResponsibleAdminValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xResponsibleAdmin == xResponsibleAdminValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xResponsibleAdmin >= xResponsibleAdminValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xResponsibleAdmin > xResponsibleAdminValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xResponsibleAdmin <= xResponsibleAdminValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xResponsibleAdmin < xResponsibleAdminValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xResponsibleAdmin != xResponsibleAdminValue;
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
            Expression<Func<AdminNotification, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(AdminNotification), "AdminNotification");
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

        public void Insert(AdminNotificationType type,string description,Dictionary<string,List<string>> notificationSetting)
        {
            try
            {
                string TelegramAccessToken = new SettingRepository().GetByKey("TelegramBotAccessToken");
                bool isJustNotification = false;
                using (AdminRepository ar = new AdminRepository())
                {
                    foreach (var admin in notificationSetting[type.ToString()])
                    {
                        var adminInstance = ar.GetByEmail(admin);

                        if (!isJustNotification)
                        {
                            AdminNotification instance = new AdminNotification();
                            if (adminInstance != null)
                            {
                                instance.xResponsibleAdmin = adminInstance.xID;
                            }
                            instance.xDate = DateTime.Now;
                            instance.xDescription = description;
                            instance.xType = (byte)type;
                            Insert(instance);
                            Save();
                        }
                        if (adminInstance != null && adminInstance.xTelegramID != null)
                        {
                            try
                            {
                                new TelegramUtils().SendMessage(TelegramAccessToken, Convert.ToInt64(adminInstance.xTelegramID), description);
                            }
                            catch { }

                        }


                        isJustNotification = true;
                    }
                }
            }
            catch
            {

            }

                
            

            
        }

        public int GetAdminUndoneJobsCount(long adminID)
        {
            return (from a in db.AdminNotification where a.xResponsibleAdmin==adminID select a).Count();
        }

        public int GetAdminUnAttachedJobsCount()
        {
            return (from a in db.AdminNotification where a.xResponsibleAdmin == null select a).Count();
        }
    }
}
