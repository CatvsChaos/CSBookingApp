<<<<<<< HEAD
﻿using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
=======
﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
>>>>>>> 9fa45e3c10010c3bd924abed766e3e123de6e4ba




namespace BookMyTradie
{
    internal class RecruitmentSystem
    {

        // ======================== DATA LISTS
        /// <summary>
        /// Creates list instances to store Jobs, Contractors and Completed Jobs records.
        /// </summary>

        public List<Job> List_Jobs = new List<Job>();
        public List<Contractor> List_Contractors = new List<Contractor>();
        public List<Completed> List_Completed = new List<Completed>();




        // ======================== SEARCH DATA IN LIST

        /// <summary>
        /// 1. DataMatched: This generic search function finds matching records in the data list using a string value.
        ///  This function is used within functions - JobSelected, ContractorSelected, AssignJob,
        ///  AssignContractor, AddJob, AddContractor, CompletedJob, CompletedContractor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchString"></param>
        /// <param name="List_Items"></param>
        /// <param name="itemSelector"></param>
        /// <returns>Either a matching item or default is returned.</returns>

        // [1] CHECK FOR DUPLICATES - SEARCH FOR MATCHING DATA IN LIST
        public T DataMatched<T>(string searchString,
                                List<T> List_Items,
                                Func<T, string> itemSelector)
        {
            // Look through each record in data list
            foreach (T item in List_Items)
            {
                // If there is a match between old and assigned records
                if (itemSelector(item).Equals(searchString))
                {
                    // Store the old record and its list index
                    return item;
                }
            }
            return default;
        }





        // ======================== LISTBOX FILTERS

        /// <summary>
        /// 1. ShowAll: This generic function resets the listbox to display all data.
        /// 2. FilterList: This generic function filters the listbox with given conditions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List_Items"></param>
        /// <param name="ListBox"></param>
        /// <param name="filterCondition"></param>
        /// <returns>Returns List_Items, filtered if a condition is provided.</returns>

        // [1] LISTBOX FILTER - LOAD ALL ITEMS
        public List<T> ShowAll<T>(List<T> List_Items,
                                  ListBox ListBox)
        {
            // Refresh and update ListBox with Jobs List
            ListBox.ItemsSource = null;
            ListBox.ItemsSource = List_Items;
            return List_Items;
        }

        // [2] LISTBOX FILTER - LOAD ITEMS BASED ON CONDITIONS
        public List<T> FilterList<T>(List<T> List_Items,
                                     ListBox ListBox,
                                     Func<T, bool> filterCondition)
        {
            // Clear ListBox
            ListBox.ItemsSource = null;
            // Filter list using LINQ
            var filteredItems = List_Items.Where(filterCondition).ToList();
            // Bind filtered list to ListBox
            ListBox.ItemsSource = filteredItems;

            return filteredItems;
        }





        // ======================== LOAD SELECTED RECORD

        /// <summary>
        /// 1. JobSelected: This function searches for selected records in the list using the
        ///     DataMatched function and loads the record's data to Jobs, Contractors and
        ///     Assignment + Completion Forms.
        /// 2. ContractorSelected: This function searches for selected records in the list using
        ///     the DataMatched function and loads the record's data to Jobs, Contractors and
        ///     Assignment + Completion Forms.
        /// </summary>
        /// <param name="ListBox_Jobs"></param>
        /// <param name="TextBox_Job_Address"></param>
        /// <param name="Label_Job_DateStart"></param>
        /// <param name="Label_Job_DateEnd"></param>
        /// <param name="Label_Job_ContractorAssigned"></param>
        /// <param name="Label_Job_Cost"></param>
        /// <param name="List_Contractors"></param>
        /// <param name="TextBox_Contractor_BusinessName"></param>
        /// <param name="Label_Contractor_DateStart"></param>
        /// <param name="TextBox_Contractor_Rate"></param>
        /// <param name="Label_Contractor_JobAssigned"></param>
        /// <param name="TextBox_Contractor_PhoneEmail"></param>
        /// <param name="Label_Assignment_Job"></param>
        /// <param name="Label_Assignment_Contractor"></param>
        /// <param name="DatePicker_Assignment_DateStart"></param>
        /// <param name="DatePicker_Assignment_DateEnd"></param>
        /// <param name="Label_Assignment_Cost"></param>


