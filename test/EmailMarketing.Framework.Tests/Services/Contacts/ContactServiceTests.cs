using Autofac.Extras.Moq;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
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


namespace EmailMarketing.Framework.Tests.Services.Contacts
{
    [ExcludeFromCodeCoverage]
    public class ContactServiceTests
    {
        private AutoMock _mock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupContactRepository> _groupContactRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IContactValueMapRepository> _contactValueMapRepositoryMock;
        private Mock<IFieldMapRepository> _fieldMapRepositoryMock;

        private Mock<IContactUnitOfWork> _contactUnitOfWorkMock;
        private Mock<IGroupUnitOfWork> _groupUnitOfWorkMock;

        private IContactService _contactService;

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
            _contactRepositoryMock = _mock.Mock<IContactRepository>();
            _fieldMapRepositoryMock = _mock.Mock<IFieldMapRepository>();
            _contactUnitOfWorkMock = _mock.Mock<IContactUnitOfWork>();
            _groupRepositoryMock = _mock.Mock<IGroupRepository>();
            _groupContactRepositoryMock = _mock.Mock<IGroupContactRepository>();
            _contactRepositoryMock = _mock.Mock<IContactRepository>();
            _contactValueMapRepositoryMock = _mock.Mock<IContactValueMapRepository>();
            _fieldMapRepositoryMock = _mock.Mock<IFieldMapRepository>();

            _groupUnitOfWorkMock = _mock.Mock<IGroupUnitOfWork>();
            _contactUnitOfWorkMock = _mock.Mock<IContactUnitOfWork>();

            _contactService = _mock.Create<ContactService>();
        }

        [TearDown]
        public void Clean()
        {
            _groupRepositoryMock.Reset();
            _groupContactRepositoryMock.Reset();
            _contactRepositoryMock.Reset();
            _contactValueMapRepositoryMock.Reset();
            _fieldMapRepositoryMock.Reset();

            _groupUnitOfWorkMock.Reset();
            _contactUnitOfWorkMock.Reset();
        }

