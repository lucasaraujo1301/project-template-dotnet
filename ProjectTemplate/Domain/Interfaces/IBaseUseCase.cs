using ProjectTemplate.Application.Dto;

namespace ProjectTemplate.Domain.Interfaces;


public interface IBaseUseCase<TEntityDto, TData>
{
    Task<ResponseDto<ModelPaginatedDto<TEntityDto>, string>> GetAll();
    Task<ResponseDto<TEntityDto, string>> GetById(int id);
    Task<ResponseDto<TEntityDto, string>> Create(TData data);
    Task<ResponseDto<TEntityDto, string>> Update(int id, TData data);
}
