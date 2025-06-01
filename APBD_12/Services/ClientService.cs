using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;

namespace Tutorial12.Services;

public class ClientService : IClientService
{
    private readonly TripDbContext _context;

    public ClientService(TripDbContext context)
    {
        _context = context;
    }
    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _context.Client
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);
        if (client == null)
            throw new KeyNotFoundException("Client not found.");
        if (client.ClientTrips.Any())
            throw new InvalidOperationException("Client is assigned to at least one trip and cannot be deleted.");
        _context.Client.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}