using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class MaterialRepo : IMaterialRepo
    {
        private readonly DevKidContext _context;
        public MaterialRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMaterial(Material material)
        {
            _context.Materials.Add(material);
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> DeleteMaterial(Guid id)
        {
            _context.Materials.Remove(_context.Materials.Find(id) ?? throw new Exception("Material not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Material>> GetAllMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material> GetMaterialById(Guid id)
        {
            return await _context.Materials.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Material not found");
        }

        public async Task<bool> UpdateMaterial(Material material)
        {
            _context.Materials.Update(material);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
