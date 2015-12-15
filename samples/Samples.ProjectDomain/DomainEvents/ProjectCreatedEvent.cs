namespace Samples.ProjectDomain.DomainEvents
{
    public class ProjectCreatedEvent
    {
        public string ProjectId { get; set; }
        public string UniqueName { get; set; }
    }
}