        // [1] JOB SELECTION - LOAD JOB INFO TO ALL FORMS
        public void JobSelected(ListBox ListBox_Jobs,
                                List<Contractor> List_Contractors,

                                Label Label_Assignment_Job,
                                Label Label_Assignment_Contractor,
                                DatePicker DatePicker_Assignment_DateStart,
                                DatePicker DatePicker_Assignment_DateEnd,
                                Label Label_Assignment_Cost,

                                TextBox TextBox_Job_Address,
                                Label Label_Job_DateStart,
                                Label Label_Job_DateEnd,
                                Label Label_Job_ContractorAssigned,
                                Label Label_Job_Cost,

                                TextBox TextBox_Contractor_BusinessName,
                                Label Label_Contractor_DateStart,
                                TextBox TextBox_Contractor_Rate,
                                Label Label_Contractor_JobAssigned,
                                TextBox TextBox_Contractor_PhoneEmail)
        {
            // If a Job is selected in the ListBox
            if (ListBox_Jobs.SelectedItem != null)
            {
                if (ListBox_Jobs.SelectedItem is Completed)
                {
                    return; // Prevents loading nothing to forms
                }
                else
                {
                    // Store selection
                    Job job = ListBox_Jobs.SelectedItem as Job;

                    // Load Job Form - with assigned/unassigned job data
                    TextBox_Job_Address.Text = job.Address;
                    Label_Job_DateStart.Content = job.DateStart;
                    Label_Job_DateEnd.Content = job.DateEnd;
                    Label_Job_ContractorAssigned.Content = job.ContractorAssigned;
                    Label_Job_Cost.Content = job.Cost;


                    // If Job is assigned
                    if (job.Status == "ASSIGNED")
                    {
                        // Find and store corresponding contractor instance in Contractors List using ContractorAssigned
                        Contractor matchedContractor = DataMatched(job.ContractorAssigned,
                                                                   List_Contractors,
                                                                   contractor => contractor.BusinessName);

                        // If there is a corresponding contractor instance
                        if (matchedContractor != null)
                        {
                            // Load Contractor Form - with assigned contractor data
                            TextBox_Contractor_BusinessName.Text = job.ContractorAssigned;
                            Label_Contractor_DateStart.Content = matchedContractor.DateStart;
                            Label_Contractor_JobAssigned.Content = job.Address;
                            TextBox_Contractor_PhoneEmail.Text = matchedContractor.PhoneEmail;
                            TextBox_Contractor_Rate.Text = matchedContractor.Rate.ToString();

                            // Load Assignment Form - with assigned job and contractor data
                            Label_Assignment_Job.Content = job.Address;
                            Label_Assignment_Contractor.Content = job.ContractorAssigned;
                            DatePicker_Assignment_DateStart.SelectedDate = DateTime.Parse(job.DateStart.ToString());
                            DatePicker_Assignment_DateEnd.SelectedDate = DateTime.Parse(job.DateEnd.ToString());
                            Label_Assignment_Cost.Content = job.Cost;
                        }
                    }
                    // If Job is unassigned
                    else
                    {
                        // Load Assignment Form - with unassigned job data
                        Label_Assignment_Job.Content = job.Address;
                        DatePicker_Assignment_DateStart.SelectedDate = null;
                        DatePicker_Assignment_DateEnd.SelectedDate = null;
                        if (Label_Assignment_Contractor.Content == null)
                        {
                            Label_Assignment_Cost.Content = "Cost 00.00";
                        }
                    }
                }
            }
        }

        // [2] CONTRACTOR SELECTION - LOAD CONTRACTOR INFO TO ALL FORMS
        public void ContractorSelected(ListBox ListBox_Contractors,
                                       List<Job> List_Jobs,

                                       TextBox TextBox_Contractor_BusinessName,
                                       Label Label_Contractor_DateStart,
                                       TextBox TextBox_Contractor_Rate,
                                       Label Label_Contractor_JobAssigned,
                                       TextBox TextBox_Contractor_PhoneEmail,

                                       Label Label_Assignment_Job,
                                       Label Label_Assignment_Contractor,
                                       DatePicker DatePicker_Assignment_DateStart,
                                       DatePicker DatePicker_Assignment_DateEnd,
                                       Label Label_Assignment_Cost,

                                       TextBox TextBox_Job_Address,
                                       Label Label_Job_DateStart,
                                       Label Label_Job_DateEnd,
                                       Label Label_Job_ContractorAssigned,
                                       Label Label_Job_Cost)
        {
            // If a Contractor is selected in the listBox
            if (ListBox_Contractors.SelectedItem != null)
            {
                // Store selection
                Contractor contractor = ListBox_Contractors.SelectedItem as Contractor;

                // Load Contractor Form - with assigned/unassigned contractor data
                TextBox_Contractor_BusinessName.Text = contractor.BusinessName;
                Label_Contractor_DateStart.Content = contractor.DateStart;
                TextBox_Contractor_Rate.Text = contractor.Rate.ToString();
                Label_Contractor_JobAssigned.Content = contractor.JobAssigned;
                TextBox_Contractor_PhoneEmail.Text = contractor.PhoneEmail;

                // If Contractor is assigned
                if (contractor.Status == "ASSIGNED")
                {
                    // Find and store corresponding Job instance in list using Job address
                    Job matchedJob = DataMatched(contractor.JobAssigned,
                                                    List_Jobs,
                                                    job => job.Address);

                    // If there is a corresponding job instance
                    if (matchedJob != null)
                    {
                        // Load Job Form - with assigned job data
                        TextBox_Job_Address.Text = matchedJob.Address;
                        Label_Job_DateStart.Content = matchedJob.DateStart;
                        Label_Job_DateEnd.Content = matchedJob.DateEnd;
                        Label_Job_ContractorAssigned.Content = matchedJob.ContractorAssigned;
                        Label_Job_Cost.Content = matchedJob.Cost;

                        // Load Assignment Form - with assigned job and contractor data
                        Label_Assignment_Job.Content = matchedJob.Address;
                        Label_Assignment_Contractor.Content = matchedJob.ContractorAssigned;
                        try
                        {
                            DatePicker_Assignment_DateStart.SelectedDate = DateTime.Parse(matchedJob.DateStart.ToString());
                            DatePicker_Assignment_DateEnd.SelectedDate = DateTime.Parse(matchedJob.DateEnd.ToString());
                        }
                        catch
                        {
                            MessageBox.Show("This contractor is no longer assigned.\n" +
                                            "Use ADD button to revert contractor record.");
                        }

                        Label_Assignment_Cost.Content = matchedJob.Cost;
                    }
                }
                // If Contractor is unassigned
                else
                {
                    // Load Assignment Form - with unassigned contractor data
                    Label_Assignment_Contractor.Content = contractor.BusinessName;
                    Label_Assignment_Cost.Content = contractor.Rate;
                }
            }

            // If Start and End dates are selected, load to Job form
            if (DatePicker_Assignment_DateStart.SelectedDate != null && DatePicker_Assignment_DateEnd.SelectedDate != null)
            {
                Label_Job_DateStart.Content = DatePicker_Assignment_DateStart.SelectedDate.Value.ToString("dd-MM-yyyy");
                Label_Job_DateEnd.Content = DatePicker_Assignment_DateEnd.SelectedDate.Value.ToString("dd-MM-yyyy");
            }
        }





