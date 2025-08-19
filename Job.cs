namespace BookMyTradie
{
    
    // Make accessible within the same assembly
    internal class Job
    {
        /// <summary>
        ///  When an object is instatiated from this Job class using the AssignJob function, the object 
        ///  stores status set to "ASSIGNED", job address, start and end dates, the assigned contractor
        ///  and the job cost, then returns an Assigned Job record. Otherwise if the AddJob or CompleteJob 
        ///  function is used, an Unassigned Job record is returned storing only the job address.
        /// </summary>

        // CLASS DATA
        public string Status { get; set; } // Status can be unassigned, assigned or completed
        public string Address { get; set; } // Takes user input
        public string DateStart { get; set; } // Takes from Assignment Form
        public string DateEnd { get; set; } // Takes from Assignment Form
        public string ContractorAssigned { get; set; } // Takes from Assignment Form
        public string Cost { get; set; } // Takes from Assignment Form


        // DISPLAY
        public override string ToString()
        {
            if (Status == "ASSIGNED")
            {
                // Yield assigned job record
                return $"[{Status}] {Address}" +
                       $"\nStart Date: {DateStart} " +
                       $"\nEnd Date: {DateEnd} " +
                       $"\nContractor Assignment: {ContractorAssigned}" +
                       $"\nCost: {Cost}\n";
            }
            else
            {
                // Yield unassigned job record
                return $"{Address}\n";
            }
        }
    }
}