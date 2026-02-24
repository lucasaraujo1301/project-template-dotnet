using ProjectTemplate.Application.Dto;

namespace ProjectTemplate.Domain.Interfaces;


public interface IBaseUseCase<TModel, TData>
{
    Task<ResponseDto<ModelPaginatedDto<TModel>, string>> GetAll();
    Task<ResponseDto<TModel, string>> GetById(int id);
    Task<ResponseDto<TModel, string>> Create(TData data);
    Task<ResponseDto<TModel, string>> Update(int id, TData data);
}
