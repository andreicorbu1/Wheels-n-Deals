using Wheels_n_Deals.API.DataLayer.Models;
namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class ImageMappingExtensions
{
    public static List<string> ToStringList(this List<Image> list)
    {
        var images = new List<string>();
        foreach (var item in list)
        {
            images.Add(item.ImageUrl);
        }
        return images;
    }
}
