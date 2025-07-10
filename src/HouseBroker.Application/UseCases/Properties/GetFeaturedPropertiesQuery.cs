using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class GetFeaturedPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
{
}

public class GetFeaturedPropertiesQueryHandler : IRequestHandler<GetFeaturedPropertiesQuery, IEnumerable<PropertyDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetFeaturedPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyDto>> Handle(GetFeaturedPropertiesQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetFeaturedPropertiesAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }
}