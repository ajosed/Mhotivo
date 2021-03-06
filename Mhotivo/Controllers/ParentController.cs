﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class ParentController : Controller
    {
        private readonly ParentRepository _parentRepo = ParentRepository.Instance;
        private readonly PeopleRepository _peopleRepo = PeopleRepository.Instance;
        private readonly ContactInformationRepository _contactRepo = ContactInformationRepository.Instance;

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

            return View(_parentRepo.Query(x => x).ToList()
                .Select(x => new DisplayParentModel
                {
                    ParentID = x.PeopleId,
                    IDNumber = x.IDNumber,
                    UrlPicture = x.UrlPicture,
                    FullName = x.FullName,
                    BirthDate = x.BirthDate.ToShortDateString(),
                    Nationality = x.Nationality,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    Gender = _peopleRepo.SexLabel(x.Gender),
                    Contacts = x.Contacts,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }));
        }

        [HttpGet]
        public ActionResult ContactEdit(long id)
        {
            var thisContactInformation = _contactRepo.GetById(id);
            var contactInformation = new ContactInformationEditModel
            {
                Type = thisContactInformation.Type,
                Value = thisContactInformation.Value,
                Id = thisContactInformation.ContactId,
                people = thisContactInformation.People,
                Controller = "Parent"
            };

            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var thisParent = _parentRepo.GetById(id);
            var parent = new ParentEditModel
            {
                FirstName = thisParent.FirstName,
                LastName = thisParent.LastName,
                FullName = (thisParent.FirstName + " " + thisParent.LastName).Trim(),
                IDNumber = thisParent.IDNumber,
                BirthDate = thisParent.BirthDate.ToShortDateString(),
                Gender = _peopleRepo.SexLabel(thisParent.Gender),
                Nationality = thisParent.Nationality,
                Country = thisParent.Country,
                State = thisParent.State,
                City = thisParent.City,
                Address = thisParent.Address,
                Id = thisParent.PeopleId,
            };

            return View("Edit", parent);
        }

        [HttpPost]
        public ActionResult Edit(ParentEditModel modelParent)
        {
            var myParent = _parentRepo.GetById(modelParent.Id);

            myParent.FirstName = modelParent.FirstName;
            myParent.LastName = modelParent.LastName;
            myParent.FullName = (modelParent.FirstName + " " + modelParent.LastName).Trim();
            myParent.Country = modelParent.Country;
            myParent.IDNumber = modelParent.IDNumber;
            myParent.BirthDate = DateTime.Parse(modelParent.BirthDate);
            myParent.Gender = _peopleRepo.IsMasculino(modelParent.Gender);
            myParent.Nationality = modelParent.Nationality;
            myParent.State = modelParent.State;
            myParent.City = modelParent.City;
            myParent.Address = modelParent.Address;

            var parent = _parentRepo.Update(myParent);
            const string title = "Padre o Tutor Actualizado";
            var content = "El Padre o Tutor " + myParent.FullName + " ha sido actualizado exitosamente.";

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
            var parent = _parentRepo.Delete(id);

            const string title = "Padre o Tutor Eliminado";
            var content = "El Padre o Tutor " + parent.FullName + " ha sido eliminado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
            {
                PeopleId = (int)id,
                Controller = "Parent"
            };
            return View("ContactAdd", model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost]
        public ActionResult Add(ParentRegisterModel modelParent)
        {
            var myParent = new Parent
            {
                FirstName = modelParent.FirstName,
                LastName = modelParent.LastName,
                FullName = (modelParent.FirstName + " " + modelParent.LastName).Trim(),
                IDNumber = modelParent.IDNumber,
                BirthDate = DateTime.Parse(modelParent.BirthDate),
                Gender = _peopleRepo.IsMasculino(modelParent.Gender),
                Nationality = modelParent.Nationality,
                State = modelParent.State,
                Country = modelParent.Country,
                City = modelParent.City,
                Address = modelParent.Address
            };

            var parent = _parentRepo.Create(myParent);
            const string title = "Padre o Tutor Agregado";
            var content = "El Padre o Tutor " + myParent.FullName + "ha sido agregado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "SUCCESS",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            var thisParent = _parentRepo.GetById(id);
            var parent = new DisplayParentModel
            {
                ParentID = thisParent.PeopleId,
                IDNumber = thisParent.IDNumber,
                UrlPicture = thisParent.UrlPicture,
                FirstName = thisParent.FirstName,
                LastName = thisParent.LastName,
                FullName = thisParent.FullName,
                BirthDate = thisParent.BirthDate.ToShortDateString(),
                Nationality = thisParent.Nationality,
                Address = thisParent.Address,
                City = thisParent.City,
                State = thisParent.State,
                Country = thisParent.Country,
                Gender = _peopleRepo.SexLabel(thisParent.Gender),
                Contacts = thisParent.Contacts
            };

            return View("Details", parent);
        }

        [HttpGet]
        public ActionResult DetailsEdit(long id)
        {
            var thisParent = _parentRepo.GetById(id);
            var parent = new ParentEditModel
            {
                FirstName = thisParent.FirstName,
                LastName = thisParent.LastName,
                FullName = (thisParent.FirstName + " " + thisParent.LastName).Trim(),
                IDNumber = thisParent.IDNumber,
                BirthDate = thisParent.BirthDate.ToShortDateString(),
                Gender = _peopleRepo.SexLabel(thisParent.Gender),
                Nationality = thisParent.Nationality,
                Country = thisParent.Country,
                State = thisParent.State,
                City = thisParent.City,
                Address = thisParent.Address,
                Id = thisParent.PeopleId
            };
            return View("DetailsEdit", parent);
        }

        [HttpPost]
        public ActionResult DetailsEdit(ParentEditModel modelParent)
        {
            var myParent = _parentRepo.GetById(modelParent.Id);
            myParent.FirstName = modelParent.FirstName;
            myParent.LastName = modelParent.LastName;
            myParent.FullName = (modelParent.FirstName + " " + modelParent.LastName).Trim();
            myParent.IDNumber = modelParent.IDNumber;
            myParent.BirthDate = DateTime.Parse(modelParent.BirthDate);
            myParent.Gender = _peopleRepo.IsMasculino(modelParent.Gender);
            myParent.Nationality = modelParent.Nationality;
            myParent.State = modelParent.State;
            myParent.City = modelParent.City;
            myParent.Country = modelParent.Country;
            myParent.Address = modelParent.Address;
            var parent = _parentRepo.Update(myParent);
            const string title = "Padre o Tutor Actualizado";
            var content = "El Padre o Tutor " + myParent.FullName + " ha sido actualizado exitosamente.";

            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Details/" + modelParent.Id);
        }
    }
}