        // ======================== REPORT BY COST RANGE FORM

        /// <summary>
        /// 1. AutoFillMinMax: Takes the minimum and maximum calculated cost of Assigned
        ///     and Completed Jobs and displays the data in an instruction.
        /// 2. ClearCostRange: Clears the cost range entry fields of the Cost Based Report Form.
        /// 3. MakeReport: Takes the min and max cost user inputs and displays filtered listboxes and
        ///     reports using FilterList function.
        /// </summary>
        /// <param name="List_Jobs"></param>
        /// <param name="ComboBox_Jobs"></param>
        /// <param name="ListBox_Jobs"></param>
        /// <param name="List_Completed"></param>
        /// <param name="TextBlock_Filter_DisplayRange"></param>
        /// <param name="TextBox_Filter_MinCost"></param>
        /// <param name="TextBox_Filter_MaxCost"></param>

        // [1] COST RANGE FILTER - UPDATE TEXTBLOCK
        public void AutoFillMinMax(List<Job> List_Jobs,
                                   List<Completed> List_Completed,
                                   ListBox ListBox_Jobs,
                                   ComboBox ComboBox_Jobs,
                                   TextBlock TextBlock_Filter_DisplayRange)
        {
            // Assigned Jobs
            if (ComboBox_Jobs.SelectedItem.ToString() == "Assigned Jobs" && ListBox_Jobs.Items.Count > 0)
            {
                // Filter out Assigned Jobs from Jobs List
                var assignedJobs = List_Jobs.Where(job => job.Status == "ASSIGNED").ToList();

                // ========== DEBUG ==============
                //Console.WriteLine($"Total assigned jobs: {assignedJobs.Count}");
                // ===============================

                // Loop through relevant list and find min and max cost.
                if (assignedJobs.Any())
                {
                    var minCost = assignedJobs.Min(job => Convert.ToDouble(job.Cost));
                    var maxCost = assignedJobs.Max(job => Convert.ToDouble(job.Cost));

                    // ========== TEST ==============
                    //MessageBox.Show($"Range is between {minCost} and {maxCost}");
                    // ==============================

                    // Auto fill min and max in TextBlock text.
                    TextBlock_Filter_DisplayRange.Text = $"You have selected Assigned Jobs. Set cost range between {minCost} and {maxCost}.";
                }
            }

            // Completed Jobs
            else if (ComboBox_Jobs.SelectedItem.ToString() == "Completed Jobs" && List_Completed.Any())
            {
                // Loop through relevant list and find min and max cost.
                var minCost = List_Completed.Min(completed => completed.Cost);
                var maxCost = List_Completed.Max(completed => completed.Cost);

                // Auto fill min and max in TextBlock text.
                TextBlock_Filter_DisplayRange.Text = $"You have selected Completed Jobs. Set cost range between {minCost} and {maxCost}.";
            }

            // Other Job lists
            else
            {
                // Use default text.
                TextBlock_Filter_DisplayRange.Text = $"Select Assigned Jobs or Completed Jobs list to set cost range filter.";
            }
        }

        // [2] COST RANGE FILTER - CLEAR TEXT
        public void ClearCostRange(TextBox TextBox_Filter_MinCost,
                                   TextBox TextBox_Filter_MaxCost)
        {
            TextBox_Filter_MinCost.Text = "Enter Min Cost";
            TextBox_Filter_MaxCost.Text = "Enter Max Cost";
        }

