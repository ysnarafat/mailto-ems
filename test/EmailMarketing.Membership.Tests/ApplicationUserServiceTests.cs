using Autofac.Extras.Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace EmailMarketing.Membership.Tests
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUserServiceTests
    {
        private AutoMock _mock;
        //private Mock<IGroupRepository> _groupRepositoryMock;
        //private Mock<IGroupUnitOfWork> _groupUnitOfWorkMock;
        //private IGroupService _groupService;

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
            // _groupRepositoryMock = _mock.Mock<IGroupRepository>();

        }

        [TearDown]
        public void Clean()
        {
            //_groupRepositoryMock.Reset();
            //_groupUnitOfWorkMock.Reset();
        }

        [Test]
        public void GetByIdAsync_GroupId_ReturnsGroupObject()
        {
            //Arrange
            //Act
            //Assert
        }

    }
}
