using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.specifications.Doctor_Specifications;
using EyeCareHub.BLL.specifications.Order_Specifications;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using EyeCareHub.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EyeCareHub.BLL.Repositories
{
    public class StoreAdminRepo : IStoreAdminRepo 
    {
        #region Inject
        private readonly IUnitOfWork<StoreContext> _unitOfWork;

        public StoreAdminRepo(IUnitOfWork<StoreContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region MangeType&Brand
        //Mange Type
        public async Task<bool> AddType(ProductTypes item)
        {
            await _unitOfWork.Repository<ProductTypes>().Add(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> UpdateType(ProductTypes item)
        {
            _unitOfWork.Repository<ProductTypes>().Update(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> DeleteType(ProductTypes item)
        {
            _unitOfWork.Repository<ProductTypes>().Delete(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> SearchTypeByName(string name)
        {
            return await _unitOfWork.Repository<ProductTypes>()
                .AnyAsync(pt => pt.Name == name);
        }

        public async Task<bool> SearchTypeById(int TypeId)
        {
            return await _unitOfWork.Repository<ProductTypes>()
                .AnyAsync(pt => pt.Id == TypeId);
        }
        public async Task<ProductTypes> GetTypeById(int TypeId)
            => await _unitOfWork.Repository<ProductTypes>().GetByIdAsync(TypeId);
        // Mange Brand 
        public async Task<bool> AddBrand(ProductBrands item)
        {
            await _unitOfWork.Repository<ProductBrands>().Add(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> UpdateBrand(ProductBrands item)
        {
            _unitOfWork.Repository<ProductBrands>().Update(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> DeleteBrand(ProductBrands item)
        {
            _unitOfWork.Repository<ProductBrands>().Delete(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> SearchBrandByName(string name)
        {
            return await _unitOfWork.Repository<ProductBrands>()
                .AnyAsync(pt => pt.Name == name);
        }

        public async Task<bool> SearchBrandById(int TypeId)
        {
            return await _unitOfWork.Repository<ProductBrands>()
                .AnyAsync(pt => pt.Id == TypeId);
        }
        public async Task<ProductBrands> GetBrandById(int BrandId)
            => await _unitOfWork.Repository<ProductBrands>().GetByIdAsync(BrandId);

        // Mange Category 
        public async Task<bool> AddCategory(ProductCategory item)
        {
            await _unitOfWork.Repository<ProductCategory>().Add(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> UpdateCategory(ProductCategory item)
        {
            _unitOfWork.Repository<ProductCategory>().Update(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> DeleteCategory(ProductCategory item)
        {
            _unitOfWork.Repository<ProductCategory>().Delete(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> SearchCategoryByName(string name)
        {
            return await _unitOfWork.Repository<ProductCategory>()
                .AnyAsync(pt => pt.Name == name);
        }

        public async Task<bool> SearchCategoryById(int TypeId)
        {
            return await _unitOfWork.Repository<ProductCategory>()
                .AnyAsync(pt => pt.Id == TypeId);
        }
        public async Task<ProductCategory> GetCategoryById(int CategoryId)
            => await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(CategoryId);

        #endregion

        #region MangeProduct

        public async Task<bool> AddProduct(Products item)
        {
            await _unitOfWork.Repository<Products>().Add(item);
            return (await _unitOfWork.Complete() > 0);
        }
        public async Task<bool> UpdateProduct(Products item)
        {
            _unitOfWork.Repository<Products>().Update(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<bool> DeleteProduct(Products item)
        {
            _unitOfWork.Repository<Products>().Delete(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<Products> GetProductById(int ProductId)
            => await _unitOfWork.Repository<Products>().GetByIdAsync(ProductId);

        #endregion

        #region ManageOrder

        public async Task<IReadOnlyList<Order>> GetAllOrder(OrderSpecParams oderparams)
        {
            var spec = new OrderSpec(oderparams);

            return await _unitOfWork.Repository<Order>().GetAllWithspecAsync(spec);
        }
        public async Task<int> GetCountOrder(OrderSpecParams oderparams)
        {
            var spec = new OrderCountSpec(oderparams);

            return await _unitOfWork.Repository<Order>().GetCountAsync(spec);
        }

        public async Task<Order> GetOrderById(int OrderId)
        {

            return await _unitOfWork.Repository<Order>().GetByIdAsync(OrderId);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _unitOfWork.Repository<Order>().Update(order);
            return (await _unitOfWork.Complete()>0);
        }

        #endregion

        #region MangeDeliveryMethod
        public async Task<bool> AddDeliveryMethod(DeliveryMethod item)
        {
            await _unitOfWork.Repository<DeliveryMethod>().Add(item);
            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethod()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            
        }

        public async Task<DeliveryMethod> GetDeliveryMethodById(int Id)
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Id);

        }

        public async Task<bool> DeleteDeliveryMethod(DeliveryMethod item)
        {
            _unitOfWork.Repository<DeliveryMethod>().Delete(item);
            return (await _unitOfWork.Complete() > 0);
        }

        #endregion


    }
}
