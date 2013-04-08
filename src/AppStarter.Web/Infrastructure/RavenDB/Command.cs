using Raven.Client;

namespace AppStarter.Infrastructure.RavenDB
{
    public abstract class Command<T> : Command
    {
        public T Result { get; protected set; }
    }

    public abstract class Command
    {
        public IDocumentSession Raven { get; set; }
        public abstract void Execute();
    }
}