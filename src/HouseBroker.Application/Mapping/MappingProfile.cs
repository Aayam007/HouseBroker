using AutoMapper;
using HouseBroker.Domain.Entities;
using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Property mappings
        CreateMap<Property, PropertyDto>();
        CreateMap<CreatePropertyDto, Property>();
        CreateMap<UpdatePropertyDto, Property>();
        
        // Commission rate mappings
        CreateMap<CommissionRate, CommissionRateDto>();
        CreateMap<CreateCommissionRateDto, CommissionRate>();
        CreateMap<UpdateCommissionRateDto, CommissionRate>();
    }
} 