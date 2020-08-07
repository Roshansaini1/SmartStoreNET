﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using SmartStore.Core.Domain.Messages;
using SmartStore.Core.Security;
using SmartStore.Services.Messages;
using SmartStore.Web.Framework.WebApi;
using SmartStore.Web.Framework.WebApi.OData;
using SmartStore.Web.Framework.WebApi.Security;

namespace SmartStore.WebApi.Controllers.OData
{
    public class NewsLetterSubscriptionsController : WebApiEntityController<NewsLetterSubscription, INewsLetterSubscriptionService>
    {
        [WebApiQueryable]
        [WebApiAuthenticate(Permission = Permissions.Customer.Read)]
        public IQueryable<NewsLetterSubscription> Get()
        {
            return GetEntitySet();
        }

        [WebApiQueryable]
        public SingleResult<NewsLetterSubscription> Get(int key)
        {
            return GetSingleResult(key);
        }

		// There is no insert permission because a subscription is always created by customer.
		[WebApiAuthenticate(Permission = Permissions.Promotion.Newsletter.Update)]
		public IHttpActionResult Post(NewsLetterSubscription entity)
		{
			var result = Insert(entity, () =>
			{
				var publishEvent = this.GetQueryStringValue("publishevent", true);

				Service.InsertNewsLetterSubscription(entity, publishEvent);
			});

			return result;
		}

		[WebApiAuthenticate(Permission = Permissions.Promotion.Newsletter.Update)]
		public async Task<IHttpActionResult> Put(int key, NewsLetterSubscription entity)
		{
			var result = await UpdateAsync(entity, key, () =>
			{
				var publishEvent = this.GetQueryStringValue("publishevent", true);

				Service.UpdateNewsLetterSubscription(entity, publishEvent);
			});

			return result;
		}

		[WebApiAuthenticate(Permission = Permissions.Promotion.Newsletter.Update)]
		public async Task<IHttpActionResult> Patch(int key, Delta<NewsLetterSubscription> model)
		{
			var result = await PartiallyUpdateAsync(key, model, entity =>
			{
				var publishEvent = this.GetQueryStringValue("publishevent", true);

				Service.UpdateNewsLetterSubscription(entity, publishEvent);
			});

			return result;
		}

		[WebApiAuthenticate(Permission = Permissions.Promotion.Newsletter.Delete)]
		public async Task<IHttpActionResult> Delete(int key)
		{
			var result = await DeleteAsync(key, entity => Service.DeleteNewsLetterSubscription(entity));
			return result;
		}
	}
}