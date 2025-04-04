﻿using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Service;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IBoughtCertificateService _boughtCertificateService;

        public OrdersController(IOrderRepo orderRepo, IMapper mapper, IPayOSService payOSService, ICourseRepo courseRepo, IPaymentRepo paymentRepo, IBoughtCertificateService boughtCertificateService)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
            _payOSService = payOSService;
            _courseRepo = courseRepo;
            _paymentRepo = paymentRepo;
            _boughtCertificateService = boughtCertificateService;
        }

        [Protected]
        [HttpGet]
        [Permission(PermissionSlug.ORDER_ALL, PermissionSlug.ORDER_VIEW)]
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

        [Protected]
        [HttpGet("{id}")]
        [Permission(PermissionSlug.ORDER_ALL, PermissionSlug.ORDER_VIEW)]
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
        [Protected]
        [HttpPost("payment-url")]
        [Permission(PermissionSlug.ORDER_ALL, PermissionSlug.ORDER_OWN)]
        public async Task<ActionResult> CreatePaymentUrl([FromQuery] Guid courseId)
        {
            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return Unauthorized(response);
                }
                var course = await _courseRepo.GetCourseById(courseId);
                if (await _orderRepo.IsCourseOrderExist(courseId, payload.UserId))
                {
                    response.Message = "Order for this course already been queue";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                var paymentUrl = await _payOSService.GeneratePaymentUrl(course, payload.UserId);
                if (paymentUrl != null)
                {
                    response.Message = "Payment url generated successfully";
                    response.Result = new ResultDto
                    {
                        Data = paymentUrl,
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
                if (webhookData.Data.OrderCode == 123)
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

        [Protected]
        [HttpDelete("{id}")]
        [Permission(PermissionSlug.ORDER_ALL)]
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
        [Protected]
        [HttpPost("own")]
        [Permission(PermissionSlug.ORDER_ALL, PermissionSlug.ORDER_OWN)]
        public async Task<ActionResult> GetOrderOwn()
        {
            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return Unauthorized(response);
                }
                var orders = await _orderRepo.GetOrdersByUserId(payload.UserId);
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

    }
}
