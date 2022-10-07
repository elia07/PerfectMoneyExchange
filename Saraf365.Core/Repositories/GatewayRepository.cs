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
    public class GatewayRepository : GenericRepository<Gateway>
    {
        public GatewayRepository() { }
        public GatewayRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<Gateway, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<Gateway, bool>>> predicates = new List<Expression<Func<Gateway, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xName":
                            Expression<Func<Gateway, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xName == "" || t.xName == null;
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
                        case "xMaxDailyAmount":
                            temp = null;
                            Int64 xMaxDailyAmountValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xMaxDailyAmount == xMaxDailyAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xMaxDailyAmount >= xMaxDailyAmountValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xMaxDailyAmount > xMaxDailyAmountValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xMaxDailyAmount <= xMaxDailyAmountValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xMaxDailyAmount < xMaxDailyAmountValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xMaxDailyAmount != xMaxDailyAmountValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xActiveforLevelsAbove":
                            temp = null;
                            Int64 xActiveforLevelsAboveValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xActiveforLevelsAbove == xActiveforLevelsAboveValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xActiveforLevelsAbove >= xActiveforLevelsAboveValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xActiveforLevelsAbove > xActiveforLevelsAboveValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xActiveforLevelsAbove <= xActiveforLevelsAboveValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xActiveforLevelsAbove < xActiveforLevelsAboveValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xActiveforLevelsAbove != xActiveforLevelsAboveValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xCommisionPercent":
                            temp = null;
                            double xCommisionPercentValue = Convert.ToDouble(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xCommisionPercent == xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xCommisionPercent >= xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xCommisionPercent > xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xCommisionPercent <= xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xCommisionPercent < xCommisionPercentValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xCommisionPercent != xCommisionPercentValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xMinBuyIn":
                            temp = null;
                            Int64 xMinBuyInValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xMinBuyIn == xMinBuyInValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xMinBuyIn >= xMinBuyInValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xMinBuyIn > xMinBuyInValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xMinBuyIn <= xMinBuyInValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xMinBuyIn < xMinBuyInValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xMinBuyIn != xMinBuyInValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        case "xTodayTotalTransactionAmounts":
                            temp = null;
                            Int64 xTodayTotalTransactionAmountsValue = Convert.ToInt64(item.Value);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xTodayTotalTransactionAmounts == xTodayTotalTransactionAmountsValue;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xTodayTotalTransactionAmounts >= xTodayTotalTransactionAmountsValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xTodayTotalTransactionAmounts > xTodayTotalTransactionAmountsValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xTodayTotalTransactionAmounts <= xTodayTotalTransactionAmountsValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xTodayTotalTransactionAmounts < xTodayTotalTransactionAmountsValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xTodayTotalTransactionAmounts != xTodayTotalTransactionAmountsValue;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;


                        case "xAllowCustomAmountForBuyIn":
                            temp = null;
                            bool xAllowCustomAmountForBuyInValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xAllowCustomAmountForBuyIn == xAllowCustomAmountForBuyInValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xAllowCustomAmountForBuyIn != xAllowCustomAmountForBuyInValue;
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
                        case "xIsPrimary":
                            temp = null;
                            bool xIsPrimaryValue = Convert.ToBoolean((string)item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsPrimary == xIsPrimaryValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsPrimary != xIsPrimaryValue;
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
                        case "xIsVip":
                            temp = null;
                            bool xIsVipValue = Convert.ToBoolean(item.Value);
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xIsVip == xIsVipValue;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xIsVip != xIsVipValue;
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
                        case "xLastSuccessTransactionDate":
                            temp = null;
                            DateTime xLastSuccessTransactionDateValue = Convert.ToDateTime(((string)item.Value));
                            DateTime xLastSuccessTransactionDateValueRange = Convert.ToDateTime(((string)item.Value)).AddDays(1).AddMinutes(-1);
                            switch (item.LogicalOperator)
                            {
                                case LogicalOperatorType.Equal:
                                    temp = t => t.xLastSuccessTransactionDate >= xLastSuccessTransactionDateValue && t.xLastSuccessTransactionDate <= xLastSuccessTransactionDateValueRange;
                                    break;
                                case LogicalOperatorType.GreaterOrEqual:
                                    temp = t => t.xLastSuccessTransactionDate >= xLastSuccessTransactionDateValue;
                                    break;
                                case LogicalOperatorType.GreaterThan:
                                    temp = t => t.xLastSuccessTransactionDate > xLastSuccessTransactionDateValue;
                                    break;
                                case LogicalOperatorType.LessOrEqual:
                                    temp = t => t.xLastSuccessTransactionDate <= xLastSuccessTransactionDateValue;
                                    break;
                                case LogicalOperatorType.LessThan:
                                    temp = t => t.xLastSuccessTransactionDate < xLastSuccessTransactionDateValue;
                                    break;
                                case LogicalOperatorType.NotEqual:
                                    temp = t => t.xLastSuccessTransactionDate < xLastSuccessTransactionDateValue || t.xLastSuccessTransactionDate > xLastSuccessTransactionDateValueRange;
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
            Expression<Func<Gateway, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(Gateway), "Gateway");
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


        public List<Gateway> GetGatewayForUser(long userID)
        {
            using (UserRepository ur = new UserRepository())
            {
                var userInstance = ur.GetByID(userID);

                List<Gateway> availGateways = new List<Gateway>();
                Gateway gatewayItem = null;

                if (userInstance.xIsVip)
                {
                    var ipgVip = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.IPG && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == true select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                    if (ipgVip == null)
                    {
                        var ipgNoneVip = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.IPG && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == false select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                        if (ipgNoneVip == null)
                        {
                            //ipgPrimary
                            gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.IPG && g.xIsPrimary select g).FirstOrDefault();
                            if (gatewayItem != null)
                            {
                                availGateways.Add(gatewayItem);
                            }
                        }
                        else
                        {
                            availGateways.Add(ipgNoneVip);
                        }
                    }
                    else
                    {
                        availGateways.Add(ipgVip);
                    }

                    var cartBeCartVip= (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.CartBeCart && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == true select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                    if (cartBeCartVip == null)
                    {
                        var cartBeCartNoneVip = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.CartBeCart && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == false select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                        if(cartBeCartNoneVip==null)
                        {
                            //cartbecartPrimary
                            gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.CartBeCart && g.xIsPrimary select g).FirstOrDefault();
                            if (gatewayItem != null)
                            {
                                availGateways.Add(gatewayItem);
                            }
                        }
                        else
                        {
                            availGateways.Add(cartBeCartNoneVip);
                        }
                    }
                    else
                    {
                        availGateways.Add(cartBeCartVip);
                    }


                    
                }
                else
                {
                    if (Convert.ToDateTime(userInstance.xIPGRestriction) < DateTime.Now)
                    {
                        var ipgNoneVip = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.IPG && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == false select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                        if (ipgNoneVip == null)
                        {
                            //ipgPrimary
                            gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.IPG && g.xIsPrimary select g).FirstOrDefault();
                            if (gatewayItem != null)
                            {
                                availGateways.Add(gatewayItem);
                            }
                        }
                        else
                        {
                            availGateways.Add(ipgNoneVip);
                        }
                    }
                    



                    var cartBeCartNoneVip = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.CartBeCart && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && g.xIsVip == false select g).OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x => x.xCommisionPercent).FirstOrDefault();
                    if (cartBeCartNoneVip == null)
                    {
                        //cartbecartPrimary
                        gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.CartBeCart && g.xIsPrimary select g).FirstOrDefault();
                        if (gatewayItem != null)
                        {
                            availGateways.Add(gatewayItem);
                        }
                    }
                    else
                    {
                        availGateways.Add(cartBeCartNoneVip);
                    }


                    

                }

                gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.PerfectMoney select g).FirstOrDefault();
                if (gatewayItem != null)
                {
                    availGateways.Add(gatewayItem);
                }

                gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.Bitcoin select g).FirstOrDefault();
                if (gatewayItem != null)
                {
                    availGateways.Add(gatewayItem);
                }

                gatewayItem = (from g in db.Gateway where g.xIsActive && g.xType == (byte)GatewayType.Voucher select g).FirstOrDefault();
                if (gatewayItem != null)
                {
                    availGateways.Add(gatewayItem);
                }

                

                return availGateways;
            }
            
        }



        public int GetAvailAndUnFilledGateways()
        {
            return (from g in db.Gateway where g.xIsActive && g.xMaxDailyAmount<=g.xTodayTotalTransactionAmounts select g.xID).Count();
        }
        public int GetPrimaryGatewaysCount()
        {
            return (from g in db.Gateway where g.xIsActive && g.xIsPrimary select g.xID).Count();
        }
    }
}