        // [3] COST RANGE FILETER - MAKE REPORT
        public void MakeReport(List<Job> List_Jobs,
                               ListBox ListBox_Jobs,
                               List<Completed> List_Completed,
                               TextBox TextBox_Filter_MinCost,
                               TextBox TextBox_Filter_MaxCost,
                               ComboBox ComboBox_Jobs)
        {
            // If combobox and cost range has been set
            if (!string.IsNullOrEmpty(TextBox_Filter_MinCost.Text) &&
                !string.IsNullOrEmpty(TextBox_Filter_MaxCost.Text))
            {
                double minCost = 0;
                double maxCost = 0;

                // Validate user input
                try
                {
                    // Reset text color and convert User input and store values
                    TextBox_Filter_MinCost.Foreground = new SolidColorBrush(Colors.Black);
                    TextBox_Filter_MaxCost.Foreground = new SolidColorBrush(Colors.Black);
                    minCost = Convert.ToDouble(TextBox_Filter_MinCost.Text);
                    maxCost = Convert.ToDouble(TextBox_Filter_MaxCost.Text);
                }
                catch
                {
                    // Display error message and highlight invalid textbox
                    TextBox_Filter_MinCost.Foreground = new SolidColorBrush(Colors.Red);
                    TextBox_Filter_MinCost.Text = "Enter only numbers for Min Cost";
                    TextBox_Filter_MaxCost.Foreground = new SolidColorBrush(Colors.Red);
                    TextBox_Filter_MaxCost.Text = "Enter only numbers for Max Cost";
                }

                // Ensure input is in correct order
                if (minCost > maxCost)
                {
                    (minCost, maxCost) = (maxCost, minCost);
                }

                // FIlter Assigned Jobs
                if (ComboBox_Jobs.SelectedItem.ToString() == "Assigned Jobs")
                {
                    try
                    {
                        FilterList(List_Jobs,
                                   ListBox_Jobs,
                                   item => double.Parse(item.Cost) >= minCost && double.Parse(item.Cost) <= maxCost);

                        // Store listbox items in temporary list
                        List<string> filteredList = new List<string>();
                        foreach (var item in ListBox_Jobs.Items)
                        {
                            filteredList.Add(item.ToString());
                        }

                        // Display any times showing in listbox in MessageBox report
                        MessageBox.Show($"REPORT \n\n" +
                                        $"Assigned Jobs between ${minCost} - {maxCost} \n\n" +
                                        $"{string.Join(Environment.NewLine, filteredList)}");
                    }
                    catch
                    {
                        MessageBox.Show("Only Assigned Jobs and Completed Jobs can be filterd by cost");
                    }
                }
                // Filter Completed Jobs
                else if (ComboBox_Jobs.SelectedItem.ToString() == "Completed Jobs")
                {
                    try
                    {
                        FilterList(List_Completed,
                                   ListBox_Jobs,
                                   item => double.Parse(item.Cost) >= minCost && double.Parse(item.Cost) <= maxCost);

                        // Store list in temporary list
                        List<string> filteredList = new List<string>();
                        foreach (var item in ListBox_Jobs.Items)
                        {
                            filteredList.Add(item.ToString());
                        }

                        // Display any times showing in listbox in MessageBox report
                        MessageBox.Show($"REPORT \n\n" +
                                        $"Completed Jobs between ${minCost} - {maxCost} \n\n" +
                                        $"{string.Join(Environment.NewLine, filteredList)}");
                    }
                    catch
                    {
                        MessageBox.Show("Only Assigned Jobs and Completed Jobs can be filterd by cost");
                    }
                }
            }
        }






        // ======================== ASSIGNMENT & COMPLETION FORM

        /// <summary>
        /// 1. CalculateCost: Multiplies number of work days with Contractor rate
        ///     and returns the cost of a job.
        /// 2. AssignJob: Creates Assigned Job record, replaces the Unassigned Job in the Jobs List,
        ///     using DataMatched function, updates the Jobs listbox using the FilterList
        ///     function, then clears the Job Form with the ClearJobForm function.
        /// 3. AssignContractor: Create Assigned Contractor record, replaces the Unassigned
        ///     Contractor in the Contractors list using DataMatched function, updates the
        ///     Contractors listbox using the FilterList function, then clears the Contractor Form
        ///     with the ClearContractorForm function.
        /// 4. ClearAssignment: Clears the Assignment Form.
        /// 5. CompleteJob: Creates Completed Job record, creates new Unassigned Job record to replace the Assigned
        ///     Job in the Jobs List, finds corrresponding Assigned Contractor record using DataMatched function,
        ///     replaces Assigned Contractor with new Unassigned Contractor record, updates the listboxes, then
        ///     clears the Assignment+Complete Form with the ClearAssignmentForm function.
        /// </summary>
        /// <param name="List_Jobs"></param>
        /// <param name="ComboBox_Jobs"></param>
        /// <param name="ListBox_Jobs"></param>
        /// <param name="TextBox_Job_Address"></param>
        /// <param name="Label_Job_DateStart"></param>
        /// <param name="Label_Job_DateEnd"></param>
        /// <param name="Label_Job_ContractorAssigned"></param>
        /// <param name="Label_Job_Cost"></param>
        /// <param name="Label_Assignment_Job"></param>
        /// <param name="DatePicker_Assignment_DateStart"></param>
        /// <param name="DatePicker_Assignment_DateEnd"></param>
        /// <param name="Label_Assignment_Contractor"></param>
        /// <param name="Label_Assignment_Cost"></param>
        /// <param name="List_Contractors"></param>
        /// <param name="ComboBox_Contractors"></param>
        /// <param name="ListBox_Contractors"></param>
        /// <param name="TextBox_Contractor_BusinessName"></param>
        /// <param name="Label_Contractor_DateStart"></param>
        /// <param name="TextBox_Contractor_Rate"></param>
        /// <param name="Label_Contractor_JobAssigned"></param>
        /// <param name="TextBox_Contractor_PhoneEmail"></param>
        /// <param name="List_Completed"></param>

