
using System.Linq;
using System.Threading.Tasks;
using SecureFlight.Core.Entities;
using SecureFlight.Core.Interfaces;


namespace SecureFlight.Core.Services.PassengerFlightService;
public class PassengerFlightService : BaseService<Passenger>
{
    private readonly IRepository<Flight> _flightRepository;
    private readonly IRepository<Passenger> _passengerRepository;

    public PassengerFlightService(
        IRepository<Flight> flightRepository,
        IRepository<Passenger> passengerRepository

        ) : base(passengerRepository)
    {
        _flightRepository = flightRepository;
        _passengerRepository = passengerRepository;
    }

    public async Task<OperationResult<Passenger>> AddPassengerToFlight(string passengerId, long flightId)
    {
        var flight = await _flightRepository.GetByIdAsync(flightId);

        if (flight == null)
            return OperationResult<Passenger>.NotFound($"Flight with key values {string.Join(", ", flightId)} was not found");

        var passenger = await _passengerRepository.GetByIdAsync(passengerId);

        if (passenger == null)
            return OperationResult<Passenger>.NotFound($"Passenger with key values {string.Join(", ", passenger)} was not found");

        flight.Passengers.Add(passenger);

        return OperationResult<Passenger>.Success(passenger);
    }

    public async Task<OperationResult<Passenger>> RemovePassengerFromFlight(string passengerId, long flightId)
    {
        var flight = await _flightRepository.GetByIdAsync(flightId);

        if (flight == null)
            return OperationResult<Passenger>.NotFound($"Flight with key values {string.Join(", ", flightId)} was not found");

        var passenger = await _passengerRepository.GetByIdAsync(passengerId);

        if (passenger == null)
            return OperationResult<Passenger>.NotFound($"Passenger with key values {string.Join(", ", passenger)} was not found");

        flight.Passengers.Remove(passenger);

        if(!passenger.Flights.Any())
            _passengerRepository.Delete(passenger);

        return OperationResult<Passenger>.Success(passenger);
    }    
}