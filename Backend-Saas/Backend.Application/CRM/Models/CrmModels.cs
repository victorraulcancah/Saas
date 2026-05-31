using Backend.Domain.CRM.Entities;

namespace Backend.Application.CRM.Models;

public record CustomerRequest(string Name, string TaxId, string? Email, string? Phone, string? Address, Customer.CustomerType Type, decimal CreditLimit, string? Notes);
public record CustomerResponse(Guid Id, string Name, string TaxId, string Email, string Phone, string Address, Customer.CustomerType Type, decimal CreditLimit, bool IsActive, string Notes);

public record OpportunityRequest(Guid CustomerId, string Title, string? Description, Opportunity.OpportunityStage Stage, decimal Amount, int Probability, DateTime? ExpectedCloseDate);
public record OpportunityResponse(Guid Id, Guid CustomerId, string CustomerName, string Title, string Description, Opportunity.OpportunityStage Stage, decimal Amount, int Probability, DateTime? ExpectedCloseDate);

public record SalesOrderItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
public record SalesOrderRequest(string OrderNumber, Guid CustomerId, string? Notes, List<SalesOrderItemRequest> Items);
public record SalesOrderItemResponse(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
public record SalesOrderResponse(Guid Id, string OrderNumber, Guid CustomerId, string CustomerName, DateTime OrderDate, SalesOrder.SalesOrderStatus Status, decimal TotalAmount, string Notes, IReadOnlyCollection<SalesOrderItemResponse> Items);

public record SupportTicketRequest(Guid CustomerId, string Subject, string? Description, SupportTicket.TicketPriority Priority, string? AssignedTo);
public record SupportTicketResponse(Guid Id, Guid CustomerId, string CustomerName, string Subject, string Description, SupportTicket.TicketPriority Priority, SupportTicket.TicketStatus Status, string AssignedTo, string Resolution);
