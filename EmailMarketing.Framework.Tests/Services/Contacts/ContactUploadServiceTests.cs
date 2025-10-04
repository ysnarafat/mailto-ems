using Autofac.Extras.Moq;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services.Contacts
{
    [ExcludeFromCodeCoverage]
    public class ContactUploadServiceTests
    {
        private AutoMock _mock;
        private Mock<IContactUploadRepository> _contactUploadRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IContactUploadUnitOfWork> _contactExcelUnitOfWork;
        private IContactUploadService _contactUploadService;


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
            _contactExcelUnitOfWork = _mock.Mock<IContactUploadUnitOfWork>();
            _contactUploadRepositoryMock = _mock.Mock<IContactUploadRepository>();
            _contactRepositoryMock = _mock.Mock<IContactRepository>();
            _contactUploadService = _mock.Create<ContactUploadService>();
        }

        [TearDown]
        public void Clean()
        {
            _contactExcelUnitOfWork.Reset();
            _contactRepositoryMock.Reset();
            _contactUploadRepositoryMock.Reset();
        }

        [Test]
        public async Task ContactExcelImportAsync_SearchByContactUploadId_ReturnContactUploadObject()
        {

            // Arrange
            var newContacts = new List<ContactUpload>()
            {
                new ContactUpload()
                {
                    Id = 1,
                    FileUrl = "c/newfolder/example.xlsx"
                }
                ,
                new ContactUpload()
                {

                    Id = 2,
                    FileUrl = "c/newfolder/example1.xlsx"
                }
                    
                //UserId = Guid.NewGuid(),
                //Created = DateTime.Now,
                //CreatedBy = null,
                //IsActive = true,
                //IsDeleted = false,
                //LastModified = DateTime.Now,
                //LastModifiedBy = null
            };

            var newContactToMatch = new ContactUpload
            {
                Id = 2
            };

            _contactExcelUnitOfWork.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactExcelUnitOfWork.Setup(x => x.ContactUploadRepository)
                .Returns(_contactUploadRepositoryMock.Object);

            //_contactUploadRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            //    It.Is<Expression<Func<ContactUpload, bool>>>(y => y.Compile()(newContactToMatch))))
            //    .Returns(new Task<bool>(() => true)).Verifiable();

            // Act

            //Should.Throw<DuplicationException>(async () =>);
            //await _groupService.AddAsync(group)
            //);

            // Assert
            //_groupRepositoryMock.VerifyAll();
        }

        public void GetAllAsync_Test()
        {
            // Arrange
            List<Group> group = new List<Group>()
            {
                new Group()
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    Name = "TeamA",
                    Created = DateTime.Now,
                    CreatedBy = null,
                    IsActive = true,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    LastModifiedBy = null

                },
                new Group()
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    Name = "TeamB",
                    Created = DateTime.Now,
                    CreatedBy = null,
                    IsActive = true,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    LastModifiedBy = null

                },
                new Group()
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    Name = "TeamB",
                    Created = DateTime.Now,
                    CreatedBy = null,
                    IsActive = true,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    LastModifiedBy = null

                }

            };

            //_groupUnitOfWorkMock.Setup(x => x.GroupRepository)
            //    .Returns(_groupRepositoryMock.Object);

            //_groupRepositoryMock.Setup(x => x.GetAsync<Group>(
            //    It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(new List<Group>(() => group))))
            //    .Returns((group,2,0)).Verifiable());

            // Act


        }


    }
}
