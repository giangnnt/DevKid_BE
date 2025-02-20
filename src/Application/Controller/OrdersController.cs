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
using DevKid.src.Application.Service;
using DevKid.src.Application.Dto;

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
        private readonly IPaymentRepo _paymentRepo;

        public OrdersController(IOrderRepo orderRepo, IMapper mapper, IPayOSService payOSService, ICourseRepo courseRepo, IPaymentRepo paymentRepo)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
            _payOSService = payOSService;
            _courseRepo = courseRepo;
            _paymentRepo = paymentRepo;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult> GetOrders()
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
                        Data = _mapper.Map<IEnumerable<OrderDto>>(orders)
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
        public async Task<ActionResult> GetOrder(long id)
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
                        Data = _mapper.Map<OrderDto>(order)
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
        public async Task<ActionResult> CreatePaymentUrl([FromQuery] Guid courseId)
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
        [HttpPost("payos-webhook")]
        public async Task<ActionResult> PayOSWebhook([FromBody] PayOSWebhookModel webhookData)
        {
            var response = new ResponseDto();
            try
            {
                if (webhookData == null)
                {
                    return BadRequest("Invalid webhook payload");
                }
                if(webhookData.Data.OrderCode == 123)
                {
                    return Ok(new { success = true });
                }
                var isValid = _payOSService.ValidateSignature(webhookData);
                if (isValid)
                {
                    var order = await _orderRepo.GetOrder(webhookData.Data.OrderCode);
                    order.Status = webhookData.Success switch
                    {
                        true => Order.StatusEnum.Completed,
                        false => Order.StatusEnum.Failed,
                    };
                    var payment = new Payment
                    {
                        OrderId = order.Id,
                        Amount = webhookData.Data.Amount,
                        Currency = webhookData.Data.Currency,
                        PaymentMethod = "PayOS",
                        Status = webhookData.Success switch
                        {
                            true => Payment.StatusEnum.Completed,
                            false => Payment.StatusEnum.Failed,
                        },
                        CreateAt = DateTime.UtcNow,

                    };
                    var result1 = await _orderRepo.UpdateOrder(order);
                    var result2 = await _paymentRepo.AddPayment(payment);
                    if (result1 && result2)
                    {
                        response.Message = "Order updated successfully";
                        response.IsSuccess = true;
                        return Ok(new { success = true });
                    }
                    else
                    {
                        response.Message = "Order not updated";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
                else
                {
                    return BadRequest("Invalid signature");
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
        public async Task<IActionResult> DeleteOrder(long id)
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
