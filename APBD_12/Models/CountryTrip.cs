using System.ComponentModel.DataAnnotations;

namespace Tutorial12.Model;


public class CountryTrip
{  [Key]
    
    public int IdCountry { get; set; }
    public int IdTrip { get; set; }

    public Country Country { get; set; } = null!;
    public Trip Trip { get; set; } = null!;
}
