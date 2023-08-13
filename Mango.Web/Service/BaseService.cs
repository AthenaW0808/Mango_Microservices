using System.Net;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;

namespace Mango.Web.Service;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            
            //token

            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8,
                    "application/json");
            }

            HttpResponseMessage apiResponse = null;


            switch (requestDto.ApiType)
            {
                case SD.ApiType.PUT:
                    message.Method=HttpMethod.Get;
                    break;
                case SD.ApiType.POST:
                    message.Method=HttpMethod.Post;
                    break;
                case SD.ApiType.DELETE:
                    message.Method=HttpMethod.Delete;
                    break;
                default:
                    message.Method=HttpMethod.Get;
                    break;
                
            }
            
            apiResponse = await client.SendAsync(message);

        
            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new()
                    {
                        IsSuccess = false,
                        Message = "Not Found"
                    };
                case HttpStatusCode.Forbidden:
                    return new()
                    {
                        IsSuccess = false,
                        Message = "Access Denied"
                    };
                case HttpStatusCode.Unauthorized:
                    return new()
                    {
                        IsSuccess = false,
                        Message = "Unauthorized"
                    };
                case HttpStatusCode.InternalServerError:
                    return new()
                    {
                        IsSuccess = false,
                        Message = "Internal Server Error"
                    };
                default:
                    // 输出 API 响应内容
                    Console.WriteLine("API Response: ");
                    string apiResponseString = await apiResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(apiResponseString);

                    // 解析 JSON 数据
                    ResponseDto? responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiResponseString);

                    // 输出 IsSuccess 和 Message
                    Console.WriteLine("IsSuccess: " + responseDto.IsSuccess);
                    Console.WriteLine("Message: " + responseDto.Message);

                    // 输出 CouponDto 对象
                    Console.WriteLine("Result: " + responseDto.Result);

                    return responseDto;

            }
        }
        catch (Exception e)
        {
            var dto = new ResponseDto
            {
                Message = e.Message.ToString(),
                IsSuccess = false
            };
            return dto;
        }
        

    

    }
}