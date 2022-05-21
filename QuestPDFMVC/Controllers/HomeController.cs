using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuestPDFLib;
using QuestPDFMVC.Models;

namespace QuestPDFMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlobService _blobService;
    private readonly string _blobStoreBaseAddress;

    public HomeController(ILogger<HomeController> logger, BlobService blobService, IConfiguration configuration)
    {
        _logger = logger;
        _blobService = blobService;
        _blobStoreBaseAddress = configuration.GetValue<string>("StorageAccount:BaseAddress");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<string> GetPDFURL([FromQuery] string fileName, [FromQuery] string title)
    {
        await _blobService.Upload(fileName, title);
        return _blobStoreBaseAddress + fileName;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
