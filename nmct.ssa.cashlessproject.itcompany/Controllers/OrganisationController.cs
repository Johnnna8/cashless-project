using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ssa.cashlessproject.itcompany.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ssa.cashlessproject.itcompany.Controllers
{
    public class OrganisationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Organisation> organisations = OrganisationDA.GetOrganisations();
            return View(organisations);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            Organisation organisation = OrganisationDA.GetOrganisationByID(id.Value);

            if (organisation == null)
                return RedirectToAction("Index");

            organisation.Login = Cryptography.Decrypt(organisation.Login);
            organisation.Password = Cryptography.Decrypt(organisation.Password);
            organisation.DbName = Cryptography.Decrypt(organisation.DbName);
            organisation.DbLogin = Cryptography.Decrypt(organisation.DbLogin);
            organisation.DbPassword = Cryptography.Decrypt(organisation.DbPassword);

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

            organisation.Login = Cryptography.Decrypt(organisation.Login);
            organisation.Password = Cryptography.Decrypt(organisation.Password);
            organisation.DbName = Cryptography.Decrypt(organisation.DbName);
            organisation.DbLogin = Cryptography.Decrypt(organisation.DbLogin);
            organisation.DbPassword = Cryptography.Decrypt(organisation.DbPassword);

            return View(organisation);
        }

        [HttpPost]
        public ActionResult Edit(Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                organisation.Login = Cryptography.Encrypt(organisation.Login);
                organisation.Password = Cryptography.Encrypt(organisation.Password);
                organisation.DbName = Cryptography.Encrypt(organisation.DbName);
                organisation.DbLogin = Cryptography.Encrypt(organisation.DbLogin);
                organisation.DbPassword = Cryptography.Encrypt(organisation.DbPassword);

                OrganisationDA.EditOrganisation(organisation);
                return View("Details", organisation);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            Organisation organisation = OrganisationDA.GetOrganisationByID(id.Value);

            if (organisation == null)
                return RedirectToAction("Index");

            organisation.Login = Cryptography.Decrypt(organisation.Login);
            organisation.Password = Cryptography.Decrypt(organisation.Password);
            organisation.DbName = Cryptography.Decrypt(organisation.DbName);
            organisation.DbLogin = Cryptography.Decrypt(organisation.DbLogin);
            organisation.DbPassword = Cryptography.Decrypt(organisation.DbPassword);

            return View(organisation);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteByID(int? id)
        {
            if (id.HasValue)
            {
                OrganisationDA.DeleteOrganisation(id.Value);
            }

            return RedirectToAction("Index");
        }
    }
}