using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using AutoMapper;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces;
using CleanArchitectureApp.Domain.Entities;

namespace CleanArchitectureApp.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        return product is null ? null : mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = mapper.Map<Product>(createProductDto);
        var createdProduct = await productRepository.AddAsync(product);
        return mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, CreateProductDto updateProductDto)
    {
        var existingProduct = await productRepository.GetByIdAsync(id)
            ?? throw new ArgumentException($"Product with id {id} not found.");

        mapper.Map(updateProductDto, existingProduct);
        existingProduct.UpdatedDate = DateTime.UtcNow;

        var updatedProduct = await productRepository.UpdateAsync(existingProduct);
        return mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await productRepository.DeleteAsync(id);
    }
}