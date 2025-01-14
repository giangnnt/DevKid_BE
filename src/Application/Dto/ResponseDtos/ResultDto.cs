using static DevKid.src.Application.Dto.PagingDtos.Pagination;

namespace DevKid.src.Application.Dto.ResponseDtos
{
    public class ResultDto
    {
        public required object Data { get; set; }
        public PaginationResp? PaginationResp { get; set; }
    }
}
