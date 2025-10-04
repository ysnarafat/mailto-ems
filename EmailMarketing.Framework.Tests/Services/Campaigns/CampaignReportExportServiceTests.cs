using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.Repositories.Campaigns;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
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

namespace EmailMarketing.Framework.Tests.Services.Campaigns
{
    [ExcludeFromCodeCoverage]
    public class CampaignReportExportServiceTests
    {
        private AutoMock _mock;
        private Mock<IDownloadQueueRepository> _downloadQueueRepositoryMock;
        private Mock<IDownloadQueueSubEntityRepository> _downloadQueueSubEntityRepositoryMock;
        private Mock<ICampaignReportRepository> _campaignReportRepositoryMock;
        private Mock<ICampaignReportExportUnitOfWork> _campaignReportExportUnitOfWorkMock;
        private Mock<ICampaignReportUnitOfWork> _campaignReportUnitOfWorkMock;
        private CampaignReportExportService _campaignReportExportService;
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
            _downloadQueueRepositoryMock = _mock.Mock<IDownloadQueueRepository>();
            _downloadQueueSubEntityRepositoryMock = _mock.Mock<IDownloadQueueSubEntityRepository>();
            _campaignReportRepositoryMock = _mock.Mock<ICampaignReportRepository>();
            _campaignReportExportUnitOfWorkMock = _mock.Mock<ICampaignReportExportUnitOfWork>();
            _campaignReportUnitOfWorkMock = _mock.Mock<ICampaignReportUnitOfWork>();
            _campaignReportExportService = _mock.Create<CampaignReportExportService>();
        }

        [TearDown]
        public void Clean()
        {
            _downloadQueueRepositoryMock.Reset();
            _downloadQueueSubEntityRepositoryMock.Reset();
            _campaignReportRepositoryMock.Reset();
            _campaignReportExportUnitOfWorkMock.Reset();
            _campaignReportUnitOfWorkMock.Reset();
        }
        [Test]
        public async Task AddDownloadQueueSubEntities_Save()
        {
            //Arrange
            var downloadQueuesubEntry = new DownloadQueueSubEntity
            {
                Id = 1,
                DownloadQueueSubEntityId = 1,
            };

            _campaignReportExportUnitOfWorkMock.Setup(x => x.DownloadQueueSubEntityRepository).Returns(_downloadQueueSubEntityRepositoryMock.Object);
            _downloadQueueSubEntityRepositoryMock.Setup(x => x.AddAsync(downloadQueuesubEntry)).Returns(Task.CompletedTask).Verifiable();

            _campaignReportExportUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
            //Act

            await _campaignReportExportService.AddDownloadQueueSubEntities(downloadQueuesubEntry);


            //Assert
            _downloadQueueSubEntityRepositoryMock.VerifyAll();
            _campaignReportExportUnitOfWorkMock.VerifyAll();

        }
        [Test]

        public void SaveDownloadQueueAsync_SaveChenge()
        {
            //Arrange
            var downloadQueue = new DownloadQueue
            {
                Id = 1,
                FileName = "AllContact",
                IsProcessing = true,
                IsSucceed = false
            };

            _campaignReportExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository).Returns(_downloadQueueRepositoryMock.Object);
            _downloadQueueRepositoryMock.Setup(x => x.AddAsync(downloadQueue)).Returns(Task.CompletedTask).Verifiable();

            _campaignReportExportUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
            //Act

