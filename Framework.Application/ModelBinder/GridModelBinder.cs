using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Framework.Application.ModelBinder
{
    public class GridModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryCollection = bindingContext.HttpContext.Request.Query;
            var sortKey = queryCollection.Keys.SingleOrDefault(item => item.Contains("sort"));

            var model = Activator.CreateInstance(bindingContext.ModelType);
            var pageIndex = bindingContext.ModelType.GetProperty("PageIndex");
            var pageSize = bindingContext.ModelType.GetProperty("PageSize");
            var orderByFieldName = bindingContext.ModelType.GetProperty("OrderByFieldName");
            var sortOrder = bindingContext.ModelType.GetProperty("SortOrder");
            var keyword = bindingContext.ModelType.GetProperty("Keyword");
            var filters = bindingContext.ModelType.GetProperty("Filters");
            var options = bindingContext.ModelType.GetProperty("Options");

            if (!string.IsNullOrEmpty(queryCollection["current"]))
            {
                pageIndex?.SetValue(model, int.Parse(queryCollection["current"]));
            }

            if (!string.IsNullOrEmpty(queryCollection["rowCount"]))
            {
                pageSize?.SetValue(model, int.Parse(queryCollection["rowCount"]));
            }

            if (sortKey != null)
            {
                orderByFieldName?.SetValue(model, sortKey.Replace("sort", "").Replace("[", "").Replace("]", ""));
                sortOrder?.SetValue(model, queryCollection[sortKey].ToString());
            }

            if (!string.IsNullOrEmpty(queryCollection["searchPhrase"]))
            {
                keyword?.SetValue(model, queryCollection["searchPhrase"].ToString());
            }

            if (!string.IsNullOrEmpty(queryCollection["filters"]))
            {
                filters?.SetValue(model, queryCollection["filters"].ToString());
            }

            if (!string.IsNullOrEmpty(queryCollection["options"]))
            {
                options?.SetValue(model, queryCollection["options"].ToString());
            }

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
