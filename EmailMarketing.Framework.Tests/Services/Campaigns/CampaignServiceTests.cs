using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Repositories.Campaigns;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
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

namespace EmailMarketing.Framework.Tests.Services.Campaigns
{
    [ExcludeFromCodeCoverage]
    public class CampaignServiceTests
    {
        private AutoMock _mock;
        private Mock<ICampaignUnitOfWork> _campaignUnitOfWorkMock;
        private Mock<ICampaignRepository> _campaignRepositoryMock;
        private Mock<ICampaignReportRepository> _campaignReportRepository;
        private Mock<IEmailTemplateRepository> _emailTemplateRepositoryMock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupUnitOfWork> _groupUnitOfWorkMock;
        private ICampaignService _campaignService;


        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _campaignUnitOfWorkMock = _mock.Mock<ICampaignUnitOfWork>();
            _campaignRepositoryMock = _mock.Mock<ICampaignRepository>();
            _campaignReportRepository = _mock.Mock<ICampaignReportRepository>();
            _emailTemplateRepositoryMock = _mock.Mock<IEmailTemplateRepository>();
            _groupRepositoryMock = _mock.Mock<IGroupRepository>();
            _groupUnitOfWorkMock = _mock.Mock<IGroupUnitOfWork>();
            _campaignService = _mock.Create<CampaignService>();
        }

        [TearDown]
        public void Cleanup()
        {
            _campaignUnitOfWorkMock.Reset();
            _campaignRepositoryMock.Reset();
            _campaignReportRepository.Reset();
            _emailTemplateRepositoryMock.Reset();
            _groupRepositoryMock.Reset();
            _groupUnitOfWorkMock.Reset();
            _campaignReportRepository.Reset();
        }

