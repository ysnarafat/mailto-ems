using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.Services.Groups;
using EmailMarketing.Web.Core;
using EmailMarketing.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class CreateContactUploadModel : ContactUploadBaseModel
    {
        [Required]
        public IFormFile ContactFile { get; set; }
        public bool IsUpdateExisting { get; set; }
        public bool HasColumnHeader { get; set; }
        public bool IsSendEmailNotify { get; set; }
        public string SendEmailAddress { get; set; }
        public IList<ValueTextModel> ContactUploadFieldMaps { get; set; }
        public IList<ValueTextModel> ContactUploadGroups { get; set; }


        private readonly IFileStorage _fileStorage;
        private readonly IGroupService _groupService;
        private readonly AppSettings _appSettings;

        public CreateContactUploadModel(IContactUploadService contactUploadService, IGroupService groupService, IFileStorage fileStorage, IOptions<AppSettings> appSettings,
            ICurrentUserService currentUserService) : base(contactUploadService, currentUserService)
        {
            this._fileStorage = fileStorage;
            this._groupService = groupService;
            this._appSettings = appSettings.Value;
        }

        public CreateContactUploadModel() : base()
        {
            this._fileStorage = Startup.AutofacContainer.Resolve<IFileStorage>();
            this._groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            this._appSettings = Startup.AutofacContainer.Resolve<IOptions<AppSettings>>().Value;
        }

        public async Task SaveContactsUploadAsync()
        {
            if (ContactUploadGroups == null) throw new Exception("Please add/active atleast one group");
            if (!this.ContactUploadGroups.Any(x => x.IsChecked)) throw new Exception("Please select at least one group.");
            if (!(await this._contactUploadService.IsSelectedEmailFieldMap(this.ContactUploadFieldMaps.Select(x => int.Parse(x.Value)).ToList()))) throw new Exception("Please select at least email field map.");

            #region file save
            if (this.ContactFile == null || this.ContactFile.Length <= 0) throw new Exception("Please select a file");

            var fileUrl = this._appSettings.ContactImportFileUrl;
            var url = await _fileStorage.StoreFileAsync(fileUrl, this.ContactFile);
            fileUrl = Path.Combine(fileUrl, url);
            var fileName = this.ContactFile.FileName;
            #endregion

            var entity = new ContactUpload();
            entity.UserId = _currentUserService.UserId;
            entity.FileUrl = fileUrl;
            entity.FileName = fileName;
            entity.HasColumnHeader = this.HasColumnHeader;
            entity.IsUpdateExisting = this.IsUpdateExisting;
            entity.IsSendEmailNotify = this.IsSendEmailNotify;
            entity.SendEmailAddress = this.SendEmailAddress;
            entity.ContactUploadFieldMaps = new List<ContactUploadFieldMap>();
            entity.ContactUploadGroups = new List<ContactUploadGroup>();

            for (int i = 0; i < this.ContactUploadFieldMaps.Count; i++)
            {
                if (int.Parse(this.ContactUploadFieldMaps[i].Value) != -1)
                {
                    var conFieldMap = new ContactUploadFieldMap();
                    conFieldMap.FieldMapId = int.Parse(this.ContactUploadFieldMaps[i].Value);
                    conFieldMap.Index = i;

                    entity.ContactUploadFieldMaps.Add(conFieldMap);
                }
            }

            for (int i = 0; i < this.ContactUploadGroups.Count; i++)
            {
                if (this.ContactUploadGroups[i].IsChecked)
                {
                    var conGrp = new ContactUploadGroup();
                    conGrp.GroupId = int.Parse(this.ContactUploadGroups[i].Value);

                    entity.ContactUploadGroups.Add(conGrp);
                }
            }

            await _contactUploadService.AddContactUploadAsync(entity);
        }

        public async Task<object> GetAllFieldMapForSelectAsync()
        {
            return (await _contactUploadService.GetAllFieldMapForSelectAsync(_currentUserService.UserId)).GroupBy(x => x.IsStandard)
                                .Select(x => new
                                {
                                    IsChecked = x.Key,
                                    Values = x.Select(y =>
                                    new ValueTextModel { Value = y.Value.ToString(), Text = y.Text, IsChecked = y.IsStandard }).ToList()
                                })
                                .OrderByDescending(x => x.IsChecked).ToList();
        }

        public async Task<IList<ValueTextModel>> GetAllGroupForSelectAsync()
        {
            return (await _groupService.GetAllGroupForSelectAsync(_currentUserService.UserId))
                                .Select(x => new ValueTextModel { Value = x.Value.ToString(), Text = x.Text, Count = x.ContactCount }).ToList();
        }
    }
}
