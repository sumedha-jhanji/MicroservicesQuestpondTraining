using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppCQRS
{
    //Write Model
    public class ESModel
    {
        public string Event { get; set; }
        public string EventData { get; set; }
    }

    //for read model
    public class PatientStatistics // actually these are tables in reead model
    {
        public int NumberOfPatient { get; set; }
        public string MaxPatientStates { get; set; }
    }

    //for read model
    public class PatientSimpleData
    {
        public string PatientName { get; set; }
        public string Age { get; set; }
    }

    public class RepositoryEventSourcing<IEvent> 
    {
        private Dictionary<Guid, ESModel> EventTable = new Dictionary<Guid, ESModel>();
        public void Add(Guid guid, IEvent @event)
        {
            var esModel = new ESModel();
            esModel.EventData  = JsonConvert.SerializeObject(@event);
            this.EventTable.Add(guid, esModel);      
            //projection.Project(@event) // internal this event will convert into one of the read model as specified and will store in DB
        }
    }

    public static class RepositoryDB
    {
        public static RepositoryEventSourcing<IEvent> EventStore { get; }

        static RepositoryDB()
        {
            EventStore = new RepositoryEventSourcing<IEvent>();
        }
    }

}
