using System;

namespace AppointmentBillingModel
{
    //will be used for code first. DB, Add migration etc
    public class PatientModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double BillAmount { get; set; }
        private List<History> _HistoryProblems { get; set; }// aggregate root example
        public void AddHistory(string description)
        {
            if(description.Length == 0)
            {
                throw new Exception("description is null, not allowed");
            }

            this._HistoryProblems.Add(new History() { Description = description });// aggregate root example
        }

        public List<History> Histories {
            get { return _HistoryProblems; }
        }

        public PatientModel()
        {
            _HistoryProblems = new List<History>();
        }
    }

    public class History
    {
        public string Description { get; set; } = string.Empty;
    }
}
