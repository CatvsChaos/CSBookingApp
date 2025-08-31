namespace BookMyTradie
{
    /// <summary>
    ///  This Completed class inherits from the Job class. When an object is instatiated, 
    ///  the object stores status, address, start and end dates, the assigned contractor and
    ///  cost, then returns a Completed Job record if the status is set to "COMPLETED" using
    ///  the CompleteJob function, otherwise it returns null.
    /// </summary>

    // Derive Completed class from Job class
    internal class Completed : Job
    {
        // CLASS DATA
        public new string? Status { get; set; } // Set to Completed
        public new string? Address { get; set; }
        public new string? DateStart { get; set; }
        public new string? DateEnd { get; set; }
        public new string? ContractorAssigned { get; set; }
        public new string? Cost { get; set; }


        // DISPLAY
        public override string? ToString()
        {
            if (Status == "COMPLETED")
            {
                // Yield new completed job using assignment values
                return $"[{Status}]  {Address}" +
                       $"\nStart Date: {DateStart} " +
                       $"\nEnd Date: {DateEnd} " +
                       $"\nContractor Assignment: {ContractorAssigned}" +
                       $"\nCost: {Cost}\n";
            }
            else
            {
                // Yield nothing
                return null;
            }
        }
    }
}