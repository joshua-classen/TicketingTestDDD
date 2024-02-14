using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.GraphQLTypes;

public class TicketType : ObjectType<Ticket>
{
    protected override void Configure(IObjectTypeDescriptor<Ticket> descriptor)
    {
        base.Configure(descriptor);
        
        descriptor.BindFieldsExplicitly();
        descriptor.Field(ticket => ticket.Id);
        descriptor.Field(ticket => ticket.Name);
    }
}