        // [1] CALCULATE COST - CHECK FOR CORRECT DATES AND UPDATE FORMS
        public void CalculateCost(DatePicker DatePicker_Assignment_DateStart,
                                  DatePicker DatePicker_Assignment_DateEnd,
                                  Label Label_Assignment_Cost,
                                  TextBox TextBox_Contractor_Rate)
        {
            if (DatePicker_Assignment_DateStart.SelectedDate == null ||
                DatePicker_Assignment_DateEnd.SelectedDate == null) return;
            {
                // Store date selections
                DateTime dateStart = DatePicker_Assignment_DateStart.SelectedDate.Value;
                DateTime dateEnd = DatePicker_Assignment_DateEnd.SelectedDate.Value;

                // Store contractor rate
                double rate = double.Parse(TextBox_Contractor_Rate.Text);

                // If job is only for one day
                if (dateStart == dateEnd)
                {
                    // Use rate
                    Label_Assignment_Cost.Content = rate.ToString();
                }
                // If start and end dates are in wrong order
                else if (dateStart > dateEnd)
                {
                    // Error message
                    MessageBox.Show("Start date can't be later than end date");
                    // Reset dates and cost
                    DatePicker_Assignment_DateStart.SelectedDate = null;
                    DatePicker_Assignment_DateEnd.SelectedDate = null;
                    Label_Assignment_Cost.Content = "Cost 00.00";
                }
                else
                {
                    // Calculate and add cost to Assignment Form
                    int days = 1 + (dateEnd - dateStart).Days;
                    Label_Assignment_Cost.Content = (days * rate).ToString();
                }
            }
        }

        // [2] ASSIGN CONTRACTOR TO JOB
        public void AssignJob(List<Job> List_Jobs,
                              ListBox ListBox_Jobs,
                              ComboBox ComboBox_Jobs,

                              ListBox ListBox_Contractors,

                              Label Label_Assignment_Job,
                              DatePicker DatePicker_Assignment_DateStart,
                              DatePicker DatePicker_Assignment_DateEnd,
                              Label Label_Assignment_Contractor,
                              Label Label_Assignment_Cost,

                              TextBox TextBox_Job_Address,
                              Label Label_Job_DateStart,
                              Label Label_Job_DateEnd,
                              Label Label_Job_ContractorAssigned,
                              Label Label_Job_Cost)
        {
            // CHECK IF DATES HAVE BEEN SELECTED
            if (DatePicker_Assignment_DateStart.SelectedDate.HasValue && DatePicker_Assignment_DateEnd.SelectedDate.HasValue)
            {
                // CREATE ASSIGNED JOB INSTANCE
                Job assignedJob = new Job
                {
                    // Set status
                    Status = "ASSIGNED",
                    // Use selected values from Assignment Form
                    Address = Label_Assignment_Job.Content.ToString(),
                    DateStart = DatePicker_Assignment_DateStart.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    DateEnd = DatePicker_Assignment_DateEnd.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    ContractorAssigned = Label_Assignment_Contractor.Content.ToString(),
                    Cost = Label_Assignment_Cost.Content.ToString()
                };


                // If a new job instance has been created
                if (assignedJob.Address == Label_Assignment_Job.Content.ToString())
                {
                    // Search for existing record using Address
                    Job matchedJob = DataMatched(assignedJob.Address,
                                                 List_Jobs,
                                                 job => job.Address);

                    // Ensure record is unique in Jobs List by replacing old instance with new
                    List_Jobs.Remove(matchedJob);
                    List_Jobs.Add(assignedJob);

                    // Refresh ListBox
                    ListBox_Jobs.ItemsSource = null;
                    ListBox_Jobs.ItemsSource = List_Jobs;

                    // Reset Job form
                    ClearJobForm(ListBox_Jobs,
                                 ListBox_Contractors,
                                 TextBox_Job_Address,
                                 Label_Job_DateStart,
                                 Label_Job_DateEnd,
                                 Label_Job_ContractorAssigned,
                                 Label_Job_Cost);
                }

                // Change Jobs ComboBox Filter to Assigned Jobs
                ComboBox_Jobs.SelectedIndex = 2;
                FilterList(List_Jobs,
                           ListBox_Jobs,
                           item => item.Status == "ASSIGNED");
            }

            // If dates have not bee selected
            else
            {
                // Clear date pickers
                DatePicker_Assignment_DateStart.SelectedDate = null;
                DatePicker_Assignment_DateEnd.SelectedDate = null;
            }
        }

        // [3] ASSIGN JOB TO CONTRACTOR
        public void AssignContractor(List<Contractor> List_Contractors,
                                     ListBox ListBox_Contractors,
                                     ComboBox ComboBox_Contractors,

                                     ListBox ListBox_Jobs,

                                     Label Label_Assignment_Job,
                                     DatePicker DatePicker_Assignment_DateStart,
                                     DatePicker DatePicker_Assignment_DateEnd,
                                     Label Label_Assignment_Contractor,
                                     Label Label_Assignment_Cost,

                                     TextBox TextBox_Contractor_BusinessName,
                                     Label Label_Contractor_DateStart,
                                     TextBox TextBox_Contractor_Rate,
                                     Label Label_Contractor_JobAssigned,
                                     TextBox TextBox_Contractor_PhoneEmail)
        {
            // CHECK IF DATES HAVE BEEN SELECTED
            if (DatePicker_Assignment_DateStart.SelectedDate.HasValue && DatePicker_Assignment_DateEnd.SelectedDate.HasValue)
            {
                // CREATE NEW CONTRACTOR INSTANCE
                Contractor assignedContractor = new Contractor
                {
                    // Set status
                    Status = "ASSIGNED",
                    // Use selected values from Assignment Form
                    BusinessName = Label_Assignment_Contractor.Content.ToString(),
                    DateStart = DatePicker_Assignment_DateStart.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    Rate = double.Parse(TextBox_Contractor_Rate.Text),
                    JobAssigned = Label_Assignment_Job.Content.ToString(),
                    PhoneEmail = TextBox_Contractor_PhoneEmail.Text
                };


                // If a new contractor instance has been created
                if (assignedContractor.BusinessName == Label_Assignment_Contractor.Content.ToString())
                {
                    // Search for existing record using Business Name
                    Contractor matchedContractor = DataMatched(assignedContractor.BusinessName,
                                                               List_Contractors,
                                                               ontractor => ontractor.BusinessName);

                    // Ensure record is unique in Contractors List by replacing old instance with new
                    List_Contractors.Remove(matchedContractor);
                    List_Contractors.Add(assignedContractor);

                    // Refresh ListBox
                    ListBox_Contractors.ItemsSource = null;
                    ListBox_Contractors.ItemsSource = List_Contractors;

                    // Reset Contractor Form
                    ClearContractorForm(ListBox_Jobs,
                                        ListBox_Contractors,
                                        TextBox_Contractor_BusinessName,
                                        Label_Contractor_DateStart,
                                        TextBox_Contractor_Rate,
                                        Label_Contractor_JobAssigned,
                                        TextBox_Contractor_PhoneEmail);
                }

                // Change Contractors ComboBox Filter to Assigned Jobs
                ComboBox_Contractors.SelectedIndex = 2;
                FilterList(List_Contractors,
                           ListBox_Contractors,
                           item => item.Status == "ASSIGNED");
            }
            // If dates have not been selected
            else
            {
                // Clear date pickers
                DatePicker_Assignment_DateStart.SelectedDate = null;
                DatePicker_Assignment_DateEnd.SelectedDate = null;
                // Display error message
                MessageBox.Show("Please select start and end dates");
            }
        }

