using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevKid.src.Domain.Entities;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Domain.IRepository;
using AutoMapper;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.ExternalService;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;
        private readonly ICourseRepo _courseRepo;

        public OrdersController(IOrderRepo orderRepo, IMapper mapper, IPayOSService payOSService, ICourseRepo courseRepo)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
            _payOSService = payOSService;
            _courseRepo = courseRepo;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepo.GetOrders();
                if (orders != null)
                {
                    response.Message = "Orders fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<Order>>(orders)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Orders not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var order = await _orderRepo.GetOrder(id);
                if (order != null)
                {
                    response.Message = "Order fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<Order>(order)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Order not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return BadRequest(response);
            }

        }

        [HttpPost("payment-url")]
        public async Task<ActionResult> CreatePaymentUrl([FromQuery]Guid courseId)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(courseId);
                var paymentUrl = await _payOSService.GeneratePaymentUrl(course);
                if (paymentUrl != null)
                {
                    response.Message = "Payment url generated successfully";
                    response.Result = new ResultDto
                    {
                        Data = paymentUrl
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Payment url not generated";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _orderRepo.DeleteOrder(id);
                if (result)
                {
                    response.Message = "Order deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Order not deleted";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }
    }
}
