using AppCQRS;
using AppointmentBillingModel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PatientAppointmentBilling;

//register services
var serviceProvider = Services.RegisterServices();

//IHandler<CreatePatient> handler = new CreatePatientHandler();// this should come via DI
//define handler options using DI
var createHandler = serviceProvider.GetService<IRequestHandler<CreatePatientCommand>>();
var updateHandler = serviceProvider.GetService<IRequestHandler<UpdatePatientCommand>>();
var deleteHandler = serviceProvider.GetService<IRequestHandler<DeletePatientCommand>>();
CancellationToken ct = new CancellationToken();

PatientModel patientModel = new PatientModel();
patientModel.Name = "CQRSPatient";
patientModel.Age = 40;
patientModel.AddHistory("BP");


var cmdCreatePatient = new CreatePatientCommand()
{
    Name = patientModel.Name,
    Age = patientModel.Age,
    HistoryProblems = patientModel.Histories
};
createHandler?.Handle(cmdCreatePatient, ct);
