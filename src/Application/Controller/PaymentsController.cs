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
using DevKid.src.Application.Dto;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Core;
using DevKid.src.Application.Constant;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentRepo paymentRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
        }

        [Protected]
        [HttpGet]
        [Permission(PermissionSlug.PAYMENT_ALL, PermissionSlug.PAYMENT_VIEW)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var response = new ResponseDto();
            try
            {
                var payments = await _paymentRepo.GetPayments();
                if (payments != null)
                {
                    response.Message = "Payments fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<PaymentDto>>(payments)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Payments not fetched";
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
        [Permission(PermissionSlug.PAYMENT_ALL, PermissionSlug.PAYMENT_VIEW)]
        public async Task<ActionResult> GetPayment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var payment = await _paymentRepo.GetPayment(id);
                if (payment != null)
                {
                    response.Message = "Payment fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<PaymentDto>(payment)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Payment not fetched";
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

        //[Protected]
        //[HttpPost]
        //public async Task<ActionResult> PostPayment(PaymentCreateDto payment)
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var order = await _orderRepo.GetOrder(payment.OrderId);
        //        var mappedPayment = _mapper.Map<Payment>(payment);
        //        var result = await _paymentRepo.AddPayment(mappedPayment);
        //        if (result)
        //        {
        //            response.Message = "Payment added successfully";
        //            response.IsSuccess = true;
        //            return Created("", response);
        //        }
        //        else
        //        {
        //            response.Message = "Payment not added";
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
        [HttpDelete("{id}")]
        [Permission(PermissionSlug.PAYMENT_ALL)]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _paymentRepo.DeletePayment(id);
                if (result)
                {
                    response.Message = "Payment deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Payment not deleted";
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
        [HttpGet("own")]
        [Permission(PermissionSlug.PAYMENT_ALL, PermissionSlug.PAYMENT_OWN)]
        public async Task<ActionResult> GetOwnPayments()
        {
            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Payload not found";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                var payments = await _paymentRepo.GetPaymentsByUserId(payload.UserId);
                if (payments != null)
                {
                    response.Message = "Payments fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<PaymentDto>>(payments)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Payments not fetched";
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
