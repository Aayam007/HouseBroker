using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class GetAllPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
{
}

public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IEnumerable<PropertyDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }
}