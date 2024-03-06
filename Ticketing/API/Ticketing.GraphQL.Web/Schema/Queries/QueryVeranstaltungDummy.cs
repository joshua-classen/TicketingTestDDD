using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.Schema.Queries;


[ExtendObjectType(Name = "Query")]
public class QueryVeranstaltungDummy
{
    public Veranstaltung GetVeranstaltungDummy()
    {
        var veranstaltung = new Veranstaltung()
        {
            Name = "dummyVeranstaltung",
            TicketPriceEuroCent = 9999,
            MaxAmountTickets = 999
        };

        return veranstaltung;
    }
}