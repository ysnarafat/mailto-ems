namespace EmailMarketing.CampaingReportExcelExportService.Templates
{
    public partial class CampReportExportTemplate
    {
        public string ReceiverName { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyUrl { get; set; }

        public CampReportExportTemplate(string name, string companyFullName, string companyShortName, string companyUrl)
        {
            this.ReceiverName = name;
            this.CompanyFullName = companyFullName;
            this.CompanyShortName = companyShortName;
            this.CompanyUrl = companyUrl;
        }
    }
}
