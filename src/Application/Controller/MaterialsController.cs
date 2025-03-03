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
using DevKid.src.Application.Service;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly ILessonRepo _lessonRepo;
        private readonly IMaterialRepo _materialRepo;
        private readonly IMapper _mapper;
        private readonly IBoughtCertificateService _boughtCertificateService;

        public MaterialsController(IMediaService mediaService, ILessonRepo lessonRepo, IMaterialRepo materialRepo, IMapper mapper, IBoughtCertificateService boughtCertificateService)
        {
            _mediaService = mediaService;
            _lessonRepo = lessonRepo;
            _materialRepo = materialRepo;
            _mapper = mapper;
            _boughtCertificateService = boughtCertificateService;
        }
        [Protected]
        [HttpPost("upload-media")]
        [Permission(PermissionSlug.MATERIAL_ALL)]
        public async Task<IActionResult> UploadMedia(IFormFile file, string type)
        {
            var response = await _mediaService.UploadMedia(file, type);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        //// GET: api/Materials
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var materials = await _materialRepo.GetAllMaterials();
        //        if (materials != null)
        //        {
        //            response.Message = "Materials fetched successfully";
        //            response.Result = new ResultDto
        //            {
        //                Data = _mapper.Map<IEnumerable<MaterialDto>>(materials)
        //            };
        //            response.IsSuccess = true;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            response.Message = "Materials not fetched";
        //            response.IsSuccess = false;
        //            return BadRequest(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //        response.IsSuccess = false;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}

        [Protected]
        [HttpGet("{id}")]
        [Permission(PermissionSlug.MATERIAL_ALL, PermissionSlug.MATERIAL_VIEW)]
        public async Task<ActionResult<Material>> GetMaterial(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var material = await _materialRepo.GetMaterialById(id);
                if (material != null)
                {
                    var payload = HttpContext.Items["payload"] as Payload;
                    if (payload == null)
                    {
                        response.Message = "Unauthorized";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
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

        [Protected]
        [HttpPut("{id}")]
        [Permission(PermissionSlug.MATERIAL_ALL)]
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

        [Protected]
        [HttpPost]
        [Permission(PermissionSlug.MATERIAL_ALL)]
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
        [Protected]
        [HttpDelete("{id}")]
        [Permission(PermissionSlug.MATERIAL_ALL)]
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
        [Protected]
        [HttpGet("lesson/{lessonId}")]
        [Permission(PermissionSlug.MATERIAL_ALL, PermissionSlug.MATERIAL_VIEW)]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterialsByLessonId(Guid lessonId)
        {
            var response = new ResponseDto();
            try
            {
                var materials = await _materialRepo.GetMaterialsByLessonId(lessonId);
                if (materials != null)
                {
                    var payload = HttpContext.Items["payload"] as Payload;
                    if (payload == null)
                    {
                        response.Message = "Unauthorized";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(lessonId, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
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
    }
}
