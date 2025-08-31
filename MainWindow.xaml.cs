using System.Windows;
using System.Windows.Controls;

namespace BookMyTradie
{

    public partial class MainWindow : Window
    {
        readonly RecruitmentSystem recruitmentSystem = new ();

        // ======================== INITIALIZE MAIN WINDOW
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            // Create instance of window
            InitializeComponent();

            // Jobs - combobox filters
            ComboBox_Jobs.SelectedIndex = 0; // Jobs ComboBox initial setting
            ComboBox_Jobs.Items.Add("All Jobs"); // 0
            ComboBox_Jobs.Items.Add("Unassigned Jobs"); // 1
            ComboBox_Jobs.Items.Add("Assigned Jobs"); // 2
            ComboBox_Jobs.Items.Add("Completed Jobs"); // 3

            // Contractors - combobox filters
            ComboBox_Contractors.SelectedIndex = 0; // Contractors ComboBox initial setting
            ComboBox_Contractors.Items.Add("All Contractors"); // 0
            ComboBox_Contractors.Items.Add("Unassigned Contractors"); // 1
            ComboBox_Contractors.Items.Add("Assigned Contractors"); // 2

            // Data source for ListBoxes
            ListBox_Jobs.ItemsSource = recruitmentSystem.List_Jobs;
            ListBox_Jobs.ItemsSource = recruitmentSystem.List_Completed;
            ListBox_Contractors.ItemsSource = recruitmentSystem.List_Contractors;
        }




        // ======================== ASSIGNMENT & COMPLETION FORMS
        /// <summary>
        /// The following are event handlers for Assignment and Completion features. Handles
        /// selecting start and end dates, and button clicks for assignment and completion. 
        /// Dates are used for calculating job cost using rate and date range.
        /// Event Handlers:
        /// - DatePicker_Assignment_DateStart_SelectionChanged: Runs CalculateCost function.
        /// - DatePicker_Assignment_DateEnd_SelectionChanged: Runs CalculateCost function.
        /// - Button_Assignment_Assign: Runs AssignJob and AssignContractor functions.
        /// - Button_Assignment_Complete: Runs CompleteJob and FilterList functions.
        /// - Button_Assignment_Clear: Runs ClearAssignment function.
        /// </summary>