        // [4] CLEAR ASSIGNMENT FORM
        public void ClearAssignmentForm(ListBox ListBox_Jobs,
                                        ListBox ListBox_Contractors,
                                        Label Label_Assignment_Job,
                                        Label Label_Assignment_Contractor,
                                        DatePicker DatePicker_Assignment_DateStart,
                                        DatePicker DatePicker_Assignment_DateEnd,
                                        Label Label_Assignment_Cost)
        {
            // Revert Assignment Form to default values
            Label_Assignment_Job.Content = "Job";
            Label_Assignment_Contractor.Content = "Contractor";
            DatePicker_Assignment_DateStart.SelectedDate = null;
            DatePicker_Assignment_DateEnd.SelectedDate = null;
            Label_Assignment_Cost.Content = "Cost 00.00";

            // Selection reset without interrupting Completion()
            ListBox_Jobs.SelectedItem = null;
            ListBox_Contractors.SelectedItem = null;
        }


        // [5] CREATE COMPLETED JOB RECORD AND RETURN CONTRACTOR AND JOB TO UNASSIGNED POOL
        public void CompleteJob(List<Job> List_Jobs,
                                ListBox ListBox_Jobs,
                                ComboBox ComboBox_Jobs,

                                List<Contractor> List_Contractors,
                                ListBox ListBox_Contractors,
                                ComboBox ComboBox_Contractors,

                                List<Completed> List_Completed,

                                Label Label_Assignment_Job,
                                DatePicker DatePicker_Assignment_DateStart,
                                DatePicker DatePicker_Assignment_DateEnd,
                                Label Label_Assignment_Contractor,
                                Label Label_Assignment_Cost)
        {
            // Store selected job
            Job job = ListBox_Jobs.SelectedItem as Job; // Correctly loads job

            // Ensure selected job is assigned
            if (ListBox_Jobs.SelectedItem != null && job.Status == "ASSIGNED" && job.Status != "COMPLETED")
            {
                // Create completed job instance
                Completed completedJob = new Completed
                {
                    // Set status
                    Status = "COMPLETED",
                    // Use loaded values from Assignment Form
                    Address = Label_Assignment_Job.Content.ToString(),
                    DateStart = DatePicker_Assignment_DateStart.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    DateEnd = DatePicker_Assignment_DateEnd.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    ContractorAssigned = Label_Assignment_Contractor.Content.ToString(),
                    Cost = Label_Assignment_Cost.Content.ToString()
                };

                // JOB
                // Instantiate New Completed Job record and update the List and ListBox
                Completed newCompleted = new Completed();
                List_Completed.Add(completedJob);
                ListBox_Jobs.ItemsSource = List_Completed;

                // Replace Assigned Job instance with newly created Unassigned Job instance
                Job unassignedJob = new Job();
                unassignedJob.Address = completedJob.Address;
                List_Jobs.Add(unassignedJob);
                List_Jobs.Remove(job);

                // Refresh and update Jobs ListBox
                ListBox_Jobs.ItemsSource = null;
                ListBox_Jobs.ItemsSource = List_Jobs;


                // CONTRACTOR
                // Find corresponding contractor instance in Contractors List using ContractorAssigned
                Contractor matchedContractor = DataMatched(job.ContractorAssigned,
                                                           List_Contractors,
                                                           contractor => contractor.BusinessName);

                // If there is an old record found
                if (matchedContractor != null)
                {
                    // Create new unassigned record
                    Contractor unassignedContractor = new Contractor();
                    unassignedContractor.Status = null;
                    unassignedContractor.BusinessName = matchedContractor.BusinessName;
                    unassignedContractor.DateStart = null;
                    unassignedContractor.Rate = matchedContractor.Rate;
                    unassignedContractor.JobAssigned = null;
                    unassignedContractor.PhoneEmail = matchedContractor.PhoneEmail;

                    // Replace assigned contractor with unassigned contractor instance
                    List_Contractors.Add(unassignedContractor);
                    List_Contractors.Remove(matchedContractor);

                    // Refresh and update ListBox with Contractors List
                    ListBox_Contractors.ItemsSource = null;
                    ListBox_Contractors.ItemsSource = List_Contractors;
                }
            }
            else
            {
                MessageBox.Show("Only Assigned Jobs can be completed.");
            }

            // Update ComboBoxes for Jobs and Contractors
            ComboBox_Jobs.SelectedIndex = 3; // Completed Jobs
            ComboBox_Contractors.SelectedIndex = 1; // Unassgined Contractors

            // Reset Assignment From
            ClearAssignmentForm(ListBox_Jobs,
                            ListBox_Contractors,
                            Label_Assignment_Job,
                            Label_Assignment_Contractor,
                            DatePicker_Assignment_DateStart,
                            DatePicker_Assignment_DateEnd,
                            Label_Assignment_Cost);
        }