            _campaignReportExportService.SaveDownloadQueueAsync(downloadQueue);


            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
            _campaignReportExportUnitOfWorkMock.VerifyAll();

        }
        [Test]
        public void GetDownloadQueue_DownloadQueueExists_ReturnDownloadQueueList()
        {
            //Arrange
            var downloadQueues = new List<DownloadQueue>
            {
                new DownloadQueue{Id = 1 , FileName = "AllContact",IsProcessing = true,IsSucceed = false},
                new DownloadQueue{Id = 2 , FileName = "AllContact",IsProcessing = true,IsSucceed = false},
                new DownloadQueue{Id = 3 , FileName = "AllContact",IsProcessing = true,IsSucceed = false},

            };

            var downloadQueuesToMatch = new DownloadQueue
            {
                Id = 1,
                FileName = "AllContact",
                IsProcessing = true,
                IsSucceed = false,
                DownloadQueueFor = DownloadQueueFor.CampaignAllReportExport
            };



            _campaignReportExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository)
                .Returns(_downloadQueueRepositoryMock.Object);

            _downloadQueueRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<DownloadQueue, DownloadQueue>>>(y => y.Compile()(new DownloadQueue()) is DownloadQueue),
                It.Is<Expression<Func<DownloadQueue, bool>>>(y => y.Compile()(downloadQueuesToMatch)),
                null,
                It.IsAny<Func<IQueryable<DownloadQueue>, IIncludableQueryable<DownloadQueue, object>>>(),
                true
                )).ReturnsAsync(downloadQueues).Verifiable();

            //Act
            var result = _campaignReportExportService.GetDownloadQueue();
            result.Result.ShouldBe(downloadQueues);


            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetDownloadQueueByIdAsync_ValidId_ReturnDownloadQueue()
        {
            //Arrange
            var downloadQueue = new DownloadQueue
            {
                Id = 1,
                FileName = "AllContact",
                IsProcessing = true,
                IsSucceed = false
            };

            var downloadQueueToMatch = new DownloadQueue
            {
                Id = 1,
            };

            _campaignReportExportUnitOfWorkMock.Setup(x => x.DownloadQueueRepository)
                .Returns(_downloadQueueRepositoryMock.Object);

            _downloadQueueRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<DownloadQueue, DownloadQueue>>>(y => y.Compile()(new DownloadQueue()) is DownloadQueue),
                It.Is<Expression<Func<DownloadQueue, bool>>>(y => y.Compile()(downloadQueueToMatch)),
                null, true
                )).ReturnsAsync(downloadQueue).Verifiable();

            //Act
            var result = _campaignReportExportService.GetDownloadQueueByIdAsync(1);
            result.Result.ShouldBe(downloadQueue);

            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetAllCampaignReportAsync_InvalidUserId_ThrowException()
        {
            //Arrange
            var userId = Guid.NewGuid();

            List<CampaignReport?> campaignReports = null;
            var campaignReportToMatch = new CampaignReport
            {
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "teama@gmail.com"
                },
                Campaign = new Campaign
                {
                    UserId = userId
                }
            };

            _campaignReportUnitOfWorkMock.Setup(x => x.CampaingReportRepository)
                .Returns(_campaignReportRepositoryMock.Object);

            _campaignReportRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReportToMatch)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                true
                )).ReturnsAsync(campaignReports).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _campaignReportExportService.GetAllCampaignReportAsync(userId)
            );

            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetAllCampaignReportAsync_ValidUserId_ReturnCampaignReportList()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var campaignReports = new List<CampaignReport>
            {
                new CampaignReport{IsDeleted = false,IsActive=true},
                new CampaignReport{IsDeleted = false,IsActive=true},
                new CampaignReport{IsDeleted = false,IsActive=true},
            };

            var campaignReportToMatch = new CampaignReport
            {
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "teama@gmail.com"
                },
                Campaign = new Campaign
                {
                    UserId = userId
                }
            };

            _campaignReportUnitOfWorkMock.Setup(x => x.CampaingReportRepository)
                .Returns(_campaignReportRepositoryMock.Object);

            _campaignReportRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReportToMatch)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                true
                )).ReturnsAsync(campaignReports).Verifiable();

            //Act
            var result = _campaignReportExportService.GetAllCampaignReportAsync(userId);
            result.Result.ShouldBe(campaignReports);

            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetCampaignWiseReportAsync_InValidUserIdAndCampaignId_ThrowException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var campaignId = 2;
            List<CampaignReport?> campaignReports = null;
            var campaignReportToMatch = new CampaignReport
            {
                Id = 1,
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "teama@gmail.com"
                },
                Campaign = new Campaign
                {
                    UserId = userId
                }
            };

            _campaignReportUnitOfWorkMock.Setup(x => x.CampaingReportRepository)
                .Returns(_campaignReportRepositoryMock.Object);

            _campaignReportRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReportToMatch)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                true
                )).ReturnsAsync(campaignReports).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _campaignReportExportService.GetCampaignWiseReportAsync(userId, campaignId)
            );

            //Assert
            _downloadQueueRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetCampaignWiseReportAsync_ValidUserIdAndCampaignId_ReturnCampaignReportList()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var campaignId = 1;

            var campaignReports = new List<CampaignReport>
            {
                new CampaignReport{
                    Id = 1,
                    CampaignId = 1,
                    IsDeleted = false,
                    IsActive = true,
                    Campaign = new Campaign{ UserId = userId} ,
                    Contact = new Contact{ Email = "teama@gmail.com"}
                },new CampaignReport{
                    Id = 2,
                    CampaignId = 1,
                    IsDeleted = false,
                    IsActive = true,
                    Campaign = new Campaign{ UserId = userId} ,
                    Contact = new Contact{ Email = "teama@gmail.com"}
                },new CampaignReport{
                    Id = 3,
                    CampaignId = 1,
                    IsDeleted = false,
                    IsActive = true,
                    Campaign = new Campaign{ UserId = userId} ,
                    Contact = new Contact{ Email = "teama@gmail.com"}
                }

            };

            var campaignReportToMatch = new CampaignReport
            {
                Id = 1,
                CampaignId = 1,
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "teama@gmail.com"
                },
                Campaign = new Campaign
                {
                    UserId = userId
                }
            };

            _campaignReportUnitOfWorkMock.Setup(x => x.CampaingReportRepository)
                .Returns(_campaignReportRepositoryMock.Object);

            _campaignReportRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReportToMatch)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                true
                )).ReturnsAsync(campaignReports).Verifiable();

            //Act
            var result = _campaignReportExportService.GetCampaignWiseReportAsync(userId, campaignId);
            result.Result.ShouldBe(campaignReports);

            //Assert
            _campaignReportRepositoryMock.VerifyAll();
        }
    }
}
