using ProjectTemplate.Domain.Interfaces;
using ProjectTemplate.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace ProjectTemplate.Controllers;


abstract class BaseController<TController, TModel, TData>(ILogger<TController> logger, IBaseUseCase<TModel, TData> useCase) : ControllerBase
{
    protected readonly ILogger<TController> _logger = logger;
    protected readonly IBaseUseCase<TModel, TData> _useCase = useCase;


    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var result = await _useCase.GetAll();
        return result.ToActionResult(this);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create(TData data)
    {
        var result = await _useCase.Create(data);
        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        var result = await _useCase.GetById(id);
        return result.ToActionResult(this);
    }

    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> Patch(TData data, int id)
    {
        var result = await _useCase.Update(id, data);
        return result.ToActionResult(this);
    }
}
