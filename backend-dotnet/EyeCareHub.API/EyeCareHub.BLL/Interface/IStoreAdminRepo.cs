using EyeCareHub.BLL.specifications.Order_Specifications;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IStoreAdminRepo
    {
        //Type
        Task<bool> AddType(ProductTypes item);
        Task<bool> UpdateType(ProductTypes item);
        Task<bool> DeleteType(ProductTypes item);
        Task<bool> SearchTypeByName(string name);
        Task<bool> SearchTypeById(int TypeId);
        Task<ProductTypes> GetTypeById(int TypeId);
        
        //Brand
        Task<bool> AddBrand(ProductBrands item);
        Task<bool> UpdateBrand(ProductBrands item);
        Task<bool> DeleteBrand(ProductBrands item);
        Task<bool> SearchBrandByName(string name);
        Task<bool> SearchBrandById(int TypeId);
        Task<ProductBrands> GetBrandById(int TypeId);
        
        //Category
        Task<bool> AddCategory(ProductCategory item);
        Task<bool> UpdateCategory(ProductCategory item);
        Task<bool> DeleteCategory(ProductCategory item);
        Task<bool> SearchCategoryByName(string name);
        Task<bool> SearchCategoryById(int TypeId);
        Task<ProductCategory> GetCategoryById(int TypeId);

        //Product
        Task<bool> AddProduct(Products item);
        Task<bool> UpdateProduct(Products item);
        Task<bool> DeleteProduct(Products item);
        Task<Products> GetProductById(int ProductId);

        //Order
        Task<int> GetCountOrder(OrderSpecParams oderparams);
        Task<IReadOnlyList<Order>> GetAllOrder(OrderSpecParams oderparams);
        Task<Order> GetOrderById(int OrderId);
        Task<bool> UpdateOrder(Order order);

        //DeliveryMethod
        Task<bool> AddDeliveryMethod(DeliveryMethod item);
        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethod();
        Task<DeliveryMethod> GetDeliveryMethodById(int Id);
        Task<bool> DeleteDeliveryMethod(DeliveryMethod item);


    }
}
