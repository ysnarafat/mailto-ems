
using Autofac.Extras.Moq;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class ContactExportServiceTests
    {
        private AutoMock _mock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupContactRepository> _groupContactRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IDownloadQueueRepository> _downloadQueueRepositoryMock;

        private Mock<IContactExportUnitOfWork> _contactExportUnitOfWorkMock;
        private Mock<IContactUnitOfWork> _contactUnitOfWorkMock;
        private Mock<IGroupUnitOfWork> _groupUnitOfWorkMock;

        private IContactExportService _contactExportService;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanUp()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _groupRepositoryMock = _mock.Mock<IGroupRepository>();
            _groupContactRepositoryMock = _mock.Mock<IGroupContactRepository>();
            _contactRepositoryMock = _mock.Mock<IContactRepository>();
            _downloadQueueRepositoryMock = _mock.Mock<IDownloadQueueRepository>();

            _groupUnitOfWorkMock = _mock.Mock<IGroupUnitOfWork>();
            _contactExportUnitOfWorkMock = _mock.Mock<IContactExportUnitOfWork>();
            _contactUnitOfWorkMock = _mock.Mock<IContactUnitOfWork>();

            _contactExportService = _mock.Create<ContactExportService>();
        }

        [TearDown]
        public void Clean()
        {
            _groupRepositoryMock.Reset();
            _groupContactRepositoryMock.Reset();
            _contactRepositoryMock.Reset();
            _downloadQueueRepositoryMock.Reset();

            _groupUnitOfWorkMock.Reset();
            _contactExportUnitOfWorkMock.Reset();
            _contactUnitOfWorkMock.Reset();
        }

        [Test]
        public void GetContactById_ForContactId_ReturnsContactObject()
        {
            //Arrange
            var contact = new Contact
            {
                Id = 1,
                Email = "devskillTeamA@gmail.com",
                UserId = Guid.Empty,
                ContactUploadId = 2
            };
            var id = 1;
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(contact)).Verifiable();

            //Act
            var returnedContact = _contactExportService.GetContactByIdAsync(id);
            var result = returnedContact.Result;

            //Assert
            result.ShouldBe(contact);
            _groupRepositoryMock.Verify();
        }

        [Test]
        public void GetDownloadQueueByIdAsync_ForDownloadQueueId_ReturnsDownloadQueueObject()
        {
            //Arrange
            var downloadQueue = new DownloadQueue
            {
                Id = 1,
                FileUrl = "C:\\EmailMarketingTeamA",
                DownloadQueueFor = DownloadQueueFor.ContactAllExport,
                IsProcessing = true
            };

            var id = 1;

            _contactExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository).Returns(_downloadQueueRepositoryMock.Object);
            _downloadQueueRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(downloadQueue)).Verifiable();

            //Act
            var returnedDownloadQueue = _contactExportService.GetDownloadQueueByIdAsync(id);
            var result = returnedDownloadQueue.Result;

            //Assert
            result.ShouldBe(downloadQueue);
            _downloadQueueRepositoryMock.Verify();
        }

        //public void UpdateDownloadQueueAync_DownloadQueueItemExist_UpdateItem()
        //{
        //    //Arrange
        //    var item = new DownloadQueue
        //    {
        //        Id = 1,
        //        FileUrl = "C:\\EmailMarketingTeamA",
        //        DownloadQueueFor = DownloadQueueFor.ContactAllExport,
        //        IsProcessing = true
        //    };
        //    var itemToUpdate = new DownloadQueue
        //    {
        //        Id = 1,
        //        FileUrl = "C:\\EmailMarketingTeamA",
        //        FileName = "ContactsExportReport.xlsx",
        //        DownloadQueueFor = DownloadQueueFor.ContactAllExport,
        //        IsProcessing = false
        //    };

        //    _contactExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository).Returns(_downloadQueueRepositoryMock.Object);
        //    _downloadQueueRepositoryMock.Setup(x => x.UpdateAsync(itemToUpdate))
        //        .Returns(Task.CompletedTask).Verifiable();

        //    //Act
        //    var result = _contactExportService.UpdateDownloadQueueAsync(item);

        //    //Assert

        //    result.ShouldBe<Task>(new Task(() => Task.FromResult(true)));

        //}
    }
}

//public void GetDownloadQueue_ForAllProcessingDownlaodQueue_ReturnsDownloadQueueList()
//{
//    var list = new List<DownloadQueue>()
//    {
//        new DownloadQueue()
//        {
//            Id = 1,
//            FileUrl = "C:\\EmailMarketingTeamA",
//            DownloadQueueFor = DownloadQueueFor.ContactAllExport,
//            IsProcessing = true
//        },
//        new DownloadQueue()
//        {
//            Id = 2,
//            FileUrl = "C:\\EmailMarketingTeamA",
//            DownloadQueueFor = DownloadQueueFor.ContactAllExport,
//            IsProcessing = true
//        },
//        new DownloadQueue()
//        {
//            Id = 3,
//            FileUrl = "C:\\EmailMarketingTeamA",
//            DownloadQueueFor = DownloadQueueFor.ContactAllExport,
//            IsProcessing = false
//        }
//    };

//    var groupToMatch = new List<DownloadQueue>
//    {
//        new DownloadQueue { Id = 1, FileUrl = "C:\\EmailMarketingTeamA",DownloadQueueFor = DownloadQueueFor.ContactAllExport },
//        new DownloadQueue { Id = 2, FileUrl = "C:\\EmailMarketingTeamA",DownloadQueueFor = DownloadQueueFor.ContactAllExport },
//        //new DownloadQueue { Id = 3, FileUrl = "C:\\EmailMarketingTeamA",DownloadQueueFor = DownloadQueueFor.ContactAllExport }
//    };

//    _contactExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository).Returns(_downloadQueueRepositoryMock.Object);
//    _downloadQueueRepositoryMock.Setup(x => x.GetAsync<IList<DownloadQueue>>(It.Is<Expression<Func<DownloadQueue, DownloadQueue>>>(y => y.Compile()(new DownloadQueue())), It.Is<Expression<Func<DownloadQueue,DownloadQueue>>>(y => y.Compile()(groupToMatch)))
//        .Returns(Task.CompletedTask).Verifiable();

