using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.SMTP;
using EmailMarketing.Framework.Repositories.SMTP;
using EmailMarketing.Framework.Services.SMTP;
using EmailMarketing.Framework.UnitOfWorks.SMTP;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services.SMTP
{
    [ExcludeFromCodeCoverage]
    public class SMTPServiceTests
    {
        private AutoMock _mock;
        private Mock<ISMTPRepository> _smtpRepositoryMock;
        private Mock<ISMTPUnitOfWork> _smtpUnitOfWorkMock;
        private ISMTPService _smtpService;

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
            _smtpRepositoryMock = _mock.Mock<ISMTPRepository>();
            _smtpUnitOfWorkMock = _mock.Mock<ISMTPUnitOfWork>();
            _smtpService = _mock.Create<SMTPService>();
        }

        [TearDown]
        public void Clean()
        {
            _smtpRepositoryMock.Reset();
            _smtpUnitOfWorkMock.Reset();
        }

        [Test]
        public void GetByIdAsync_ValidSmtpId_ReturnsSmtpObject()
        {
            var id = Guid.NewGuid();
            //Arrange
            var sMTPConfig = new SMTPConfig
            {
                Id = id,
                Server = "Example",
                Port = 8080,
                SenderName = "ABC",
                SenderEmail = "ABC@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.GetByIdAsync(sMTPConfig.Id)).ReturnsAsync(sMTPConfig).Verifiable();

            //Act
            var result = _smtpService.GetByIdAsync(id);

            //Assert
            result.Result.ShouldBe(sMTPConfig);
            _smtpRepositoryMock.VerifyAll();

        }

        [Test]
        public void GetAllAsync_SmtpConfigExists_ReturnSMTPLists()
        {
            //Arrange
            var userId = Guid.NewGuid();
            string searchText = "", orderBy = "Name asc";
            int pageIndex = 1, pageSize = 10;


            var smtpConfigList = new List<SMTPConfig>
            {
                new SMTPConfig { Id = Guid.NewGuid(), Server = "smtp.gmail.com", SenderEmail = "sam@gmail.com" },
                new SMTPConfig { Id = Guid.NewGuid(), Server = "smtp.yahoo.com", SenderEmail = "sam@yahoo.com" },
            };

            var smtptoMatch = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Server = "smtp.gmail.com",
                Port = 8080,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<SMTPConfig, SMTPConfig>>>(y => y.Compile()(new SMTPConfig()) is SMTPConfig),
                It.Is<Expression<Func<SMTPConfig, bool>>>(y => y.Compile()(smtptoMatch)),
                It.IsAny<Func<IQueryable<SMTPConfig>, IOrderedQueryable<SMTPConfig>>>(),
                null, 1, 10, true
                )).ReturnsAsync((smtpConfigList, 3, 2)).Verifiable();

            _smtpRepositoryMock.Setup(x => x.GetCountAsync(
                It.Is<Expression<Func<SMTPConfig, bool>>>(y => y.Compile()(smtptoMatch))
                )).ReturnsAsync(3);

            //Act
            var result = _smtpService.GetAllAsync(userId, searchText, orderBy, pageIndex, pageSize);


            //Assert
            result.Result.ShouldBe((smtpConfigList, 3, 2));
            _smtpRepositoryMock.VerifyAll();

        }

        [Test]
        public void AddAsync_DuplicateSmtpConfigExists_ThrowsDuplicationException()
        {
            //Arrange
            var id = Guid.NewGuid();

            var smtpConfig = new SMTPConfig
            {
                Id = id,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            var smtptoMatch = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<SMTPConfig, bool>>>(y => y.Compile()(smtptoMatch))
                )).ReturnsAsync(true).Verifiable();

            //Act
            Should.Throw<DuplicationException>(() =>
                _smtpService.AddAsync(smtpConfig)
            );

            //Assert
            _smtpRepositoryMock.VerifyAll();

        }

        [Test]
        public void AddAsync_DuplicateSmtpConfigDoesNotExists_SaveSmtpConfig()
        {
            //Arrange
            var id = Guid.NewGuid();

            var smtpConfig = new SMTPConfig
            {
                Id = id,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            var smtptoMatch = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.IsExistsAsync(
                It.IsAny<Expression<Func<SMTPConfig, bool>>>()
                )).ReturnsAsync(false).Verifiable();

            _smtpRepositoryMock.Setup(x => x.AddAsync(smtpConfig)).Returns(Task.CompletedTask).Verifiable();
            _smtpUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _smtpService.AddAsync(smtpConfig);

            //Assert
            _smtpRepositoryMock.VerifyAll();
            _smtpUnitOfWorkMock.VerifyAll();

        }

        [Test]
        public void UpdateAsync_DuplicateSmtpConfigExists_ThrowsDuplicationException()
        {
            //Arrange
            var id = Guid.NewGuid();

            var smtpConfig = new SMTPConfig
            {
                Id = id,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            var smtptoMatch = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<SMTPConfig, bool>>>(y => y.Compile()(smtptoMatch))
                )).ReturnsAsync(true).Verifiable();

            //Act
            Should.Throw<DuplicationException>(() =>
                _smtpService.UpdateAsync(smtpConfig)
            );

            //Assert
            _smtpRepositoryMock.VerifyAll();

        }

        [Test]
        public void UpdateAsync_DuplicateSmtpConfigDoesNotExists_UpdateSmtpConfig()
        {
            //Arrange
            var id = Guid.NewGuid();

            var smtpConfig = new SMTPConfig
            {
                Id = id,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            var smtptoMatch = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);
            _smtpRepositoryMock.Setup(x => x.IsExistsAsync(
                It.IsAny<Expression<Func<SMTPConfig, bool>>>()
                )).ReturnsAsync(false).Verifiable();

            _smtpRepositoryMock.Setup(x => x.GetByIdAsync(smtpConfig.Id)).ReturnsAsync(smtpConfig).Verifiable();

            _smtpRepositoryMock.Setup(x => x.UpdateAsync(smtpConfig)).Returns(Task.CompletedTask).Verifiable();
            _smtpUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _smtpService.UpdateAsync(smtpConfig);

            //Assert
            _smtpRepositoryMock.VerifyAll();
            _smtpUnitOfWorkMock.VerifyAll();

        }

        [Test]
        public void DeleteAsync_ValidSmtpId_RemoveSmtpConfig()
        {
            //Arrange
            var id = Guid.NewGuid();

            var smtpConfig = new SMTPConfig
            {
                Id = id,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);

            _smtpRepositoryMock.Setup(x => x.GetByIdAsync(smtpConfig.Id)).ReturnsAsync(smtpConfig).Verifiable();

            _smtpRepositoryMock.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();
            _smtpUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _smtpService.DeleteAsync(id);


            //Assert
            _smtpRepositoryMock.VerifyAll();
            _smtpUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetAllSMTPConfig_ValidUserId_ReturnAllSmtpConfig()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var smtpConfigToMacth = new SMTPConfig
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Server = "smtp.gmail.com",
                Port = 465,
                SenderName = "ABC",
                SenderEmail = "sam@gmail.com",
                UserName = "Abc",
                Password = "12345",
                EnableSSL = true
            };

            var smtpConfigList = new List<SMTPConfig>
            {
                new SMTPConfig { Id = Guid.NewGuid(), Server = "smtp.gmail.com", SenderEmail = "sam@gmail.com", UserId = userId },
                new SMTPConfig { Id = Guid.NewGuid(), Server = "smtp.yahoo.com", SenderEmail = "sam@yahoo.com", UserId = userId },
            };

            _smtpUnitOfWorkMock.Setup(x => x.SMTPRepository).Returns(_smtpRepositoryMock.Object);

            _smtpRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<SMTPConfig, SMTPConfig>>>(y => y.Compile()(new SMTPConfig()) is SMTPConfig),
                It.Is<Expression<Func<SMTPConfig, bool>>>(y => y.Compile()(smtpConfigToMacth)),
                null, null, true
                )).ReturnsAsync(smtpConfigList).Verifiable();

            //Act
            var result = _smtpService.GetAllSMTPConfig(userId);


            //Assert
            result.Result.ShouldBe(smtpConfigList);
            _smtpRepositoryMock.VerifyAll();
        }

    }
}
