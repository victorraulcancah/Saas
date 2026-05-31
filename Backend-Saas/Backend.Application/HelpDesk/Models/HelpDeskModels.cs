using Backend.Domain.HelpDesk.Entities;

namespace Backend.Application.HelpDesk.Models;

public record HelpDeskTicketRequest(
    string TicketNumber,
    string Subject,
    string? Description,
    string? Channel,
    HelpDeskTicket.HelpDeskPriority Priority,
    string? AssignedTo,
    DateTime? FirstResponseDueAt,
    DateTime? ResolutionDueAt);

public record HelpDeskTicketStatusRequest(HelpDeskTicket.HelpDeskStatus Status, string? Resolution);

public record HelpDeskTicketResponse(
    Guid Id,
    string TicketNumber,
    string Subject,
    string Description,
    string Channel,
    HelpDeskTicket.HelpDeskPriority Priority,
    HelpDeskTicket.HelpDeskStatus Status,
    string AssignedTo,
    DateTime? FirstResponseDueAt,
    DateTime? ResolutionDueAt,
    string Resolution);
