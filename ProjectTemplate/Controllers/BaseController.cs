using Microsoft.AspNetCore.Mvc;

using ProjectTemplate.Application.Dto;
using ProjectTemplate.Domain.Interfaces;
using ProjectTemplate.Helpers;


namespace ProjectTemplate.Controllers;


internal abstract class BaseController<TController, TEntityDto, TData>(IBaseUseCase<TEntityDto, TData> useCase) : ControllerBase
{
    protected readonly IBaseUseCase<TEntityDto, TData> _useCase = useCase;


    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        ResponseDto<ModelPaginatedDto<TEntityDto>, string> result = await _useCase.GetAll();
        return result.ToActionResult(this);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync(TData data)
    {
        ResponseDto<TEntityDto, string> result = await _useCase.Create(data);
        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetByIdAsync(int id)
    {
        ResponseDto<TEntityDto, string> result = await _useCase.GetById(id);
        return result.ToActionResult(this);
    }

    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> PatchAsync(TData data, int id)
    {
        ResponseDto<TEntityDto, string> result = await _useCase.Update(id, data);
        return result.ToActionResult(this);
    }
}
