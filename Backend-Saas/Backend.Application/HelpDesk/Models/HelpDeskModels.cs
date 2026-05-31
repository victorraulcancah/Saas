using Backend.Domain.HelpDesk.Entities;

namespace Backend.Application.HelpDesk.Models;

// Help Desk Tickets
public record HelpDeskTicketRequest(string TicketNumber, string Subject, string? Description, string? Channel, HelpDeskTicket.HelpDeskPriority Priority, string? AssignedTo, DateTime? FirstResponseDueAt, DateTime? ResolutionDueAt);
public record HelpDeskTicketResponse(Guid Id, string TicketNumber, string Subject, string Description, string Channel, HelpDeskTicket.HelpDeskPriority Priority, HelpDeskTicket.HelpDeskStatus Status, string AssignedTo, DateTime? FirstResponseDueAt, DateTime? ResolutionDueAt, string Resolution);
public record HelpDeskTicketStatusRequest(HelpDeskTicket.HelpDeskStatus Status, string? Resolution);

// Ticket Messages
public record TicketMessageRequest(Guid HelpDeskTicketId, TicketMessage.MessageType Type, string Message, bool IsInternal);
public record TicketMessageResponse(Guid Id, Guid HelpDeskTicketId, TicketMessage.MessageType Type, Guid? UserId, string UserName, string Message, bool IsInternal, DateTime CreatedAt);

// SLA Policies
public record SlaPolicyRequest(string Name, string Description, HelpDeskTicket.HelpDeskPriority Priority, int FirstResponseMinutes, int ResolutionMinutes);
public record SlaPolicyResponse(Guid Id, string Name, HelpDeskTicket.HelpDeskPriority Priority, int FirstResponseMinutes, int ResolutionMinutes, bool IsActive);

// Ticket Assignments
public record TicketAssignmentRequest(Guid HelpDeskTicketId, Guid AgentId, string AgentName, Guid? QueueId, string? AssignmentReason);
public record TicketAssignmentResponse(Guid Id, Guid HelpDeskTicketId, Guid AgentId, string AgentName, DateTime AssignedAt);