        [Test]
        public void GetByIdAsync_InValidId_ThrowException()
        {
            //Arrange
            int id = 1;

            Contact contact = null;
            var contactsToMatch = new Contact
            {
                Id = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactsToMatch)),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                true
                )).ReturnsAsync(contact).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _contactService.GetByIdAsync(id)
            );

            //Assert
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetByIdAsync_ValidId_ReturnContact()
        {
            //Arrange
            int id = 1;

            var contact = new Contact
            {
                Id = 1,
                Email = "teama@gmail.com"
            };

            var contactsToMatch = new Contact
            {
                Id = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactsToMatch)),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                true
                )).ReturnsAsync(contact).Verifiable();

            //Act

            var result = _contactService.GetByIdAsync(id);
            result.Result.ShouldBe(contact);

            //Assert
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void DeleteAsync_InValidId_ThrowException()
        {
            //Arrange
            int id = 1;

            Contact contact = null;
            var contactsToMatch = new Contact
            {
                Id = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactsToMatch)),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                true
                )).ReturnsAsync(contact).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
                _contactService.DeleteAsync(id)
            );

            //Assert
            _contactRepositoryMock.VerifyAll();
        }
        [Test]
        public void GroupContactCountAsync_GroupContactNotNull_GroupCountContact()
        {
            //Arrange

            var contactGroup = new ContactGroup
            {
                Id = 1,
                ContactId = 1,
                GroupId = 1
            };
            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);

            _groupContactRepositoryMock.Setup(x => x.GetCountAsync(
              It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroup)))).Returns(Task.FromResult(1)).Verifiable();

            //Act
            _contactService.GroupContactCountAsync(contactGroup.ContactId);

            //Assert
            _contactRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void AddContact_ContactNotNull_AddContact()
        {
            //Arrange
            var contact = new Contact
            {
                Id = 1,
                Email = "teama@gmail.com"
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.AddAsync(contact)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.AddContact(contact);

            //Assert
            _contactRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();
        }
        [Test]
        public void AddContactValueMapsList_ContactValueMapsListNotNull_AddContactValueMapsList()
        {
            //Arrange
            var contactValueMapslist = new List<ContactValueMap>
            {
                new ContactValueMap { Value = "ABCDEF", ContactId = 1,FieldMapId = 1 },
                new ContactValueMap { Value = "ALFOAO", ContactId = 2,FieldMapId = 2 },
                new ContactValueMap { Value = "ELAGLA", ContactId = 3,FieldMapId = 3 },
                new ContactValueMap { Value = "ALJOAJ", ContactId = 4,FieldMapId = 4 },
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactValueMapRepository).Returns(_contactValueMapRepositoryMock.Object);

            _contactValueMapRepositoryMock.Setup(x => x.AddRangeAsync(contactValueMapslist)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.AddContacValueMaps(contactValueMapslist);

            //Assert
            _contactValueMapRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetContactValueMapByIdAsync_ForContactValueMapId_ReturnsContactValueMapObject()
        {
            //Arrange
            var contactValueMap = new ContactValueMap()
            {
                Id = 1,
                ContactId = 1,
                FieldMapId = 2,
                Value = "Team A"
            };
            int id = 1;

            _contactUnitOfWorkMock.Setup(x => x.ContactValueMapRepository).Returns(_contactValueMapRepositoryMock.Object);
            _contactValueMapRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(contactValueMap).Verifiable();

            //Act
            var returnedDownloadQueue = _contactService.GetContactValueMapByIdAsync(id);

            //Assert
            _contactValueMapRepositoryMock.Verify();
        }

        [Test]
        public void AddContactGroupList_AddContactGroupListNotNull_AddContactGroupList()
        {
            //Arrange
            var contactGrouplist = new List<ContactGroup>
            {
                new ContactGroup { ContactId = 1,GroupId = 1 },
                new ContactGroup { ContactId = 2,GroupId = 2 },
                new ContactGroup { ContactId = 3,GroupId = 3 },
                new ContactGroup { ContactId = 4,GroupId = 4 },
            };

            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);


            _groupContactRepositoryMock.Setup(x => x.AddRangeAsync(contactGrouplist)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.AddContactGroups(contactGrouplist);

            //Assert
            _contactValueMapRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();
        }

        //             int groupId = 2, contactId = 1;
        //             List<ContactGroup> list = null;

        //             _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
        //             _groupContactRepositoryMock.Setup(x => x.GetAsync(
        //                 It.Is<Expression<Func<ContactGroup,ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
        //                 It.Is<Expression<Func<ContactGroup,bool>>>(y => y.Compile()(contactGroup)),
        //                 null,null,
        //                 true)).ReturnsAsync(list).Verifiable();

        //             //Act
        //             Should.Throw<NotFoundException>(
        //             () => _contactService.DeleteContactGroupAsync(contactId)
        //             );

        //             //Assert
        //             _groupContactRepositoryMock.VerifyAll();
        //         } 

        [Test]
        public void GetGroupByIdAsync_ValidGroupId_ReturnGroupObject()
        {
            //Arrange
            var group = new Group
            {
                Id = 1,
                Name = "Friends"
            };
            int id = 1;
            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);


            _groupRepositoryMock.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(group)).Verifiable();

            //Act
            var result = _contactService.GetGroupByIdAsync(group.Id);
            var a = result.Result;
            //Assert
            a.ShouldBe(group);
            _groupRepositoryMock.VerifyAll();

        }

        //             int groupId = 2, contactId = 1;
        //             List<ContactGroup> list = new List<ContactGroup>();

        //             _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
        //             _groupContactRepositoryMock.Setup(x => x.GetAsync(
        //                 It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
        //                 It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroupToMatch)),
        //                 null,null,
        //                 true)).ReturnsAsync(list).Verifiable();

        //             _groupContactRepositoryMock.Setup(x => x.DeleteRangeAsync(list)).Returns(Task.CompletedTask).Verifiable();
        //             _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        //             //Act
        //             _contactService.DeleteContactGroupAsync(contactId);



        //[Test]
        //public void DeleteContactGroupAsync_ForInvalidId_ThrowsException()
        //{
        //    //Arrange
        //    var contactGroup = new ContactGroup()
        //    {
        //        Id = 1,
        //        GroupId = 2,
        //        ContactId = 1
        //    };
        //    int groupId = 2, contactId = 1;
        //    ContactGroup? nullContactGroup = null;

        //    _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
        //    _groupContactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
        //        It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
        //        It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroup)),
        //        null,
        //        true)).ReturnsAsync(nullContactGroup).Verifiable();

        //    //Act
        //    //Should.Throw<NotFoundException>(
        //    //() => _contactService.DeleteContactGroupAsync(groupId, contactId)
        //    //);

        //    //Assert
        //    _groupContactRepositoryMock.VerifyAll();
        //}

        //[Test]
        //public void DeleteContactGroupAsync_ForValidId_DeleteContactGroup()
        //{
        //    //Arrange
        //    var contactGroup = new ContactGroup()
        //    {
        //        Id = 1,
        //        GroupId = 2,
        //        ContactId = 1
        //    };

        //    var contactGroupToMatch = new ContactGroup()
        //    {
        //        Id = 1,
        //        GroupId = 2,
        //        ContactId = 1
        //    };

        //    int groupId = 2, contactId = 1;

        //    _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
        //    _groupContactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
        //        It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
        //        It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroupToMatch)),
        //        null,
        //        true)).ReturnsAsync(contactGroup).Verifiable();

        //    _groupContactRepositoryMock.Setup(x => x.DeleteAsync(contactGroup.Id)).Returns(Task.CompletedTask).Verifiable();
        //    _groupUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        //    //Act
        //    //_contactService.DeleteContactGroupAsync(groupId, contactId);

        //    //Assert
        //    _groupContactRepositoryMock.VerifyAll();
        //    _contactUnitOfWorkMock.VerifyAll();

        //}

        [Test]
        public void UpdateAsync_ForInvalidContact_ThrowsException()
        {
            //Assign
            var contact = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 2,
                Email = "teamA@gmail.com"
            };
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch))
                )).ReturnsAsync(true).Verifiable();

            //Act
            Should.Throw<DuplicationException>(
                () => _contactService.UpdateAsync(contact));

            //Assert
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void DeleteAsync_ValidId_ReturnContact()
        {
            //Arrange
            int id = 1;

            var contact = new Contact
            {
                Id = 1,
                Email = "teama@gmail.com"
            };
            var contactsToMatch = new Contact
            {
                Id = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository)
                .Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactsToMatch)),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                true
                )).ReturnsAsync(contact).Verifiable();

            _contactRepositoryMock.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            var result = _contactService.DeleteAsync(id);
            result.Result.ShouldBe(contact);

            //Assert
            _contactUnitOfWorkMock.VerifyAll();
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void UpdateAsync_ForValidContactId_UpdateContact()
        {
            //Assign
            var existingContact = new Contact
            {
                Id = 1,
                Email = "teama@gmail.com"
            };
            var contactToUpdate = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 2,
                Email = "teamA@gmail.com"
            };
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.IsExistsAsync(
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch))
                )).ReturnsAsync(false).Verifiable();

            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);
            _contactUnitOfWorkMock.Setup(x => x.ContactValueMapRepository).Returns(_contactValueMapRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(existingContact)),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                false
                )).ReturnsAsync(existingContact).Verifiable();

            _contactRepositoryMock.Setup(x => x.UpdateAsync(existingContact)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.UpdateAsync(contactToUpdate);

            //Assert
            _contactRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();
        }



        //[Test]
        //public void GetIdByEmail_ValidEmail_ReturnContact()
        //{
        //    //Arrange
        //    string email = "n@gmail.com";
        //    var contact = new Contact
        //    {
        //        Id = 1,
        //        Email = "n@gmail.com",
        //    };

        //    var contactToMatch = new Contact
        //    {
        //        Id = 1,
        //        Email = "n@gmail.com",
        //    };

        //    _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
        //    _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
        //        It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
        //        It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch)),
        //        null, true
        //        )).ReturnsAsync(contact).Verifiable();

        //    //Act

        //    var result = _contactService.GetIdByEmail(email);

        //    result.Result.ShouldBe(contact);

        //    //Assert
        //    _contactRepositoryMock.VerifyAll();
        //}

        [Test]
        public void GetAllContactAsync_ForUserId_ShowContactList()
        {
            //Assign
            int pageIndex = 1;
            int pageSize = 10;
            var searchText = "team";
            var userId = Guid.NewGuid();
            var orderBy = "asc";

            var contactListToReturn = new List<Contact>
            {
                new Contact
                {
                    Id = 1,
                    Email = "teamA@gmail.com"
                },
                new Contact
                {
                    Id = 2,
                    Email= "teamB@gmail.com"
                },
                new Contact
                {
                    Id = 3,
                    Email = "teamC@gmail.com"
                }
            };
            var contactToMatch = new Contact
            {
                UserId = userId,
                Email = "team"
            };

            var columnsMap = new Dictionary<string, Expression<Func<Entities.Contacts.Contact, object>>>()
            {
                ["Email"] = v => v.Email
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.GetAsync<Contact>(
                It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch)),
                It.IsAny<Func<IQueryable<Contact>, IOrderedQueryable<Contact>>>(),
                It.IsAny<Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>>>(),
                pageIndex,
                pageSize,
                true
            )).ReturnsAsync((contactListToReturn, 3, 3)).Verifiable();
            _contactRepositoryMock.Setup(x => x.GetCountAsync(
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch))
            )).ReturnsAsync((3)).Verifiable();

            //Act
            var result = _contactService.GetAllContactAsync(userId, searchText, orderBy, pageIndex, pageSize);
            result.Result.ShouldBe((contactListToReturn, 3, 3));

            //Assert
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetContactByIdAsync_validContactId_contactObjectReturn()
        {
            //Assign
            var contact = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
               It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch)), null,
                true
                )).ReturnsAsync(contact).Verifiable();

            //Act
            var result = _contactService.GetContactByIdAsync(contact.Id);

            //Assert
            result.Result.ShouldBe(contact);
            _contactRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetContactByIdAsync_InvalidContactId_contactObjectReturn()
        {
            //Assign
            var contact = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 1,
                Email = "teamA@gmail.com"
            };
            Contact con = new Contact();
            con = null;
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);
            _contactRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
               It.Is<Expression<Func<Contact, Contact>>>(y => y.Compile()(new Contact()) is Contact),
                It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch)), null,
                true
                )).ReturnsAsync(con).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
               _contactService.GetContactByIdAsync(contact.Id)
            );


            //Assert
            _contactRepositoryMock.VerifyAll();
        }

        [Test]
        public void DeleteRangeAsync_ContactValueMapListNotNull_DeleteContactValueMapList()
        {
            //Arrange
            var contactValueMapslist = new List<ContactValueMap>
            {
                new ContactValueMap { Value = "ABCDEF", ContactId = 1,FieldMapId = 1 },
                new ContactValueMap { Value = "ALFOAO", ContactId = 2,FieldMapId = 2 },
                new ContactValueMap { Value = "ELAGLA", ContactId = 3,FieldMapId = 3 },
                new ContactValueMap { Value = "ALJOAJ", ContactId = 4,FieldMapId = 4 },
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactValueMapRepository).Returns(_contactValueMapRepositoryMock.Object);

            _contactValueMapRepositoryMock.Setup(x => x.DeleteRangeAsync(contactValueMapslist)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.DeleteRangeAsync(contactValueMapslist);

            //Assert
            _contactValueMapRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();

        }

        [Test]
        public void GetAllGroupsAsync_ValidUserId_GetGroupList()
        {
            //Arrange

            var a = new List<ValueTuple<int, string, int>>
            {
               (1, "sam", 2 ),
               (2, "shamim", 3 )
            };
            var group = new Group
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                IsDeleted = false,
                IsActive = true,

            };

            _groupUnitOfWorkMock.Setup(x => x.GroupRepository).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<Group, ValueTuple<int, string, int>>>>(),
                It.Is<Expression<Func<Group, bool>>>(y => y.Compile()(group)),
                It.IsAny<Func<IQueryable<Group>, IOrderedQueryable<Group>>>(),
                It.IsAny<Func<IQueryable<Group>, IIncludableQueryable<Group, object>>>(), true
                )).ReturnsAsync(a).Verifiable();

            //Assert
            var result = _contactService.GetAllGroupsAsync(group.UserId);

            //Act
            _groupRepositoryMock.VerifyAll();
        }

        [Test]
        public void DeleteContactGroupAsync_ValidContactGroupId_DeleteContact()
        {
            //Arrange
            var contactGrouplist = new List<ContactGroup>
            {
                new ContactGroup { ContactId = 1,GroupId = 1 },
                new ContactGroup { ContactId = 2,GroupId = 2 },
                new ContactGroup { ContactId = 3,GroupId = 3 },
                new ContactGroup { ContactId = 4,GroupId = 4 },
            };
            var contactGroup = new ContactGroup()
            {
                Id = 1,
                GroupId = 2,
                ContactId = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);

            _groupContactRepositoryMock.Setup(x => x.GetAsync(
               It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
               It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroup)),
               null,
               null,
               true
              )).ReturnsAsync(contactGrouplist).Verifiable();

            _groupContactRepositoryMock.Setup(x => x.DeleteRangeAsync(contactGrouplist)).Returns(Task.CompletedTask).Verifiable();
            _contactUnitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            //Act
            _contactService.DeleteContactGroupAsync(contactGroup.Id);

            //Assert
            _groupContactRepositoryMock.VerifyAll();
            _contactUnitOfWorkMock.VerifyAll();

        }
        [Test]
        public void DeleteContactGroupAsync_InValidContactGroupId_ThrowExceptions()
        {
            //Arrange
            var contactGrouplist = new List<ContactGroup>
            {
                new ContactGroup { ContactId = 1,GroupId = 1 },
                new ContactGroup { ContactId = 2,GroupId = 2 },
                new ContactGroup { ContactId = 3,GroupId = 3 },
                new ContactGroup { ContactId = 4,GroupId = 4 },
            };
            var contactGroup = new ContactGroup()
            {
                Id = 1,
                GroupId = 2,
                ContactId = 1
            };
            List<ContactGroup> contactGroups = new List<ContactGroup>();
            contactGroups = null;

            _contactUnitOfWorkMock.Setup(x => x.GroupContactRepository).Returns(_groupContactRepositoryMock.Object);

            _groupContactRepositoryMock.Setup(x => x.GetAsync(
               It.Is<Expression<Func<ContactGroup, ContactGroup>>>(y => y.Compile()(new ContactGroup()) is ContactGroup),
               It.Is<Expression<Func<ContactGroup, bool>>>(y => y.Compile()(contactGroup)),
               null,
               null,
               true
              )).ReturnsAsync(contactGroups).Verifiable();

            //Act
            Should.Throw<NotFoundException>(() =>
             _contactService.DeleteContactGroupAsync(contactGroup.Id)
            );

            //Assert
            _groupContactRepositoryMock.VerifyAll();

        }

        [Test]
        public void GetContactCountAsync_UserIdNotNull_CountContact()
        {
            //Arrange
            var id = new Guid();
            var contact = new Contact
            {
                Id = 1,
                UserId = id,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 1,
                UserId = id,
                Email = "teamA@gmail.com"
            };
            int count = 4;
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetCountAsync(
              It.Is<Expression<Func<Contact, bool>>>(y => y.Compile()(contactToMatch)))).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _contactService.GetContactCountAsync(contact.UserId);

            //Assert
            _contactRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetContactCountAsync_NullParameter_CountContact()
        {
            //Arrange
            var id = new Guid();
            var contact = new Contact
            {
                Id = 1,
                UserId = id,
                Email = "teamA@gmail.com"
            };
            var contactToMatch = new Contact
            {
                Id = 1,
                UserId = id,
                Email = "teamA@gmail.com"
            };
            int count = 4;
            _contactUnitOfWorkMock.Setup(x => x.ContactRepository).Returns(_contactRepositoryMock.Object);

            _contactRepositoryMock.Setup(x => x.GetCountAsync(null)).Returns(Task.FromResult(count)).Verifiable();

            //Act
            _contactService.GetContactCountAsync();


            //Assert
            _contactRepositoryMock.VerifyAll();
        }
        [Test]
        public void GetAllExistingContactValueMapByContactId_ValidContactId_ContactValueMapList()
        {
            //Arrange
            var contactValueMapslist = new List<ContactValueMap>
            {
                new ContactValueMap { Id =1, Value = "ABCDEF", ContactId = 1,FieldMapId = 1 },
                new ContactValueMap { Id =2,Value = "ALFOAO", ContactId = 2,FieldMapId = 2 },
                new ContactValueMap {Id =3, Value = "ELAGLA", ContactId = 3,FieldMapId = 3 },
                new ContactValueMap { Id = 4,Value = "ALJOAJ", ContactId = 4,FieldMapId = 4 },
            };
            var contactValueMap = new ContactValueMap()
            {
                Id = 1,
                Value = "ABCDEF",
                ContactId = 1,
                FieldMapId = 1
            };

            _contactUnitOfWorkMock.Setup(x => x.ContactValueMapRepository).Returns(_contactValueMapRepositoryMock.Object);

            _contactValueMapRepositoryMock.Setup(x => x.GetAsync(
               It.Is<Expression<Func<ContactValueMap, ContactValueMap>>>(y => y.Compile()(new ContactValueMap()) is ContactValueMap),
               It.Is<Expression<Func<ContactValueMap, bool>>>(y => y.Compile()(contactValueMap)),
               null,
               null,
               true
              )).ReturnsAsync(contactValueMapslist).Verifiable();

            //Act
            _contactService.GetAllExistingContactValueMapByContactId(1);

            //Assert
            _contactValueMapRepositoryMock.VerifyAll();

        }

        [Test]
        public void GetAllContactValueMapsCustom_ForUserId_ReturnsFieldMapList()
        {
            var userId = Guid.NewGuid();
            var list = new List<ValueTuple<int, string>>
            {
                (1, "Email"),
                (2, "Address"),
                (3, "Date of Birth")
            };

            var fieldMapTemp = new FieldMap
            {
                IsActive = true,
                IsDeleted = false,
                IsStandard = false,
                UserId = userId
            };

            _contactUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
            _fieldMapRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<FieldMap, ValueTuple<int, string>>>>(),
                It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapTemp)),
                null,
                null,
                true
               )).ReturnsAsync(list).Verifiable();

            //Act
            var result = _contactService.GetAllContactValueMapsCustom(userId);

            ////Assert
            _fieldMapRepositoryMock.VerifyAll();

        }


        //[Test]
        //public void GetAllContactValueMapsStandard_ForUserIdAndContactId_ReturnsFieldMapList()
        //{
        //    var list = new List<ValueTuple<int , string>>
        //    {
        //        (1,"Email"),
        //        (2,"Address"),
        //        (4,"Age")
        //    };

        //    var userId = new Guid();

        //    var fieldMapTemp = new FieldMap
        //    {
        //        IsActive = true,
        //        IsDeleted = false,
        //        IsStandard = false,
        //        UserId = userId
        //    };

        //    _contactUnitOfWorkMock.Setup(x => x.FieldMapRepository).Returns(_fieldMapRepositoryMock.Object);
        //    _fieldMapRepositoryMock.Setup(x => x.GetAsync(
        //        It.IsAny<Expression<Func<FieldMap, ValueTuple<int, string>>>>(),
        //        It.Is<Expression<Func<FieldMap, bool>>>(y => y.Compile()(fieldMapTemp)),
        //        null,
        //        null,
        //        true
        //       )).ReturnsAsync(list).Verifiable();

        //    //Act
        //    var result = _contactService.GetAllContactValueMapsStandard();

        //    ////Assert
        //    _fieldMapRepositoryMock.VerifyAll();

        //}
    }
}