        // [A] START DATE DATEPICKER SELECTION
        private void DatePicker_Assignment_DateStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If a Start Date is selected in the datepicker
            if (DatePicker_Assignment_DateStart.SelectedDate != null)
            {
                try
                {
                    BookMyTradie.RecruitmentSystem.CalculateCost(DatePicker_Assignment_DateStart,
                                                    DatePicker_Assignment_DateEnd,
                                                    Label_Assignment_Cost,
                                                    TextBox_Contractor_Rate);
                }
                catch
                {
                    MessageBox.Show("Select a job and contractor from the lists.");
                }
            }
        }

        // [B] END DATE DATEPICKER SELECTION
        private void DatePicker_Assignment_DateEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If an End Date is selected in the datepicker
            if (DatePicker_Assignment_DateEnd.SelectedDate != null)
            {
                try
                {
                    BookMyTradie.RecruitmentSystem.CalculateCost(DatePicker_Assignment_DateStart,
                                                    DatePicker_Assignment_DateEnd,
                                                    Label_Assignment_Cost,
                                                    TextBox_Contractor_Rate);
                }
                catch
                {
                    MessageBox.Show("Select a job and contractor from the lists.");
                }
            }
        }

        // [C] BUTTON ASSIGN
        private void Button_Assignment_Assign(object sender, RoutedEventArgs e)
        {
            try
            {
                recruitmentSystem.AssignJob(recruitmentSystem.List_Jobs,
                                            ListBox_Jobs,
                                            ComboBox_Jobs,

                                            ListBox_Contractors,

                                            Label_Assignment_Job,
                                            DatePicker_Assignment_DateStart,
                                            DatePicker_Assignment_DateEnd,
                                            Label_Assignment_Contractor,
                                            Label_Assignment_Cost,

                                            TextBox_Job_Address,
                                            Label_Job_DateStart,
                                            Label_Job_DateEnd,
                                            Label_Job_ContractorAssigned,
                                            Label_Job_Cost);

                recruitmentSystem.AssignContractor(recruitmentSystem.List_Contractors,
                                                   ListBox_Contractors,
                                                   ComboBox_Contractors,

                                                   ListBox_Jobs,

                                                   Label_Assignment_Job,
                                                   DatePicker_Assignment_DateStart,
                                                   DatePicker_Assignment_DateEnd,
                                                   Label_Assignment_Contractor,
                                                   Label_Assignment_Cost,

                                                   TextBox_Contractor_BusinessName,
                                                   Label_Contractor_DateStart,
                                                   TextBox_Contractor_Rate,
                                                   Label_Contractor_JobAssigned,
                                                   TextBox_Contractor_PhoneEmail);
            }
            catch
            {
                MessageBox.Show("Select Property, Contractor and Dates");
            }
        }

        // [D] BUTTON COMPLETED
        private void Button_Assignment_Complete(object sender, RoutedEventArgs e)
        {
            try
            {
                BookMyTradie.RecruitmentSystem.CompleteJob(recruitmentSystem.List_Jobs,
                                              ListBox_Jobs,
                                              ComboBox_Jobs,

                                              recruitmentSystem.List_Contractors,
                                              ListBox_Contractors,
                                              ComboBox_Contractors,

                                              recruitmentSystem.List_Completed,

                                              Label_Assignment_Job,
                                              DatePicker_Assignment_DateStart,
                                              DatePicker_Assignment_DateEnd,
                                              Label_Assignment_Contractor,
                                              Label_Assignment_Cost);
            }
            catch
            {
                MessageBox.Show("Select an assigned Job");
            }

            // Change Jobs ComboBox Filter to Completed Jobs
            ComboBox_Jobs.SelectedIndex = 3;

            if (ComboBox_Jobs.SelectedIndex == 3)
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Completed,
                                             ListBox_Jobs,
                                             item => item.Status == "COMPLETED");
            }

            // Change Contractors ComboBox Filter to Unassigned Contractors
            ComboBox_Contractors.SelectedIndex = 1;

            if (ComboBox_Contractors.SelectedIndex == 1)
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Contractors,
                                             ListBox_Contractors,
                                             item => item.Status == null);
            }
        }

        // [D] BUTTON CLEAR
        private void Button_Assignment_Clear(object sender, RoutedEventArgs e)
        {
            BookMyTradie.RecruitmentSystem.ClearAssignmentForm(ListBox_Jobs,
                                                  ListBox_Contractors,
                                                  Label_Assignment_Job,
                                                  Label_Assignment_Contractor,
                                                  DatePicker_Assignment_DateEnd,
                                                  DatePicker_Assignment_DateStart,
                                                  Label_Assignment_Cost);
        }




        // ======================== COST RANGE FILTER FROM
        /// <summary>
        /// The following are event handlers for the cost range filtering feature. Handles button clicks
        /// for the make report and clear buttons. Used for filtering assigned or completed job 
        /// records based on cost range and clearing filter range input fields.
        /// Event Handlers:
        /// - Button_Filter_Report: Runs MakeReport function.
        /// - Button_Filter_ClearCostRange: Runs ClearCostRange function.
        /// </summary>

        // BUTTON MAKE REPORT
        private void Button_Filter_Report(object sender, RoutedEventArgs e)
        {
            recruitmentSystem.MakeReport(recruitmentSystem.List_Jobs,
                                         ListBox_Jobs,
                                         recruitmentSystem.List_Completed,
                                         TextBox_Filter_MinCost,
                                         TextBox_Filter_MaxCost,
                                         ComboBox_Jobs);
        }

        // BUTTON CLEAR COST FIITER
        private void Button_Filter_ClearCostRange(object sender, RoutedEventArgs e)
        {
            BookMyTradie.RecruitmentSystem.ClearCostRange(TextBox_Filter_MinCost,
                                             TextBox_Filter_MaxCost);
        }




        // ========================  JOB FORM & LIST VIEW
        /// <summary>
        /// The following are event handlers for the job form feature. Handles button clicks
        /// for adding and deleting records, clearing text fields, updating and filtering listboxes for jobs.
        /// Event Handlers:
        /// - Button_Job_Add: Runs AddJob and FilterList functions.
        /// - Button_Job_Delete: Runs DeleteJob function.
        /// - Button_Job_Clear: Runs ClearJobForm function.
        /// - ComboBox_Jobs_SelectionChanged: Runs ShowAll, FilterList and AutoFill functions.
        /// - ListBox_Jobs_SelectionChanged: Runs JobSelected function.
        /// </summary>

        // [A] BUTTON ADD JOB
        private void Button_Job_Add(object sender, RoutedEventArgs e)
        {
            // If a new job instance has been created
            if (TextBox_Job_Address.Text != "Enter job address")
            {
                BookMyTradie.RecruitmentSystem.AddJob(TextBox_Job_Address,
                                                      Label_Job_DateStart,
                                                      Label_Job_DateEnd,
                                                      Label_Job_ContractorAssigned,
                                                      Label_Job_Cost,
                                                      recruitmentSystem.List_Jobs,
                                                      ListBox_Jobs,
                                                      ListBox_Contractors);

                // Change Jobs ComboBox Filter to Unassigned Jobs
                ComboBox_Jobs.SelectedIndex = 1;

                if (ComboBox_Jobs.SelectedIndex == 1)
                {
                    BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Jobs,
                                                              ListBox_Jobs,
                                                              item => item.Status == null);
                }
            }
        }

        // [B] BUTTON DELETE JOB
        private void Button_Job_Delete(object sender, RoutedEventArgs e)
        {
            // DELETE FROM LISTVIEWER
            // If a job to be deleted is selected in the listviewer
            if (ListBox_Jobs.SelectedItem != null)
            {
                try
                {
                    // Store selected job in a field of type Job
                    Job selectedItem = (Job)ListBox_Jobs.SelectedItem;

                    // Call Delete Function [1]
                    BookMyTradie.RecruitmentSystem.DeleteJob(selectedItem,
                                                TextBox_Job_Address,
                                                Label_Job_DateStart,
                                                Label_Job_DateEnd,
                                                Label_Job_Cost,
                                                Label_Job_ContractorAssigned,
                                                recruitmentSystem.List_Jobs,
                                                ListBox_Jobs);
                }
                catch
                {
                    MessageBox.Show("Listviewer selection is invalid!");
                }
            }
        }

        // [C] BUTTON CLEAR JOB
        private void Button_Job_Clear(object sender, RoutedEventArgs e)
        {
            BookMyTradie.RecruitmentSystem.ClearJobForm(ListBox_Jobs,
                                           ListBox_Contractors,
                                           TextBox_Job_Address,
                                           Label_Job_DateStart,
                                           Label_Job_DateEnd,
                                           Label_Job_ContractorAssigned,
                                           Label_Job_Cost);
        }

        // [D] COMBOBOX FILTER JOBS
        private void ComboBox_Jobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No Filter - Show All
            if (ComboBox_Jobs.SelectedItem.ToString() == "All Jobs")
            {
                BookMyTradie.RecruitmentSystem.ShowAll(recruitmentSystem.List_Jobs,
                                                       ListBox_Jobs);

                BookMyTradie.RecruitmentSystem.AutoFillMinMax(recruitmentSystem.List_Jobs,
                                                              recruitmentSystem.List_Completed,
                                                              ListBox_Jobs,
                                                              ComboBox_Jobs,
                                                              TextBlock_Filter_DisplayRange);
            }

            // Filter Unassigned Jobs
            if (ComboBox_Jobs.SelectedItem.ToString() == "Unassigned Jobs")
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Jobs,
                                                          ListBox_Jobs,
                                                          item => item.Status == null);

                BookMyTradie.RecruitmentSystem.AutoFillMinMax(recruitmentSystem.List_Jobs,
                                                              recruitmentSystem.List_Completed,
                                                              ListBox_Jobs,
                                                              ComboBox_Jobs,
                                                              TextBlock_Filter_DisplayRange);
            }

            // Filter Assigned Jobs
            if (ComboBox_Jobs.SelectedItem.ToString() == "Assigned Jobs")
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Jobs,
                                                          ListBox_Jobs,
                                                          item => item.Status == "ASSIGNED");

                BookMyTradie.RecruitmentSystem.AutoFillMinMax(recruitmentSystem.List_Jobs,
                                                              recruitmentSystem.List_Completed,
                                                              ListBox_Jobs,
                                                              ComboBox_Jobs,
                                                              TextBlock_Filter_DisplayRange);
            }

            // Filter Completed Jobs
            if (ComboBox_Jobs.SelectedItem.ToString() == "Completed Jobs")
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Completed,
                                                          ListBox_Jobs,
                                                          item => item.Status == "COMPLETED");

                BookMyTradie.RecruitmentSystem.AutoFillMinMax(recruitmentSystem.List_Jobs,
                                                              recruitmentSystem.List_Completed,
                                                              ListBox_Jobs,
                                                              ComboBox_Jobs,
                                                              TextBlock_Filter_DisplayRange);
            }
        }

        // [E] LISTBOX SELECT JOB
        private void ListBox_Jobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Fill Job, Contractor and Assignment Forms - Function [3]
            BookMyTradie.RecruitmentSystem.JobSelected(ListBox_Jobs,
                                          recruitmentSystem.List_Contractors,

                                          Label_Assignment_Job,
                                          Label_Assignment_Contractor,
                                          DatePicker_Assignment_DateStart,
                                          DatePicker_Assignment_DateEnd,
                                          Label_Assignment_Cost,

                                          TextBox_Job_Address,
                                          Label_Job_DateStart,
                                          Label_Job_DateEnd,
                                          Label_Job_ContractorAssigned,
                                          Label_Job_Cost,

                                          TextBox_Contractor_BusinessName,
                                          Label_Contractor_DateStart,
                                          TextBox_Contractor_Rate,
                                          Label_Contractor_JobAssigned,
                                          TextBox_Contractor_PhoneEmail);
        }




        // ======================== CONTRACTOR FORM & LIST VIEW
        /// <summary>
        /// The following are event handlers for the contractor form feature. Handles button clicks for 
        /// adding and deleting records, clearing text fields, updating and filtering listboxes for contractors.
        /// Event Handlers:
        /// - Button_Contractor_Add: Runs DataMatched, AddContractor and FilterList functions.
        /// - Button_Contractor_Delete: Runs DeleteContractor function.
        /// - Button_Contractor_Clear: Runs ClearContractorForm function.
        /// - ComboBox_Contractors_SelectionChanged: Runs ShowAll, FilterList functions.
        /// - ListBox_Contractors_SelectionChanged: Runs ContractorSelected function.
        /// </summary>

        // [A] BUTTON ADD CONTRACTOR
        private void Button_Contractor_Add(object sender, RoutedEventArgs e)
        {
            int firstCount = recruitmentSystem.List_Contractors.Count;

            // Check if the Contractor's BusinessName in the TextBox is already in the List
            Contractor matchedContractor = BookMyTradie.RecruitmentSystem.DataMatched(TextBox_Contractor_BusinessName.Text,
                                                                                      recruitmentSystem.List_Contractors,
                                                                                      contractor => contractor?.BusinessName!);

            recruitmentSystem.AddContractor(TextBox_Contractor_BusinessName,
                                            Label_Contractor_DateStart,
                                            TextBox_Contractor_Rate,
                                            Label_Contractor_JobAssigned,
                                            TextBox_Contractor_PhoneEmail,
                                            recruitmentSystem.List_Contractors,
                                            ListBox_Contractors,
                                            ListBox_Jobs);

            int secondCount = recruitmentSystem.List_Contractors.Count;

            if (secondCount > firstCount)
            {
                // Change Contractors ComboBox Filter to 1 - Unassigned Contractors
                ComboBox_Contractors.SelectedIndex = 1;

                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Contractors,
                                                          ListBox_Contractors,
                                                          item => item.Status == null);
            }
            else if (matchedContractor != null)
            {
                ComboBox_Contractors.SelectedIndex = 1;
            }
        }

        // [B] BUTTON DELETE CONTRACTOR
        private void Button_Contractor_Delete(object sender, RoutedEventArgs e)
        {
            // DELETE FROM LISTVIEWER
            // If a contractor is selected
            if (ListBox_Contractors.SelectedItem != null)
            {
                // Store selected contractor
                Contractor selectedItem = (Contractor)ListBox_Contractors.SelectedItem;

                try
                {
                    // Call Delete Function [1]
                    BookMyTradie.RecruitmentSystem.DeleteContractor(selectedItem,
                                                       TextBox_Contractor_BusinessName,
                                                       Label_Contractor_DateStart,
                                                       TextBox_Contractor_Rate,
                                                       Label_Contractor_JobAssigned,
                                                       TextBox_Contractor_PhoneEmail,
                                                       recruitmentSystem.List_Contractors,
                                                       ListBox_Contractors);
                }
                catch
                {
                    MessageBox.Show("Listviewer selection is invalid!");
                }
            }
        }

        // [C] BUTTON CLEAR JOB
        private void Button_Contractor_Clear(object sender, RoutedEventArgs e)
        {
            BookMyTradie.RecruitmentSystem.ClearContractorForm(ListBox_Jobs,
                                                  ListBox_Contractors,
                                                  TextBox_Contractor_BusinessName,
                                                  Label_Contractor_DateStart,
                                                  TextBox_Contractor_Rate,
                                                  Label_Contractor_JobAssigned,
                                                  TextBox_Contractor_PhoneEmail);
        }

        // [D] COMBOBOX FILTER CONTRACTORS
        private void ComboBox_Contractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No Filter - Show All
            if (ComboBox_Contractors.SelectedItem.ToString() == "All Contractors")
            {
                BookMyTradie.RecruitmentSystem.ShowAll(recruitmentSystem.List_Contractors,
                                          ListBox_Contractors);
            }

            // Filter Unassigned Jobs
            if (ComboBox_Contractors.SelectedItem.ToString() == "Unassigned Contractors")
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Contractors,
                                             ListBox_Contractors,
                                             item => item.Status == null);
            }

            // Filter Assigned Jobs
            if (ComboBox_Contractors.SelectedItem.ToString() == "Assigned Contractors")
            {
                BookMyTradie.RecruitmentSystem.FilterList(recruitmentSystem.List_Contractors,
                                             ListBox_Contractors,
                                             item => item.Status == "ASSIGNED");
            }
        }

        // [E] LISTVIEW SELECT CONTRACTOR
        private void ListBox_Contractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If a Contractor is selected in the listviewer
            if (ListBox_Contractors.SelectedItem != null)
            {
                // Fill Job, Contractor and Assignment Forms - Function [3]
                recruitmentSystem.ContractorSelected(ListBox_Contractors,
                                                     recruitmentSystem.List_Jobs,

                                                     TextBox_Contractor_BusinessName,
                                                     Label_Contractor_DateStart,
                                                     TextBox_Contractor_Rate,
                                                     Label_Contractor_JobAssigned,
                                                     TextBox_Contractor_PhoneEmail,

                                                      Label_Assignment_Job,
                                                     Label_Assignment_Contractor,
                                                     DatePicker_Assignment_DateStart,
                                                     DatePicker_Assignment_DateEnd,
                                                     Label_Assignment_Cost,

                                                     TextBox_Job_Address,
                                                     Label_Job_DateStart,
                                                     Label_Job_DateEnd,
                                                     Label_Job_ContractorAssigned,
                                                     Label_Job_Cost);
            }
        }
    }
}
