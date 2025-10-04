
using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Services.Groups;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Tests.Services.Groups
{
    [ExcludeFromCodeCoverage]
    public class GroupServiceTests
    {
        private AutoMock _mock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupUnitOfWork> _groupUnitOfWorkMock;
        private IGroupService _groupService;

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
            _groupUnitOfWorkMock = _mock.Mock<IGroupUnitOfWork>();
            _groupService = _mock.Create<GroupService>();
        }

        [TearDown]
        public void Clean()
        {
            _groupRepositoryMock.Reset();
            _groupUnitOfWorkMock.Reset();
        }

        [Test]
        public void GetByIdAsync_GroupId_ReturnsGroupObject()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };
            var id = 1;

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(group)).Verifiable();
            //Act
            var result = _groupService.GetByIdAsync(id);
            var a = result.Result;
            //Assert
            a.ShouldBe(group);
            _groupRepositoryMock.Verify();
        }
        [Test]
        public void GetAllAsync_GroupLists_GetAllGroup()
        {
            //Arrange 
            var userId = Guid.NewGuid();
            int total = 4, totalFilter = 3;
            string searchText = "", orderBy = "Name";
            int pageIndex = 1, pageSize = 10;

            var groupList = new List<Group>
            {
                new Group { Id = 1, Name = "Friends" },
                new Group {Id = 2, Name = "Colleague"},

            };

            var group = new Group
            {
                Id = 1,
                Name = "Friends"

            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<Group, Group>>>(y => y.Compile()(new Group()) is Group),
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(group)),
                It.IsAny<Func<IQueryable<Group>, IOrderedQueryable<Group>>>(),
                //It.IsAny<Func<IQueryable<Group>, IIncludableQueryable<Group, object>>>(),
                null, pageIndex, pageSize, true)).ReturnsAsync((groupList, 2, 2)).Verifiable();

            _groupRepositoryMock.Setup(x => x.GetCountAsync(

                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(group))
            )).ReturnsAsync((2)).Verifiable();

            //Act
            _groupService.GetAllAsync(userId, searchText, orderBy, pageIndex, pageSize);


            //Assert
            _groupRepositoryMock.VerifyAll();
            _groupUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void AddAsync_GroupAlreadyExists_ThrowsException()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)))).Returns(Task.FromResult(true)).Verifiable();


            //Act
            Should.Throw<DuplicationException>(() =>
                _groupService.AddAsync(group)
            );


            //Assert
            _groupRepositoryMock.Verify();
        }

        [Test]
        public void AddAsync_GroupDoesNotExists_SaveGroup()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)))).Returns(Task.FromResult(false)).Verifiable();

            _groupRepositoryMock.Setup(x => x.AddAsync(It.Is<Group>(y => y.Id == group.Id))).Returns(Task.CompletedTask).Verifiable();
            _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _groupService.AddAsync(group);


            //Assert
            _groupRepositoryMock.VerifyAll();
            _groupUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void UpdateAsync_GroupAlreadyExists_ThrowsException()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)))).Returns(Task.FromResult(true)).Verifiable();


            //Act
            Should.Throw<DuplicationException>(() =>
                _groupService.UpdateAsync(group)
            );


            //Assert
            _groupRepositoryMock.Verify();
        }

        [Test]
        public void UpdateAsync_GroupDoesNotExists_UpdateGroup()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };

            var id = 1;

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)))).Returns(Task.FromResult(false)).Verifiable();

            _groupRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(group)).Verifiable();

            _groupRepositoryMock.Setup(x => x.UpdateAsync(It.Is<Group>(y => y.Id == group.Id))).Returns(Task.CompletedTask).Verifiable();
            _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _groupService.UpdateAsync(group);


            //Assert
            _groupRepositoryMock.VerifyAll();
            _groupUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void DeleteAsync_GroupIdExists_DeleteGroup()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.GetByIdAsync(group.Id)).Returns(Task.FromResult(group)).Verifiable();

            _groupRepositoryMock.Setup(x => x.DeleteAsync(group.Id)).Returns(Task.CompletedTask).Verifiable();
            _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            var result = _groupService.DeleteAsync(group.Id);


            //Assert
            result.Result.ShouldBe(group);
            _groupRepositoryMock.VerifyAll();
            _groupUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void ActivateUpdateAsync_GroupAlreadyExists_ThrowsException()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.UpdateAsync(It.Is<Group>(y => y.Id == group.Id))).Returns(Task.CompletedTask).Verifiable();
            _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _groupService.UpdateActiveStatusAsync(group);

            //Assert
            _groupRepositoryMock.VerifyAll();
            _groupUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void GetGroupCountAsync_UserIdNotNull_CountGroup()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };
            int count = 4;
            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.GetCountAsync(
              It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)))).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _groupService.GetGroupCountAsync(group.UserId);

            //Assert
            _groupRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetGroupCountAsync_NullParameter_CountGroup()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };

            var groupToMatch = new Group
            {
                Id = 2,
                Name = "Friends"
            };
            int count = 4;
            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

            _groupRepositoryMock.Setup(x => x.GetCountAsync(null)).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _groupService.GetGroupCountAsync();

            //Assert
            _groupRepositoryMock.VerifyAll();
        }

        //[Test]
        //public void GetAllGroupForSelectAsync_ValidUserId_GetList()
        //{
        //    //Arrange
        //    var userId = Guid.NewGuid();

        //    var groupList = new List<Group>
        //    {
        //        new Group { Id = 1, Name = "Friends" },
        //        new Group {Id = 2, Name = "Colleague"},
        //        new Group {Id = 3, Name = "Employee"},
        //        new Group {Id = 4, Name = "Managars"},
        //    };

        //    var groupToMatch = new Group
        //    {
        //        Id = 1, 
        //        Name = "Friends"
        //    };

        //    _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);

        //    _groupRepositoryMock.Setup(x => x.GetAsync(
        //        It.Is<Expression<Func<Group, Group>>>(y => y.Compile()(new Group()) is Group),
        //        It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(groupToMatch)),
        //        It.IsAny<Func<IQueryable<Group>, IOrderedQueryable<Group>>>(),
        //        It.IsAny<Func<IQueryable<Group>, IIncludableQueryable<Group, object>>>(),
        //         true
        //        )).ReturnsAsync(groupList).Verifiable();

        //    //Act
        //    _groupService.GetAllGroupForSelectAsync(userId);

        //    //Assert
        //    _groupRepositoryMock.VerifyAll();

        //}

    }
}
