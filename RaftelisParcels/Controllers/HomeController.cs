using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using RaftelisParcels.Models;
using System.Diagnostics;

namespace RaftelisParcels.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            string? file = Directory.GetFiles("App_Data").FirstOrDefault();
            if (file != null)
                ViewBag.File = file.Substring(file.IndexOf("\\") + 1);
            else
                ViewBag.NoFile = "No file available to sort.";
            return View();
        }

        public IActionResult SortByStreet() 
        {
            return RedirectToAction("ViewParcel", new { sortBy = 1 });
        }

        public IActionResult SortByName()
        {
            return RedirectToAction("ViewParcel", new { sortBy = 2 });
        }

        public IActionResult ViewParcel(int sortBy)
        {
            ViewBag.SortBy = sortBy;

            var parcels = System.IO.File.ReadAllLines("App_Data/Parcels.txt").ToList();

            IList<ParcelViewModel> parcelViewModel = new List<ParcelViewModel>();

            foreach (var parcel in parcels.Skip(1))
            {
                string[] parcelDetail = parcel.Split('|');
                string pin = parcelDetail[0];
                string address = parcelDetail[1].Trim();
                string unit = "";
                int.TryParse(address.Substring(0, address.IndexOf(" ")), out int houseNumber);
                string street = address.Substring(address.IndexOf(" ")).Trim();
                int index = street.IndexOf(" ");

                //get unit if exists
                if (index == 1)
                {
                        unit = street.Substring(0, 1);
                        street = street.Substring(1).Trim();
                }  
                
                string owner = parcelDetail[2];

                string firstName = owner.Substring(owner.IndexOf(",") + 1).Trim();
                    
                //check for more than one first name if wanting to display only one
                if(firstName.IndexOf(" ") > 0)
                {                      
                        firstName = firstName.Substring(0, firstName.IndexOf(" "));
                }

                decimal value = Convert.ToDecimal(parcelDetail[3]);
                string saleDate = parcelDetail[4];
                decimal salePrice = Convert.ToDecimal(parcelDetail[5]);
                string link = parcelDetail[6];
                string addressURL = "https://www.google.com/maps/search/?api=1&query="+Uri.EscapeDataString(address + " Mazama WA");

                var parcelView = new ParcelViewModel
                {
                    Pin = pin,
                    HouseNumber = houseNumber,
                    Street = street,
                    Owner = owner,
                    Value = value,
                    SaleDate = saleDate,
                    SalePrice = salePrice,
                    Link = link,
                    LocationUrl= addressURL,
                    Unit = unit,
                    FirstName = firstName
                };

                parcelViewModel.Add(parcelView);
            }

            if (sortBy == 1)
            {
                parcelViewModel = parcelViewModel.OrderBy(p => p.Street).ThenBy(p => p.HouseNumber).ThenBy(p => p.Unit).ToList();
            }
            else if (sortBy == 2)
            {
                parcelViewModel = parcelViewModel.OrderBy(p => p.FirstName).ToList();
            }

            return View("ParcelView", parcelViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}