using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EFManagement.Mvc
{
    public class TransactionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!UnitOfWork.IsStarted)
            //{
            //    unitOfWork = UnitOfWork.Start();
            //}
            //else
            //{
            //    unitOfWork = UnitOfWork.Current;
            //}
            //transaction = unitOfWork.BeginTransaction();
            //EntityFrameworkWebSessionStorage.Instance.CurrentObjectContext().Connection.BeginTransaction();
            //NHibernateSession.Current.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult viewResult = filterContext.Result as ViewResult;

            bool isValid = true;
            if (viewResult != null)
                isValid = viewResult.ViewData.ModelState.IsValid;

            if (filterContext.Exception == null && isValid)
            {
                EntityFrameworkWebSessionStorage.Instance.CurrentObjectContext().SaveChanges();
                //NHibernateSession.Current.Transaction.Commit();
            }
            else
            {
                //NHibernateSession.Current.Transaction.Rollback();
            }

        }
    }
}
