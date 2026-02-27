using Microsoft.AspNetCore.Mvc;

using ProjectTemplate.Application.Dto;
using ProjectTemplate.Domain.Interfaces;
using ProjectTemplate.Helpers;


namespace ProjectTemplate.Controllers;


internal abstract class BaseController<TController, TModel, TData>(ILogger<TController> logger, IBaseUseCase<TModel, TData> useCase) : ControllerBase
{
    protected readonly ILogger<TController> _logger = logger;
    protected readonly IBaseUseCase<TModel, TData> _useCase = useCase;


    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        ResponseDto<ModelPaginatedDto<TModel>, string> result = await _useCase.GetAll();
        return result.ToActionResult(this);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync(TData data)
    {
        ResponseDto<TModel, string> result = await _useCase.Create(data);
        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetByIdAsync(int id)
    {
        ResponseDto<TModel, string> result = await _useCase.GetById(id);
        return result.ToActionResult(this);
    }

    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> PatchAsync(TData data, int id)
    {
        ResponseDto<TModel, string> result = await _useCase.Update(id, data);
        return result.ToActionResult(this);
    }
}
