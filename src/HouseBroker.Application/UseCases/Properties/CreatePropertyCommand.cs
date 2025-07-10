using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class CreatePropertyCommand : IRequest<PropertyDto>
{
    public CreatePropertyDto Property { get; set; } = null!;
    public string BrokerId { get; set; } = string.Empty;
}

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDto>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly ICommissionService _commissionService;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper, ICommissionService commissionService)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _commissionService = commissionService;
    }

    public async Task<PropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = _mapper.Map<Property>(request.Property);
        property.BrokerId = request.BrokerId;
        property.CreatedAt = DateTime.UtcNow;

        // Calculate commission
        property.CommissionRate = await _commissionService.GetCommissionRateAsync(property.Price);
        property.CommissionAmount = await _commissionService.CalculateCommissionAsync(property.Price);

        var createdProperty = await _propertyRepository.AddAsync(property);
        return _mapper.Map<PropertyDto>(createdProperty);
    }
}