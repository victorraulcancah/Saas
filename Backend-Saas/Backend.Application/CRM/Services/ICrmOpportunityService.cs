using Backend.Application.CRM.Models;
using Backend.Domain.CRM.Entities;

namespace Backend.Application.CRM.Services;

public interface ICrmOpportunityService
{
    Task<IEnumerable<OpportunityResponse>> GetOpportunitiesAsync();
    Task<OpportunityResponse> CreateOpportunityAsync(OpportunityRequest request);
    Task<OpportunityResponse?> UpdateStageAsync(Guid id, Opportunity.OpportunityStage stage);
}
