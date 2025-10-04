namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ContactValueTextModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public int Count { get; set; }
        public string Input { get; set; }
        public bool IsChecked { get; set; }
        //public bool IsStandard { get; set; }
    }
}