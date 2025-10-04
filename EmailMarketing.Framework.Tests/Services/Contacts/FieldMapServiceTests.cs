using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
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
    public class FieldMapServiceTests
    {
        private AutoMock _mock;
        private Mock<IFieldMapRepository> _fieldMapRepositoryMock;
        private Mock<IFieldMapUnitOfWork> _fieldMapUnitOfWorkMock;
        private IFieldMapService _fieldMapService;

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
            _fieldMapRepositoryMock = _mock.Mock<IFieldMapRepository>();
            _fieldMapUnitOfWorkMock = _mock.Mock<IFieldMapUnitOfWork>();
            _fieldMapService = _mock.Create<FieldMapService>();
        }

        [TearDown]
        public void Clean()
        {
            _fieldMapRepositoryMock.Reset();
            _fieldMapUnitOfWorkMock.Reset();
        }

        [Test]
        public void AddAsync_FieldMapAlreadyExists_ThrowsException()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            var fieldMapToMatch = new FieldMap
            {
                Id = 2,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)))).Returns(Task.FromResult(true)).Verifiable();

            //Act
            Should.Throw<DuplicationException>(() =>
                _fieldMapService.AddAsync(fieldMap)
            );

            //Assert
            _fieldMapRepositoryMock.Verify();
        }
        [Test]
        public void AddAsync_FieldMapDoesNotExists_SaveFieldMap()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            var fieldMapToMatch = new FieldMap
            {
                Id = 2,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)))).Returns(Task.FromResult(false)).Verifiable();

            _fieldMapRepositoryMock.Setup(x => x.AddAsync(It.Is<FieldMap>(y => y.Id == fieldMap.Id))).Returns(Task.CompletedTask).Verifiable();
            _fieldMapUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _fieldMapService.AddAsync(fieldMap);

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
            _fieldMapUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void UpdateAsync_FieldMapAlreadyExists_ThrowsException()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            var fieldMapToMatch = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)))).Returns(Task.FromResult(true)).Verifiable();

            //Act
            Should.Throw<DuplicationException>(() =>
                _fieldMapService.UpdateAsync(fieldMap)
            );

            //Assert
            _fieldMapRepositoryMock.Verify();
        }
        [Test]
        public void UpdateAsync_FieldMapDoesNotExists_SaveFieldMap()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            var fieldMapToMatch = new FieldMap
            {
                Id = 2,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)))).Returns(Task.FromResult(false)).Verifiable();

            _fieldMapRepositoryMock.Setup(x => x.GetByIdAsync(fieldMap.Id)).Returns(Task.FromResult(fieldMap)).Verifiable();

            _fieldMapRepositoryMock.Setup(x => x.UpdateAsync(It.Is<FieldMap>(y => y.Id == fieldMap.Id))).Returns(Task.CompletedTask).Verifiable();
            _fieldMapUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _fieldMapService.UpdateAsync(fieldMap);

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
            _fieldMapUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void ActivateUpdateAsync_FieldMapAlreadyExists_ThrowsException()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            var fieldMapToMatch = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
            _fieldMapRepositoryMock.Setup(x => x.GetByIdAsync(fieldMapToMatch.Id)).Returns(Task.FromResult(fieldMap)).Verifiable();

            _fieldMapRepositoryMock.Setup(x => x.UpdateAsync(It.Is<FieldMap>(y => y.Id == fieldMap.Id))).Returns(Task.CompletedTask).Verifiable();
            _fieldMapUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _fieldMapService.ActivateUpdateAsync(fieldMap.Id);

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
            _fieldMapUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void ActivateUpdateAsync_FieldMapAlreadyExists_SaveFieldMap()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            var fieldMapToMatch = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            FieldMap fieldmap = null;

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
            _fieldMapRepositoryMock.Setup(x => x.GetByIdAsync(fieldMapToMatch.Id)).ReturnsAsync(fieldmap).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _fieldMapService.ActivateUpdateAsync(fieldMap.Id)
           );

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetByIdAsync_FieldMapId_ReturnsFieldMapObject()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            int id = 1;

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
            _fieldMapRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(fieldMap)).Verifiable();

            //Act
            var result = _fieldMapService.GetByIdAsync(fieldMap.Id);
            var a = result.Result;

            //Assert
            a.ShouldBe(fieldMap);
            _fieldMapRepositoryMock.Verify();
        }
        [Test]
        public void GetByIdAsync_FieldMapIdDoesNotExists_ThrowExceptions()
        {
            //Arrange
            var fieldMap = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            var fieldMapToMatch = new FieldMap
            {
                Id = 1,
                DisplayName = "ABCDEF",
                IsActive = true,
                IsDeleted = false,
                IsStandard = true

            };
            FieldMap fieldmap = null;

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
            _fieldMapRepositoryMock.Setup(x => x.GetByIdAsync(fieldMapToMatch.Id)).ReturnsAsync(fieldmap).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _fieldMapService.GetByIdAsync(fieldMap.Id)
           );

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetAllAsync_FieldMapLists_GetAllFieldMap()
        {
            var id = new Guid();
            //Arrange
            Guid userId = id;
            int total = 4, totalFilter = 3;
            string searchText = "", orderBy = "asc";
            int pageIndex = 1, pageSize = 10;

            var fieldMapListToReturn = new List<FieldMap>
            {
                new FieldMap { Id = 1, DisplayName = "ABCDEF", IsActive = true,IsDeleted = false,IsStandard = false },
                new FieldMap { Id = 2, DisplayName = "GAFHALHK", IsActive = true,IsDeleted = false,IsStandard = false },
                new FieldMap { Id = 3, DisplayName = "KOETAVA", IsActive = true,IsDeleted = false,IsStandard = false },
                new FieldMap { Id = 4, DisplayName = "LARANLAEF", IsActive = true,IsDeleted = false,IsStandard = false },
            };
            int count = fieldMapListToReturn.Count();
            var fieldMapToMatch = new FieldMap
            {
                Id = 1,
                UserId = id,
                DisplayName = "",
                IsActive = true,
                IsDeleted = false,
                IsStandard = false
            };
            FieldMap NullFieldMap = new FieldMap();
            NullFieldMap = null;

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.GetAsync(
                It.Is<Expression<Func<FieldMap, FieldMap>>>(y => y.Compile()(new FieldMap()) is FieldMap),
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)),
                It.IsAny<Func<IQueryable<FieldMap>, IOrderedQueryable<FieldMap>>>(),
                It.IsAny<Func<IQueryable<FieldMap>, IIncludableQueryable<FieldMap, object>>>(),
                pageIndex, pageSize, true)).ReturnsAsync((fieldMapListToReturn, total, totalFilter)).Verifiable();

            _fieldMapUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);

            _fieldMapRepositoryMock.Setup(x => x.GetCountAsync(
               It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapToMatch)))).Returns(Task.FromResult(count)).Verifiable();

            //Act
            var result = _fieldMapService.GetAllAsync(userId, searchText, orderBy, pageIndex, pageSize);
            result.Result.ShouldBe((fieldMapListToReturn, total, totalFilter));

            //Assert
            _fieldMapRepositoryMock.VerifyAll();
        }

    }
}