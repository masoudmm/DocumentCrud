using DocumentCrud.Domain.Contracts.Persistence.Repositories;
using DocumentCrud.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using DocumentCrud.Infrastructure.Persistance.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentCrud.Domain.BaseEntities;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Data.Sqlite;
using DocumentCrud.Application.Exceptions;
using DocumentCrud.Domain.CreditAggregate;
using DocumentCrud.Domain.InvoiceAggregate;

namespace Infrastructure.Tests.Repositories;

public class IndependentCreditRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IIndependentCreditRepository _repository;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly SqliteConnection _sqliteConnection;

    public IndependentCreditRepositoryTests()
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
        _repository = new IndependentCreditRepository(_context);

        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_Should_Succeed()
    {
        // Arrange
        var credit = new IndependentCreditNote("1234567890",
            "123456inv1",
            -200m);

        // Act
        await _repository.AddAsync(credit);
        await _context.SaveChangesAsync();

        // Assert
        var savedIndependentCredit = await _context.IndependentCreditNotes.FindAsync(credit.Id);
        Assert.NotNull(savedIndependentCredit);
        Assert.Equal(credit.Number, savedIndependentCredit.Number);
        Assert.Equal(credit.ExternalCreditNumber, savedIndependentCredit.ExternalCreditNumber);
        Assert.Equal(credit.Status, savedIndependentCredit.Status);
        Assert.Equal(credit.TotalAmount, savedIndependentCredit.TotalAmount);
    }


    [Fact]
    public async Task GetByIdAsync_Should_Succeed()
    {
        // Arrange
        var credit = new IndependentCreditNote("1234567890",
            "123456inv1",
            -200m);

        await _context.IndependentCreditNotes.AddAsync(credit);
        await _context.SaveChangesAsync();

        // Act
        var retrievedIndependentCredit = await _repository.GetByIdAsync(credit.Id);

        // Assert
        Assert.NotNull(retrievedIndependentCredit);
        Assert.Equal(credit.Id, retrievedIndependentCredit.Id);
        Assert.Equal(credit.Number, retrievedIndependentCredit.Number);
        Assert.Equal(credit.ExternalCreditNumber, retrievedIndependentCredit.ExternalCreditNumber);
        Assert.Equal(credit.Status, retrievedIndependentCredit.Status);
        Assert.Equal(credit.TotalAmount, retrievedIndependentCredit.TotalAmount);
    }

    [Fact]
    public async Task Remove_Should_Succeed()
    {
        // Arrange
        var credit = new IndependentCreditNote("1234567890",
                    "123456inv1",
                    -200m);

        await _repository.AddAsync(credit);
        await _context.SaveChangesAsync();

        // Act
        var removedIndependentCreditId = _repository.Remove(credit);

        // Assert
        Assert.True(removedIndependentCreditId > 0);
    }

    [Fact]
    public async Task Removed_IndependentCredit_Should_Fail()
    {
        // Arrange
        var credit = new IndependentCreditNote("1234567890",
                    "123456inv1",
                    -200m);

        await _repository.AddAsync(credit);
        await _context.SaveChangesAsync();

        // Act
        _repository.Remove(credit);
        await _context.SaveChangesAsync();

        // Assert
        await Assert.ThrowsAsync<DbEntityNotFoundException>(async () => await _repository.GetByIdAsync(credit.Id));
    }

    public void Dispose()
    {
        //In case we use sqllite we should close the connection here
        //_sqliteConnection.Close();
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}