using System.ComponentModel.DataAnnotations;

namespace Tutorial12.Model;

public class Country
{
    [Key]
    public int IdCountry { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<CountryTrip> CountryTrips { get; set; } = new List<CountryTrip>();
}
