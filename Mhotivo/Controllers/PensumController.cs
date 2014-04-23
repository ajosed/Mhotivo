using System.Linq;
using System.Web.Mvc;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class PensumController : Controller
    {
        private readonly PensumRepository _PensumRepo = PensumRepository.Instance;
        private readonly CourseRepository _courseRepo = CourseRepository.Instance;

        [AllowAnonymous]
        public ActionResult Index()
        {
            var message = (MessageModel)TempData["MessageInfo"];

            if (message != null)
            {
                ViewBag.MessageType = message.MessageType;
                ViewBag.MessageTitle = message.MessageTitle;
                ViewBag.MessageContent = message.MessageContent;
            }

            return View(_PensumRepo.Query(x => x).ToList()
                .Select(x => new DisplayPensumModel
                {
                    Id = x.Id,
                    Grade = x.Grade.Name,
                    Course = x.Course.Name
                    
                }));   
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var thisUser = _PensumRepo.GetById(id);
            var user = new PensumEditModel
            {
                CourseId = thisUser.Course.CourseId,
                Id = thisUser.Id, 
                Grade = thisUser.Grade
            };

            ViewBag.CourseId = new SelectList(_courseRepo.Query(x => x), "CourseId", "Name", thisUser.Course.CourseId);

            return View("Edit", user);
        }

        [HttpPost]
        public ActionResult Edit(PensumEditModel modelUser)
        {
            var updateRole = false;
            var myUser = _PensumRepo.GetById(modelUser.Id);
            myUser.Id=modelUser.Id;
            myUser.Grade = modelUser.Grade;

            if (myUser.Course.CourseId != modelUser.CourseId)
            {
                myUser.Course = _courseRepo.GetById(modelUser.CourseId);
                updateRole = true;
            }
            
            var user = _PensumRepo.Update(myUser, updateRole);
            const string title = "Pensum Actualizado";
            var content = "El pensum " + user.Id + " ha sido actualizado exitosamente.";

            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var user = _PensumRepo.Delete(id);

            const string title = "Pensum Eliminado";
            var content = "El Pensum " + user.Id + " ha sido eliminado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO", MessageTitle = title, MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CourseId = new SelectList(_courseRepo.Query(x => x), "CourseId", "Name");
            return View("Create");
        }

        [HttpPost]
        public ActionResult Add(PensumRegisterModel modelUser)
        {
            var myUser = new Pensum
            {
                Grade = modelUser.Grade,
                Course = _courseRepo.GetById(modelUser.CourseId)
            };

            var user = _PensumRepo.Create(myUser);
            const string title = "Pensum Agregado";
            var content = "El Pensum " + user.Id + " ha sido agregado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "SUCCESS",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }
    }
}