        // ======================== JOB FORM

        /// <summary>
        /// 1. AddJob: Creates a Job record, ensures the record is unique by using DataMatched function and
        ///     replacing the old record with the duplicate, updates the listbox display then clears the Job form
        ///     using ClearJobForm function.
        /// 2. DeleteJob: Takes the listbox selection, removes the record from Jobs List, then refreshes the Jobs ListBox.
        /// 3. ClearJobForm: Loads default text to Job Form.
        /// </summary>
        /// <param name="TextBox_Job_Address"></param>
        /// <param name="Label_Job_DateStart"></param>
        /// <param name="Label_Job_DateEnd"></param>
        /// <param name="Label_Job_ContractorAssigned"></param>
        /// <param name="Label_Job_Cost"></param>
        /// <param name="List_Jobs"></param>
        /// <param name="ListBox_Jobs"></param>

        // [1] ADD JOB
        public void AddJob(TextBox TextBox_Job_Address,
                            Label Label_Job_DateStart,
                            Label Label_Job_DateEnd,
                            Label Label_Job_ContractorAssigned,
                            Label Label_Job_Cost,
                            List<Job> List_Jobs,
                            ListBox ListBox_Jobs,
                            ListBox ListBox_Contractors)
        {
            // Create new job instance
            Job newJob = new Job
            {
                Status = null,
                Address = TextBox_Job_Address.Text, // Take user input
                DateStart = "No start date booked",
                DateEnd = "No end date booked",
                ContractorAssigned = "No work required",
                Cost = "Cost calculated on assignment"
            };

            if (newJob.Address == TextBox_Job_Address.Text)
            {
                // Search for existing record using Address
                Job matchedJob = DataMatched(newJob.Address,
                                             List_Jobs,
                                             job => job.Address);

                // Ensure record is unique in Jobs List by replacing old instance with new
                List_Jobs.Remove(matchedJob);
                List_Jobs.Add(newJob);

                // Refresh ListBox
                ListBox_Jobs.ItemsSource = null;
                ListBox_Jobs.ItemsSource = List_Jobs;

                // Reset Job form
                ClearJobForm(ListBox_Jobs,
                             ListBox_Contractors,
                             TextBox_Job_Address,
                             Label_Job_DateStart,
                             Label_Job_DateEnd,
                             Label_Job_ContractorAssigned,
                             Label_Job_Cost);
            }
        }

        // [2] DELETE JOB
        public void DeleteJob(Job selectedItem,
                              TextBox TextBox_Job_Address,
                              Label Label_Job_DateStart,
                              Label Label_Job_DateEnd,
                              Label Label_Job_Cost,
                              Label Label_Job_ContractorAssigned,
                              List<Job> List_Jobs,
                              ListBox ListBox_Jobs)
        {
            // If a job is selected
            if (selectedItem != null)
            {
                // Incase deletion not intentional
                // Display job to be deleted in the text fields
                TextBox_Job_Address.Text = $"{selectedItem.Address}";
                Label_Job_DateStart.Content = $"{selectedItem.DateStart}";
                Label_Job_DateEnd.Content = $"{selectedItem.DateEnd}";
                Label_Job_Cost.Content = $"{selectedItem.Cost}";
                Label_Job_ContractorAssigned.Content = $"{selectedItem.ContractorAssigned}";

                // Delete selected job from Jobs List
                List_Jobs.Remove(selectedItem);

                // Refresh Jobs ListBox
                ListBox_Jobs.ItemsSource = null;
                // Add the list to Jobs ListBox
                ListBox_Jobs.ItemsSource = List_Jobs;
            }
        }

        // [3] CLEAR JOB FORM
        public void ClearJobForm(ListBox ListBox_Jobs,
                                 ListBox ListBox_Contractors,
                                 TextBox TextBox_Job_Address,
                                 Label Label_Job_DateStart,
                                 Label Label_Job_DateEnd,
                                 Label Label_Job_ContractorAssigned,
                                 Label Label_Job_Cost)
        {
            // Revert Job Form to default text
            TextBox_Job_Address.Text = "Enter job address";
            Label_Job_DateStart.Content = "No start date booked";
            Label_Job_DateEnd.Content = "No end date booked";
            Label_Job_ContractorAssigned.Content = "No work required";
            Label_Job_Cost.Content = "Cost calculated on assignment";

            // Prevents interrupting Completion() during selection reset
            ListBox_Jobs.SelectedItem = null;
            ListBox_Contractors.SelectedItem = null;
        }





        // ======================== CONTRACTOR FORM

