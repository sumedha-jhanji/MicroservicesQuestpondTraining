using AppointmentBillingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCQRS
{
    //example of aggregate root and event sourcing. we can skip this if we don't want this.
    public interface IAggregateRoot
    {
        Guid Guid { get;  }
        int Version { get; }
        IEnumerable<IEvent> Events { get; }      

    }

    public class PatientAggregateRoot : IAggregateRoot
    {
       
        private Guid _guid;
        public Guid Guid { 
            get { return _guid; }
        }

        private int _version;  
        public int Version {
            get { return _version; }
        }

        private IList<IEvent> _events = new List<IEvent>();
        public IEnumerable<IEvent> Events {
            get { return _events; }
        }

        public IEvent CreatePatient(string name, int age, List<History> histories)
        {
            var patientEvent = new CreatePatientEvent() { Name = name, Age = age, HistoryProblems = histories };
            PublishEvent(patientEvent);
            return patientEvent;
        }
        private void PublishEvent(IEvent @event){// event is reserved keyword so add @
            this._version = this._version + 1;

            if (@event is CreatePatientEvent)
            {
                this._guid = Guid.NewGuid();
            }
            RepositoryDB.EventStore.Add(this.Guid, @event);
            this._events.Add(@event);//complete aggregate will be loaded.
        }
    }
}
