using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class ImageRepository : IImageRepository
{

    private readonly AppDbContext _context;
    private readonly DbSet<Image> _images;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
        _images = context.Images;
    }

    public bool Any(Func<Image, bool> predicate)
    {
        if (_images == null) { return false; }

        return _images.Any(predicate);
    }

    public async Task<Image?> GetImageAsync(Guid id)
    {
        if (_images == null) return null;

        return await _images.FindAsync(id);
    }

    public async Task<Image?> GetImageAsync(string url)
    {
        if (_images == null) return null;

        return await _images.FirstOrDefaultAsync(im => im.ImageUrl == url);
    }

    public async Task<List<Image>> GetImagesAsync()
    {
        if (_images == null) return new List<Image>();

        return await _images.ToListAsync();
    }

    public async Task<Guid> InsertAsync(Image image)
    {
        if (_images == null) return Guid.Empty;

        await _images.AddAsync(image);
        await _context.SaveChangesAsync();

        return image.Id;
    }

    public async Task<Image?> RemoveAsync(Guid id)
    {
        if (_images == null) return null;

        var image = await _images.FindAsync(id) ?? throw new Exception("Image not found in DB");
        _images.Remove(image);
        await _context.SaveChangesAsync();

        return image;
    }

    public async Task<Image?> UpdateAsync(Image image)
    {
        if (_images == null) return null;

        _context.ChangeTracker.Clear();
        _images.Update(image);
        await _context.SaveChangesAsync();

        return image;
    }
}