/* availGateways= (from g in db.Gateway where g.xIsActive && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && ((g.xType==(byte)GatewayType.CartBeCart || g.xType == (byte)GatewayType.IPG) && g.xIsVip == userInstance.xIsVip) group g by g.xType into sg select sg.OrderBy(x => x.xLastSuccessTransactionDate).ThenBy(x=>x.xCommisionPercent).FirstOrDefault()).ToList();
                //availGateways = (from g in db.Gateway where g.xIsActive && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xMaxDailyAmount <= g.xTodayTotalTransactionAmounts) && g.xIsVip == userInstance.xIsVip && g.xType != (byte)GatewayType.GiftGateWay select g).ToList();

                if (availGateways.Count() == 0)
                {
                    availGateways = (from g in db.Gateway where g.xIsActive && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && ((g.xType == (byte)GatewayType.CartBeCart || g.xType == (byte)GatewayType.IPG) && g.xIsVip == userInstance.xIsVip) && g.xIsPrimary == true select g).ToList();
                }
                if(!availGateways.Where(x=>x.xType==(byte)GatewayType.IPG).Any())
                {
                    var ipgGateway=(from g in db.Gateway where g.xIsActive && g.xActiveforLevelsAbove <= userInstance.xLevel && (g.xMaxDailyAmount == -1 || g.xTodayTotalTransactionAmounts <= g.xMaxDailyAmount) && ((g.xType == (byte)GatewayType.CartBeCart || g.xType == (byte)GatewayType.IPG) && g.xIsVip == userInstance.xIsVip) && g.xType == (byte)GatewayType.IPG && g.xIsPrimary == true select g).SingleOrDefault();
                    if(ipgGateway!=null)
                    {
                        if (availGateways == null)
                        {
                            availGateways = new List<Gateway>();
                        }
                        availGateways.Add(ipgGateway);
                    }
                }

                if (Convert.ToDateTime(userInstance.xIPGRestriction) > DateTime.Now)
                {
                    availGateways = availGateways.Where(x => x.xType != (byte)GatewayType.IPG).ToList();
                }*/
