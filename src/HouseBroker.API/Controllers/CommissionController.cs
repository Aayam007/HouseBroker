using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;
using HouseBroker.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Broker")]
public class CommissionController : ControllerBase
{
    private readonly ICommissionService _commissionService;
    private readonly HouseBrokerDbContext _context;

    public CommissionController(ICommissionService commissionService, HouseBrokerDbContext context)
    {
        _commissionService = commissionService;
        _context = context;
    }

    [HttpGet("rates")]
    public async Task<ActionResult<IEnumerable<CommissionRate>>> GetCommissionRates()
    {
        var rates = await _commissionService.GetActiveCommissionRatesAsync();
        return Ok(rates);
    }

    [HttpGet("my-commissions")]
    public async Task<ActionResult<object>> GetMyCommissions()
    {
        var brokerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(brokerId))
            return Unauthorized();

        var properties = await _context.Properties
            .Where(p => p.BrokerId == brokerId && !p.IsDeleted)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                p.CommissionRate,
                p.CommissionAmount,
                p.Status
            })
            .ToListAsync();

        var totalCommission = properties.Sum(p => p.CommissionAmount ?? 0);
        var totalProperties = properties.Count;

        return Ok(new
        {
            TotalCommission = totalCommission,
            TotalProperties = totalProperties,
            Properties = properties
        });
    }

    [HttpGet("calculate/{propertyId}")]
    public async Task<ActionResult<object>> CalculateCommission(Guid propertyId)
    {
        var brokerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(brokerId))
            return Unauthorized();

        var property = await _context.Properties
            .FirstOrDefaultAsync(p => p.Id == propertyId && p.BrokerId == brokerId && !p.IsDeleted);

        if (property == null)
            return NotFound();

        var commissionRate = await _commissionService.GetCommissionRateAsync(property.Price);
        var commissionAmount = await _commissionService.CalculateCommissionAsync(property.Price);

        return Ok(new
        {
            PropertyId = property.Id,
            PropertyTitle = property.Title,
            Price = property.Price,
            CommissionRate = commissionRate,
            CommissionAmount = commissionAmount
        });
    }
}