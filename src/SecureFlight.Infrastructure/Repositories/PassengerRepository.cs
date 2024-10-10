
using System.Linq;
using System.Threading.Tasks;
using SecureFlight.Core.Entities;

namespace SecureFlight.Infrastructure.Repositories;

public class PassengerRepository : BaseRepository<Passenger>
{
    private readonly SecureFlightDbContext _context;
    public PassengerRepository(SecureFlightDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task RemovePassengerIfNoFlightsAsync(string passengerId)
    {
        var passenger = await GetByIdAsync(passengerId);
        if (passenger != null && !passenger.Flights.Any())
        {
            _context.Passengers.Remove(passenger);
            await SaveChangesAsync();
        }
    }
}