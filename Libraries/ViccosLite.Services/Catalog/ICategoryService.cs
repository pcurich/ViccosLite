using System.Collections.Generic;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Services.Catalog
{
    public interface ICategoryService
    {
        void DeleteCategory(Category category);
        IPagedList<Category> GetAllCategories(string categoryName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false);
        Category GetCategoryById(int categoryId);
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
    }
}