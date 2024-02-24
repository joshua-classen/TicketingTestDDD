namespace Ticketing2.GraphQL.Web.Inputs;

public record VeranstaltungCreateInput(
    string Name,
    uint TicketPriceEuroCent,
    uint MaxAmountTickets
    );