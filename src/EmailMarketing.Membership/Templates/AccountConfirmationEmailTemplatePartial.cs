namespace EmailMarketing.Membership.Templates
{
    public partial class AccountConfirmationEmailTemplate
    {
        public string ReceiverName { get; private set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyUrl { get; set; }
        public string Url { get; private set; }

        public AccountConfirmationEmailTemplate(string name, string url,
            string companyFullName, string companyShortName, string companyUrl)
        {
            this.ReceiverName = name;
            this.CompanyFullName = companyFullName;
            this.CompanyShortName = companyShortName;
            this.CompanyUrl = companyUrl;
            this.Url = url;
        }
    }
}
