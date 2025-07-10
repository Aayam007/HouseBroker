using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Interfaces;
using MediatR;

namespace HouseBroker.Application.UseCases.Properties;

public class UpdatePropertyCommand : IRequest<PropertyDto?>
{
    public Guid Id { get; set; }
    public CreatePropertyDto Property { get; set; } = null!;
}

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var existingProperty = await _propertyRepository.GetByIdAsync(request.Id);
        if (existingProperty == null)
            return null;

        _mapper.Map(request.Property, existingProperty);
        existingProperty.UpdatedAt = DateTime.UtcNow;

        await _propertyRepository.UpdateAsync(existingProperty);
        return _mapper.Map<PropertyDto>(existingProperty);
    }
}