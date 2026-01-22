using Contracts.Publishing;

namespace Contracts.Common
{
    public class ApplicationService
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
    }

}
