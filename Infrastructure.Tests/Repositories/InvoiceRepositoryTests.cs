using DocumentCrud.Domain.Contracts.Persistence.Repositories;
using DocumentCrud.Domain.InvoiceAggregate;
using DocumentCrud.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using DocumentCrud.Infrastructure.Persistance.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentCrud.Domain.BaseEntities;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Data.Sqlite;
using DocumentCrud.Application.Exceptions;

namespace Infrastructure.Tests.Repositories;

public class InvoiceRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IInvoiceRepository _repository;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly SqliteConnection _sqliteConnection;

    public InvoiceRepositoryTests()
    {
        //Using InMemoryDatabase, the fastest but checks no constraints, foreign keys and column lengths
        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //    .Options;

        //Using Sqlite, takes !2x longer and it checks foreign keys, but not constraints and column lengths
        //_sqliteConnection = new SqliteConnection("Filename=:memory:");
        //_sqliteConnection.Open();
        //_options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseSqlite(_sqliteConnection)
        //    .Options;

        //Using SqlServer, takes ~5-15x longer then InMemoryDatabase, but checks constraints, foreign keys and column lengths and ...
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=.;Database=DocumentCrud.Test;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True")
            .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new InvoiceRepository(_context);

        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_ShouldAddInvoice_Without_Dependent_Credit()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
            "123456inv1",
            AccountingDocumentStatus.WaitingForApproval,
            200m);

        // Act
        await _repository.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Assert
        var savedInvoice = await _context.Invoices.FindAsync(invoice.Id);
        Assert.NotNull(savedInvoice);
        Assert.Equal(invoice.Number, savedInvoice.Number);
        Assert.Equal(invoice.ExternalInvoiceNumber, savedInvoice.ExternalInvoiceNumber);
        Assert.Equal(invoice.Status, savedInvoice.Status);
        Assert.Equal(invoice.TotalAmount, savedInvoice.TotalAmount);
        Assert.Empty(savedInvoice.DependentCreditNotes);
    }

    [Fact]
    public async Task AddAsync_ShouldAddInvoice_With_Dependent_Credit()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
            "123456inv1",
            AccountingDocumentStatus.WaitingForApproval,
            200m);

        var creditNumber = "1234567890";
        var externalCreditNumber = "1234567dc1";
        var creditStatus = AccountingDocumentStatus.WaitingForApproval;
        var creditTotalAmount = -200m;
        invoice.AddDependentCredit(creditNumber, 
            externalCreditNumber,
            creditStatus,
            creditTotalAmount);

        // Act
        await _repository.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Assert
        var savedInvoice = await _context.Invoices
            .Include(i => i.DependentCreditNotes)
            .FirstAsync(i => i.Id == invoice.Id);

        Assert.NotNull(savedInvoice);
        Assert.Equal(invoice.Number, savedInvoice.Number);
        Assert.Equal(invoice.ExternalInvoiceNumber, savedInvoice.ExternalInvoiceNumber);
        Assert.Equal(invoice.Status, savedInvoice.Status);
        Assert.Equal(invoice.TotalAmount, savedInvoice.TotalAmount);
        Assert.NotEmpty(savedInvoice.DependentCreditNotes);

        var credit = savedInvoice.DependentCreditNotes
            .First();
        Assert.Equal(creditNumber, credit.Number);
        Assert.Equal(externalCreditNumber, credit.ExternalCreditNumber);
        Assert.Equal(creditStatus, credit.Status);
        Assert.Equal(creditTotalAmount, credit.TotalAmount);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnInvoice()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
            "123456inv1",
            AccountingDocumentStatus.WaitingForApproval,
            200m);

        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Act
        var retrievedInvoice = await _repository.GetByIdAsync(invoice.Id);

        // Assert
        Assert.NotNull(retrievedInvoice);
        Assert.Equal(invoice.Id, retrievedInvoice.Id);
        Assert.Equal(invoice.Number, retrievedInvoice.Number);
        Assert.Equal(invoice.ExternalInvoiceNumber, retrievedInvoice.ExternalInvoiceNumber);
        Assert.Equal(invoice.Status, retrievedInvoice.Status);
        Assert.Equal(invoice.TotalAmount, retrievedInvoice.TotalAmount);
        Assert.Empty(retrievedInvoice.DependentCreditNotes);
    }

    [Fact]
    public async Task Remove_Should_Succeed()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
                    "123456inv1",
                    AccountingDocumentStatus.WaitingForApproval,
                    200m);

        await _repository.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Act
        var removedInvoiceId = _repository.Remove(invoice);

        // Assert
        Assert.True(removedInvoiceId > 0);
    }

    [Fact]
    public async Task Get_Removed_Invoice_Should_Fail()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
                    "123456inv1",
                    AccountingDocumentStatus.WaitingForApproval,
                    200m);

        await _repository.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Act
        _repository.Remove(invoice);
        await _context.SaveChangesAsync();

        // Assert
        await Assert.ThrowsAsync<DbEntityNotFoundException>(async () => await _repository.GetByIdAsync(invoice.Id));
    }

    [Fact]
    public async Task Remove_Dependent_Credit_From_Invoice_Should_Succeed()
    {
        // Arrange
        var invoice = Invoice.CreateNew("1234567890",
            "123456inv1",
            AccountingDocumentStatus.WaitingForApproval,
            200m);

        var creditNumber = "1234567890";
        var externalCreditNumber = "1234567dc1";
        var creditStatus = AccountingDocumentStatus.WaitingForApproval;
        var creditTotalAmount = -200m;
        invoice.AddDependentCredit(creditNumber,
            externalCreditNumber,
            creditStatus,
            creditTotalAmount);

        await _repository.AddAsync(invoice);
        await _context.SaveChangesAsync();

        // Act
        invoice.RemoveDependentCredit(invoice.DependentCreditNotes.First());
        await _context.SaveChangesAsync();

        var retreivedInvoice = await _repository.GetByIdAsync(invoice.Id);

        // Assert
        Assert.NotNull(retreivedInvoice);
        Assert.Empty(retreivedInvoice.DependentCreditNotes);
    }

    public void Dispose()
    {
        //In case we use sqllite we should close the connection here
        //_sqliteConnection.Close();
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}