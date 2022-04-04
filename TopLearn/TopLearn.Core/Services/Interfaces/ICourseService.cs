using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Course;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetAllGroups();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatuses();

        #endregion

        #region Course

        List<ShowCourseForAdminViewModel> GetCourseForAdmin();
        int AddCourse(Course course, IFormFile imageCourse, IFormFile demoCourse);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imageCourse, IFormFile demoCourse);

        List<ShowCourseListItemViewModel> GetCourse(int pageId = 1, string filter = "", string getType = "all",
            string orderByType="date", int minPrice = 0, int maxPrice = 0, List<int> selectedGroups=null,int take=0);
        #endregion

        #region Episode

        List<CourseEpisode> GetListEpisodeCourse(int courseId);
        bool CheckExistFile(string fileName);
        int AddEpisode(CourseEpisode episode, IFormFile episodeFile);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);
        #endregion
    }
}
