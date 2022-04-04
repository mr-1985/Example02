using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date",
            int minPrice = 0, int maxPrice = 0, List<int> selectedGroups = null)
        {
            return View(_courseService.GetCourse(pageId,filter,getType,orderByType,minPrice,maxPrice,selectedGroups,9));
        }
    }
}
