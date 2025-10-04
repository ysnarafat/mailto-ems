using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Repositories.Campaigns;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services.Campaigns
{
    [ExcludeFromCodeCoverage]
    public class EmailTemplateServiceTests
    {
        private AutoMock _mock;
        private Mock<IEmailTemplateRepository> _emailTemplateRepositoryMock;
        private Mock<ICampaignUnitOfWork> _campaignUnitOfWorkMock;
        private IEmailTemplateService _emailTemplateService;

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
            _emailTemplateRepositoryMock = _mock.Mock<IEmailTemplateRepository>();
            _campaignUnitOfWorkMock = _mock.Mock<ICampaignUnitOfWork>();
            _emailTemplateService = _mock.Create<EmailTemplateService>();
        }

        [TearDown]
        public void Cleanup()
        {
            _emailTemplateRepositoryMock?.Reset();
            _campaignUnitOfWorkMock?.Reset();
        }

        [Test]
        public void AddEmailTemplateAsync_DuplicateEmailTemplateExists_ReturnDuplicationException()
        {
            //Arrange
            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                EmailTemplateName = "Demo",
                EmailTemplateBody = "Demo Body"
            };

            var emailTemplateToMatch = new EmailTemplate
            {
                Id = 1,
                EmailTemplateName = "Demo",
                EmailTemplateBody = "Demo Body"
            };

            _campaignUnitOfWorkMock.Setup(x => x.EmailTemplateRepository).Returns(_emailTemplateRepositoryMock.Object);
            _emailTemplateRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<EmailTemplate, bool>>>(y => y.Compile()(emailTemplate))
                )).ReturnsAsync(true).Verifiable();

            //Act
            Should.Throw<DuplicationException>(() =>
                _emailTemplateService.AddEmailTemplateAsync(emailTemplate)
            );

            //Assert
            _emailTemplateRepositoryMock.VerifyAll();
        }

        [Test]
        public void AddEmailTemplateAsync_DuplicateEmailTemplateDoesNotExists_SaveEmailTemplate()
        {
            //Arrange
            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                EmailTemplateName = "Demo",
                EmailTemplateBody = "Demo Body"
            };

            var emailTemplateToMatch = new EmailTemplate
            {
                Id = 1,
                EmailTemplateName = "Demo",
                EmailTemplateBody = "Demo Body"
            };

            _campaignUnitOfWorkMock.Setup(x => x.EmailTemplateRepository).Returns(_emailTemplateRepositoryMock.Object);
            _emailTemplateRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<EmailTemplate, bool>>>(y => y.Compile()(emailTemplate))
                )).ReturnsAsync(false).Verifiable();

            _emailTemplateRepositoryMock.Setup(x => x.AddAsync(It.Is<EmailTemplate>(y => y.Id == emailTemplate.Id))).Returns(Task.CompletedTask).Verifiable();
            _campaignUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
            //Act

            _emailTemplateService.AddEmailTemplateAsync(emailTemplate);


            //Assert
            _emailTemplateRepositoryMock.VerifyAll();
            _campaignUnitOfWorkMock.VerifyAll();
        }
    }
}
