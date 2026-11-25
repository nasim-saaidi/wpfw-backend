using main.db;
using Microsoft.EntityFrameworkCore;

namespace backend_portfolio.Tests;
public static class TestDb
{
    public static PortDB Create() => new(new DbContextOptionsBuilder<PortDB>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    
}