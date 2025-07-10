using HouseBroker.Application.DTOs;
using HouseBroker.Application.UseCases.Properties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HouseBroker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;

    public PropertiesController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties()
    {
        var cacheKey = "all_properties";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PropertyDto> properties))
        {
            properties = await _mediator.Send(new GetAllPropertiesQuery());
            _cache.Set(cacheKey, properties, TimeSpan.FromMinutes(5));
        }
        return Ok(properties);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PropertyDto>> GetProperty(Guid id)
    {
        var property = await _mediator.Send(new GetPropertyByIdQuery { Id = id });
        if (property == null)
            return NotFound();
        return Ok(property);
    }

    [HttpPost]
    [Authorize(Roles = "Broker")]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // Get broker id from JWT
        var brokerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(brokerId))
            return Unauthorized();
        var command = new CreatePropertyCommand { Property = dto, BrokerId = brokerId };
        var property = await _mediator.Send(command);
        _cache.Remove("all_properties"); // Invalidate cache
        return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Broker")]
    public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] CreatePropertyDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var command = new UpdatePropertyCommand { Id = id, Property = dto };
        var result = await _mediator.Send(command);
        if (result == null)
            return NotFound();
        _cache.Remove("all_properties"); // Invalidate cache
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Broker")]
    public async Task<IActionResult> DeleteProperty(Guid id)
    {
        var result = await _mediator.Send(new DeletePropertyCommand { Id = id });
        if (!result)
            return NotFound();
        _cache.Remove("all_properties"); // Invalidate cache
        return NoContent();
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> SearchProperties([FromQuery] PropertySearchDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cacheKey = $"search_{searchDto.City}_{searchDto.State}_{searchDto.MinPrice}_{searchDto.MaxPrice}_{searchDto.MinBedrooms}_{searchDto.PropertyType}_{searchDto.Page}_{searchDto.PageSize}";
        if (!_cache.TryGetValue(cacheKey, out object result))
        {
            var properties = await _mediator.Send(new SearchPropertiesQuery
            {
                City = searchDto.City,
                State = searchDto.State,
                ZipCode = searchDto.ZipCode,
                MinPrice = searchDto.MinPrice,
                MaxPrice = searchDto.MaxPrice,
                MinBedrooms = searchDto.MinBedrooms,
                MaxBedrooms = searchDto.MaxBedrooms,
                MinBathrooms = searchDto.MinBathrooms,
                MaxBathrooms = searchDto.MaxBathrooms,
                MinSquareFeet = searchDto.MinSquareFeet,
                MaxSquareFeet = searchDto.MaxSquareFeet,
                PropertyType = searchDto.PropertyType,
                Status = searchDto.Status,
                MinYearBuilt = searchDto.MinYearBuilt,
                MaxYearBuilt = searchDto.MaxYearBuilt,
                Features = searchDto.Features,
                SortBy = searchDto.SortBy,
                SortDescending = searchDto.SortDescending,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize
            });

            result = new
            {
                Properties = properties,
                TotalCount = properties.Count(),
                searchDto.Page,
                searchDto.PageSize,
                TotalPages = (int)Math.Ceiling((double)properties.Count() / searchDto.PageSize)
            };

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        }
        return Ok(result);
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetFeaturedProperties()
    {
        var cacheKey = "featured_properties";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<PropertyDto> properties))
        {
            properties = await _mediator.Send(new GetFeaturedPropertiesQuery());
            _cache.Set(cacheKey, properties, TimeSpan.FromMinutes(5));
        }
        return Ok(properties);
    }
}