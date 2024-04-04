using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.Contracts.Persistence.Repositories;
using DocumentCrud.Domain.CreditAggregate;
using FluentAssertions;
using Moq;

namespace Application.Tests.Commands
{
    public class IndependentCreditTests
    {
        [Fact]
        public async Task Create_Independent_Credit_Command_Should_Succeed()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIndependentCreditNoteRepository = new Mock<IIndependentCreditRepository>();

            var command = new CreateIndependentCreditCommand(
                "1234567890",
                "123456icr1",
                100m);

            var createdCredit = new IndependentCreditNote(command.Number,
                command.ExternalCreditNumber,
                command.TotalAmount);

            mockUnitOfWork.Setup(uow => uow.IndependentCreditNotes)
                .Returns(mockIndependentCreditNoteRepository.Object);

            mockIndependentCreditNoteRepository.Setup(repo => repo.AddAsync(It.IsAny<IndependentCreditNote>()))
                .Returns(Task.CompletedTask)
                .Callback<IndependentCreditNote>(creditNote =>
                {
                    creditNote.Should().NotBeNull();
                    creditNote.Number.Should().Be(command.Number);
                });

            mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.FromResult(0));

            var dto = new DocumentDto
            {
                Number = command.Number,
                ExternalNumber = command.ExternalCreditNumber,
                TotalAmount = command.TotalAmount,
            };
            mockMapper.Setup(mapper => mapper.Map<DocumentDto>(It.IsAny<IndependentCreditNote>()))
                .Returns(dto);

            var handler = new CreateIndependentCreditCommandHandler(mockMapper.Object, mockUnitOfWork.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(dto);
            mockIndependentCreditNoteRepository.Verify(repo => repo.AddAsync(It.IsAny<IndependentCreditNote>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}