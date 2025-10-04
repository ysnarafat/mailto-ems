namespace EmailMarketing.ExcelExportWorkerService.Templates
{
    public partial class ExcelExportConfirmationTemplate
    {
        public string ReceiverName { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyUrl { get; set; }

        public ExcelExportConfirmationTemplate(string name,
            string companyFullName, string companyShortName, string companyUrl)
        {
            this.ReceiverName = name;
            this.CompanyFullName = companyFullName;
            this.CompanyShortName = companyShortName;
            this.CompanyUrl = companyUrl;
        }
    }
}
