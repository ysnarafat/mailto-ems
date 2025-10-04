namespace EmailMarketing.ExcelWorkerService.Templates
{
    public partial class FileUploadConfirmationEmailTemplate
    {
        public string ReceiverName { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyUrl { get; set; }
        public int SuccessCount { get; set; }
        public int ExistsCount { get; set; }
        public int InvalidCount { get; set; }

        public FileUploadConfirmationEmailTemplate(string name, int successCount, int existsCount, int invalidCount,
            string companyFullName, string companyShortName, string companyUrl)
        {
            this.ReceiverName = name;
            this.CompanyFullName = companyFullName;
            this.CompanyShortName = companyShortName;
            this.CompanyUrl = companyUrl;
            this.SuccessCount = successCount;
            this.ExistsCount = existsCount;
            this.InvalidCount = invalidCount;
        }
    }
}
