using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Ticketing.GraphQL.Web.Schema.Mutations;
using Ticketing.GraphQL.Web.Schema.Queries;

namespace Ticketing.GraphQL.Web.Schema.Subscriptions;

public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;


    [SubscribeAndResolve]
    public ValueTask<ISourceStream<CourseResult>>  CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
    {
        string topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
        return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
    }
}