using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevKid.src.Domain.Entities;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Infrastructure.Repository;
using DevKid.src.Domain.IRepository;
using DevKid.src.Application.Dto.ResponseDtos;
using AutoMapper;
using DevKid.src.Application.Dto;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialRepo _materialRepo;
        private readonly IMapper _mapper;
        private readonly ILessonRepo _lessonRepo;

        public MaterialsController(IMaterialRepo materialRepo, IMapper mapper, ILessonRepo lessonRepo)
        {
            _materialRepo = materialRepo;
            _mapper = mapper;
            _lessonRepo = lessonRepo;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        {
            var response = new ResponseDto();
            try
            {
                var materials = await _materialRepo.GetAllMaterials();
                if (materials != null)
                {
                    response.Message = "Materials fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<MaterialDto>>(materials)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Materials not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var material = await _materialRepo.GetMaterialById(id);
                if (material != null)
                {
                    response.Message = "Material fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<MaterialDto>(material)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Material not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        // PUT: api/Materials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(Guid id, MaterialUpdateDto material)
        {
            var response = new ResponseDto();
            try
            {
                var materialToUpdate = await _materialRepo.GetMaterialById(id);
                var mappedMaterial = _mapper.Map(material, materialToUpdate);
                var result = await _materialRepo.UpdateMaterial(mappedMaterial);
                if (result)
                {
                    response.Message = "Material updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Material not updated";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        // POST: api/Materials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(MaterialCreateDto material)
        {
            var response = new ResponseDto();
            try
            {
                var lesson = await _lessonRepo.GetLessonById(material.LessonId);
                var mappedMaterial = _mapper.Map<Material>(material);
                mappedMaterial.Lesson = lesson;
                var result = await _materialRepo.AddMaterial(mappedMaterial);
                if (result)
                {
                    response.Message = "Material added successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Material not added";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _materialRepo.DeleteMaterial(id);
                if (result)
                {
                    response.Message = "Material deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Material not deleted";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
