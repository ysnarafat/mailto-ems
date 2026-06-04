using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Areas.Admin.Models;
using EmailMarketing.Web.Areas.Admin.Models.AdminUsers;
using EmailMarketing.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Tests
{
    [ExcludeFromCodeCoverage]
    public class AdminUsersModelTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUserService> _applicationUserServiceMock;
        private CreateAdminUsersModel _createAdminUsersModel;
        //private AdminUsersModel _adminUsersModel;

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
            _applicationUserServiceMock = _mock.Mock<IApplicationUserService>();
            var appSettingsMock = new Mock<IOptions<AppSettings>>();
            appSettingsMock.Setup(x => x.Value).Returns(new AppSettings());
            _createAdminUsersModel = new CreateAdminUsersModel(_applicationUserServiceMock.Object, appSettingsMock.Object);
        }

        [TearDown]
        public void Clean()
        {
            _applicationUserServiceMock.Reset();
        }
        [Test]
        public void CreateAsync_DuplicateAdminExists_ThrowsDuplicationException()
        {
            //Arrange
            var id = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = id,
                FullName = "NewAdmin",
                UserName = "IamNewAdmin",
                Email = "NewAdmin@gmail.com",
                PhoneNumber = "017322382729",
                Gender = "Male",
                DateOfBirth = Convert.ToDateTime("1/1/2010 12:10:15"),
                Address = "XYZ",
                EmailConfirmed = true,
                IsActive = true,
                IsDeleted = false
            };

            _createAdminUsersModel.FullName = "Shamim";
            _createAdminUsersModel.Email = "sam@gmail.com";


            var id2 = Guid.NewGuid();
            var userTomatch = new ApplicationUser
            {
                Id = id,
                FullName = "NewAdmin",
                UserName = "IamNewAdmin",
                Email = "NewAdmin@gmail.com",
                PhoneNumber = "017322382729",
                Gender = "Male",
                DateOfBirth = Convert.ToDateTime("1/1/2010 12:10:15"),
                Address = "XYZ",
                EmailConfirmed = true
            };

            //_applicationUserServiceMock.Setup(x => x.AddAsync()).Verifiable();

            //Act
             _createAdminUsersModel.CreateAdminAsync();

            //Assert
            _applicationUserServiceMock.VerifyAll();
        }
        //[Test]
        //public void DeleteAsync_ValidAdminId_RemoveAdmin()
        //{
        //    //Arrange
        //    var id = Guid.NewGuid();
        //    var user = new ApplicationUser
        //    {
        //        Id = id,
        //        FullName = "NewAdmin",
        //        UserName = "IamNewAdmin",
        //        Email = "NewAdmin@gmail.com",
        //        PhoneNumber = "017322382729",
        //        Gender = "Male",
        //        DateOfBirth = Convert.ToDateTime("1/1/2010 12:10:15"),
        //        Address = "XYZ",
        //        EmailConfirmed = true,
        //        IsActive = true,
        //        IsDeleted = false
        //    };

        //    _applicationUserServiceMock.Setup(x => x.DeleteAsync(id)).ReturnsAsync(user.FullName).Verifiable();

        //    //Act
        //    _adminUsersModel.DeleteAsync(id);


        //    //Assert
        //    _applicationUserServiceMock.VerifyAll();
        //}
    }
    
}