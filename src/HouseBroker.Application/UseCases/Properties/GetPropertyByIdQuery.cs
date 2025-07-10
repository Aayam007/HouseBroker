using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class GetPropertyByIdQuery : IRequest<PropertyDto?>
{
    public Guid Id { get; set; }
}

public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }
}