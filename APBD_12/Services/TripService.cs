using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.DTOs;
using Tutorial12.Model;

namespace Tutorial12.Services;

public class TripService : ITripService
{
    private readonly TripDbContext _context;

    public TripService(TripDbContext context)
    {
        _context = context;
    }
    public async Task<TripResponseDto> GetTripsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        var query = _context.Trip
            .Include(t => t.CountryTrips).ThenInclude(ct => ct.Country)
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.Client)
            .OrderByDescending(t => t.DateFrom);
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = new TripResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips.Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new CountryDto
                {
                    Name = ct.Country.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.Client.FirstName,
                    LastName = ct.Client.LastName
                }).ToList()
            }).ToList()
        };
        return result;
    }
    
    public async Task AssignClientToTripAsync(int idTrip, AssignClientDto dto)
    {
        var trip = await _context.Trip.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null)
            throw new KeyNotFoundException("Trip not found.");
        if (trip.DateFrom <= DateTime.Now)
            throw new InvalidOperationException("Cannot register for a past trip.");

        var existingClient = await _context.Client.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (existingClient != null)
        {
            var alreadyRegistered = await _context.ClientTrip.AnyAsync(ct => ct.IdTrip == idTrip && ct.IdClient == existingClient.IdClient);
            if (alreadyRegistered)
                throw new InvalidOperationException("Client with this PESEL is already registered for the trip.");

            throw new InvalidOperationException("Client with this PESEL already exists.");
        }

        var client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };
        _context.Client.Add(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };
        _context.ClientTrip.Add(clientTrip);
        await _context.SaveChangesAsync();
    }



}