using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ContactUploadModel : ContactUploadBaseModel
    {
        //public int UserId { get; set; }
        //public string FileUrl { get; set; }
        //public string FileName { get; set; }
        //public bool IsSucceed { get; set; }
        //public bool IsUpdateExisting { get; set; }
        //public bool HasColumnHeader { get; set; }
        //public bool IsSendEmailNotify { get; set; }
        //public string SendEmailAddress { get; set; }
        //public int SucceedEntryCount { get; set; }
        //public bool IsProcessing { get; set; }
        public ContactUploadModel(IContactUploadService contactUploadService,
            ICurrentUserService currentUserService) : base(contactUploadService, currentUserService)
        {

        }

        public ContactUploadModel() : base()
        {

        }

        public async Task<object> GetAllAsync(DataTablesAjaxRequestModel tableModel)
        {
            var userId = _currentUserService.UserId;

            var result = await _contactUploadService.GetAllAsync(userId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "FileName", "Created" }),
                tableModel.PageIndex, tableModel.PageSize);

            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,

                data = (from item in result.Items
                        where (item.UserId == userId)
                        select new string[]
                        {
                                    item.FileName,
                                    item.Created.ToString(),
                                    item.IsSendEmailNotify?"Yes":"No",
                                    item.IsUpdateExisting?"Yes":"No",
                                    item.IsProcessing?"Processing":"Finished",
                                    item.IsSucceed?"Yes":"No",
                                    item.SucceedEntryCount.ToString(),
                                    item.Id.ToString()
                        }
                        ).ToArray()

            };
        }

        public async Task<ContactUpload> FinishUploadAsync(int id)
        {
            var result = await _contactUploadService.GetByIdAsync(id);
            await _contactUploadService.UpdateAsync(result);
            return result;
        }
    }
}
