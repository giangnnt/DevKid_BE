using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IService
{
    public interface IMaterialService
    {
        Task<ResponseDto> GetAllMaterials();
        Task<ResponseDto> GetMaterialById(Guid id);
        Task<ResponseDto> AddMaterial(Material material);
        Task<ResponseDto> UpdateMaterial(Material material);
        Task<ResponseDto> DeleteMaterial(Guid id);
    }
}
