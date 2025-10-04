using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Areas.Admin.Models;
using EmailMarketing.Web.Areas.Admin.Models.AdminUsers;
using Microsoft.AspNetCore.Http;
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
            _createAdminUsersModel = _mock.Create<CreateAdminUsersModel>();
            //_adminUsersModel = _mock.Create<AdminUsersModel>();
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


            var UserRoleName = "Admin";
            var Password = "Shohag16030";

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
            var UserRoleName2 = "Admin";
            var Password2 = "Shohag16030";

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