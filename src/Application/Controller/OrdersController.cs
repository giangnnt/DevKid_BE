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

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepo orderRepo, IMapper mapper)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            throw new NotImplementedException();
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
