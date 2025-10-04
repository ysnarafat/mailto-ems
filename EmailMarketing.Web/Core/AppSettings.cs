namespace EmailMarketing.Web.Core
{
    public class AppSettings
    {
        public string UserDefaultPassword { get; set; }
        public string TokenSecretKey { get; set; }
        public int TokenExpiresHours { get; set; }
        public string EncryptionDecryptionKey { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyWebsiteUrl { get; set; }
        public string CompanyPhoneNo { get; set; }
        public string CompanyEmail { get; set; }
        public string DeveloperCompanyFullName { get; set; }
        public string DeveloperCompanyShortName { get; set; }
        public string DeveloperCompanyWebsiteUrl { get; set; }
        public string CampaignExportFileUrl { get; set; }
        public string EmailSenderFileUrl { get; set; }
        public string ContactExportFileUrl { get; set; }
        public string ContactImportFileUrl { get; set; }
    }
}
