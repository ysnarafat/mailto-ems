
using Autofac.Extras.Moq;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services.Contacts
{
    [ExcludeFromCodeCoverage]
    public class ContactExportServiceTests
    {
        private AutoMock _mock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupContactRepository> _groupContactRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IDownloadQueueRepository> _downloadQueueRepositoryMock;
        private Mock<IDownloadQueueSubEntityRepository> _downloadQueueSubEntitiesRepositoryMock;
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
            _downloadQueueSubEntitiesRepositoryMock = _mock.Mock<IDownloadQueueSubEntityRepository>();
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
            _downloadQueueSubEntitiesRepositoryMock.Reset();
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

        [Test]
        public void GetDownloadQueueAsync_FieldValueIsProcessingIsTrue_ReturnsDownloadQueue()
        {
            //Arrange
            var downloadQueuesToReturn = new List<DownloadQueue>
            {
                new DownloadQueue
            {
                Id = 1,
                FileUrl = "C:\\EmailMarketingTeamA",
                DownloadQueueFor = DownloadQueueFor.ContactAllExport,
                IsProcessing = true,
                IsSucceed = true
            },
                new DownloadQueue
            {
                Id = 2,
                FileUrl = "C:\\EmailMarketingTeamA",
                DownloadQueueFor = DownloadQueueFor.ContactAllExport,
                IsProcessing = true,
                IsSucceed = true
            }
            };

            var downloadQueueToMatch = new DownloadQueue
            {
                IsProcessing = true,
                IsSucceed = true,
                DownloadQueueFor = DownloadQueueFor.ContactAllExport,


            };

            _contactExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository).Returns(_downloadQueueRepositoryMock.Object);
            _downloadQueueRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<DownloadQueue, DownloadQueue>>>(y => y.Compile()(new DownloadQueue()) is DownloadQueue),
                It.Is<Expression<Func<DownloadQueue, bool>>>(y => y.Compile()(downloadQueueToMatch)),
                null,
                It.IsAny<Func<IQueryable<DownloadQueue>, IIncludableQueryable<DownloadQueue, object>>>(),
                true
                )).ReturnsAsync(downloadQueuesToReturn).Verifiable();

            //Act
            var returnedDownloadQueue = _contactExportService.GetDownloadQueueAsync();
            //var result = returnedDownloadQueue.Result;

            //Assert
            //result.ShouldBe(downloadQueue);
            _downloadQueueRepositoryMock.Verify();
        }

        [Test]
        public void GetAllContactAsync_ForUserId_ReturnsAllAssociatedContacts()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var contacts = new List<Contact>
            {
                new Contact
                {
                    Id = 1,
                    Email = "devskillTeamA@gmail.com",
                    UserId = userId,
                    ContactUploadId = 1
                },
                new Contact
                {
                    Id = 2,
                    Email = "devskillTeamB@gmail.com",
                    UserId = userId,
                    ContactUploadId = 2
                },
                new Contact
                {
                    Id = 3,
                    Email = "devskillTeamC@gmail.com",
                    UserId = userId,
                    ContactUploadId = 3
                },
                new Contact
                {
                    Id = 4,
                    Email = "devskillTeamNull@gmail.com",
                    UserId = userId,
                    ContactUploadId = null
                }
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.GetAsync<Contact>(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(new Contact { UserId = userId })),
                null,
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                true
                )).ReturnsAsync(contacts).Verifiable();

            //Act
            var returnedContact = _contactExportService.GetAllContactAsync(userId);
            var result = returnedContact.Result;

            //Assert
            result.ShouldBe(contacts);
            _contactRepositoryMock.Verify();
        }

        [Test]
        public void GetAllContactGroupByUserIdAsync_ForUserIdAndGroupId_ReturnsUserContactsByGroup()
        {
            //Arrange 
            var contactGroupsToReturn = new List<ContactGroup>
            {
                 new ContactGroup
                {
                    Id = 1,
                    ContactId = 1,
                    GroupId = 1,
                },
                new ContactGroup
                {
                    Id = 2,
                    ContactId = 2,
                    GroupId = 1
                },
                new ContactGroup
                {
                    Id = 4,
                    ContactId = 4,
                    GroupId = 1
                }
            };
            int groupId = 1;
            var userId = Guid.NewGuid();
            var contactGroupsToMatch = new ContactGroup { GroupId = 1 };
            contactGroupsToMatch.Contact = new Contact();
            contactGroupsToMatch.Contact.UserId = userId;

            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
            _groupContactRepositoryMock.Setup(x => x.GetAsync<ContactGroup>(
                It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
                It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroupsToMatch)),
                null,
                It.IsAny<Func<IQueryable<ContactGroup>, IIncludableQueryable<ContactGroup, object>>>(),
                true
                )).ReturnsAsync(contactGroupsToReturn).Verifiable();

            //Act
            var returnedContact = _contactExportService.GetAllContactGroupByUserIdAsync(userId, groupId);

            //Assert
            _groupContactRepositoryMock.Verify();
        }

    }
}



