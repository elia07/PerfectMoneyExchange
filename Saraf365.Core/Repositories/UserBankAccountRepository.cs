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
    public class UserBankAccountRepository : GenericRepository<UserBankAccount>
    {
        public UserBankAccountRepository() { }
        public UserBankAccountRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<UserBankAccount, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<UserBankAccount, bool>>> predicates = new List<Expression<Func<UserBankAccount, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xUserID":
                            Expression<Func<UserBankAccount, bool>> temp = null;
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
                    }
                }
            }
            Expression<Func<UserBankAccount, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(UserBankAccount), "UserBankAccount");
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

        public bool IsCardNumberExist(string cardNumber)
        {
            return (from ub in db.UserBankAccount where ub.xCartNumber == cardNumber select ub).Any();
        }

        public bool IsShebaNumberExist(string shebaNumber)
        {
            return (from ub in db.UserBankAccount where ub.xShebaNumber == shebaNumber select ub).Any();
        }

        public bool IsAccountNumberExist(string accountNumber)
        {
            return (from ub in db.UserBankAccount where ub.xAccountNumber == accountNumber select ub).Any();
        }


        public IQueryable<UserBankAccount> GetUserBankAccounts(long userID)
        {
            return (from ub in db.UserBankAccount where ub.xUserID == userID select ub);
        }
        public int InActiveUserBankAccounts()
        {
            return (from u in db.UserBankAccount where u.xIsVerified==false select u.xID).Count();
        }
    }
}
