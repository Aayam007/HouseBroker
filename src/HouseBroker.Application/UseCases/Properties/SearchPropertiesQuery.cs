using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class SearchPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
{
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinBedrooms { get; set; }
    public int? MaxBedrooms { get; set; }
    public int? MinBathrooms { get; set; }
    public int? MaxBathrooms { get; set; }
    public int? MinSquareFeet { get; set; }
    public int? MaxSquareFeet { get; set; }
    public string? PropertyType { get; set; }
    public string? Status { get; set; }
    public int? MinYearBuilt { get; set; }
    public int? MaxYearBuilt { get; set; }
    public string? Features { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchPropertiesQueryHandler : IRequestHandler<SearchPropertiesQuery, IEnumerable<PropertyDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public SearchPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyDto>> Handle(SearchPropertiesQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.SearchPropertiesAsync(
            request.City, request.State, request.ZipCode, request.MinPrice, request.MaxPrice,
            request.MinBedrooms, request.MaxBedrooms, request.MinBathrooms, request.MaxBathrooms,
            request.MinSquareFeet, request.MaxSquareFeet, request.PropertyType, request.Status,
            request.MinYearBuilt, request.MaxYearBuilt, request.Features, request.SortBy,
            request.SortDescending, request.Page, request.PageSize);

        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }
}