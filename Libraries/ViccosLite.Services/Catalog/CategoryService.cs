using System;
using System.Collections.Generic;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Core.Caching;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Catalog;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        #region Ctr

        public CategoryService(IRepository<Category> categoryRepository,
            IRepository<StoreMapping> storeMappingRepository, IWorkContext workContext, IStoreContext storeContext,
            ICacheManager cacheManager)
        {
            _categoryRepository = categoryRepository;
            _storeMappingRepository = storeMappingRepository;
            _workContext = workContext;
            _storeContext = storeContext;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Metodos

        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            category.Deleted = true;
            UpdateCategory(category);
        }

        public virtual IPagedList<Category> GetAllCategories(string categoryName = "", int pageIndex = 0,
            int pageSize = Int32.MaxValue,
            bool showHidden = false)
        {
            var query = _categoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!String.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder);
            //Store mapping
            var currentStoreId = _storeContext.CurrentStore.Id;
            query = from c in query
                    join sm in _storeMappingRepository.Table
                        on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                    from sm in c_sm.DefaultIfEmpty()
                    where !c.LimitedToStores || currentStoreId == sm.StoreId
                    select c;


            //only distinct categories (group by ID)
            query = from c in query
                    group c by c.Id
                        into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
            query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder);


            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = unsortedCategories.SortCategoriesForTree();

            //paging
            return new PagedList<Category>(sortedCategories, pageIndex, pageSize);
        }

        public virtual IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false)
        {
            var key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentCategoryId, showHidden,
                _workContext.CurrentUser.Id, _storeContext.CurrentStore.Id);
            return _cacheManager.Get(key, () =>
            {
                var query = _categoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.Where(c => c.ParentCategoryId == parentCategoryId);
                query = query.Where(c => !c.Deleted);
                query = query.OrderBy(c => c.DisplayOrder);

                //Store mapping
                var currentStoreId = _storeContext.CurrentStore.Id;
                query = from c in query
                        join sm in _storeMappingRepository.Table
                            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                        from sm in c_sm.DefaultIfEmpty()
                        where !c.LimitedToStores || currentStoreId == sm.StoreId
                        select c;

                //only distinct categories (group by ID)
                query = from c in query
                        group c by c.Id
                            into cGroup
                            orderby cGroup.Key
                            select cGroup.FirstOrDefault();
                query = query.OrderBy(c => c.DisplayOrder);


                var categories = query.ToList();
                return categories;
            });
        }

        public virtual Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            var key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
            return _cacheManager.Get(key, () => _categoryRepository.GetById(categoryId));
        }

        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Insert(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
        }

        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            //validate category hierarchy
            var parentCategory = GetCategoryById(category.ParentCategoryId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            }

            _categoryRepository.Update(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
        }

        #endregion

        #region Const

        /// <summary>
        ///     Key for caching
        /// </summary>
        /// <remarks>
        ///     {0} : category ID
        /// </remarks>
        private const string CATEGORIES_BY_ID_KEY = "Soft.category.id-{0}";

        /// <summary>
        ///     Key for caching
        /// </summary>
        /// <remarks>
        ///     {0} : parent category ID
        ///     {1} : show hidden records?
        ///     {2} : current user ID
        ///     {3} : store ID
        /// </remarks>
        private const string CATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "Soft.category.byparent-{0}-{1}-{2}-{3}";

        /// <summary>
        ///     Key pattern to clear cache
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "Soft.category.";

        #endregion

        #region Campos

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;

        #endregion
    }
}