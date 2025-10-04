namespace EmailMarketing.EmailSendingWorkerService.Templates
{
    public partial class DemoEmailTemplate
    {
        public string ReceiverName { get; private set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyUrl { get; set; }
        public int TotalCount { get; private set; }
        public int TotalFailed { get; private set; }

        public DemoEmailTemplate(string name, int totalCount, int totalFailed,
            string companyFullName, string companyShortName, string companyUrl)
        {
            this.ReceiverName = name;
            this.CompanyFullName = companyFullName;
            this.CompanyShortName = companyShortName;
            this.CompanyUrl = companyUrl;
            this.TotalCount = totalCount;
            this.TotalFailed = totalFailed;
        }
    }
}