        /// <summary>
        /// 1. AddContractor: Creates a Contractor record, ensures the record is unique by using DataMatched function and
        ///     replacing the old record with the duplicate, updates the listbox display then clears the Contractor form
        ///     using ClearContractorForm function.
        /// 2. DeleteContractor: Takes the listbox selection, removes the record from Contractors List, then refreshes the
        ///     Contractors ListBox.
        /// 3. ClearContractorForm: Loads default text to Contractor Form.
        /// </summary>
        /// <param name="TextBox_Contractor_BusinessName"></param>
        /// <param name="Label_Contractor_DateStart"></param>
        /// <param name="TextBox_Contractor_Rate"></param>
        /// <param name="Label_Contractor_JobAssigned"></param>
        /// <param name="TextBox_Contractor_PhoneEmail"></param>
        /// <param name="List_Contractors"></param>
        /// <param name="ListBox_Contractors"></param>

        // [1] ADD CONTRACTOR
        public void AddContractor(TextBox TextBox_Contractor_BusinessName,
                                  Label Label_Contractor_DateStart,
                                  TextBox TextBox_Contractor_Rate,
                                  Label Label_Contractor_JobAssigned,
                                  TextBox TextBox_Contractor_PhoneEmail,
                                  List<Contractor> List_Contractors,
                                  ListBox ListBox_Contractors,
                                  ListBox ListBox_Jobs)
        {
            // Create Contractor instance
            Contractor newContractor = new Contractor
            {
                // Set status
                Status = null,
                // Take user input
                BusinessName = TextBox_Contractor_BusinessName.Text,
                PhoneEmail = TextBox_Contractor_PhoneEmail.Text,
                // Use default values from Contractor Form
                DateStart = "No start date booked",
                JobAssigned = "Job to be assigned"
            };

            // If user input for rate is invalid
            if (!Double.TryParse(TextBox_Contractor_Rate.Text, out double rate))
            {
                // Display error message
                TextBox_Contractor_Rate.Foreground = new SolidColorBrush(Colors.Red);
                TextBox_Contractor_Rate.Text = "Enter only numbers for rate";
                return;
            }
            else
            {
                // Add rate to new instance
                TextBox_Contractor_Rate.Foreground = new SolidColorBrush(Colors.Black);
                newContractor.Rate = rate;
            }

            // If a new contractor instance has been created
            if (newContractor.BusinessName == TextBox_Contractor_BusinessName.Text)
            {
                // Search for existing record using Business Name
                Contractor matchedContractor = DataMatched(newContractor.BusinessName,
                                             List_Contractors,
                                             ontractor => ontractor.BusinessName);

                // Ensure record is unique in Contractors List by replacing old instance with new
                List_Contractors.Remove(matchedContractor);
                List_Contractors.Add(newContractor);

                // Refresh ListBox
                ListBox_Contractors.ItemsSource = null;
                ListBox_Contractors.ItemsSource = List_Contractors;

                // Reset Contractor Form
                ClearContractorForm(ListBox_Jobs,
                                    ListBox_Contractors,
                                    TextBox_Contractor_BusinessName,
                                    Label_Contractor_DateStart,
                                    TextBox_Contractor_Rate,
                                    Label_Contractor_JobAssigned,
                                    TextBox_Contractor_PhoneEmail);
            }
        }

        // [2] DELETE CONTRACTOR
        public void DeleteContractor(Contractor selectedItem,
                                    TextBox TextBox_Contractor_BusinessName,
                                    Label Label_Contractor_DateStart,
                                    TextBox TextBox_Contractor_Rate,
                                    Label Label_Contractor_JobAssigned,
                                    TextBox TextBox_Contractor_PhoneEmail,
                                    List<Contractor> List_Contractors,
                                    ListBox ListBox_Contractors)
        {
            // If selected contractor is not null
            if (selectedItem.BusinessName != null)
            {
                // Incase deletion not intentional
                // Display contractor to be deleted in the text box
                TextBox_Contractor_BusinessName.Text = $"{selectedItem.BusinessName}";
                Label_Contractor_DateStart.Content = $"{selectedItem.DateStart}";
                TextBox_Contractor_Rate.Text = $"{selectedItem.Rate}";
                Label_Contractor_JobAssigned.Content = $"{selectedItem.JobAssigned}";
                TextBox_Contractor_PhoneEmail.Text = $"{selectedItem.PhoneEmail}";

                // Delete selected contractor from Contractors List
                List_Contractors.Remove(selectedItem);

                // Refresh Contractors ListBox
                ListBox_Contractors.ItemsSource = null;
                // Add the record to Contractors ListBox
                ListBox_Contractors.ItemsSource = List_Contractors;
            }
        }

        // [3] CLEAR CONTRACTOR FORM
        public void ClearContractorForm(ListBox ListBox_Jobs,
                                        ListBox ListBox_Contactors,
                                        TextBox TextBox_Contractor_BusinessName,
                                        Label Label_Contractor_DateStart,
                                        TextBox TextBox_Contractor_Rate,
                                        Label Label_Contractor_JobAssigned,
                                        TextBox TextBox_Contractor_PhoneEmail)
        {
            // Revert Contractor Form to default text
            TextBox_Contractor_BusinessName.Text = "Business Name";
            Label_Contractor_DateStart.Content = "No start date booked";
            TextBox_Contractor_Rate.Text = "Rate 00.00";
            Label_Contractor_JobAssigned.Content = "Job to be assigned";
            TextBox_Contractor_PhoneEmail.Text = "Phone/Email";

            // Selection reset without interrupting Completion()
            ListBox_Jobs.SelectedItem = null;
            ListBox_Contactors.SelectedItem = null;
        }
    }
}

