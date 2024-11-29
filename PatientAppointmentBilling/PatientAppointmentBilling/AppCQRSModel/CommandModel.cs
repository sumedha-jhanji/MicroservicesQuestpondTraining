using AppCQRS;
using AppointmentBillingModel;
using MediatR;

namespace AppCQRS
{
    #region Commands   

    //commands - change state of object
    public class CreatePatientCommand : BaseCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double BillAmount { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public List<History> HistoryProblems { get; set; }
    }

    public class UpdatePatientCommand : BaseCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime LastUpdatedDateTime { get; set; }

    }

    public class DeletePatientCommand : BaseCommand
    {
        public int Id { get; set; }
    }

    public class QueryPatient
    {
    }

    #endregion

    #region Handlers
    public class CreatePatientHandler : IRequestHandler<CreatePatientCommand>
    {
        public Task Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            //if we don't want event sourcing, we can directly use repository here.
            //add to repository

            PatientAggregateRoot patientAggregate = new PatientAggregateRoot();
            patientAggregate.CreatePatient(request.Name, request.Age, request.HistoryProblems);
            return null;
        }
    }
    public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand>
    {

        public Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
    public class DeletePatientHandler : IRequestHandler<DeletePatientCommand>
    {
        public Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
    #endregion

    #region Events
    public class CreatePatientEvent : IEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double BillAmount { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public List<History> HistoryProblems { get; set; }
    }
    #endregion
}
