namespace Request.Module.Web.Models.ViewModel
{
    public class LayoutModel<TModel> where TModel : class
    {
        public LayoutModel(TModel pageModel)
        {
            PageModel = pageModel;
        }

        public TModel PageModel { get; set; }
    }
}
