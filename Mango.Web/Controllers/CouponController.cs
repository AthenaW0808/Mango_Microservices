using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Mango.Web.Controllers;

public class CouponController : Controller
{

    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto> list = new List<CouponDto>();
        ResponseDto? response = await _couponService.GetAllCouponsAsync();

        // 输出响应的 JSON 数据，用于检查
        Console.WriteLine("GET ALL API Response: " + response.Result);
        Console.WriteLine("GET ALL API Respinse Success: " + response.IsSuccess);
        
        if (response != null && response.IsSuccess)
        {
            list= JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        
    
        return View(list);
    }

  
    public async Task<IActionResult> CouponCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CouponCreate(CouponDto model)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("!ModelState.IsValid");
            ModelState.Clear(); // Clear previous model state
            return View(model);
        }

        ResponseDto? response = await _couponService.CreateCouponAsync(model);

      
        if (response.IsSuccess)
        {
            TempData["success"] = "Coupon created successfully";
            return RedirectToAction(nameof(CouponIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(model);
    }
    
    public async Task<IActionResult> CouponDelete(int couponId)
    {
        ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

        if (response != null && response.IsSuccess)
        {
            CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> CouponDelete(CouponDto couponDto)
    {
        
        ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
        Console.WriteLine("Coupon Delete Response" + response);
        Console.WriteLine("Coupon Delete Response success: " + response.IsSuccess);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Coupon deleted successfully";

            return RedirectToAction(nameof(CouponIndex));
        }

        return View(couponDto);
    }

}