using System;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace AppStarter.App_Start
{
    public class RavenConfig
    {
        // http://ayende.com/blog/160161/managing-ravendb-document-store-startup
        private static readonly Lazy<IDocumentStore> documentStore = new Lazy<IDocumentStore>(() =>
        {
            var docStore = new DocumentStore
            {
                ConnectionStringName = "RavenDB",

            };
            docStore.Conventions.IdentityPartsSeparator = "-";
            docStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;

            //docStore.Conventions.RegisterIdConvention<Passenger>((dbname, commands, passenger) => passenger.Id);

            docStore.Initialize();
            return docStore;
        });

        public static IDocumentStore DocumentStore { get { return documentStore.Value; } }


        public static void TryCreatingIndexesOrRedirectToErrorPage()
        {
            try
            {
                IndexCreation.CreateIndexes(typeof(MvcApplication).Assembly, DocumentStore);
            }
            catch (WebException e)
            {
                var socketException = e.InnerException as SocketException;
                if (socketException == null)
                    throw;

                switch (socketException.SocketErrorCode)
                {
                    case SocketError.AddressNotAvailable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                    case SocketError.TimedOut:
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.HostNotFound:
                        HttpContext.Current.Response.Redirect("~/RavenNotReachable.htm");
                        break;
                    default:
                        throw;
                }
            }
        }
    }
}