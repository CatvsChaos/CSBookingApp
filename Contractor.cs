namespace BookMyTradie
{
    // Make accessible within the same assembly
    internal class Contractor
    {
        /// <summary>
        ///  When an object is instatiated from this Contractor class using the AssignContractor function,
        ///  the object stores status set to "ASSIGNED", business name, start date, contractor's rate, the
        ///  assigned job address and the business contact details, then returns an Assigned Contractor
        ///  record. Otherwise if the AddContractor or CompleteJob function is used, an Unassigned Contractor
        ///  record is returned storing only the business name, rate and contact details.
        /// </summary>

        // CLASS DATA
        public string? Status { get; set; } // Status can be unassigned, assigned or completed
        public string? BusinessName { get; set; } // Takes user input
        public string? DateStart { get; set; } // Takes from Assignment Form
        public double? Rate { get; set; } // Takes user input
        public string? JobAssigned { get; set; } // Takes from Assignment Form
        public string? PhoneEmail { get; set; } // Takes user input


        // DISPLAY
        public override string ToString()
        {
            if (Status == "ASSIGNED")
            {
                // Yield assigned contractor record
                return $"[{Status}] {BusinessName}" +
                       $"\nStart: {DateStart}" +
                       $"\nRate: {Rate}" +
                       $"\nJob Site: {JobAssigned}" +
                       $"\nPhone: {PhoneEmail}\n";
            }
            else
            {
                // Yield unassigned contractor record
                return $"{BusinessName}" +
                       $"\nRate: {Rate}" +
                       $"\nPhone: {PhoneEmail}\n";
            }
        }
    }
}