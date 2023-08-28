namespace Wheels_n_Deals.API.DataLayer.DTO;

public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string County { get; set; }
    public string City { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public List<string> Images { get; set; }
    public VehicleDto Vehicle { get; set; }
    public UserDto User { get; set; }
}
