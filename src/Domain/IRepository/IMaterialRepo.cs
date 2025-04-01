using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IMaterialRepo
    {
        Task<IEnumerable<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid id);
        Task<bool> AddMaterial(Material material);
        Task<bool> UpdateMaterial(Material material);
        Task<bool> DeleteMaterial(Guid id);
        Task<IEnumerable<Material>> GetMaterialsByLessonId(Guid lessonId);
        Task<Material> ReturnAddMaterial(Material material);
    }
}
