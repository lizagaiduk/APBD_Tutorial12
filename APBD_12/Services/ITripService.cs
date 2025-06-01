namespace Tutorial12.Services;
using Tutorial12.DTOs;


public interface ITripService
{
    Task<TripResponseDto> GetTripsAsync(int page, int pageSize);
    Task AssignClientToTripAsync(int idTrip,AssignClientDto dto);

}
