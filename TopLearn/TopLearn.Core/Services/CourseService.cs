using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        private TopLearnContext _context;

        public CourseService(TopLearnContext context)
        {
            _context = context;
        }
        public List<CourseGroup> GetAllGroups()
        {
            return _context.CourseGroups.ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.CourseGroups
                .Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
        {
            return _context.CourseGroups
                .Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles
                .Where(r => r.RoleId == 2)
                .Include(u => u.User)
                .Select(u => new SelectListItem()
                {
                    Value = u.RoleId.ToString(),
                    Text = u.User.UserName
                })
                .ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels
                .Select(l => new SelectListItem()
                {
                    Value = l.LevelId.ToString(),
                    Text = l.LevelTitle
                }).ToList();
        }

        public List<SelectListItem> GetStatuses()
        {
            return _context.CourseStatus
                .Select(s => new SelectListItem()
                {
                    Value = s.StatusId.ToString(),
                    Text = s.StatusTitle
                }).ToList();
        }

        public List<ShowCourseForAdminViewModel> GetCourseForAdmin()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseId = c.CourseId,
                ImageName = c.CourseImageName,
                Title = c.CourseTitle,
                EpisodeCount = c.CourseEpisodes.Count
            }).ToList();
        }

        public int AddCourse(Course course, IFormFile imageCourse, IFormFile demoCourse)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";

            //Check Image

            if (imageCourse != null && imageCourse.IsImage())
            {
                course.CourseImageName = NameGenertor.GenerateUniqCode() + Path.GetExtension(imageCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Image",
                    course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageCourse.CopyTo(stream);
                }

                //Resize Image
                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Thumb",
                    course.CourseImageName);
                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            //Upload Demo
            if (demoCourse != null)
            {
                course.DemoFileName = NameGenertor.GenerateUniqCode() + Path.GetExtension(demoCourse.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Demoes",
                    course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoCourse.CopyTo(stream);
                }
            }

            _context.Add(course);
            _context.SaveChanges();
            return course.CourseId;
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void UpdateCourse(Course course, IFormFile imageCourse, IFormFile demoCourse)
        {
            course.UpdateDate = DateTime.Now;

            if (imageCourse != null && imageCourse.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Image",
                        course.CourseImageName);
                    if (File.Exists(deleteImagePath))
                    {
                        File.Delete(deleteImagePath);
                    }

                    string deleteThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Thumb",
                        course.CourseImageName);
                    if (File.Exists(deleteThumbPath))
                    {
                        File.Delete(deleteThumbPath);
                    }
                }

                course.CourseImageName = NameGenertor.GenerateUniqCode() + Path.GetExtension(imageCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Image",
                    course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageCourse.CopyTo(stream);
                }

                //Resize Image
                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Thumb",
                    course.CourseImageName);
                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            //Upload Demo
            if (demoCourse != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Demoes",
                        course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenertor.GenerateUniqCode() + Path.GetExtension(demoCourse.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Demoes",
                    course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoCourse.CopyTo(stream);
                }
            }

            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public List<ShowCourseListItemViewModel> GetCourse(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date",
            int minPrice = 0, int maxPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    break;

                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice > 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updateDate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (minPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > minPrice);
            }

            if (maxPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < maxPrice);
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int groupId in selectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
                }
            }

            int skip = (pageId - 1) * take;

            return result.Include(c => c.CourseEpisodes)
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    Title = c.CourseTitle,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    CourseEpisodes = c.CourseEpisodes
                }).Skip(skip).Take(take).ToList();
        }

        public List<CourseEpisode> GetListEpisodeCourse(int courseId)
        {
            return _context.CourseEpisodes.Where(e => e.CourseId == courseId).ToList();
        }

        public bool CheckExistFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles",
                fileName);
            return File.Exists(path);
        }

        public int AddEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            episode.EpisodeFileName = episodeFile.FileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles",
                episode.EpisodeFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles",
                    episode.EpisodeFileName);
                File.Delete(deleteFilePath);

                episode.EpisodeFileName = episodeFile.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles",
                    episode.EpisodeFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }

            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }
    }
}
