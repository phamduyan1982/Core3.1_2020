﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Application.Common;
using eShopSolution.ViewModels.Catalog.ProductImages;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            // Save  Image
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;                
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description= request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            // Save  Image
            if(request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage ()
                    {
                        Caption = "Thumnail caption",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault =true,
                        SortOrder =1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int ProductId)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) throw new eShopException($"cannot find a product: {ProductId}");

            var images =  _context.ProductImages.Where(i => i.ProductId == ProductId);
            foreach(var image in images)
            {
               await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);       
            return await _context.SaveChangesAsync();
        }

        

        public  async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1. select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if(request.categoryIds.Count>0)
            {
                query = query.Where(p => request.categoryIds.Contains(p.pic.CategoryId));
            }
            //3. paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select (x => new ProductViewModel() { 
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated =x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice =x.p.OriginalPrice,
                    Price =x.p.Price,
                    SeoAlias =x.pt.SeoAlias,
                    SeoDescription =x.pt.SeoDescription,
                    SeoTitle =x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount= x.p.ViewCount
                }).ToListAsync();
            //4. select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex =request.PageIndex,
                Items = data
            };
            return pageResult;

        }

        public async  Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;

        }

        public async  Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
            {
                throw new eShopException($"Can not file an image with id {imageId}");
            }
            var viewModel = new ProductImageViewModel()
                {
                    Caption = image.Caption,
                    DateCreated = image.DateCreated,
                    FileSize = image.FileSize,
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    IsDefault = image.IsDefault,
                    ProductId = image.ProductId,
                    SortOrder = image.SortOrder
                };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return  await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
            {
                Caption =i.Caption,
                DateCreated = i.DateCreated,
                FileSize =i.FileSize,
                Id =i.Id,
                ImagePath = i.ImagePath,
                IsDefault =i.IsDefault,
                ProductId =i.ProductId,
                SortOrder= i.SortOrder

            }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new eShopException($"Can not file an image with id {imageId}");
            }
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product== null || productTranslations == null) throw new eShopException($"cannot find a product with Id: {request.Id}");
            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;
            // Save  Image
            if (request.ThumbnailImage != null)
            {
                var thumnailImage = await  _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumnailImage != null)
                {
                    thumnailImage.FileSize = request.ThumbnailImage.Length;
                    thumnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumnailImage);     
                }                
            }
            return await _context.SaveChangesAsync();
        }

        public async  Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await  _context.ProductImages.FindAsync(imageId);
            if(productImage == null)
            {
                throw new eShopException($"Can not file an image with id {imageId}");
            }
            // Save  Image
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"cannot find a product with Id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;

        }

        public async  Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"cannot find a product with Id: {productId}");
            product.Price += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile (IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        //
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            // 1. select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //2. filter

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            //4. select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pageResult;

        }
        //
    }
}
