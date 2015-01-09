using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ssa.cashlessproject.itcompany.DataAccess;
using nmct.ssa.cashlessproject.itcompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ssa.cashlessproject.itcompany.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrganisationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //vereniging die geen vereniging voorstel niet tonen
            return View(OrganisationDA.GetOrganisations().Where(r => r.ID != -1));
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            Organisation organisation = OrganisationDA.GetOrganisationByID(id.Value);

            if (organisation == null)
                return RedirectToAction("Index");

            return View(organisation);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Organisation organisation = new Organisation();
            return View(organisation);
        }

        [HttpPost]
        public ActionResult Create(Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                organisation.Login = Cryptography.Encrypt(organisation.Login);
                organisation.Password = Cryptography.Encrypt(organisation.Password);
                organisation.DbName = Cryptography.Encrypt(organisation.DbName);
                organisation.DbLogin = Cryptography.Encrypt(organisation.DbLogin);
                organisation.DbPassword = Cryptography.Encrypt(organisation.DbPassword);

                CreateDatabaseOrganisation.CreateDatabase(organisation);
                OrganisationDA.NewOrganisation(organisation);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            Organisation organisation = OrganisationDA.GetOrganisationByID(id.Value);

            if (organisation == null)
                return RedirectToAction("Index");

            return View(organisation);
        }

        [HttpPost]
        public ActionResult Edit(Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                OrganisationDA.EditOrganisation(organisation);
                return View("Details", organisation);
            }

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (!id.HasValue)
        //        return RedirectToAction("Index");

        //    Organisation organisation = OrganisationDA.GetOrganisationByID(id.Value);

        //    if (organisation == null)
        //        return RedirectToAction("Index");

        //    return View(organisation);
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //public ActionResult DeleteByID(int? id)
        //{
        //    if (id.HasValue)
        //    {
        //        OrganisationDA.DeleteOrganisation(id.Value);
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}