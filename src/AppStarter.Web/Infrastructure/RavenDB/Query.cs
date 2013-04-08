using Raven.Client;

namespace AppStarter.Infrastructure.RavenDB
{
    public abstract class Query<T> : Query
    {
        public T Result { get; protected set; }

        protected TResult Execute<TResult>(Query<TResult> cmd)
        {
            cmd.Raven = this.Raven;
            cmd.Execute();
            return cmd.Result;
        }
    }

    public abstract class Query
    {
        public IDocumentSession Raven { get; set; }
        public abstract void Execute();

        protected TResult SubQuery<TResult>(Query<TResult> cmd)
        {
            cmd.Raven = Raven;
            cmd.Execute();
            return cmd.Result;
        }
    }
}