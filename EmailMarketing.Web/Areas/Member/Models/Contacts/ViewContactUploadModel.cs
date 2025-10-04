using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Contacts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ViewContactUploadModel : ContactsBaseModel
    {
        public int Id { get; set; }
        public int ContactUploadId { set; get; }
        public int UserId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public bool IsSucceed { get; set; }
        public bool IsUpdateExisting { get; set; }
        public bool HasColumnHeader { get; set; }
        public bool IsSendEmailNotify { get; set; }
        public string SendEmailAddress { get; set; }
        public int SucceedEntryCount { get; set; }
        public bool IsProcessing { get; set; }
        public DateTime Created { get; set; }

        private readonly IContactService _contactService;
        public ViewContactUploadModel(IContactUploadService contactUploadService, IContactService contactService,
           ICurrentUserService currentUserService) : base(contactUploadService, currentUserService)
        {
            _contactService = contactService;
        }

        internal async Task SetContactUploadId(int id)
        {
            this.ContactUploadId = id;

        }
        public ViewContactUploadModel() : base()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
        }

        public async Task GetContactUploadData(int id)
        {
            this.Id = id;
            var result = await _contactUploadService.GetByIdAsync(id);
            this.FileName = result.FileName;

            this.SucceedEntryCount = result.SucceedEntryCount;
            this.IsSucceed = result.IsSucceed;
            this.IsProcessing = result.IsProcessing;
            this.Created = result.Created;


        }



        public async Task<object> GetContactByContactUploadIdAsync(DataTablesAjaxRequestModel tableModel, int contactUploadId)
        {

            var result = await _contactService.GetContactByContactUploadIdAsync(
                _currentUserService.UserId,
                contactUploadId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Group", "Email" }),
                tableModel.PageIndex, tableModel.PageSize);


            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            string.Join(", ",item.ContactGroups.Select(x=>x.Group.Name)),
                            item.Email,
                        }).ToArray()

            };
        }


    }
}