        [Test]
        public void GetAllEmailByCampaignId_ValidCampaignId_ReturnCampaign()
        {
            //Arrange
            var campaign = new Campaign
            {
                Id = 1,
                CampaignGroups = new List<CampaignGroup>
                {
                    new CampaignGroup
                    {
                        Group = new Group
                        {
                            ContactGroups = new List<ContactGroup>
                            {
                                new ContactGroup{Contact = new Contact{Email = "Shamim@gmail.com"}},
                                new ContactGroup{Contact = new Contact{Email = "Shamim@gmail.com"}},
                            }
                        }
                    }
                },
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaign)),
                It.IsAny<Func<IQueryable<Campaign>, IIncludableQueryable<Campaign, object>>>(),
                true
                )).ReturnsAsync(campaign).Verifiable();

            //Act
            var result = _campaignService.GetAllEmailByCampaignId(campaign.Id);


            //Assert
            result.Result.ShouldBe(campaign);
            _campaignRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllProcessingCampaign_IfProcessingTrue_ReturnCampaginList()
        {
            //Arrange
            var campaignList = new List<Campaign>
            {
                new Campaign {Id = 1, EmailSubject = "Demo", SendDateTime = DateTime.Now, IsProcessing = true},
                new Campaign {Id = 2, EmailSubject = "test", SendDateTime = DateTime.Now, IsProcessing = true}
            };

            var campaign = new Campaign
            {
                Id = 1,
                IsProcessing = true,
                SendDateTime = DateTime.Now
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaign)),
                null, null, true
                )).ReturnsAsync(campaignList).Verifiable();

            //Act
            var result = _campaignService.GetAllProcessingCampaign();

            //Assert
            result.Result.ShouldBe(campaignList);
            _campaignRepositoryMock.VerifyAll();
        }

        [Test]
        public void AddCampaign_CampaignNotNull_AddCampaign()
        {
            //Arrange
            var campaign = new Campaign
            {
                Id = 1,
                EmailSubject = "Test Subject"
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.AddAsync(campaign)).Returns(Task.CompletedTask).Verifiable();
            _campaignUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _campaignService.AddCampaign(campaign);

            //Assert
            _campaignRepositoryMock.VerifyAll();
            _campaignUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetEmailTemplateByUserIdAsync_ValidUserId_GetEmailTemplateList()
        {
            //Arrange
            var emailTemplateList = new List<EmailTemplate>
            {
                new EmailTemplate { Id = 1, EmailTemplateName = "Offer", EmailTemplateBody = "Hello sir", UserId = Guid.NewGuid() },
                new EmailTemplate { Id = 2, EmailTemplateName = "Discount", EmailTemplateBody = "Hello", UserId = Guid.NewGuid() }
            };

            var emailTemplate = new EmailTemplate
            {
                UserId = Guid.NewGuid(),
                EmailTemplateName = "Demo",
                EmailTemplateBody = "Hello"
            };

            _campaignUnitOfWorkMock.Setup(x => x.EmailTemplateRepository).Returns(_emailTemplateRepositoryMock.Object);
            _emailTemplateRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<EmailTemplate, EmailTemplate>>>(y => y.Compile()(new EmailTemplate()) is EmailTemplate),
                It.Is<Expression<Func<EmailTemplate, bool>>>(y => y.Compile()(emailTemplate)),
                null, null, true
                )).ReturnsAsync(emailTemplateList).Verifiable();

            //Act
            var result = _campaignService.GetEmailTemplateByUserIdAsync(emailTemplate.UserId);

            //Assert
            result.Result.ShouldBe(emailTemplateList);
            _emailTemplateRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllCampaignAsync_CampaignExists_ReturnCampaginListTotalToalFilter()
        {
            //Arrange 
            var columnsMap = new Dictionary<string, Expression<Func<Campaign, object>>>()
            {
                ["Name"] = v => v.Name
            };

            var campaignList = new List<Campaign>
            {
                new Campaign {Id = 1, Name = "Demoname", EmailSubject = "Demo", SendDateTime = DateTime.Now, IsProcessing = true},
                new Campaign {Id = 2, Name = "Demoname", EmailSubject = "test", SendDateTime = DateTime.Now, IsProcessing = true}
            };

            var userId = Guid.NewGuid();
            var searchText = "Demo";
            var orderBy = "asc";
            var pageIndex = 1;
            var pageSize = 10;


            var campaign = new Campaign
            {
                Id = 1,
                IsProcessing = true,
                SendDateTime = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                UserId = userId,
                Name = "Demo"
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaign)),
                It.IsAny<Func<IQueryable<Campaign>, IOrderedQueryable<Campaign>>>(),
                It.IsAny<Func<IQueryable<Campaign>, IIncludableQueryable<Campaign, object>>>(),
                pageIndex, pageSize,
                true
                )).ReturnsAsync((campaignList, 2, 2)).Verifiable();

            _campaignRepositoryMock.Setup(x => x.GetCountAsync(
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaign))
                )).ReturnsAsync((2)).Verifiable();
            //Act
            var result = _campaignService.GetAllCampaignAsync(userId, searchText, orderBy, pageIndex, pageSize);

            //Assert
            result.Result.ShouldBe((campaignList, 2, 2));
            _campaignRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllCampaignAsync_CampaignDoesNotExists_ReturnNotFoundException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var searchText = "Demo";
            var orderBy = "asc";
            var pageIndex = 1;
            var pageSize = 10;


            var campaign = new Campaign
            {
                Id = 1,
                IsProcessing = true,
                SendDateTime = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                UserId = userId,
                Name = "DemoCampaign"
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaign)),
                It.IsAny<Func<IQueryable<Campaign>, IOrderedQueryable<Campaign>>>(),
                It.IsAny<Func<IQueryable<Campaign>, IIncludableQueryable<Campaign, object>>>(),
                pageIndex, pageSize,
                true
                )).ReturnsAsync((null, 1, 1)).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _campaignService.GetAllCampaignAsync(userId, searchText, orderBy, pageIndex, pageSize)
            );

            //Assert
            _campaignRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllCampaignReportAsync_CampaignReportExists_GetCampaignReportListTotalTotalFilter()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var searchText = "sam";
            var campaignId = 1;
            var orderBy = "asc";
            var pageIndex = 1;
            var pageSize = 10;

            var campaignReportList = new List<CampaignReport>
            {
                new CampaignReport { Id = 1, CampaignId = 1, ContactId = 1 },
                new CampaignReport { Id = 2, CampaignId = 1, ContactId = 3 }
            };


            var campaignReport = new CampaignReport
            {
                Id = 1,
                SendDateTime = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "sam@gmail.com"
                },
                CampaignId = 1,
                Campaign = new Campaign
                {
                    Id = 1,
                    UserId = userId,

                }
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignReportRepository).Returns(_campaignReportRepository.Object);
            _campaignReportRepository.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReport)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                pageIndex, pageSize,
                true
                )).ReturnsAsync((campaignReportList, 1, 2)).Verifiable();

            //Act
            var result = _campaignService.GetAllCampaignReportAsync(userId, campaignId, searchText, orderBy, pageIndex, pageSize);

            //Assert
            result.Result.ShouldBe((campaignReportList, 1, 2));
            _campaignReportRepository.VerifyAll();
        }

        [Test]
        public void GetAllCampaignReportAsync_CampaignReportDoesNotExists_ReturnNotFoundException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var searchText = "sam";
            var campaignId = 1;
            var orderBy = "asc";
            var pageIndex = 1;
            var pageSize = 10;

            var campaignReport = new CampaignReport
            {
                Id = 1,
                SendDateTime = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                Contact = new Contact
                {
                    Email = "sam@gmail.com"
                },
                CampaignId = 1,
                Campaign = new Campaign
                {
                    Id = 1,
                    UserId = userId,

                }
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignReportRepository).Returns(_campaignReportRepository.Object);
            _campaignReportRepository.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CampaignReport, CampaignReport>>>(y => y.Compile()(new CampaignReport()) is CampaignReport),
                It.Is<Expression<Func<CampaignReport, bool>>>(y => y.Compile()(campaignReport)),
                It.IsAny<Func<IQueryable<CampaignReport>, IOrderedQueryable<CampaignReport>>>(),
                It.IsAny<Func<IQueryable<CampaignReport>, IIncludableQueryable<CampaignReport, object>>>(),
                pageIndex, pageSize,
                true
                )).ReturnsAsync((null, 1, 1)).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _campaignService.GetAllCampaignReportAsync(userId, campaignId, searchText, orderBy, pageIndex, pageSize)
            );


            //Assert
            _campaignReportRepository.VerifyAll();
        }

        [Test]
        public void UpdateCampaignAsync_ValidCampaign_CampaignUpdated()
        {
            //Arrange
            var campaign = new Campaign
            {
                Id = 1,
                IsProcessing = true,
                IsActive = true,
                IsDeleted = false
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(x => x.GetByIdAsync(campaign.Id)).ReturnsAsync(campaign).Verifiable();
            _campaignRepositoryMock.Setup(x => x.UpdateAsync(It.Is<Campaign>(y => y.Id == campaign.Id))).Returns(Task.CompletedTask).Verifiable();
            _campaignUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _campaignService.UpdateCampaignAsync(campaign);

            //Assert
            _campaignRepositoryMock.VerifyAll();
            _campaignUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetAllGroupsAsync_ValidUserId_GetGroupList()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                IsDeleted = false,
                IsActive = true,

            };

            var list = new List<(int Value, string Text, int Count)>();
            list.Add((Value: 1, Text: "Samir", Count: 5));

            var a = new List<ValueTuple<int, string, int>>
            {
               (1, "sam", 2 ),
               (2, "shamim", 3 )
            };

            var groupList = new List<Group>
            {
                new Group{ Id = 1, Name = "Friend", UserId = group.UserId }
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<Group, ValueTuple<int, string, int>>>>(),
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(group)),
                It.IsAny<Func<IQueryable<Group>, IOrderedQueryable<Group>>>(),
                null, true
                )).ReturnsAsync(a).Verifiable();

            //Assert
            var result = _campaignService.GetAllGroupsAsync(group.UserId);

            //Act
            _groupRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetCampaignCountAsync_UserIdNotNull_CountCampaign()
        {
            //Arrange
            var id = new Guid();
            var campaign = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            var campaignToMatch = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            int count = 4;
            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);

            _campaignRepositoryMock.Setup(x => x.GetCountAsync(
              It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaignToMatch)))).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _campaignService.GetCampaignCountAsync(campaign.UserId);

            //Assert
            _campaignRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetCampaignCountAsync_NullParameter_CountCampaign()
        {
            //Arrange
            var id = new Guid();
            var campaign = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            var campaignToMatch = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            int count = 4;
            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);

            _campaignRepositoryMock.Setup(x => x.GetCountAsync(null)).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _campaignService.GetCampaignCountAsync();

            //Assert
            _campaignRepositoryMock.VerifyAll();
        }
        [Test]
        public void ActivateUpdateAsync_ValidCampaignId_ActivateUpdate()
        {
            //Arrange
            var id = new Guid();
            var campaign = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            var campaignToMatch = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);

            _campaignRepositoryMock.Setup(x => x.GetByIdAsync(campaignToMatch.Id)).ReturnsAsync(campaign).Verifiable();

            _campaignRepositoryMock.Setup(x => x.UpdateAsync(It.Is<Campaign>(y => y.Id == campaign.Id))).Returns(Task.CompletedTask).Verifiable();
            _campaignUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _campaignService.ActivateCampaignAsync(campaign.Id);

            //Assert
            _campaignRepositoryMock.VerifyAll();
            _campaignUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetCampaignByIdAsync_InValidId_ThrowException()
        {
            //Arrange
            var id = new Guid();
            var campaign = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            var campaignToMatch = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };
            Campaign nullCampaing = new Campaign();
            nullCampaing = null;
            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);

            _campaignRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaignToMatch)),
                It.IsAny<Func<IQueryable<Campaign>, IIncludableQueryable<Campaign, object>>>(),
                true
                )).ReturnsAsync(nullCampaing).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _campaignService.GetCampaignByIdAsync(campaign.UserId, campaign.Id)
            );

            //Assert
            _campaignRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetCampaignByIdAsync_ValidId_SaveCampaign()
        {
            //Arrange
            var id = new Guid();
            var campaign = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };

            var campaignToMatch = new Campaign
            {
                Id = 1,
                UserId = id,
                EmailSubject = "Test Subject"
            };
            Campaign nullCampaing = new Campaign();
            nullCampaing = null;
            _campaignUnitOfWorkMock.Setup(x => x.CampaignRepository).Returns(_campaignRepositoryMock.Object);

            _campaignRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Campaign, Campaign>>>(y => y.Compile()(new Campaign()) is Campaign),
                It.Is<Expression<Func<Campaign, bool>>>(y => y.Compile()(campaignToMatch)),
                It.IsAny<Func<IQueryable<Campaign>, IIncludableQueryable<Campaign, object>>>(),
                true
                )).ReturnsAsync(campaign).Verifiable();

            //Act
            _campaignService.GetCampaignByIdAsync(campaign.UserId, campaign.Id);

            //Assert
            _campaignRepositoryMock.VerifyAll();
            _campaignUnitOfWorkMock.VerifyAll();
        }

    }


}
